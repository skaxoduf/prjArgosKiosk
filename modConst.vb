﻿Module modConst



    Public Const INI_FILENAME = "ArgosAPTKiosk.ini"   ''ini 파일명
    Public Const SELECT_TIMEOUT = 60   ''조회시 기본 타임아웃 값 

    Public gAppPath As String
    Public gAppPath2 As String
    Public gPosNo As String
    Public gCompanyIdx As String
    Public gCompanyCode As String
    Public gSeralPortNo As String

    ' 디비접속정보 전역변수 (Form1이 로딩될때 웹에서 Json으로 정보를 받아와 이 변수에 담는다.)
    Public gServer As String
    Public gDatabase As String
    Public gUser As String
    Public gPass As String
    Public gUrl As String


End Module
