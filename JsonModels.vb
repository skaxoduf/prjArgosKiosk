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
' 유니온 출입기록 전송 결과 클래스 항목
Public Class UnionFaceLogResponse
    <JsonPropertyName("intResult")>
    Public Property IntResult As String

    <JsonPropertyName("strResult")>
    Public Property StrResult As String
End Class


