Module mdlBXLAPI

#Region " Constant variables "

    Public Delegate Function StatusCallBackDelegate(ByVal nStatus As Integer) As Integer

    Private callbackFunc As StatusCallBackDelegate = Nothing

    Public Const ISerial As Integer = 0
    Public Const IParallel As Integer = 1
    Public Const IUsb As Integer = 2
    Public Const ILan As Integer = 3
    Public Const IWLan As Integer = 4
    Public Const IBluetooth As Integer = 5

    Enum IMAGE_WIDTH
        BXL_WIDTH_FULL = -1
        BXL_WIDTH_NONE = -2
    End Enum

    Enum BXL_RESULT
        BXL_SUCCESS = 0
        BXL_READBUFFER_EMPTY = -1
        BXL_CONNECT_FAIL = -100
        BXL_NOT_OPENED = -101
        BXL_NOT_SUPPORT = -107
        BXL_INVALID_PARAM = -108
        BXL_BUFFER_ERROR = -109
        BXL_NO_PRINT_DATA = -110
        BXL_COMPLETE_ERROR = -111
        BXL_NO_TRANSACTION_START = -112
        BXL_TEXT_ENCODING_ERROR = -120
        BXL_PAGE_MODE_ALREADY_IN = -130
        BXL_NOT_IN_PAGE_MODE = -131
        BXL_REGISTRY_ERROR = -200
        BXL_WRITE_ERROR = -300
        BXL_READ_ERROR = -301
        BXL_BMP_LOAD_ERROR = -400
        BXL_BMP_BUFFER_SIZE_ERROR = -401
        BXL_BMP_CREATE_ERROR = -402
        BXL_BC_DATA_ERROR = -500
        BXL_BC_NOT_SUPPORT = -501
    End Enum

    Enum DRAWER
        BXL_CASHDRAWER_PIN2 = 0
        BXL_CASHDRAWER_PIN5 = 1
    End Enum

    Enum PRN_STATUS
        BXL_STS_NORMAL = 0
        BXL_STS_PAPEREMPTY = 1
        BXL_STS_COVEROPEN = 2
        BXL_STS_PAPER_NEAR_END = 4
        BXL_STS_AUTOCUTTER_ERROR = 8
        BXL_STS_CASHDRAWER_HIGH = 16
        BXL_STS_ERROR = 32
        BXL_STS_NOT_OPEN = 64
        BXL_STS_BATTERY_LOW = 128
        BXL_STS_PAPER_TO_BE_TAKEN = 256
        BXL_STS_CASHDRAWER_LOW = 512
    End Enum

    Enum PRT_STATUS
        BXL_PRT_STS_NORMAL = 0
        BXL_PRT_STS_PAPER_NEAR_END = 1
        BXL_PRT_STS_PAPEREMPTY = 4
        BXL_PRT_STS_PAPER_IN = 8
        BXL_PRT_STS_PAPER_JAM = 128
    End Enum

    Enum PM_MODE
        BXL_PRINTER_PAGEMODE_OUT = 0
        BXL_PRINTER_PAGEMODE_IN = 1
    End Enum
    Enum PM_DIRECTION
        BXL_PRINTER_PD_0 = 0
        BXL_PRINTER_PD_LEFT90 = 1
        BXL_PRINTER_PD_180 = 2
        BXL_PRINTER_PD_RIGHT90 = 3
    End Enum

    Enum ALIGN
        BXL_ALIGNMENT_LEFT = 0
        BXL_ALIGNMENT_CENTER = 1
        BXL_ALIGNMENT_RIGHT = 2
    End Enum

    Enum BCS
        BXL_BCS_UPCA = 101
        BXL_BCS_UPCE = 102
        BXL_BCS_EAN8 = 103
        BXL_BCS_EAN13 = 104
        BXL_BCS_JAN8 = 105
        BXL_BCS_JAN13 = 106
        BXL_BCS_ITF = 107
        BXL_BCS_CODABAR = 108
        BXL_BCS_CODE39 = 109
        BXL_BCS_CODE93 = 110
        BXL_BCS_CODE128 = 111
        BXL_BCS_1D_GS1_128 = 112
        BXL_BCS_1D_GS1DATABAR_OMNIDIRECTION = 113
        BXL_BCS_1D_GS1DATABAR_TRUNCATED = 114
        BXL_BCS_1D_GS1DATABAR_LIMITED = 115
        BXL_BCS_PDF417 = 200
        BXL_BCS_QRCODE_MODEL2 = 202
        BXL_BCS_QRCODE_MODEL1 = 203
        BXL_BCS_DATAMATRIX = 204
        BXL_BCS_MAXICODE_MODE2 = 205
        BXL_BCS_MAXICODE_MODE3 = 206
        BXL_BCS_MAXICODE_MODE4 = 207
        BXL_BCS_2D_GS1DATABAR_STACKED = 208
        BXL_BCS_2D_GS1DATABAR_STACKED_OMNIDIRECTIONAL = 209
        BXL_BCS_COMPOSITE_SYMBOLE_EAN8 = 210
        BXL_BCS_COMPOSITE_SYMBOLE_EAN13 = 211
        BXL_BCS_COMPOSITE_SYMBOLE_UPCA = 212
        BXL_BCS_COMPOSITE_SYMBOLE_UPCE = 213
        BXL_BCS_COMPOSITE_SYMBOLE_GS1DATABAR_OMNIDIRECTIONAL = 214
        BXL_BCS_COMPOSITE_SYMBOLE_GS1DATABAR_TRUNCATED = 215
        BXL_BCS_COMPOSITE_SYMBOLE_GS1DATABAR_STACKED = 216
        BXL_BCS_COMPOSITE_SYMBOLE_GS1DATABAR_STACKED_OMNIDIRECTIONAL = 217
        BXL_BCS_COMPOSITE_SYMBOLE_GS1DATABAR_LIMITED = 218
        BXL_BCS_COMPOSITE_SYMBOLE_GS1DATABAR_EXPANDED = 219
        BXL_BCS_COMPOSITE_SYMBOLE_GS1_128 = 220
    End Enum

    Enum QRCODE_TYPE
        BXL_QRCODE_MODEL1 = 49
        BXL_QRCODE_MODEL2 = 50
    End Enum

    Enum QRCODE_ECC
        BXL_QRCODE_ECC_LEVEL_L = 48
        BXL_QRCODE_ECC_LEVEL_M = 49
        BXL_QRCODE_ECC_LEVEL_Q = 50
        BXL_QRCODE_ECC_LEVEL_H = 51
    End Enum

    Enum PDF417_ECC
        BXL_PDF417_ECC_LEVEL_0 = 48
        BXL_PDF417_ECC_LEVEL_1 = 49
        BXL_PDF417_ECC_LEVEL_2 = 50
        BXL_PDF417_ECC_LEVEL_3 = 51
        BXL_PDF417_ECC_LEVEL_4 = 52
        BXL_PDF417_ECC_LEVEL_5 = 53
        BXL_PDF417_ECC_LEVEL_6 = 54
        BXL_PDF417_ECC_LEVEL_7 = 55
        BXL_PDF417_ECC_LEVEL_8 = 56
    End Enum
    Enum GS1DATABAR
        BXL_GS1DATABAR_STACKED = 0
        BXL_GS1DATABAR_STACKED_OMNIDIRECTIONAL = 1
    End Enum
    Enum COMPOSITE_1D
        ' COMPOSITE SYMBOLOGY (LINEAR)
        BXL_COMPOSITE_LINE_EAN8 = 65
        BXL_COMPOSITE_LINE_EAN13 = 66
        BXL_COMPOSITE_LINE_UPCA = 67
        BXL_COMPOSITE_LINE_UPCE = 69
        BXL_COMPOSITE_LINE_GS1DATABAR_OMNIDIRECTIONAL = 70
        BXL_COMPOSITE_LINE_GS1DATABAR_TRUNCATED = 71
        BXL_COMPOSITE_LINE_GS1DATABAR_STACKED = 72
        BXL_COMPOSITE_LINE_GS1DATABAR_STACKED_OMNIDIRECTIONAL = 73
        BXL_COMPOSITE_LINE_GS1DATABAR_LIMITED = 74
        BXL_COMPOSITE_LINE_GS1DATABAR_EXPANDED = 75
        BXL_COMPOSITE_LINE_GS1_128 = 77
    End Enum
    Enum COMPOSITE_2D
        ' COMPOSITE SYMBOLOGY (2D)
        BXL_COMPOSITE_2D_AUTO = 65
        BXL_COMPOSITE_2D_CC_C = 66
    End Enum
    Enum PDF417_TYPE
        BXL_PDF417_TYPE1 = 49
        BXL_PDF417_TYPE2 = 50
    End Enum

    Enum HRI
        BXL_BC_TEXT_NONE = 0
        BXL_BC_TEXT_ABOVE = 1
        BXL_BC_TEXT_BELOW = 2
        BXL_BC_TEXT_BOTH = 3
    End Enum
    Enum BCD_CHARACTER_SET
        BXL_TEXT_ENCODE_PC437 = 437 ' 437 (USA, Standard Europe)
        BXL_TEXT_ENCODE_PC866 = 866 ' 866 (Cyrillic #2)
        BXL_TEXT_ENCODE_PC852 = 852 ' 852 (Latin 2)
        BXL_TEXT_ENCODE_PC857 = 857 ' 857 (Turkish)
        BXL_TEXT_ENCODE_PC850 = 850 ' 850 (Multilingual)
        BXL_TEXT_ENCODE_PC860 = 860 ' 860 (Portuguese)
        BXL_TEXT_ENCODE_PC863 = 863 ' 863 (Canadian-French)
        BXL_TEXT_ENCODE_PC865 = 865 ' 865 (Nordic)
        BXL_TEXT_ENCODE_PC1250 = 1250 ' 1250 (Czech)
        BXL_TEXT_ENCODE_WPC1251 = 1251 ' 1251 (Cyrillic)
        BXL_TEXT_ENCODE_WPC1252 = 1252 ' 1252 (Latin I)
        BXL_TEXT_ENCODE_PC858 = 858     ' 858 (Euro)
        BXL_TEXT_ENCODE_PC862 = 862     ' 862 (Hebrew DOS code)
        BXL_TEXT_ENCODE_WPC1254 = 1254  ' 1254 (Turkish)
        BXL_TEXT_ENCODE_WPC1257 = 1257  ' 1257 (Baltic)
        BXL_TEXT_ENCODE_PC775 = 775     ' 775 (Baltic)
        BXL_TEXT_ENCODE_PC737 = 737     ' 737 (Greek)
        BXL_TEXT_ENCODE_WPC1253 = 1253  ' 1253 (Greek)
        BXL_TEXT_ENCODE_WPC1255 = 1255  ' 1255 (Hebrew New Code)
        BXL_TEXT_ENCODE_PC855 = 855     ' 855 (Cyrillic)
        BXL_TEXT_ENCODE_PC928 = 928     ' 928 (Greek)
        BXL_TEXT_ENCODE_PC1258 = 1258   '1258 (Vietnam)
        BXL_TEXT_ENCODE_TCVN = 49       ' TCVN-3 (Vietnamese)
        BXL_TEXT_ENCODE_TCVN_CAPITAL = 50 ' TCVN-3 (Vietnamese)
        BXL_TEXT_ENCODE_VISCII = 51       ' VISCII (Vietnamese)
    End Enum
    Enum INTERNATIONAL_CHARACTER_SET
        BXL_ICS_USA = 0
        BXL_ICS_FRANCE = 1
        BXL_ICS_GERMANY = 2
        BXL_ICS_UK = 3
        BXL_ICS_DENMARK1 = 4
        BXL_ICS_SWEDEN = 5
        BXL_ICS_ITALY = 6
        BXL_ICS_SPAIN = 7
        BXL_ICS_JAPAN = 8
        BXL_ICS_NORWAY = 9
        BXL_ICS_DENMARK2 = 10
        BXL_ICS_SPAIN2 = 11
        BXL_ICS_LATIN = 12
        BXL_ICS_KOREA = 13
        BXL_ICS_SLOVENIA = 14
        BXL_ICS_CHINA = 15
    End Enum
    Enum CHARACTER_SET
        BXL_CS_PC437 = 0            ' (USA, Standard Europe)
        BXL_CS_KATAKANA = 1         ' Katakana
        BXL_CS_PC850 = 2            ' 850 (Multilingual)
        BXL_CS_PC860 = 3            ' 860 (Portuguese)
        BXL_CS_PC863 = 4            ' 863 (Canadian-French)
        BXL_CS_PC865 = 5            ' 865 (Nordic)
        BXL_CS_WPC1252 = 16         ' 1252 (Latin I)
        BXL_CS_PC866 = 17           ' 866 (Cyrillic #2)
        BXL_CS_PC852 = 18           ' 852 (Latin 2)
        BXL_CS_PC858 = 19           ' 858 (Euro)
        BXL_CS_PC862 = 21           ' 862 (Hebrew DOS code)
        BXL_CS_PC864 = 22           ' NOT Supported / 864 (Arabic)
        BXL_CS_THAI42 = 23          ' THAI42
        BXL_CS_WPC1253 = 24         ' 1253 (Greek)
        BXL_CS_WPC1254 = 25         ' 1254 (Turkish)
        BXL_CS_WPC1257 = 26         ' 1257 (Baltic)
        BXL_CS_FARSI = 27           ' NOT Supported / FARSI
        BXL_CS_WPC1251 = 28         ' 1251 (Cyrillic)
        BXL_CS_PC737 = 29           ' 737 (Greek)
        BXL_CS_PC775 = 30           ' 775 (Baltic)
        BXL_CS_THAI14 = 31          ' THAI14
        BXL_CS_WPC1255 = 33         ' 1255 (Hebrew New Code)
        BXL_CS_THAI11 = 34          ' THAI11
        BXL_CS_THAI18 = 35          ' THAI18
        BXL_CS_PC855 = 36           ' 855 (Cyrillic)
        BXL_CS_PC857 = 37           ' 857 (Turkish)
        BXL_CS_PC928 = 38           ' 928 (Greek)
        BXL_CS_THAI16 = 39          'THAI16
        BXL_CS_WPC1256 = 40         ' NOT Supported / 1256 (Arabic)
        BXL_CS_PC1258 = 41          ' 1258 (Vietnam)
        BXL_CS_KHMER = 42           ' NOT Supported / KHMER(Cambodia)
        BXL_CS_PC1250 = 47          ' 1250 (Czech)
        BXL_CS_LATIN9 = 48          'Latin 9
        BXL_CS_TCVN = 49            ' TCVN-3 (Vietnamese)
        BXL_CS_TCVN_CAPITAL = 50    ' TCVN-3 (Vietnamese)
        BXL_CS_VISCII = 51          ' ISCII (Vietnamese)
        BXL_CS_CP912 = 52           ' 912 (Albania)
        BXL_CS_USER = 255           ' User Code Page
        BXL_CS_UTF8 = 65001         ' UTF-8
        BXL_CS_KS5601 = 949         ' KOREAN
        BXL_CS_BIG5 = 950           ' CHINESE (BIG5)
        BXL_CS_GB2312 = 936         ' Simplified CHINESE (GB2312)
        BXL_CS_SHIFT_JIS = 932      ' JAPAN (Shift JIS)
    End Enum

    Enum FT_ATTRIBUTE
        BXL_FT_DEFAULT = 0
        BXL_FT_FONTB = 1
        BXL_FT_FONTC = 16
        BXL_FT_BOLD = 2
        BXL_FT_UNDERLINE = 4
        BXL_FT_UNDERTHICK = 6
        BXL_FT_REVERSE = 8
        BXL_FT_UPSIDEDOWN = 10
        BXL_FT_RED_COLOR = 64
    End Enum

    Enum FT_WIDTH
        BXL_TS_0WIDTH = 0
        BXL_TS_1WIDTH = 16
        BXL_TS_2WIDTH = 32
        BXL_TS_3WIDTH = 48
        BXL_TS_4WIDTH = 64
        BXL_TS_5WIDTH = 80
        BXL_TS_6WIDTH = 96
        BXL_TS_7WIDTH = 112
    End Enum

    Enum FT_HEIGHT
        BXL_TS_0HEIGHT = 0
        BXL_TS_1HEIGHT = 1
        BXL_TS_2HEIGHT = 2
        BXL_TS_3HEIGHT = 3
        BXL_TS_4HEIGHT = 4
        BXL_TS_5HEIGHT = 5
        BXL_TS_6HEIGHT = 6
        BXL_TS_7HEIGHT = 7
    End Enum

    Enum EJ_OPTION
        BXL_EJT_NONE = 0
        BXL_EJT_HOLD = 1
    End Enum

