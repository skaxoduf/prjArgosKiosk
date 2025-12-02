Imports System.Runtime.InteropServices
Imports System.Text

Public Module mdlBXLAPI_x86

#Region " DLL API Function "

    Public Declare Function PrinterOpen_x86 Lib "BXLPAPI.dll" Alias "PrinterOpen" ( _
                            ByVal nInterface As Integer, _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal szPortName As String, _
                            ByVal nBaudRate As Integer, _
                            ByVal nDataBits As Integer, _
                            ByVal nParity As Integer, _
                            ByVal nStopBits As Integer, _
                            ByVal nFlowControl As Integer) As Integer

    Public Declare Function PrinterOpenW_x86 Lib "BXLPAPI.dll" Alias "PrinterOpenW" ( _
                            ByVal nInterface As Integer, _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal szPortName As String, _
                            ByVal nBaudRate As Integer, _
                            ByVal nDataBits As Integer, _
                            ByVal nParity As Integer, _
                            ByVal nStopBits As Integer, _
                            ByVal nFlowControl As Integer) As Integer

    Public Declare Function PrinterClose_x86 Lib "BXLPAPI.dll" Alias "PrinterClose" () As Boolean

    Public Declare Function SetCharacterSet_x86 Lib "BXLPAPI.dll" Alias "SetCharacterSet" (ByVal value As Integer) As Integer
    Public Declare Function SetInterChrSet_x86 Lib "BXLPAPI.dll" Alias "SetInterChrSet" (ByVal value As Integer) As Integer

    Public Declare Function InitializePrinter_x86 Lib "BXLPAPI.dll" Alias "InitializePrinter" () As Boolean
    Public Declare Function CutPaper_x86 Lib "BXLPAPI.dll" Alias "CutPaper" () As Boolean
    Public Declare Function OpenDrawer_x86 Lib "BXLPAPI.dll" Alias "OpenDrawer" (ByVal value As Integer) As Integer

    Public Declare Function DirectIO_x86 Lib "BXLPAPI.dll" Alias "DirectIO" ( _
                        ByVal Data As Byte(), _
                        ByVal DataLength As UInt32, _
                        ByVal ReadByte As Byte(), _
                        ByRef ReadByteLength As UInt32) As Integer

    Public Declare Function WriteBuff_x86 Lib "BXLPAPI.dll" Alias "WriteBuff" ( _
                         ByVal Data As Byte(), _
                         ByVal DataLength As Integer, _
                         ByRef ReadByteLength As Integer) As Integer

    Public Declare Function ReadBuff_x86 Lib "BXLPAPI.dll" Alias "ReadBuff" ( _
                        ByVal ReadBuffer As Byte(), _
                        ByVal ReadBufferLength As Integer, _
                        ByRef LengthRead As Integer) As Integer

    Public Declare Function GetPrinterCurrentStatus_x86 Lib "BXLPAPI.dll" Alias "GetPrinterCurrentStatus" () As Integer
    Public Declare Function GetCharacterSet_x86 Lib "BXLPAPI.dll" Alias "GetCharacterSet" () As Integer
    Public Declare Function GetInterChrSet_86 Lib "BXLPAPI.dll" Alias "GetInterChrSet" () As Integer

    Public Declare Function BidiSetCallBack_x86 Lib "BXLPAPI.dll" Alias "BidiSetCallBack" (ByVal callbackFunc As clsBXLAPI.StatusCallBackDelegate) As Integer
    Public Declare Function BidiCancelCallBack_x86 Lib "BXLPAPI.dll" Alias "BidiCancelCallBack" () As Integer

    Public Declare Function PrintText_x86 Lib "BXLPAPI.dll" Alias "PrintText" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String, _
                            ByVal Alignment As Integer, _
                            ByVal Attribute As Integer, _
                            ByVal TextSize As Integer) As Integer

    Public Declare Function PrintTextW_x86 Lib "BXLPAPI.dll" Alias "PrintTextW" ( _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal Data As String, _
                            ByVal Alignment As Integer, _
                            ByVal Attribute As Integer, _
                            ByVal TextSize As Integer, _
                            ByVal CodePage As Integer) As Integer

    Public Declare Function PrintTextInImage_x86 Lib "BXLPAPI.dll" Alias "PrintTextInImage" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String, _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal FontName As String, _
                            ByVal Italic As Boolean, _
                            ByVal Bold As Boolean, _
                            ByVal Underline As Boolean, _
                            ByVal FontSize As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function PrintTextInImageW_x86 Lib "BXLPAPI.dll" Alias "PrintTextInImageW" ( _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal Data As String, _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal FontName As String, _
                            ByVal Italic As Boolean, _
                            ByVal Bold As Boolean, _
                            ByVal Underline As Boolean, _
                            ByVal FontSize As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function LineFeed_x86 Lib "BXLPAPI.dll" Alias "LineFeed" (ByVal nFeed As Integer) As Integer

    Public Declare Function SetPagemode_x86 Lib "BXLPAPI_x64.dll" Alias "SetPagemode" (ByVal pageMode As Integer) As Integer
    Public Declare Function SetPagemodeDirection_x86 Lib "BXLPAPI_x64.dll" Alias "SetPagemodeDirection" (ByVal direction As Integer) As Integer
    Public Declare Function SetPagemodePrintArea_x86 Lib "BXLPAPI_x64.dll" Alias "SetPagemodePrintArea" (ByVal x As Integer, _
                                                                                                         ByVal y As Integer, _
                                                                                                         ByVal width As Integer, _
                                                                                                         ByVal height As Integer) As Integer
    Public Declare Function SetPagemodePosition_x86 Lib "BXLPAPI_x64.dll" Alias "SetPagemodePosition" (ByVal x As Integer, _
                                                                                                       ByVal y As Integer) As Integer

    Public Declare Function PrintBitmap_x86 Lib "BXLPAPI.dll" Alias "PrintBitmap" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal FileName As String, _
                            ByVal Width As Integer, _
                            ByVal Alignment As Integer, _
                            ByVal Level As Integer, _
                            ByVal bDithering As Boolean) As Integer

    Public Declare Function PrintBitmapLZMA_x86 Lib "BXLPAPI.dll" Alias "PrintBitmapLZMA" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal FileName As String, _
                            ByVal Width As Integer, _
                            ByVal Alignment As Integer, _
                            ByVal Level As Integer, _
                            ByVal bDithering As Boolean) As Integer

    Public Declare Function PrintBarcode_x86 Lib "BXLPAPI.dll" Alias "PrintBarcode" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String, _
                            ByVal Symbology As Integer, _
                            ByVal Height As Integer, _
                            ByVal Width As Integer, _
                            ByVal Alignment As Integer, _
                            ByVal TextPosition As Integer) As Integer

    Public Declare Function PrintPDF417_x86 Lib "BXLPAPI.dll" Alias "PrintPDF417" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String, _
                            ByVal Type As Integer, _
                            ByVal ColumnNumber As Integer, _
                            ByVal RowNumber As Integer, _
                            ByVal ModuleWidth As Integer, _
                            ByVal ModuleHeight As Integer, _
                            ByVal EccLevel As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function PrintQRCode_x86 Lib "BXLPAPI.dll" Alias "PrintQRCode" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String, _
                            ByVal Model As Integer, _
                            ByVal ModuleSize As Integer, _
                            ByVal EccLevel As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function PrintQRCodeW_x86 Lib "BXLPAPI.dll" Alias "PrintQRCodeW" ( _
                        <MarshalAs(UnmanagedType.LPWStr)> ByVal Data As String, _
                        ByVal Model As Integer, _
                        ByVal ModuleSize As Integer, _
                        ByVal EccLevel As Integer, _
                        ByVal Alignment As Integer) As Integer

    Public Declare Function PrintDataMatrix_x86 Lib "BXLPAPI.dll" Alias "PrintDataMatrix" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String, _
                            ByVal ModuleSize As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function PrintGS1DataBar_x86 Lib "BXLPAPI.dll" Alias "PrintGS1DataBar" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String, _
                            ByVal Symbology As Integer, _
                            ByVal ModuleSize As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function PrintCompositeSymbology_x86 Lib "BXLPAPI.dll" Alias "PrintCompositeSymbology" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data1d As String, _
                            ByVal Symbol1d As Integer, _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data2d As String, _
                            ByVal Symbol2d As Integer, _
                            ByVal ModuleSize As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function TransactionStart_x86 Lib "BXLPAPI.dll" Alias "TransactionStart" () As Integer

    Public Declare Function TransactionEnd_x86 Lib "BXLPAPI.dll" Alias "TransactionEnd" ( _
                        ByVal SendCompletedCheck As Boolean, _
                        ByVal Timeout As Integer) As Integer

    Public Declare Function ClearScreen_x86 Lib "BXLPAPI.dll" Alias "ClearScreen" () As Integer

    Public Declare Function ClearLine_x86 Lib "BXLPAPI.dll" Alias "ClearLine" (ByVal Line As Integer) As Integer

    Public Declare Function DisplayString_x86 Lib "BXLPAPI.dll" Alias "DisplayString" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String) As Integer

    Public Declare Function DisplayStringW_x86 Lib "BXLPAPI.dll" Alias "DisplayStringW" ( _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal Data As String, _
                            ByVal Codepage As Integer) As Integer

    Public Declare Function DisplayStringAtLine_x86 Lib "BXLPAPI.dll" Alias "DisplayStringAtLine" ( _
                            ByVal Line As Integer, _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String) As Integer

    Public Declare Function DisplayStringAtLineW_x86 Lib "BXLPAPI.dll" Alias "DisplayStringAtLineW" ( _
                            ByVal Line As Integer, _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal Data As String, _
                            ByVal Codepage As Integer) As Integer

    Public Declare Function DisplayImage_x86 Lib "BXLPAPI.dll" Alias "DisplayImage" ( _
                            ByVal index As Integer, _
                            ByVal x As Integer, _
                            ByVal y As Integer) As Integer

    Public Declare Function StoreImageFile_x86 Lib "BXLPAPI.dll" Alias "StoreImageFile" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal fileName As String, _
                            ByVal index As Integer) As Integer

    Public Declare Function StoreImageFileW_x86 Lib "BXLPAPI.dll" Alias "StoreImageFileW" ( _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal fileName As String, _
                            ByVal index As Integer) As Integer

    Public Declare Function SetInternationalCharacterset_x86 Lib "BXLPAPI.dll" Alias "SetInternationalCharacterset" ( _
                            ByVal characterSet As Integer) As Integer

    Public Declare Function PaperEject_x86 Lib "BXLPAPI.dll" Alias "PaperEject" ( _
                            ByVal nOption As Integer) As Integer

    Public Declare Function GetPresenterStatus_x86 Lib "BXLPAPI.dll" Alias "GetPresenterStatus" () As Integer

#End Region

End Module
