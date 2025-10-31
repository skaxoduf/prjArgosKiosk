Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports Microsoft.Identity

Public Class Form1
    ' 서버 리스너 객체
    Private listener As TcpListener
    ' 비동기 작업을 취소하기 위한 토큰 소스
    Private cts As CancellationTokenSource
    ' 클라이언트 목록 (선택적)
    Private connectedClients As New List(Of TcpClient)
    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cts = New CancellationTokenSource()
        Try
            listener = New TcpListener(IPAddress.Any, 36001)
            listener.Start()
            Await Task.Run(Function() AcceptClientsAsync(cts.Token), cts.Token)
        Catch ex As SocketException
            WriteLog($"서버 시작 오류: {ex.Message}", "KioskLog.log")
        Catch ex As Exception
            WriteLog($"오류: {ex.Message}", "KioskLog.log")
        Finally
            StopServer()
        End Try

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
                                    responseMessage = "SUCCESS" & message.Substring("ARGOS".Length)
                                Else
                                    responseMessage = $"ERROR: Unknown format ({message})"
                                End If
                                Await writer.WriteLineAsync(responseMessage)

                                WriteLog($"전송: {responseMessage}", "KioskLog.log")

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

    ' 폼이 닫힐 때 서버가 실행 중이면 중지
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        cts?.Cancel()
        listener?.Stop()
    End Sub

End Class