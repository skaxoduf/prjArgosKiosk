Imports System.IO.Ports
Imports System.Diagnostics ' Debug.WriteLine을 위해 추가
Imports prjArgosKiosk.clsBXLAPI

Module modFunc

    Public WithEvents serialPort As New SerialPort()

    ' 시리얼포트 연결 함수
    Public Sub ConnectSerialPort(portName As String, baudRate As Integer)
        ' 만약 포트가 이미 열려있다면 닫기
        If serialPort.IsOpen Then
            serialPort.Close()
        End If

        Try
            ' 포트 설정
            serialPort.PortName = portName
            serialPort.BaudRate = baudRate
            serialPort.DataBits = 8
            serialPort.Parity = Parity.None
            serialPort.StopBits = StopBits.One
            serialPort.ReadBufferSize = 8192

            ' 포트 열기
            serialPort.Open()
            'MessageBox.Show($"{portName} 포트가 {baudRate} 통신 속도로 성공적으로 열렸습니다.", "연결 성공")
        Catch ex As Exception
            MessageBox.Show($"포트 연결 오류: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    ' 시리얼 포트를 닫는 함수
    Public Sub DisconnectSerialPort()
        If serialPort.IsOpen Then
            serialPort.Close()
            'MessageBox.Show("시리얼 포트가 닫혔습니다.", "연결 종료")
        End If
    End Sub
    Public Function Get_BalGwonHmClassName(hmClass As String) As String
        Select Case hmClass
            Case "0"
                Return "남자대인"
            Case "1"
                Return "남자소인"
            Case "2"
                Return "여자대인"
            Case "3"
                Return "여자소인"
            Case Else
                Return "남자대인"
        End Select
    End Function
    Public Function Get_BalGwonHmClassNo(hmClass As String) As String
        Select Case hmClass
            Case "남자대인"
                Return "0"
            Case "남자소인"
                Return "1"
            Case "여자대인"
                Return "2"
            Case "여자소인"
                Return "3"
            Case Else
                Return "0"
        End Select
    End Function
    Public Sub WriteLog(message As String, Optional baseFileName As String = "ArgosAPT.log")
        ' 1. 타임스탬프가 포함된 전체 로그 메시지 생성
        Dim logEntry As String = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}"

        Try
            ' --- 파일명에 날짜를 추가하는 로직 (기존과 동일) ---
            Dim fileNameWithoutExt As String = IO.Path.GetFileNameWithoutExtension(baseFileName)
            Dim fileExtension As String = IO.Path.GetExtension(baseFileName)
            Dim currentDate As String = DateTime.Now.ToString("yyyy-MM-dd")
            Dim datedFileName As String = $"{fileNameWithoutExt}_{currentDate}{fileExtension}"

            ' --- 로그 저장 경로 설정 (실행 파일 경로 하위의 "Logs" 폴더) ---
            ' 1. 실행 파일이 있는 기본 폴더 (예: C:\...MyProject\bin\Debug)
            Dim baseDirectory As String = My.Application.Info.DirectoryPath
            ' 2. "Logs"라는 하위 폴더 이름과 결합
            Dim logDirectory As String = IO.Path.Combine(baseDirectory, "Logs")

            ' 3. "Logs" 폴더가 없으면 자동으로 생성
            If Not IO.Directory.Exists(logDirectory) Then
                IO.Directory.CreateDirectory(logDirectory)
            End If

            ' 4. 최종 로그 파일의 전체 경로 (예: C:\...\bin\Debug\Logs\AppLog_2025-10-23.log)
            Dim logFilePath As String = IO.Path.Combine(logDirectory, datedFileName)

            ' 5. 파일에 로그 기록 (AppendAllText는 스레드에 안전하고 간편함)
            '    파일이 존재하면 내용을 추가하고, 없으면 새로 만듭니다.
            IO.File.AppendAllText(logFilePath, logEntry & Environment.NewLine)

        Catch ex As Exception
            ' 파일 쓰기 실패 시, UI 대신 디버그 콘솔(출력 창)에 오류를 기록합니다.
            ' 이렇게 하면 로거 때문에 프로그램이 멈추는 것을 방지할 수 있습니다.
            Debug.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] FILE LOGGING ERROR: {ex.Message}")
        End Try


        '' 프로그램 시작 시
        'WriteLog("프로그램을 시작합니다.", "MyProgram.log")
        ' SDK 초기화 실패 시
        'WriteLog("SDK 초기화 실패. 오류 코드: " & result, "MyProgram.log")

    End Sub

    ' 전자락 Bixolon 프린터로 티켓 출력하는 함수
    Public Sub PrintTicket(ByVal ticketNum As String)
        Dim bxl As New clsBXLAPI()
        Dim nResult As Integer

        ' 1. 프린터 연결 (USB 인터페이스 기준)
        ' PrinterOpen(nInterface, szPortName, nBaudRate, nDataBits, nParity, nStopBits, nFlowControl) 
        ' - nInterface: 0 (USB), 1 (Serial), ...
        ' - szPortName: USB의 경우 "USB:", Serial의 경우 "COM1" 등
        nResult = bxl.PrinterOpen(0, "USB:", 0, 0, 0, 0, 0)

        ' 연결 실패 시 처리 (Bixolon 성공 코드는 보통 0 또는 1이므로, 실제 리턴값 확인 필요)
        ' 연결이 안 되면 nResult가 에러 코드를 반환합니다.
        If nResult <> 0 And nResult <> 1 Then
            ' (성공 상수가 0인지 1인지 모듈에 따라 다를 수 있어 둘 다 체크하거나, 실패 시 로그 확인)
            WriteLog("프린터 연결 실패: " & nResult, "KioskLog.log")
            Exit Sub
        End If

        ' --- 인자값 설정 ---
        ' Alignment: 0(Left), 1(Center), 2(Right)
        ' Attribute: 0(Normal), 4(Bold) 등 (비트 마스크)
        ' TextSize: 0(Normal), 17(2배 확대) 등 (상위/하위 바이트로 가로/세로 배율 지정)
        ' -------------------

        ' 2. 헤더 출력 (가운데 정렬, 굵게)
        ' PrintText(Data, Alignment, Attribute, TextSize) 
        bxl.PrintText("=== 발권 정보 ===" & vbCrLf, 1, 4, 0)
        bxl.PrintText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & vbCrLf, 1, 0, 0)
        bxl.PrintText("------------------------------" & vbCrLf, 1, 0, 0)

        ' 3. 발권 번호 (크게 강조)
        bxl.PrintText("발권 번호" & vbCrLf, 1, 0, 0)

        ' [중요] TextSize = 17 (0x11) -> 가로 2배, 세로 2배 확대를 의미하는 경우가 많음
        ' 만약 크기가 안 변하면 1, 2, 16, 32 등을 시도해보세요.
        bxl.PrintText(ticketNum & vbCrLf, 1, 4, 17)

        bxl.PrintText(vbCrLf, 1, 0, 0)

        ' 4. 하단 문구
        bxl.PrintText("------------------------------" & vbCrLf, 1, 0, 0)
        bxl.PrintText("이용해 주셔서 감사합니다." & vbCrLf, 1, 0, 0)

        ' 용지 배출을 위한 공백 라인 (보통 3~4줄)
        bxl.PrintText(vbCrLf & vbCrLf & vbCrLf, 1, 0, 0)

        ' 5. 용지 커팅
        ' CutPaper() 인자 없음 
        bxl.CutPaper()

        ' 6. 연결 종료 [cite: 3]
        bxl.PrinterClose()
    End Sub


End Module
