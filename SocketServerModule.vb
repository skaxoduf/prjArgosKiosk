Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks

Public Class SocketServerModule

    ' --- 멤버 변수 ---
    Private _listener As TcpListener
    Private ReadOnly _port As Integer
    Private _cts As CancellationTokenSource ' 리스닝 루프 중지용 토큰

    ' --- 이벤트 선언 ---
    ' 이 모듈(클래스)이 Form1(외부)에게 로그를 남겨달라고 요청하는 신호
    ' Form1의 WriteLog 함수 시그니처와 동일하게 (message, fileName)을 전달
    Public Event LogOccurred(message As String, fileName As String)

    Public Event DataReceived(sender As Object, e As DataReceivedEventArgs)

    ' --- 생성자 ---
    Public Sub New(port As Integer)
        _port = port
    End Sub
    ' --- 1. 서버 시작 (Form1에서 호출) ---
    Public Sub Start()
        _cts = New CancellationTokenSource()
        ' 백그라운드 스레드에서 리스닝 시작
        Task.Run(Async Function() As Task
                     Await StartListeningAsync(_cts.Token)
                 End Function)
    End Sub

    ' --- 2. 서버 중지 (Form1에서 호출) ---
    Public Sub StopServer()
        Try
            ' 1. 리스닝 루프(While)에 중지 신호 전송
            _cts?.Cancel()

            ' 2. Await AcceptTcpClientAsync()에서 대기 중인 상태를 강제 해제
            _listener?.Stop()

            _cts?.Dispose()
        Catch ex As Exception
            ' 중지 시 예외는 무시
        End Try
        ' "RaiseEvent"를 사용해 Form1에 로그 기록 요청
        RaiseEvent LogOccurred("[서버] 서버가 중지되었습니다.", "KioskLog.log")
    End Sub
    ' --- 3. 리스닝 핵심 로직 (백그라운드) ---
    Private Async Function StartListeningAsync(token As CancellationToken) As Task
        Try
            _listener = New TcpListener(IPAddress.Any, _port)
            _listener.Start()

            RaiseEvent LogOccurred($"[서버] 리스닝을 시작합니다...", "KioskLog.log")

            While Not token.IsCancellationRequested
                Dim client As TcpClient = Await _listener.AcceptTcpClientAsync(token)
                RaiseEvent LogOccurred($"[서버] 클라이언트 연결됨: {client.Client.RemoteEndPoint}", "KioskLog.log")
                Dim clientTask As Task = Task.Run(Async Sub()
                                                      Await HandleClientAsync(client, token)
                                                  End Sub)
            End While


        Catch ex As OperationCanceledException
            ' StopServer() 호출 시 정상적으로 발생 (무시)
        Catch ex As SocketException
            If Not (ex.SocketErrorCode = SocketError.Interrupted) Then
                RaiseEvent LogOccurred($"[소켓 오류] {ex.Message}", "KioskLog.log")
            End If
        Catch ex As Exception
            RaiseEvent LogOccurred($"[서버 오류] {ex.Message}", "KioskLog.log")
        End Try
    End Function
    ' --- 4. 클라이언트 처리 핵심 로직 (백그라운드) ---
    Private Async Function HandleClientAsync(ByVal client As TcpClient, token As CancellationToken) As Task

        Try
            Using client
                Using stream As NetworkStream = client.GetStream()

                    Dim clientEndPointStr As String = client.Client.RemoteEndPoint.ToString()
                    Dim buffer(1024) As Byte
                    Dim bytesRead As Integer

                    While (bytesRead = Await stream.ReadAsync(buffer, 0, buffer.Length, token)) > 0
                        ' 1. 데이터 수신 및 로그
                        Dim receivedData As String = Encoding.UTF8.GetString(buffer, 0, bytesRead)
                        RaiseEvent LogOccurred($"[수신:{clientEndPointStr}] {receivedData}", "KioskLog.log")

                        ' 2. Form1에 처리를 요청할 이벤트 객체 생성
                        Dim args As New DataReceivedEventArgs(receivedData, clientEndPointStr)

                        ' 3. Form1에 이벤트 발생
                        RaiseEvent DataReceived(Me, args)

                        ' 4. Form1이 담아준 응답을 가져옴
                        Dim responseBytes As Byte() = Encoding.UTF8.GetBytes(args.ResponseData)

                        ' 5. Form1이 결정한 응답을 클라이언트로 전송
                        If responseBytes.Length > 0 Then
                            Await stream.WriteAsync(responseBytes, 0, responseBytes.Length, token)
                            RaiseEvent LogOccurred($"[발신:{clientEndPointStr}] {args.ResponseData}", "KioskLog.log")
                        End If

                    End While
                End Using
            End Using
            RaiseEvent LogOccurred($"[서버] 클라이언트 연결 종료.", "KioskLog.log")

        Catch ex As OperationCanceledException
        Catch ex As System.IO.IOException
            RaiseEvent LogOccurred($"[연결 종료] (강제 종료)", "KioskLog.log")
        Catch ex As Exception
            If Not token.IsCancellationRequested Then
                RaiseEvent LogOccurred($"[클라이언트 오류] {ex.Message}", "KioskLog.log")
            End If
        End Try

    End Function

End Class