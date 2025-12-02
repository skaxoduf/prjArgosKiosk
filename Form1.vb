Imports System.ComponentModel
Imports System.IO
Imports System.IO.Ports
Imports System.Net
Imports System.Net.Http
Imports System.Net.Sockets
Imports System.Security.Policy
Imports System.Text
Imports System.Text.Json
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.Data.SqlClient
Imports Microsoft.Identity
Imports Microsoft.Web.WebView2.Core


Public Class Form1

    Private listenerHttp As New HttpListener()  ' 키오스크에서는 http리스너를 사용하지않는다. 게이트데몬에서 사용해서 키오스크로 TCP 소켓으로 보낸다.
    Private Shared ReadOnly client As HttpClient = New HttpClient()

    Private gFormGb As String
    Private sTestYN As Boolean = False   ' TEST 환경인지 플래그, 테스트 : True,  배포 : False

    Private gPosPort As String
    Private listener As TcpListener
    Private cts As CancellationTokenSource
    Private connectedClients As New List(Of TcpClient)

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' 유니락 발권 테스트 버튼 보이기
        ' Button1.Visible = True

        ' 현재 모니터 해상도 가져오기
        Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width
        Dim screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height

        ' 폼 크기 설정
        Me.Width = screenWidth
        Me.Height = screenHeight
        Me.WindowState = FormWindowState.Maximized

        WebView21.Left = 0
        WebView21.Top = 0

        Await subFormInutialize()

    End Sub
    Private Async Function subFormInutialize() As Task

        ' 폼 로드
        Await subFormLoad()

        ' 업체코드 확인
        If String.IsNullOrEmpty(gCompanyCode) Then
            WriteLog("업체코드가 설정되지 않아 프로그램을 시작 하지 않습니다.", "KioskLog.log")
            Return
        End If

        WebView21.Visible = True

        ' 포트 번호가 비어있는지 먼저 확인
        If String.IsNullOrEmpty(gPosPort) Then
            MessageBox.Show("포트 번호가 설정되지 않아 서버를 시작할 수 없습니다.", "서버 시작 오류", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ' 포트가 없으면 서버 시작 로직을 중단
        End If

        ' 키오스크는 무조건 TCP 소켓 대기만 한다. 게이트 데몬으로부터 전문을 받아서 처리한다.
        If listener Is Nothing Then
            cts = New CancellationTokenSource()
            Try
                listener = New TcpListener(IPAddress.Any, CInt(gPosPort))
                listener.Start()
                Task.Run(Function() AcceptClientsAsync(cts.Token), cts.Token)
                WriteLog($"TCP 서버 시작 성공 (Port: {gPosPort})", "KioskLog.log")
            Catch ex As SocketException
                WriteLog($"TCP 서버 시작 오류: {ex.Message}", "KioskLog.log")
                MessageBox.Show($"TCP 서버 시작 실패: {ex.Message}", "서버 시작 오류", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                WriteLog($"TCP 서버 시작 오류: {ex.Message}", "KioskLog.log")
                MessageBox.Show($"TCP 서버 시작 실패: {ex.Message}", "서버 시작 오류", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If


        ' 2. 인증타입에 따라 분기처리
        ' kiosk가 직접 대기하지않고 게이트데몬에서 받아서 처리한다. 
        'Select Case gAuthType

        '    Case 2 ' 유니온 안면장비
        '        cts = New CancellationTokenSource()
        '        Try
        '            listener = New TcpListener(IPAddress.Any, CInt(gPosPort))
        '            listener.Start()
        '            Task.Run(Function() AcceptClientsAsync(cts.Token), cts.Token)  ' Await 형식을 사용하면 폼로드에 심각한 오류가 발생할수 있다.
        '            WriteLog($"TCP 서버 시작 성공 (Port: {gPosPort})", "KioskLog.log")
        '        Catch ex As SocketException
        '            WriteLog($"TCP 서버 시작 오류: {ex.Message}", "KioskLog.log")
        '            MessageBox.Show($"TCP 서버 시작 실패: {ex.Message}", "서버 시작 오류", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        Catch ex As Exception
        '            WriteLog($"TCP 서버 시작 오류: {ex.Message}", "KioskLog.log")
        '            MessageBox.Show($"TCP 서버 시작 실패: {ex.Message}", "서버 시작 오류", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        End Try

        '    Case 3 ' HttpListener 방식 (유엘루트)
        '        Try
        '            listenerHttp.Prefixes.Add($"http://+:{gPosPort}/kiosk/")
        '            listenerHttp.Start()
        '            listenerHttp.BeginGetContext(AddressOf OnRequestReceived, listenerHttp)
        '            WriteLog($"HTTP 서버 시작 성공 (Port: {gPosPort})", "KioskLog.log")

        '        Catch ex As Exception
        '            WriteLog($"HTTP 서버 시작 오류 (Port: {gPosPort}): {ex.Message}", "KioskLog.log")
        '            MessageBox.Show($"HTTP 서버 시작 실패: {ex.Message}{vbCrLf}프로그램을 관리자 권한으로 실행했는지, 방화벽을 해제했는지 확인바랍니다.",
        '                    "서버 시작 오류", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        End Try
        'End Select




    End Function
    Private Async Function subFormLoad() As Task

        ' 현재 모니터 해상도 가져오기
        Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width
        Dim screenHeight As Integer = Screen.PrimaryScreen.Bounds.Height
        Dim posInfo As GetPosInfoAsyncApiResponse = Nothing
        Dim url As String

        If sTestYN = True Then  ' 테스트 환경
            Me.Width = 1080
            Me.Height = 1920
            Me.WindowState = FormWindowState.Normal
            Me.StartPosition = FormStartPosition.CenterScreen
        Else   ' 리얼 환경
            ' 폼 최대화
            Me.WindowState = FormWindowState.Maximized
            ' 폼 상단바 제거
            Me.FormBorderStyle = FormBorderStyle.None
        End If

        WebView21.Visible = False
        pnlCSMain.Visible = False

        ' ini 파일에서 설정값 읽어오기
        If Config_Load() = False Then
            gFormGb = "C"
        Else
            gFormGb = "W"
        End If

        If gCompanyCode <> "" And gPosNo <> "" Then
            ' DB정보 요청 
            Await FetchDbInfoAndProcessAsync()

            ' 업체 정보 요청 (지문, 안면 등..인증방식 여부를 받아오기 위해)
            Await FetchCompanyInfoAsync(gCompanyIdx, gCompanyCode)

            ' 포스번호를 가지고 포스정보를 받아온다.
            posInfo = Await GetPosInfoAsync(gCompanyIdx, gCompanyCode, gPosNo)
            If posInfo IsNot Nothing AndAlso posInfo.IntResult = 1 Then
                gPosPort = posInfo.PosPort
            End If

            ' 시리얼 포트번호가 있으면 시리얼에 연결한다.
            If IsNumeric(gSeralPortNo) = True Then
                modFunc.ConnectSerialPort("COM" + gSeralPortNo, 9600)
                AddHandler modFunc.serialPort.DataReceived, AddressOf HandleSerialData
            End If

            ' 키오스크 대기화면 웹뷰 로딩
            url = $"{gUrl}kiosk/{gCompanyCode}/"
        Else
            ' 설정이 안되어 있으면 로그인 화면으로 보낸다.
            url = $"{gUrl}login/"
        End If


        Await WebView21.EnsureCoreWebView2Async(Nothing)
        WebView21.Width = Me.ClientSize.Width
        WebView21.Height = Me.ClientSize.Height
        WebView21.Source = New Uri(url)

        ' CS 웹뷰는 폼 로딩될때 자바스크립트로부터 수신받을 준비를 한다.
        RemoveHandler WebView21.WebMessageReceived, AddressOf WebView21_WebMessageReceived
        AddHandler WebView21.WebMessageReceived, AddressOf WebView21_WebMessageReceived

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
                        Await Get_WebPosInfo() 'CS가 설치된 pc의 ini 파일을 호출하는 함수

                    Case "Bas_ConfigLoad"
                        Await Bas_ConfigLoad() '업체코드와 포스번호를 입력받는 설정창을 표시해주는 함수

                    Case "Bas_LockerBalGwon"

                        Dim sBalGwonJangbiType As String = ""   '유니락/ 이에스택 구분값  (2 : 이에스택, 1 : 유니락)
                        Dim sBalGwonJangbiIp As String = ""     '전자락 아이피 ex) 192.168.0.242
                        Dim sBalGwonJangbiPort As String = ""   '전자락 포트 ex) 33001
                        Dim sBalGwonHmClass As String = ""      '성별구분(0: 남자대인, 1 : 남자소인, 2: 여자대인, 3: 여자소인)
                        Dim sMemIdx As String = ""
                        Dim sGongDongGwaGeumIDX As String = ""

                        If data.TryGetProperty("BalGwon_Jangbi_Type", Nothing) Then
                            sBalGwonJangbiType = data.GetProperty("BalGwon_Jangbi_Type").GetInt64
                        End If
                        If data.TryGetProperty("BalGwon_Jangbi_Ip", Nothing) Then
                            sBalGwonJangbiIp = data.GetProperty("BalGwon_Jangbi_Ip").GetString()
                        End If
                        If data.TryGetProperty("BalGwon_Jangbi_Port", Nothing) Then
                            sBalGwonJangbiPort = data.GetProperty("BalGwon_Jangbi_Port").GetString()
                        End If
                        If data.TryGetProperty("BalGwon_Hm_Class", Nothing) Then
                            sBalGwonHmClass = data.GetProperty("BalGwon_Hm_Class").GetInt64
                        End If
                        If data.TryGetProperty("Mem_Idx", Nothing) Then
                            sMemIdx = data.GetProperty("Mem_Idx").GetString
                        End If
                        If data.TryGetProperty("GongDongGwaGeum_IDX", Nothing) Then
                            sGongDongGwaGeumIDX = data.GetProperty("GongDongGwaGeum_IDX").GetInt64
                        End If

                        'sBalGwonJangbiType = "1"
                        'sBalGwonJangbiIp = "192.168.0.242"
                        'sBalGwonJangbiPort = "33000"
                        'sBalGwonHmClass = "0"
                        'sMemIdx = "250"
                        'sGongDongGwaGeumIDX = "1"

                        Await Bas_LockerBalGwon(sBalGwonJangbiType, sBalGwonJangbiIp, sBalGwonJangbiPort, sBalGwonHmClass, sMemIdx, sGongDongGwaGeumIDX)

                End Select
            End If
        Catch ex As Exception
            MessageBox.Show("메시지 수신 중 오류 발생: " & ex.Message & vbCrLf & "받은 데이터: " & e.WebMessageAsJson,
                            "오류", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Async Function Bas_LockerBalGwon(ByVal sBalGwonJangbiType As String, ByVal sBalGwonJangbiIp As String,
                                            ByVal sBalGwonJangbiPort As String, ByVal sBalGwonHmClass As String,
                                            ByVal sMemIdx As String, ByVal sGongDongGwaGeumIDX As String) As Task

        Try
            Dim lockerNum As String = ""
            Select Case sBalGwonJangbiType
                Case "1" ' 유니락
                    lockerNum = Await Task.Run(Function()
                                                   Return BAS_UniLocker(sBalGwonHmClass, sBalGwonJangbiIp, sBalGwonJangbiPort)
                                               End Function)

                Case "2" ' 이에스택

                    ' 구현해야함,,,,,

                    Return
                Case Else
                    WriteLog($"정의되지 않은 락카 장비 타입코드 ::: (Type: {sBalGwonJangbiType})", "KioskLog.log")
                    Return
            End Select

            If Not String.IsNullOrEmpty(lockerNum) Then
                Await SendLockerNumToWebView(lockerNum, Get_BalGwonHmClassName(sBalGwonHmClass))
                If IsNumeric(lockerNum) Then
                    ' 발권번호가 정상일때만 저장 api호출
                    Await SendLockerBalGwonHistoryInsertJson(sMemIdx, sGongDongGwaGeumIDX, sBalGwonHmClass, 1, lockerNum, 1, "공동과금 전자락카 자동 발권")
                End If
            Else
                WriteLog($"발권 실패: {lockerNum}", "KioskLog.log")
            End If
        Catch ex As Exception
            WriteLog($"발권 프로세스 오류: " & ex.Message, "KioskLog.log")
        End Try

    End Function
    Public Async Function SendLockerNumToWebView(lockerNum As String, hmClassStr As String) As Task
        Try
            Dim jsCode As String = $"$.fnCsToWebCallLockerNumResult('{lockerNum}', '{hmClassStr}');"
            If WebView21 IsNot Nothing AndAlso WebView21.CoreWebView2 IsNot Nothing Then
                Await WebView21.CoreWebView2.ExecuteScriptAsync(jsCode)
            End If
        Catch ex As Exception
            WriteLog($"웹뷰 통신 중 오류: " & ex.Message, "KioskLog.log")
        End Try
    End Function
    Private Function BAS_UniLocker(hmclass As String, ip As String, port As String) As String
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
    Private Function ParseLockerNumber(responseBytes() As Byte) As String
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
    Private Function BAS_SUB_UniLocker(hmclass As String) As Byte()
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
    Public Async Function Get_WebPosInfo() As Task

        ' webpos가 설치된 곳의 ini 파일을 읽어오는 함수 
        gAppPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), INI_FILENAME)
        gPosNo = GetIni("Settings", "PosNo", gAppPath)
        gCompanyCode = GetIni("Settings", "CompanyCode", gAppPath)

        Dim sPosNo As String = gPosNo.Replace("'", "\'")
        Dim sCompanyCode As String = gCompanyCode.Replace("'", "\'")

        ' 웹에다가 업체코드와 포스번호를 받는 함수에 던진다.
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
        gTimer = GetIni("Settings", "Timer", gAppPath)

        txtPosNo.Text = gPosNo
        txtCompanyCode.Text = gCompanyCode
        txtTimer.Text = gTimer

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
                gAuthType = response.JsonData(0).AuthType
            End If

        Catch ex As Exception
            MessageBox.Show($"업체 장비 설정 불러오기 오류: {ex.Message}{vbCrLf}{vbCrLf}원본 데이터:{vbCrLf}{jsonStr}", "오류")
        End Try

    End Function
    Private Async Function SendLockerBalGwonHistoryInsertJson(memIdx As String,
                                                          orderDetailInfoIdx As String,
                                                          balgwonClssType As String,
                                                          balgwonType As String,
                                                          balgwonKeyNo As String,
                                                          balgwonChkType As String,
                                                          ordermsg As String) As Task

        Try
            Dim encodedOrderMsg As String = System.Web.HttpUtility.UrlEncode(ordermsg)
            'http://julist.webpos.co.kr/api/cs/LockerBalGwonHistory_Insert.asp?company_code=022405130012375D&mem_idx=180&order_detail_info_idx=151&balgwon_clss_type=0&balgwon_type=1&balgwon_key_no=52&balgwon_chk_type=1&order_msg=강좌 이용 락카 자동 발권
            Dim apiUrl As String = $"{gUrl}api/cs/LockerBalGwonHistory_Insert.asp?" &
                               $"company_code={gCompanyCode}&" &
                               $"mem_idx={memIdx}&" &
                               $"order_detail_info_idx={orderDetailInfoIdx}&" &
                               $"balgwon_clss_type={balgwonClssType}&" &
                               $"balgwon_type={balgwonType}&" &
                               $"balgwon_key_no={balgwonKeyNo}&" &
                               $"balgwon_chk_type={balgwonChkType}&" &
                               $"order_msg={encodedOrderMsg}"

            ' API 호출
            Dim response As HttpResponseMessage = Await client.GetAsync(apiUrl)
            If response.IsSuccessStatusCode Then
                Dim responseBody = Await response.Content.ReadAsStringAsync()
                Try
                    Dim options As New JsonSerializerOptions With {
                    .PropertyNameCaseInsensitive = True
                }
                    Dim apiResult As LockerHistoryApiResponse = JsonSerializer.Deserialize(Of LockerHistoryApiResponse)(responseBody, options)
                    '{"intResult":1,"strResult":"발권내역을 정상적으로 등록하였습니다.","JSON_DATA":[{"INSERTED_BALGWON_IDX":1,"AUTH_MESSAGE":"공동과금 전자락카 자동 발권"}]}
                    If apiResult IsNot Nothing AndAlso apiResult.IntResult = 1 Then
                        Dim logMsg As String = $"락카 이력 저장 성공: {apiResult.StrResult}"
                        If apiResult.JsonData IsNot Nothing AndAlso apiResult.JsonData.Count > 0 Then
                            Dim newIdx As Integer = apiResult.JsonData(0).InsertedBalgwonIdx
                            logMsg &= $" (생성된 IDX: {newIdx}, KeyNo: {balgwonKeyNo})"
                        End If
                        WriteLog(logMsg, "KioskLog.log")
                    Else
                        Dim failMsg As String = If(apiResult IsNot Nothing, apiResult.StrResult, "응답 내용 없음")
                        WriteLog($"락카 이력 저장 실패(API반환): {failMsg}", "KioskLog.log")
                    End If

                Catch jsonEx As Exception
                    WriteLog($"락카 이력 응답 파싱 에러: {jsonEx.Message} / 원본: {responseBody}", "KioskLog.log")
                End Try
            Else
                WriteLog($"락카 이력 전송 통신 실패: {response.StatusCode}", "KioskLog.log")
            End If
        Catch ex As Exception
            WriteLog($"락카 이력 전송 중 예외 발생: {ex.Message}", "KioskLog.log")
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
            gTimer = GetIni("Settings", "Timer", gAppPath)
            If IsNumeric(gTimer) = False Then
                gTimer = "30"
            End If

            If IsNumeric(gPosNo) = False Or gCompanyCode = "" Or gUrl = "" Then
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
        gTimer = txtTimer.Text.Trim

        PutIni("Settings", "PosNo", gPosNo, gAppPath)
        PutIni("Settings", "CompanyCode", gCompanyCode, gAppPath)
        PutIni("Settings", "Timer", gTimer, gAppPath)

        '다시 웹뷰 로딩
        Await Get_WebPosInfo()

        ' 다시 폼로딩
        Await subFormInutialize()

        Try
            ' 로그인화면이 잠깐 보이는걸 방지하기위해 임의의 딜레이를 준다.
            Await Task.Delay(1500) ' 1.5초
        Catch ex As Exception
        End Try

    End Sub

    ' 클라이언트 접속을 대기
    Private Async Function AcceptClientsAsync(ByVal token As CancellationToken) As Task
        Try
            While Not token.IsCancellationRequested
                ' 클라이언트 접속 대기 (비동기 방식)
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

                                If message.StartsWith("ARGOS|", StringComparison.OrdinalIgnoreCase) Then ' ARGOS|022405130012375D|180
                                    Dim parts As String() = message.Split("|")
                                    If parts.Length >= 3 Then
                                        Dim memIdx As String = parts(2).Trim()   ' 180
                                        Try
                                            Dim uiTask As Task = CType(Me.Invoke(Function()
                                                                                     Return SendMemIDXToWebView(memIdx, gCompanyCode, gPosNo, gTimer)
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
    Private Async Sub OnRequestReceived(ByVal result As IAsyncResult)
        ' HTTP 수신 
        If Not listenerHttp.IsListening Then Return

        Dim context As HttpListenerContext
        Try
            context = listenerHttp.EndGetContext(result)
        Catch ex As ObjectDisposedException
            Return
        Catch ex As System.Net.HttpListenerException
            Return
        Catch ex As Exception
            WriteLog($"EndGetContext 오류: {ex.Message}", "KioskLog.log")
            Return
        End Try

        listenerHttp.BeginGetContext(AddressOf OnRequestReceived, listenerHttp)

        ' --- ASP의 요청 및 응답 처리 ---
        Dim request As HttpListenerRequest = context.Request
        Dim response As HttpListenerResponse = context.Response

        Dim responseMessage As String = "ERROR: 알 수없는 에러"
        Dim statusCode As Integer = 500 ' Internal Server Error (기본값)
        Dim message As String = "" ' 웹에서 보낸 전문

        Try
            ' 웹에서 POST로 보낸 전문 데이터(Body)를 읽는다.
            Using reader As New StreamReader(request.InputStream, request.ContentEncoding)
                message = Await reader.ReadToEndAsync()
            End Using

            If String.IsNullOrEmpty(message) Then
                statusCode = 400 ' Bad Request
                responseMessage = "ERROR: 수신 데이타가 존재하지않음."
            Else
                WriteLog($"HTTP 전문 수신: {message}", "KioskLog.log")

                ' 수신받은 전문 파싱 
                If message.StartsWith("ARGOS|", StringComparison.OrdinalIgnoreCase) Then
                    Dim parts As String() = message.Split("|")
                    If parts.Length >= 3 Then
                        Dim memIdx As String = parts(2).Trim()

                        ' UI 스레드로 웹뷰 호출
                        Try
                            Dim uiTask As Task = CType(Me.Invoke(Function()
                                                                     Return SendMemIDXToWebView(memIdx, gCompanyCode, gPosNo, gTimer)
                                                                 End Function), Task)
                            Await uiTask

                            ' 성공 응답 생성
                            responseMessage = "SUCCESS" & message.Substring("ARGOS".Length)
                            statusCode = 200

                        Catch ex As Exception
                            WriteLog($"웹뷰 호출 오류 (memIdx: {memIdx}): {ex.Message}", "KioskLog.log")
                            responseMessage = "ERROR: Exception 에러"
                            statusCode = 500
                        End Try
                    Else
                        WriteLog($"전문 파싱 오류: {message}", "KioskLog.log")
                        responseMessage = $"ERROR: 잘못된 전문 포맷 ({message})"
                        statusCode = 400 ' Bad Request
                    End If
                Else  ' 이 부분은 혹시모르니 남겨놓는다. MEM_IDX만 날라왔을경우 
                    Dim memIdx As String = message.Trim()
                    WriteLog($"HTTP 단순 수신: {memIdx}", "KioskLog.log")
                    Try
                        Dim uiTask As Task = CType(Me.Invoke(Function()
                                                                 Return SendMemIDXToWebView(memIdx, gCompanyCode, gPosNo, gTimer)
                                                             End Function), Task)
                        Await uiTask
                        responseMessage = "SUCCESS"
                        statusCode = 200
                    Catch ex As Exception
                        WriteLog($"웹뷰 호출 오류 (memIdx: {memIdx}): {ex.Message}", "KioskLog.log")
                        responseMessage = "ERROR: UI processing failed."
                        statusCode = 500
                    End Try
                End If
            End If

        Catch ex As Exception
            WriteLog($"HTTP 처리 오류 : {ex.Message}", "KioskLog.log")
            responseMessage = $"ERROR: HTTP 프로세스 실패. {ex.Message}"
            statusCode = 500
        Finally
            Try
                Dim responseBytes = Encoding.UTF8.GetBytes(responseMessage)
                response.StatusCode = statusCode
                response.ContentLength64 = responseBytes.Length
                response.OutputStream.Write(responseBytes, 0, responseBytes.Length)
            Catch ex As Exception
                WriteLog($"HTTP 응답 전송 오류: {ex.Message}", "KioskLog.log")
            Finally
                response.OutputStream.Close()
            End Try

            If statusCode = 200 Then
                WriteLog($"클라이언트로 전송: {responseMessage}", "KioskLog.log")
            End If
        End Try

    End Sub

    ' 서버 중지 로직
    Private Sub StopServer()
        listener?.Stop()
        For Each tcpClient In connectedClients
            tcpClient?.Close()
        Next
        connectedClients.Clear()
        WriteLog($"서버 중지됨", "KioskLog.log")

    End Sub
    Public Async Function SendMemIDXToWebView(memIdx As String, company_code As String, posno As String, timer As String) As Task
        ' 지문 또는 안면장비에서 수신된 MemIDX를 WebView2로 전송하는 함수
        Try
            ' 페이지 로드가 완료된 후 JavaScript 함수 호출
            Dim jsCode As String = $"$.fnCsToWebCallMemAuthK('{company_code}', '{memIdx}', '{posno}', '{timer}');"  ' 키오스크 웹뷰에 던지는 자바스크립트 함수
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

        ' 1. TCP 서버(listener)가 실행 중이면 중지
        If listener IsNot Nothing Then
            cts?.Cancel()
            listener.Stop()
            WriteLog("TCP 서버 중지됨.", "KioskLog.log")
        End If

        ' 2. HTTP 서버(listenerHttp)가 실행 중이면 중지
        If listenerHttp IsNot Nothing AndAlso listenerHttp.IsListening Then
            listenerHttp.Stop()
            listenerHttp.Close()
            WriteLog("HTTP 서버 중지됨.", "KioskLog.log")
        End If

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

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click

        Me.Close()

    End Sub

    Private Sub Form1_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        frmSplash.Close()
    End Sub

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim sBalGwonJangbiType As String = ""   '유니락/ 이에스택 구분값  (2 : 이에스택, 1 : 유니락)
        Dim sBalGwonJangbiIp As String = ""     '전자락 아이피 ex) 192.168.0.242
        Dim sBalGwonJangbiPort As String = ""   '전자락 포트 ex) 33001
        Dim sBalGwonHmClass As String = ""      '성별구분(0: 남자대인, 1 : 남자소인, 2: 여자대인, 3: 여자소인)
        Dim sMemIdx As String = ""
        Dim sGongDongGwaGeumIDX As String = ""

        sBalGwonJangbiType = "1"
        sBalGwonJangbiIp = "192.168.0.242"
        sBalGwonJangbiPort = "31000"  ' 31000이 기본포트이다. 
        sBalGwonHmClass = "0"
        sMemIdx = "250"
        sGongDongGwaGeumIDX = "1"

        Await Bas_LockerBalGwon(sBalGwonJangbiType, sBalGwonJangbiIp, sBalGwonJangbiPort, sBalGwonHmClass, sMemIdx, sGongDongGwaGeumIDX)

    End Sub

End Class