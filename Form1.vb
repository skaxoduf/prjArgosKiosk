Imports System.Text
Imports System.IO

Public Class Form1

    Private _server As SocketServerModule
    Private Shared ReadOnly logLocker As New Object()
    ' --- 3. 폼 로드 ---
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        _server = New SocketServerModule(36000)   ' 36000 포트로 대기

        ' 로그 이벤트 핸들러 연결
        AddHandler _server.LogOccurred, AddressOf OnServerLogOccurred
        ' 데이터 수신 이벤트 핸들러 연결
        AddHandler _server.DataReceived, AddressOf OnDataReceived
        _server.Start()
    End Sub
    Private Sub OnServerLogOccurred(message As String, fileName As String)
        WriteLog(message, fileName)
    End Sub
    Private Sub OnDataReceived(sender As Object, e As DataReceivedEventArgs)
        Dim receivedMessage = e.ReceivedData.Trim()
        Dim response As String = "" ' 클라이언트에게 보낼 최종 응답

        ' 전문 파싱
        ' "ARGOS|022405130012375D|180" 같은 문자열을 "|" 기준으로 자름
        Dim parts As String() = receivedMessage.Split("|"c)  ' (c는 |를 '문자'로 취급하라는 VB.NET 문법입니다)

        If parts.Length = 3 Then
            Dim identifier As String = parts(0)  ' ex) ARGOS
            Dim companyCode As String = parts(1) ' ex) 022405130012375D
            Dim memIDX As String = parts(2)    ' ex) "180"

            Try
                ' 웹에다가 mem_idx 전송 하는 부분을 여기에다가 작업
                ' $.fnCsToWebCallMemAuthK(회원IDX)

                response = $"SUCCESS|{companyCode}|{memIDX}"
                'WriteLog($"[로직 성공] {receivedMessage} -> {response}", "KioskLog.log")

            Catch ex As Exception
                response = $"ERROR: Logic failed ({ex.Message})"
                'WriteLog($"[로직 오류] {receivedMessage} -> {ex.Message}", "KioskLog.log")
            End Try
        Else
            ' 전문 형식이 안 맞는 경우 (예: ARGOS|1234)
            response = "ERROR: Invalid message format."
            'WriteLog($"[전문 오류] {receivedMessage}", "KioskLog.log")
        End If

        ' [5. 최종 응답 설정]
        ' Form1이 결정한 응답을 모듈(e)에게 다시 전달
        e.ResponseData = response

    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Task.Run(Sub()
                     _server?.StopServer()
                 End Sub)

        If _server IsNot Nothing Then
            RemoveHandler _server.LogOccurred, AddressOf OnServerLogOccurred
            RemoveHandler _server.DataReceived, AddressOf OnDataReceived
        End If
    End Sub

End Class