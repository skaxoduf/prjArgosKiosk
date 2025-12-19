Imports System.Diagnostics ' Debug.WriteLine을 위해 추가
Imports System.IO.Ports
Imports System.Net.Sockets
Imports System.Text
Imports prjArgosKiosk.clsBXLAPI

Module modFunc

    Public WithEvents serialPort As New SerialPort()

    ' 빅솔론 프린터 인터페이스 열거형 선언
    Public Enum BXL_INTERFACE
        SERIAL = 0      ' Serial (RS-232)
        PARALLEL = 1    ' Parallel (LPT)
        USB = 2         ' USB
        ETHERNET = 3    ' Ethernet (LAN)
        WLAN = 4        ' Wi-Fi
        BLUETOOTH = 5   ' Bluetooth
    End Enum


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

    End Sub

    ''' <summary>
    ''' 영수증 프린터로 발권 번호를 출력하는 공통함수
    ''' </summary>
    ''' <param name="ticketNum">발권 번호 (예: 101)</param>
    ''' <param name="nInterface">연결 방식 (0:Serial, 2:USB, 3:LAN)</param>
    ''' <param name="szPortName">포트명 (USB:, COM1, 192.168.0.10)</param>
    ''' <param name="nBaudRate">통신속도 (기본값 9600)</param>
    Public Sub PrintTicket(ByVal ticketNum As String, ByVal hmClass As String, ByVal nInterface As Integer, ByVal szPortName As String, Optional ByVal nBaudRate As Integer = 9600)

        '    ' 1. USB 연결 (값: 2)
        'PrintTicket("101", 2, "USB:") 
        '' 또는
        'PrintTicket("101", BXL_INTERFACE.USB, "USB:")

        '' 2. 시리얼 연결 (값: 0) - COM3 포트, 9600bps
        'PrintTicket("102", 0, "COM3", 9600)

        '' 3. LAN 연결 (값: 3) - 프린터 IP: 192.168.0.200
        'PrintTicket("103", 3, "192.168.0.200", 9100)


        ' 클래스 인스턴스 생성
        Dim bxl As New clsBXLAPI()
        Dim nResult As Integer = 0

        ' 이더넷 포트 처리를 위한 변수
        Dim nEthernetPort As Integer = 9100

        ' ---------------------------------------------------------
        ' 1. 프린터 연결 (타입 변환 명시하여 문법 오류 방지)
        ' ---------------------------------------------------------
        Select Case nInterface
            Case CInt(BXL_INTERFACE.SERIAL) ' [값: 0]
                ' 시리얼: 포트명(COMx), 속도(9600 등), 데이터비트(8), 패리티(0), 스톱비트(0), 흐름제어(0)
                nResult = bxl.PrinterOpen(nInterface, szPortName, nBaudRate, 8, 0, 0, 0)

            Case CInt(BXL_INTERFACE.USB)    ' [값: 2]
                ' USB: 포트명 "USB:", 나머지는 0
                nResult = bxl.PrinterOpen(nInterface, "USB:", 0, 0, 0, 0, 0)

            Case CInt(BXL_INTERFACE.ETHERNET) ' [값: 3]
                ' 이더넷: 포트번호(9100) 설정
                If nBaudRate > 0 Then nEthernetPort = nBaudRate
                ' IP주소(szPortName), 포트번호(nEthernetPort)
                nResult = bxl.PrinterOpen(nInterface, szPortName, nEthernetPort, 0, 0, 0, 0)

            Case Else
                MsgBox("지원하지 않는 인터페이스 번호입니다: " & nInterface)
                WriteLog("지원하지 않는 인터페이스 번호입니다: " & nInterface, "PrintLog.log")
                Exit Sub
        End Select

        ' ---------------------------------------------------------
        ' 2. 연결 결과 확인 (성공 시 0 또는 1 반환)
        ' ---------------------------------------------------------
        If nResult <> 1 AndAlso nResult <> 0 Then
            MsgBox("프린터 연결 실패! 에러코드: " & nResult)
            WriteLog("프린터 연결 실패! 에러코드: " & nResult, "PrintLog.log")
            Exit Sub
        End If

        ' ---------------------------------------------------------
        ' 3. 출력 로직
        ' ---------------------------------------------------------
        Try

            Dim nTicket As Integer
            Dim displayNum As String = ticketNum
            If Integer.TryParse(ticketNum, nTicket) Then
                If nTicket >= 5000 Then  ' 번호가 5000 이상일 때만 5000을 뺌
                    nTicket = nTicket - 5000
                    displayNum = nTicket.ToString()
                End If
            End If

            ' [헤더]
            bxl.PrintText("=== 전자락 발권 ===" & vbCrLf, 1, 4, 0)
            bxl.PrintText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & vbCrLf, 1, 0, 0)
            bxl.PrintText("------------------------------" & vbCrLf, 1, 0, 0)

            ' 1) "발권번호" 타이틀 출력 (중앙 정렬, 기본 크기)
            bxl.PrintText("발권번호" & vbCrLf, 1, 0, 0)

            ' 2) 구분 명칭 가져오기
            Dim sClassName As String = Get_BalGwonHmClassName(hmClass)

            ' 3) 내용 구성 (예: 남자대인 - 255)
            Dim detailMsg As String = $"{sClassName} - {displayNum}"

            ' 4) 내용 출력 (중앙 정렬(1), 굵게(4), 2배 확대(17))
            bxl.PrintText(detailMsg & vbCrLf, 1, 4, 17)

            ' [바닥글]
            bxl.PrintText(vbCrLf, 1, 0, 0)
            bxl.PrintText("------------------------------" & vbCrLf, 1, 0, 0)
            bxl.PrintText("이용해 주셔서 감사합니다." & vbCrLf, 1, 0, 0)
            bxl.PrintText(vbCrLf & vbCrLf & vbCrLf, 1, 0, 0)

            ' [커팅]
            bxl.CutPaper()

        Catch ex As Exception
            MsgBox("인쇄 중 오류 발생: " & ex.Message)
            WriteLog("인쇄 중 오류 발생: " & ex.Message, "PrintLog.log")
        Finally
            bxl.PrinterClose()  ' 프린터 연결 종료
        End Try

    End Sub

    ' 유니락 발권
    Public Function BAS_UniLocker(hmclass As String, ip As String, port As String) As String

        Try
            Dim packet() As Byte = BAS_SUB_UniLocker(hmclass)

            Using client As New TcpClient(ip, Integer.Parse(port))
                Using stream As NetworkStream = client.GetStream()
                    stream.Write(packet, 0, packet.Length)

                    Dim buffer(255) As Byte
                    Dim bytesRead As Integer = stream.Read(buffer, 0, buffer.Length)

                    If bytesRead > 0 Then
                        Return ParseLockerNumber(buffer)
                    Else
                        Return "발권 실패"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try

    End Function
    Public Function ParseLockerNumber(responseBytes() As Byte) As String
        Try
            ' 24~28번째 바이트 (5자리 번호)
            Dim lockerNum As String = Encoding.ASCII.GetString(responseBytes, 24, 5)
            lockerNum = lockerNum.TrimStart("0"c)
            If String.IsNullOrEmpty(lockerNum) Then
                Return "Error: Empty"
            End If
            Return lockerNum
        Catch ex As Exception
            Return "Error: Parse Fail"
        End Try
    End Function
    Public Function BAS_SUB_UniLocker(hmclass As String) As Byte()
        Dim packet As New List(Of Byte)
        Dim userId As String = "USER1"    ' 발권요청한 아이디인데 그냥 붙박이 
        Dim userIdBytes As Byte() = Encoding.ASCII.GetBytes(userId)

        ' Header
        packet.Add(&H70) ' Version
        packet.Add(&H41) ' Command
        packet.Add(&HFF) ' Address
        packet.Add(&HFF)
        packet.Add(&HFF)
        packet.Add(&HFF) ' Encryption(스펙상 0xFF/0x00 둘다 허용. 0xFF 많이 사용)
        packet.AddRange(Encoding.ASCII.GetBytes("00037"))    ' Length (5자리)  '' 자동발권은 37이 그냥 고정 붙박이다.
        packet.AddRange(Encoding.ASCII.GetBytes("POS1"))     ' 발권하는 포스명칭인데 그냥 붙박이 

        ' User ID (예: "USER1" + 패딩)
        packet.AddRange(userIdBytes)
        For i = userIdBytes.Length To 19
            packet.Add(&H0)
        Next

        ' CheckInCondition (1번그룹)
        packet.Add(&H30)

        ' CheckInDataCount (자동발권 요청 데이터 수량, 4자리)
        packet.AddRange(Encoding.ASCII.GetBytes("0001"))    ' 이것도 자동발권일때는 발권요청수량으로 쓰인다. 밑에꺼랑 같음..그냥 1로 하드코딩..

        ' 발권 요청수량 
        packet.AddRange(Encoding.ASCII.GetBytes("00001"))  ' 자동발권이니까 무조건 1개로 하드코딩

        ' HumanClass
        Select Case hmclass
            Case "0"  ' 남자대인
                packet.Add(&H30)
            Case "1"  ' 남자소인
                packet.Add(&H31)
            Case "2"  ' 여자대인
                packet.Add(&H32)
            Case "3"  ' 여자소인
                packet.Add(&H33)
            Case Else
                packet.Add(&H30)
        End Select

        ' CheckInGroup (0: 0번그룹)
        packet.Add(&H30)

        ' CheckInType (1: 사우나)
        packet.Add(&H31)

        ' 총 바이트 48바이트가 되어야 정상
        Return packet.ToArray()
    End Function


End Module
