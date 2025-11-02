Imports System.ComponentModel
Imports System.IO
Imports System.IO.Ports
Imports System.Net
Imports System.Net.Http
Imports System.Net.Sockets
Imports System.Text
Imports System.Text.Json
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.Data.SqlClient
Imports Microsoft.Identity
Imports Microsoft.Web.WebView2.Core


Public Class Form1

    Private Shared ReadOnly client As HttpClient = New HttpClient()

    Private gFormGb As String
    Private sTestYN As Boolean = True   ' TEST 환경인지 플래그, 테스트 : True,  배포 : False

    Private gPort As String
    Private listener As TcpListener
    Private cts As CancellationTokenSource
    Private connectedClients As New List(Of TcpClient)



    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        If sTestYN = True Then  ' 테스트 환경
            Me.Width = 1080
            Me.Height = 1920
            Me.WindowState = FormWindowState.Normal
            Me.StartPosition = FormStartPosition.CenterScreen
        Else   ' 배포 환경
            ' 폼 최대화
            Me.WindowState = FormWindowState.Maximized
            ' 폼 상단바 제거
            Me.FormBorderStyle = FormBorderStyle.None
        End If


        ' 초기화면은 웹뷰만 보이도록
        WebView21.Visible = True
        pnlCSMain.Visible = False


        ' 설정파일 읽기
        Await subFormLoad()

        ' 소켓 대기
        If gPort <> "" Then
            cts = New CancellationTokenSource()
            Try
                listener = New TcpListener(IPAddress.Any, gPort)
                listener.Start()
                Await Task.Run(Function() AcceptClientsAsync(cts.Token), cts.Token)
            Catch ex As SocketException
                WriteLog($"서버 시작 오류: {ex.Message}", "KioskLog.log")
            Catch ex As Exception
                WriteLog($"오류: {ex.Message}", "KioskLog.log")
            Finally
                StopServer()
            End Try
        Else
            'WriteLog("포트 번호가 설정되지 않아 소켓 서버를 시작할 수 없습니다.", "KioskLog.log")
            MessageBox.Show("포트 번호가 설정되지 않아 소켓 서버를 시작할 수 없습니다.", "서버 시작 오류", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub
    Private Async Function subFormLoad() As Task

        ' 현재 모니터 해상도 가져오기
        Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width
        Dim screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height
        Dim posInfo As GetPosInfoAsyncApiResponse = Nothing

        If Config_Load() = False Then
            gFormGb = "C"
        Else
            gFormGb = "W"
        End If

        If gCompanyCode <> "" Then
            ' DB정보 요청 
            Await FetchDbInfoAndProcessAsync()
            ' 업체 정보 요청 (지문, 안면 등..인증방식 여부를 받아오기 위해)
            Await FetchCompanyInfoAsync(gCompanyIdx, gCompanyCode)

            ' 포스번호를 가지고 포스의 기타 정보를 받아온다.
            posInfo = Await GetPosInfoAsync(gCompanyIdx, gCompanyCode, gPosNo)
            If posInfo IsNot Nothing AndAlso posInfo.IntResult = 1 Then
                gPort = posInfo.PosPort
            End If
        End If


        ' 시리얼 포트번호가 있으면 시리얼에 연결한다.
        If IsNumeric(gSeralPortNo) = True Then
            modFunc.ConnectSerialPort("COM" + gSeralPortNo, 9600)
            AddHandler modFunc.serialPort.DataReceived, AddressOf HandleSerialData
        End If


        ' 아래주소는 키오스크 대기화면 주소로 변경해야함..
        Dim url As String = $"{gUrl}login/"  ' Dim url As String = "http://julist.webpos.co.kr/login/"
        Await WebView21.EnsureCoreWebView2Async(Nothing)

        WebView21.Width = Me.ClientSize.Width
        WebView21.Height = Me.ClientSize.Height
        WebView21.Source = New Uri(url)

        ' CS 웹뷰는 폼 로딩될때 자바스크립트로부터 수신받을 준비를 한다.
        RemoveHandler WebView21.WebMessageReceived, AddressOf WebView21_WebMessageReceived
        AddHandler WebView21.WebMessageReceived, AddressOf WebView21_WebMessageReceived

        ' 폼 로딩될때 CS 웹뷰는 자바스크립트에다가 아무 액션도 하지 않는다.
        ' 테스트시 주석해제 , 배포시 주석처리
        'RemoveHandler WebView21.NavigationCompleted, AddressOf WebView_NavigationCompleted
        'AddHandler WebView21.NavigationCompleted, AddressOf WebView_NavigationCompleted

    End Function
    Private Async Sub WebView21_WebMessageReceived(sender As Object, e As CoreWebView2WebMessageReceivedEventArgs)
        Try
            Dim receivedJson As String = e.WebMessageAsJson

            If receivedJson.StartsWith("""") AndAlso receivedJson.EndsWith("""") Then
                receivedJson = JsonDocument.Parse(receivedJson.Trim(""""c)).RootElement.GetRawText()
            End If

            Dim doc As JsonDocument = JsonDocument.Parse(receivedJson)
            Dim data As JsonElement = doc.RootElement

            If data.TryGetProperty("call", Nothing) Then
                Dim methodName = data.GetProperty("call").GetString()

                Select Case methodName
                    Case "Get_WebPosInfo"
                        Await Get_WebPosInfo() '웹포스가 설치된 pc의 ini 파일을 호출하는 함수

                    Case "Bas_ConfigLoad"
                        Await Bas_ConfigLoad() '단지코드와 포스번호를 입력받는 설정창을 표시해주는 함수

                    Case Else
                End Select
            End If
        Catch ex As Exception
            MessageBox.Show("메시지 수신 중 오류 발생: " & ex.Message & vbCrLf & "받은 데이터: " & e.WebMessageAsJson,
                            "오류", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Async Function Get_WebPosInfo() As Task

        WebView21.Visible = True
        pnlCSMain.Visible = False

        ' webpos가 설치된 곳의 ini 파일을 읽어오는 함수 
        gAppPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), INI_FILENAME)
        gPosNo = GetIni("Settings", "PosNo", gAppPath)
        gCompanyCode = GetIni("Settings", "CompanyCode", gAppPath)

        Dim sPosNo As String = gPosNo.Replace("'", "\'")
        Dim sCompanyCode As String = gCompanyCode.Replace("'", "\'")

        ' 웹에다가 단지코드와 포스번호를 받는 함수(fnCsWebPosInfoGetter)에 던진다.
        Dim jsCode As String = $"$.fnCsWebPosInfoGetter('{sPosNo}', '{sCompanyCode}');"
        Await WebView21.ExecuteScriptAsync(jsCode)

    End Function
    Public Async Function Bas_ConfigLoad() As Task

        pnlCSMain.Visible = True   ' 환경설정창을 표시한다.
        pnlCSMain.Left = (Me.ClientSize.Width - pnlCSMain.Width) \ 2
        pnlCSMain.Top = (Me.ClientSize.Height - pnlCSMain.Height) \ 2

        gAppPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), INI_FILENAME)
        gPosNo = GetIni("Settings", "PosNo", gAppPath)
        gCompanyCode = GetIni("Settings", "CompanyCode", gAppPath)
        txtPosNo.Text = gPosNo
        txtCompanyCode.Text = gCompanyCode

    End Function
    Private Async Function GetPosInfoAsync(ByVal companyIdx As String, ByVal companyCode As String, ByVal pos_no As String) As Task(Of GetPosInfoAsyncApiResponse)
        Try
            Dim url As String = $"{gUrl}api/cs/pos_info_select.asp?company_IDX={companyIdx}&company_code={companyCode}&pos_no={pos_no}"
            Dim jsonStr As String = Await client.GetStringAsync(url)

            ' {"intResult":1,"strResult":"정상","POS_NAME":"사무실포스","POS_TYPE":2,"POS_IP":"192.168.0.242","POS_PORT":"36001"}

            Dim response As GetPosInfoAsyncApiResponse = JsonSerializer.Deserialize(Of GetPosInfoAsyncApiResponse)(jsonStr)
            Return response
        Catch ex As Exception
            WriteLog($"포스정보 확인 중 오류 발생: {ex.Message}", "KioskLog.log")
            Return Nothing
        End Try
    End Function
    Private Async Function FetchDbInfoAndProcessAsync() As Task
        Dim jsonStr As String = ""
        Try
            Dim url As String = $"{gUrl}api/cs/GateConnInfo.asp"
            jsonStr = Await client.GetStringAsync(url)

            Get_DBInfoRecv(jsonStr)

        Catch ex As Exception
            WriteLog($"API 호출 또는 데이터 처리 중 오류 발생: {ex.Message}", "KioskLog.log")
        End Try
    End Function
    Private Sub Get_DBInfoRecv(ByVal jsonStr As String)
        Try
            Dim response As DbApiResponse = JsonSerializer.Deserialize(Of DbApiResponse)(jsonStr)

            If response IsNot Nothing AndAlso response.IntResult = 1 Then
                If Not String.IsNullOrWhiteSpace(response.JsonData) AndAlso response.JsonData <> "null" Then

                    Dim dbInfo As DbConnectionInfo = JsonSerializer.Deserialize(Of DbConnectionInfo)(response.JsonData)
                    gServer = dbInfo.Server
                    gDatabase = dbInfo.Database
                    gUser = dbInfo.UserID
                    gPass = dbInfo.Password

                    modDBConn.ConnectionString = $"Data Source={gServer};Initial Catalog={gDatabase};User ID={gUser};Password={gPass};TrustServerCertificate=True"
                End If
            ElseIf response IsNot Nothing Then
                MessageBox.Show($"DB 정보 수신 실패: {response.StrResult}")
            End If

        Catch ex As Exception
            MessageBox.Show("DB 정보 처리 중 오류 발생: " & ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Async Function FetchCompanyInfoAsync(ByVal companyIdx As String, ByVal companyCode As String) As Task
        Try
            Dim url As String = $"{gUrl}api/cs/company_info_select.asp?company_IDX={companyIdx}&company_code={companyCode}"
            Dim jsonStr As String = Await client.GetStringAsync(url)

            Await Get_CompanyInfo(jsonStr)

        Catch ex As Exception
            MessageBox.Show($"업체 정보 API 호출 중 오류 발생: {ex.Message}", "네트워크 오류")
            'WriteLog($"업체 정보 API 호출 중 오류 발생: {ex.Message}", LOG_TO_FILE, LOG_FILE_NAME)
        End Try
    End Function
    Public Async Function Get_CompanyInfo(ByVal jsonStr As String) As Task
        Try
            '{"intResult":1,"strResult":"검색완료","JSON_DATA":[{"F_IDX":12,"F_COMPANY_NAME":"테스트_본점","F_AUTH_TYPE":1}]}

            Dim cleanJsonStr As String = jsonStr
            If cleanJsonStr.StartsWith("""") AndAlso cleanJsonStr.EndsWith("""") Then
                cleanJsonStr = cleanJsonStr.Substring(1, cleanJsonStr.Length - 2)
            End If
            cleanJsonStr = Regex.Unescape(cleanJsonStr)

            Dim response As CompanyApiResponse = JsonSerializer.Deserialize(Of CompanyApiResponse)(cleanJsonStr)
            If response IsNot Nothing AndAlso response.IntResult = 1 AndAlso response.JsonData.Count > 0 Then
                Dim authType As Integer = response.JsonData(0).AuthType
            End If

        Catch ex As Exception
            MessageBox.Show($"업체 장비 설정 불러오기 오류: {ex.Message}{vbCrLf}{vbCrLf}원본 데이터:{vbCrLf}{jsonStr}", "오류")
        End Try

    End Function
    Private Function Config_Load() As Boolean

        Config_Load = True
        Try
            gAppPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), INI_FILENAME)
            gPosNo = GetIni("Settings", "PosNo", gAppPath)
            gCompanyCode = GetIni("Settings", "CompanyCode", gAppPath)
            gSeralPortNo = GetIni("Settings", "SerialPortNo", gAppPath)
            gUrl = GetIni("Settings", "Url", gAppPath)

            If IsNumeric(gPosNo) = False Or gCompanyCode = "" Then
                'MessageBox.Show("설정이 잘못되었습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Config_Load = False
            End If
        Catch ex As Exception
            Config_Load = False
        End Try

    End Function
    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If txtPosNo.Text.Trim = "" Or txtCompanyCode.Text.Trim = "" Then
            MessageBox.Show("포스번호와 업체코드를 입력해주세요.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        '입력받은값 ini 저장후 
        gPosNo = txtPosNo.Text.Trim
        gCompanyCode = txtCompanyCode.Text.Trim

        PutIni("Settings", "PosNo", gPosNo, gAppPath)
        PutIni("Settings", "CompanyCode", gCompanyCode, gAppPath)

        '다시 웹뷰 로딩
        Await Get_WebPosInfo()

    End Sub

    ' 클라이언트 접속을 대기 루프
    Private Async Function AcceptClientsAsync(ByVal token As CancellationToken) As Task
        Try
            While Not token.IsCancellationRequested
                ' 클라이언트 접속 대기 (비동기)
                Dim client As TcpClient = Await listener.AcceptTcpClientAsync(token)

                ' 클라이언트 접속 성공
                Dim clientIp As String = CType(client.Client.RemoteEndPoint, IPEndPoint).ToString()
                WriteLog($"클라이언트 연결됨: {clientIp}", "KioskLog.log")

                connectedClients.Add(client)

                Dim clientTask As Task = HandleClientAsync(client, token)
            End While
        Catch ex As OperationCanceledException
            WriteLog($"서버 대기 루프 중지됨.", "KioskLog.log")
        Catch ex As Exception
            WriteLog($"대기 루프 오류: {ex.Message}", "KioskLog.log")
        End Try
    End Function

    ' 개별 클라이언트와의 통신 처리
    Private Async Function HandleClientAsync(ByVal client As TcpClient, ByVal token As CancellationToken) As Task
        Using client
            Try
                Using stream As NetworkStream = client.GetStream()
                    ' 한글 처리를 위해 UTF-8 사용
                    Using reader As New StreamReader(stream, Encoding.UTF8)
                        Using writer As New StreamWriter(stream, Encoding.UTF8)
                            writer.AutoFlush = True ' 즉시 전송
                            While client.Connected AndAlso Not token.IsCancellationRequested
                                Dim message As String = Await reader.ReadLineAsync().WaitAsync(token)

                                If message Is Nothing Then  ' 전문수신된게 없으면 종료  
                                    Exit While
                                End If

                                WriteLog($"전문 수신: {message}", "KioskLog.log")
                                If message.Equals("exit", StringComparison.OrdinalIgnoreCase) Then
                                    Exit While
                                End If

                                Dim responseMessage As String

                                If message.StartsWith("ARGOS|", StringComparison.OrdinalIgnoreCase) Then
                                    Dim parts As String() = message.Split("|")
                                    If parts.Length >= 3 Then
                                        Dim memIdx As String = parts(2).Trim()   ' ARGOS|022405130012375D|180
                                        Try
                                            Dim uiTask As Task = CType(Me.Invoke(Function()
                                                                                     Return SendMemIDXToWebView(memIdx, gCompanyCode)
                                                                                 End Function), Task)
                                            Await uiTask
                                        Catch ex As Exception
                                            WriteLog($"웹뷰 호출 오류 (memIdx: {memIdx}): {ex.Message}", "KioskLog.log")
                                        Finally
                                            responseMessage = "SUCCESS" & message.Substring("ARGOS".Length)
                                        End Try
                                    Else
                                        WriteLog($"전문 파싱 오류: {message}", "KioskLog.log")
                                        responseMessage = $"ERROR: Invalid ARGOS format. Parts < 3. ({message})"
                                    End If
                                Else
                                    responseMessage = $"[오류] 전문 포맷 잘못됨 : ({message})"
                                End If

                                Await writer.WriteLineAsync(responseMessage)
                                WriteLog($"클라이언트로 전송: {responseMessage}", "KioskLog.log")

                            End While
                        End Using
                    End Using
                End Using
            Catch ex As OperationCanceledException
                ' 서버 중지 시 클라이언트 핸들러도 종료
            Catch ex As IOException
                WriteLog($"클라이언트 연결 끊어짐 (IO).", "KioskLog.log")
            Catch ex As Exception
                WriteLog($"클라이언트 처리 오류 : {ex.Message}", "KioskLog.log")
            End Try
        End Using

        connectedClients.Remove(client)
        WriteLog($"클라이언트 연결 종료", "KioskLog.log")

    End Function

    ' 서버 중지 로직
    Private Sub StopServer()
        listener?.Stop()
        For Each tcpClient In connectedClients
            tcpClient?.Close()
        Next
        connectedClients.Clear()
        WriteLog($"서버 중지됨", "KioskLog.log")

    End Sub
    Public Async Function SendMemIDXToWebView(memIdx As String, company_code As String) As Task
        ' 지문 또는 안면장비에서 수신된 MemIDX를 WebView2로 전송하는 함수
        Try
            ' 페이지 로드가 완료된 후 JavaScript 함수 호출
            Dim jsCode As String = $"$.fnCsToWebCallMemAuthK('{memIdx}');"  ' 키오스크 웹뷰에 던지는 자바스크립트 함수
            If WebView21 IsNot Nothing AndAlso WebView21.CoreWebView2 IsNot Nothing Then
                Await WebView21.CoreWebView2.ExecuteScriptAsync(jsCode)
            Else
                WriteLog("WebView2가 초기화되지 않아 memIDX 전송 실패.", "KioskLog.log")
            End If
        Catch ex As Exception
            WriteLog($"WebView2로 memIDX 전송 중 오류 발생: {ex.Message}", "KioskLog.log")
        End Try

    End Function

    ' 폼이 닫힐 때 서버가 실행 중이면 중지
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        cts?.Cancel()
        listener?.Stop()
    End Sub

    ' 시리얼 포트에서 데이터가 수신될 때 호출될 이벤트 핸들러
    Private Sub HandleSerialData(sender As Object, e As SerialDataReceivedEventArgs)

        Me.Invoke(Sub()
                      Try
                          ' 데이터가 완전히 도착할 시간을 확보하기 위해 대기시간 
                          Thread.Sleep(50) '50ms

                          Dim receivedData As String = modFunc.serialPort.ReadExisting()

                          ' 받은 바코드 값 처리
                          Dim barcodeValue As String = receivedData.Trim()
                          If Not String.IsNullOrEmpty(barcodeValue) Then
                              'TextBox1.Text = $"읽은 바코드: {barcodeValue}"
                          End If
                      Catch ex As Exception
                          MessageBox.Show("바코드 데이터 수신 오류: " & ex.Message)
                      End Try
                  End Sub)
    End Sub

End Class