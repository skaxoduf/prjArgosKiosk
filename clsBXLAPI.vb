Public Class clsBXLAPI

    Public Delegate Function StatusCallBackDelegate(ByVal nStatus As Integer) As Integer

    Public Function IsWow64() As Boolean
        If (System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") = "x86") Then
            Return False
        End If
        Return True
    End Function

    Public Function PrinterOpen(ByVal nInterface As Integer, ByVal szPortName As String, ByVal nBaudRate As Integer, _
                                ByVal nDataBits As Integer, ByVal nParity As Integer, ByVal nStopBits As Integer, _
                                ByVal nFlowControl As Integer) As Integer
        If (IsWow64()) Then
            Return PrinterOpen_x64(nInterface, szPortName, nBaudRate, nDataBits, nParity, nStopBits, nFlowControl)
        End If
        Return PrinterOpen_x86(nInterface, szPortName, nBaudRate, nDataBits, nParity, nStopBits, nFlowControl)
    End Function

    Public Function PrinterClose() As Boolean
        If (IsWow64()) Then
            Return PrinterClose_x64()
        End If
        Return PrinterClose_x86()
    End Function

    Public Function InitializePrinter() As Integer
        If (IsWow64()) Then
            Return InitializePrinter_x64()
        End If
        Return InitializePrinter_x86()
    End Function

    Public Function LineFeed(ByVal nFeed As Integer) As Integer
        If (IsWow64()) Then
            Return LineFeed_x64(nFeed)
        End If
        Return LineFeed_x86(nFeed)
    End Function

    Public Function SetPagemode(ByVal pageMode As Integer) As Integer
        If (IsWow64()) Then
            Return SetPagemode_x64(pageMode)
        End If
        Return SetPagemode_x86(pageMode)
    End Function
    Public Function SetPagemodeDirection(ByVal direction As Integer) As Integer
        If (IsWow64()) Then
            Return SetPagemodeDirection_x64(direction)
        End If
        Return SetPagemodeDirection_x86(direction)
    End Function
    Public Function SetPagemodePrintArea(ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer) As Integer
        If (IsWow64()) Then
            Return SetPagemodePrintArea_x64(x, y, width, height)
        End If
        Return SetPagemodePrintArea_x86(x, y, width, height)
    End Function
    Public Function SetPagemodePosition(ByVal x As Integer, ByVal y As Integer) As Integer
        If (IsWow64()) Then
            Return SetPagemodePosition_x64(x, y)
        End If
        Return SetPagemodePosition_x86(x, y)
    End Function

    Public Function SetCharacterSet(ByVal Value As Integer) As Integer
        If (IsWow64()) Then
            Return SetCharacterSet_x64(Value)
        End If
        Return SetCharacterSet_x86(Value)
    End Function

    Public Function SetInterChrSet(ByVal Value As Integer) As Integer
        If (IsWow64()) Then
            Return SetInterChrSet_x64(Value)
        End If
        Return SetInterChrSet_x86(Value)
    End Function

    Public Function OpenDrawer(ByVal Value As Integer) As Integer
        If (IsWow64()) Then
            Return OpenDrawer_x64(Value)
        End If
        Return OpenDrawer_x86(Value)
    End Function

    Public Function CutPaper() As Integer
        If (IsWow64()) Then
            Return CutPaper_x64()
        End If
        Return CutPaper_x86()
    End Function

    Public Function DirectIO(ByVal Data As Byte(), _
                            ByVal DataLength As UInt32, _
                            ByVal ReadByte As Byte(), _
                            ByRef ReadByteLength As UInt32) As Integer
        If (IsWow64()) Then
            Return DirectIO_x64(Data, DataLength, ReadByte, ReadByteLength)
        End If
        Return DirectIO_x86(Data, DataLength, ReadByte, ReadByteLength)
    End Function

    Public Function WriteBuff(ByVal Data As Byte(), _
                        ByVal DataLength As Integer, _
                        ByRef DataWritten As Integer) As Integer
        If (IsWow64()) Then
            Return WriteBuff_x64(Data, DataLength, DataWritten)
        End If
        Return WriteBuff_x86(Data, DataLength, DataWritten)
    End Function

    Public Function ReadBuff(ByVal ReadBuffer As Byte(), _
                    ByVal ReadBufferLength As Integer, _
                    ByRef LengthRead As Integer) As Integer
        If (IsWow64()) Then
            Return ReadBuff_x64(ReadBuffer, ReadBufferLength, LengthRead)
        End If
        Return ReadBuff_x86(ReadBuffer, ReadBufferLength, LengthRead)
    End Function


    Public Function BidiSetCallBack(ByVal callbackFunc As StatusCallBackDelegate) As Integer
        If (IsWow64()) Then
            Return BidiSetCallBack_x64(callbackFunc)
        End If
        Return BidiSetCallBack_x86(callbackFunc)
    End Function

    Public Function BidiCancelCallBack() As Integer
        If (IsWow64()) Then
            Return BidiCancelCallBack_x64()
        End If
        Return BidiCancelCallBack_x86()
    End Function

    Public Function GetPrinterCurrentStatus() As Integer
        If (IsWow64()) Then
            Return GetPrinterCurrentStatus_x64()
        End If
        Return GetPrinterCurrentStatus_x86()
    End Function

    Public Function PrintText(ByVal Data As String, ByVal Alignment As Integer, ByVal Attribute As Integer, ByVal TextSize As Integer) As Integer
        If (IsWow64()) Then
            Return PrintText_x64(Data, Alignment, Attribute, TextSize)
        End If
        Return PrintText_x86(Data, Alignment, Attribute, TextSize)
    End Function

    Public Function PrintTextW(ByVal Data As String, ByVal Alignment As Integer, ByVal Attribute As Integer, ByVal TextSize As Integer, ByVal CodePage As Integer) As Integer
        If (IsWow64()) Then
            Return PrintTextW_x64(Data, Alignment, Attribute, TextSize, CodePage)
        End If
        Return PrintTextW_x86(Data, Alignment, Attribute, TextSize, CodePage)
    End Function

    Public Function PrintTextInImage(ByVal Data As String, ByVal FontName As String, ByVal Italic As Boolean, ByVal Bold As Boolean, ByVal Underline As Boolean, ByVal FontSize As Integer, ByVal Alignment As Integer) As Integer
        If (IsWow64()) Then
            Return PrintTextInImage_x64(Data, FontName, Italic, Bold, Underline, FontSize, Alignment)
        End If
        Return PrintTextInImage_x86(Data, FontName, Italic, Bold, Underline, FontSize, Alignment)
    End Function

    Public Function PrintTextInImageW(ByVal Data As String, ByVal FontName As String, ByVal Italic As Boolean, ByVal Bold As Boolean, ByVal Underline As Boolean, ByVal FontSize As Integer, ByVal Alignment As Integer) As Integer
        If (IsWow64()) Then
            Return PrintTextInImageW_x64(Data, FontName, Italic, Bold, Underline, FontSize, Alignment)
        End If
        Return PrintTextInImageW_x86(Data, FontName, Italic, Bold, Underline, FontSize, Alignment)
    End Function

    Public Function PrintBarcode(ByVal Data As String, ByVal Symbology As Integer, ByVal Height As Integer, ByVal Width As Integer, ByVal Alignment As Integer, ByVal TextPosition As Integer) As Integer
        If (IsWow64()) Then
            Return PrintBarcode_x64(Data, Symbology, Height, Width, Alignment, TextPosition)
        End If
        Return PrintBarcode_x86(Data, Symbology, Height, Width, Alignment, TextPosition)
    End Function

    Public Function PrintPDF417(ByVal Data As String, ByVal Type As Integer, ByVal ColumnNumber As Integer, ByVal RowNumber As Integer, _
                                    ByVal ModuleWidth As Integer, ByVal ModuleHeight As Integer, ByVal EccLevel As Integer, ByVal Alignment As Integer) As Integer
        If (IsWow64()) Then
            Return PrintPDF417_x64(Data, Type, ColumnNumber, RowNumber, ModuleWidth, ModuleHeight, EccLevel, Alignment)
        End If
        Return PrintPDF417_x86(Data, Type, ColumnNumber, RowNumber, ModuleWidth, ModuleHeight, EccLevel, Alignment)
    End Function

    Public Function PrintQRCode(ByVal Data As String, ByVal Model As Integer, ByVal ModuleSize As Integer, ByVal EccLevel As Integer, ByVal Alignment As Integer) As Integer
        If (IsWow64()) Then
            Return PrintQRCode_x64(Data, Model, ModuleSize, EccLevel, Alignment)
        End If
        Return PrintQRCode_x86(Data, Model, ModuleSize, EccLevel, Alignment)
    End Function

    Public Function PrintQRCodeW(ByVal Data As String, ByVal Model As Integer, ByVal ModuleSize As Integer, ByVal EccLevel As Integer, ByVal Alignment As Integer) As Integer
        If (IsWow64()) Then
            Return PrintQRCodeW_x64(Data, Model, ModuleSize, EccLevel, Alignment)
        End If
        Return PrintQRCodeW_x86(Data, Model, ModuleSize, EccLevel, Alignment)
    End Function

    Public Function PrintDataMatrix(ByVal Data As String, ByVal ModuleSize As Integer, ByVal Alignment As Integer) As Integer
        If (IsWow64()) Then
            Return PrintDataMatrix_x64(Data, ModuleSize, Alignment)
        End If
        Return PrintDataMatrix_x86(Data, ModuleSize, Alignment)
    End Function

    Public Function PrintGS1DataBar(ByVal Data As String, ByVal Symbology As Integer, ByVal ModuleSize As Integer, ByVal Alignment As Integer) As Integer
        If (IsWow64()) Then
            Return PrintGS1DataBar_x64(Data, Symbology, ModuleSize, Alignment)
        End If
        Return PrintGS1DataBar_x86(Data, Symbology, ModuleSize, Alignment)
    End Function

    Public Function PrintCompositeSymbology(ByVal Data1d As String, ByVal Symbol1d As Integer, _
                                            ByVal Data2d As String, ByVal Symbol2d As Integer, _
                                            ByVal ModuleSize As Integer, ByVal Alignment As Integer) As Integer
        If (IsWow64()) Then
            Return PrintCompositeSymbology_x64(Data1d, Symbol1d, Data2d, Symbol2d, ModuleSize, Alignment)
        End If
        Return PrintCompositeSymbology_x86(Data1d, Symbol1d, Data2d, Symbol2d, ModuleSize, Alignment)
    End Function

    Public Function PrintBitmap(ByVal FileName As String, _
                                ByVal Width As Integer, _
                                ByVal Alignment As Integer, _
                                ByVal Level As Integer, _
                                ByVal Dithering As Boolean) As Integer
        If (IsWow64()) Then
            Return PrintBitmap_x64(FileName, Width, Alignment, Level, Dithering)
        End If
        Return PrintBitmap_x86(FileName, Width, Alignment, Level, Dithering)
    End Function
    Public Function PrintBitmapLZMA(ByVal FileName As String, _
                               ByVal Width As Integer, _
                               ByVal Alignment As Integer, _
                               ByVal Level As Integer, _
                               ByVal Dithering As Boolean) As Integer
        If (IsWow64()) Then
            Return PrintBitmapLZMA_x64(FileName, Width, Alignment, Level, Dithering)
        End If
        Return PrintBitmapLZMA_x86(FileName, Width, Alignment, Level, Dithering)
    End Function

    Public Function TransactionStart() As Integer
        If (IsWow64()) Then
            Return TransactionStart_x64()
        End If
        Return TransactionStart_x86()
    End Function

    Public Function TransactionEnd(ByVal SendCompletedCheck As Boolean, ByVal Timeout As Integer) As Integer
        If (IsWow64()) Then
            Return TransactionEnd_x64(SendCompletedCheck, Timeout)
        End If
        Return TransactionEnd_x86(SendCompletedCheck, Timeout)
    End Function

    Public Function ClearScreen() As Integer
        If (IsWow64()) Then
            Return ClearScreen_x64()
        End If
        Return ClearScreen_x86()
    End Function

    Public Function ClearLine(ByVal Line As Integer) As Integer
        If (IsWow64()) Then
            Return ClearLine_x64(Line)
        End If
        Return ClearLine_x86(Line)
    End Function

    Public Function DisplayString(ByVal Data As String) As Integer
        If (IsWow64()) Then
            Return DisplayString_x64(Data)
        End If
        Return DisplayString_x86(Data)
    End Function

    Public Function DisplayStringW(ByVal Data As String, ByVal Codepage As Integer) As Integer
        If (IsWow64()) Then
            Return DisplayStringW_x64(Data, Codepage)
        End If
        Return DisplayStringW_x86(Data, Codepage)
    End Function

    Public Function DisplayStringAtLine(ByVal Line As Integer, ByVal Data As String) As Integer
        If (IsWow64()) Then
            Return DisplayStringAtLine_x64(Line, Data)
        End If
        Return DisplayStringAtLine_x86(Line, Data)
    End Function

    Public Function DisplayStringAtLineW(ByVal Line As Integer, ByVal Data As String, ByVal Codepage As Integer) As Integer
        If (IsWow64()) Then
            Return DisplayStringAtLineW_x64(Line, Data, Codepage)
        End If
        Return DisplayStringAtLineW_x86(Line, Data, Codepage)
    End Function

    Public Function DisplayImage(ByVal index As Integer, ByVal x As Integer, ByVal y As Integer) As Integer
        If (IsWow64()) Then
            Return DisplayImage_x64(index, x, y)
        End If
        Return DisplayImage_x86(index, x, y)
    End Function

    Public Function StoreImageFile(ByVal fileName As String, ByVal index As Integer) As Integer
        If (IsWow64()) Then
            Return StoreImageFile_x64(fileName, index)
        End If
        Return StoreImageFile_x86(fileName, index)
    End Function

    Public Function SetInternationalCharacterset(ByVal characterSet As Integer) As Integer
        If (IsWow64()) Then
            Return SetInternationalCharacterset_x64(characterSet)
        End If
        Return SetInternationalCharacterset_x86(characterSet)
    End Function

    Public Function PaperEject(ByVal nOption As Integer) As Integer
        If (IsWow64()) Then
            Return PaperEject_x64(nOption)
        End If
        Return PaperEject_x86(nOption)
    End Function

    Public Function GetPresenterStatus() As Integer
        If (IsWow64()) Then
            Return GetPresenterStatus_x64()
        End If
        Return GetPresenterStatus_x86()
    End Function
End Class
