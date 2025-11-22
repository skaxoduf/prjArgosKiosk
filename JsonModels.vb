Imports System.Text.Json.Serialization

' 업체설정정보 받아오는 클래스 항목
Public Class CompanyApiResponse
    <JsonPropertyName("intResult")>
    Public Property IntResult As Integer

    <JsonPropertyName("strResult")>
    Public Property StrResult As String

    <JsonPropertyName("JSON_DATA")>
    Public Property JsonData As List(Of CompanyInfo)
End Class
Public Class CompanyInfo
    <JsonPropertyName("F_IDX")>
    Public Property Idx As Integer

    <JsonPropertyName("F_COMPANY_NAME")>
    Public Property CompanyName As String

    <JsonPropertyName("F_AUTH_TYPE")>
    Public Property AuthType As Integer
End Class

' 디비 커넥션 정보 받아오는 클래스 항목
Public Class DbApiResponse
    <JsonPropertyName("intResult")>
    Public Property IntResult As Integer

    <JsonPropertyName("strResult")>
    Public Property StrResult As String

    <JsonPropertyName("JSON_DATA")>
    Public Property JsonData As String

End Class
Public Class DbConnectionInfo
    <JsonPropertyName("sServer")>
    Public Property Server As String

    <JsonPropertyName("sPort")>
    Public Property Port As String

    <JsonPropertyName("sDatabase")>
    Public Property Database As String

    <JsonPropertyName("sUserID")>
    Public Property UserID As String

    <JsonPropertyName("sPassword")>
    Public Property Password As String
End Class
' 포스 정보 받아오는 클래스 항목
Public Class GetPosInfoAsyncApiResponse
    <JsonPropertyName("intResult")>
    Public Property IntResult As Integer

    <JsonPropertyName("strResult")>
    Public Property StrResult As String

    <JsonPropertyName("POS_NAME")>
    Public Property PosName As String

    <JsonPropertyName("POS_TYPE")>
    Public Property PosType As Integer

    <JsonPropertyName("POS_IP")>
    Public Property PosIp As String

    <JsonPropertyName("POS_PORT")>
    Public Property PosPort As String

End Class
' 락카 발권 이력 저장 API 응답용 클래스

' 1. API 응답 전체를 감싸는 메인 클래스
Public Class LockerHistoryApiResponse
    <JsonPropertyName("intResult")>
    Public Property IntResult As Integer

    <JsonPropertyName("strResult")>
    Public Property StrResult As String

    <JsonPropertyName("JSON_DATA")>
    Public Property JsonData As List(Of LockerInsertData)
End Class

' 2. JSON_DATA 배열 안에 들어있는 상세 항목 클래스
Public Class LockerInsertData
    <JsonPropertyName("INSERTED_BALGWON_IDX")>
    Public Property InsertedBalgwonIdx As Integer

    <JsonPropertyName("AUTH_MESSAGE")>
    Public Property AuthMessage As String
End Class


Public Class DataReceivedEventArgs
    Inherits EventArgs

    ' [IN] 클라이언트가 보낸 원본 데이터 (모듈 -> Form1)
    Public ReadOnly Property ReceivedData As String

    ' [IN] 어느 클라이언트인지 식별용 (모듈 -> Form1)
    Public ReadOnly Property ClientEndPoint As String

    ' [OUT] Form1이 처리 후 클라이언트에게 보낼 응답 (Form1 -> 모듈)
    Public Property ResponseData As String

    ' 생성자
    Public Sub New(receivedData As String, clientEndPoint As String)
        Me.ReceivedData = receivedData
        Me.ClientEndPoint = clientEndPoint
        Me.ResponseData = String.Empty ' 기본 응답은 빈 문자열
    End Sub
End Class