#End Region


#Region " ETC "

    ' 숫자 입력 처리
    Public Sub pInputValidateDigit(ByRef e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = "." Then
            e.Handled = True
        ElseIf e.KeyChar = Convert.ToChar(System.Windows.Forms.Keys.Back) OrElse e.KeyChar = Convert.ToChar(System.Windows.Forms.Keys.Delete) Then
            e.Handled = False
        ElseIf Char.IsDigit(e.KeyChar) = True Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    ' 숫자 입력 처리(핸들, 소숫점 포함여부) 
    Public Sub pInputValidateFloat(ByRef e As System.Windows.Forms.KeyPressEventArgs, ByVal sender As Object)
        Dim nPreLen As Integer = 4    ' 정수부 길이
        Dim nPostLen As Integer = 3   ' 소수부 길이 

        '백스페이스는 그냥 허용
        If e.KeyChar = Convert.ToChar(System.Windows.Forms.Keys.Back) Then Exit Sub

        'sender 로부터 텍스트 박스 구함
        Dim editor As TextBox = sender

        '소숫점의 점(dot)이 포함되어 있는지 여부.
        '단, 현재 selection 상태인 텍스트에 점이 포함되어 있으면 비포함으로 간주
        Dim bDotContains As Boolean = editor.Text.Contains(".") AndAlso Not editor.SelectedText.Contains(".")

        '전체 길이 체크를 위한 변수(selection 길이는 뺀다)
        Dim nTextLen As Integer = editor.Text.Length - editor.SelectedText.Length
        '현재 커서 위치
        Dim nCursor As Integer = editor.SelectionStart

        '점과 숫자 이외의 값은 받아들이지 않음.
        If Not e.KeyChar = "." AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
            '소숫점 이하 값이 없는 경우 - 2010.12.29 추가
        ElseIf e.KeyChar = "." AndAlso nPostLen < 1 Then
            e.Handled = True
            '점이 포함되어 있을 경우
        ElseIf bDotContains Then

            '전체 길이 체크 정수부와 소수부의 길이 더하기 점의 길이보다 같거나 크면 받아들이지 않음.
            '또한, 이미 점이 포함되어 있으므로, 점이 들어오면 받아들이지 않음.
            If nTextLen >= nPreLen + nPostLen + 1 OrElse e.KeyChar = "." Then
                e.Handled = True
            Else
                '점의 위치를 구한다.
                Dim nDotPos As Integer = editor.Text.IndexOf(".")
                '텍스트를 정수부와 소수부로 나눈다.
                Dim sSep() As String = editor.Text.Split(".")

                '현재 커서가 점 앞에 있고, 정수부의 길이가 지정된 길이보다 길어지면 받아들이지 않음.
                If nDotPos > nCursor AndAlso sSep(0).Length >= nPreLen Then
                    e.Handled = True
                    '현재 커서가 점 뒤에 있고, 소수부의 길이가 지정된 길이보다 길어지면 받아들이지 않음.
                ElseIf nDotPos < nCursor AndAlso sSep(1).Length >= nPostLen Then
                    e.Handled = True
                End If
            End If
            '들어온 값이 점이 아니고, 현재 텍스트가 점을 포함하지 않으면
            '현재 값은 정수인데, 정수부의 길이가 지정된 길이보다 길어지면 받아들이지 않음.
        ElseIf Not e.KeyChar = "." AndAlso Not bDotContains AndAlso nTextLen >= nPreLen Then
            e.Handled = True
        End If

    End Sub
#End Region

End Module
