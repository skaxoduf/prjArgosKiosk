Imports System.Runtime.InteropServices
Imports System.Text

Public Module mdlBXLAPI_x64

#Region " DLL API Function "

    Public Declare Function PrinterOpen_x64 Lib "BXLPAPI_x64.dll" Alias "PrinterOpen" ( _
                            ByVal nInterface As Integer, _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal szPortName As String, _
                            ByVal nBaudRate As Integer, _
                            ByVal nDataBits As Integer, _
                            ByVal nParity As Integer, _
                            ByVal nStopBits As Integer, _
                            ByVal nFlowControl As Integer) As Integer

    Public Declare Function PrinterOpenW_x64 Lib "BXLPAPI_x64.dll" Alias "PrinterOpenW" ( _
                            ByVal nInterface As Integer, _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal szPortName As String, _
                            ByVal nBaudRate As Integer, _
                            ByVal nDataBits As Integer, _
                            ByVal nParity As Integer, _
                            ByVal nStopBits As Integer, _
                            ByVal nFlowControl As Integer) As Integer

    Public Declare Function PrinterClose_x64 Lib "BXLPAPI_x64.dll" Alias "PrinterClose" () As Boolean
    Public Declare Function InitializePrinter_x64 Lib "BXLPAPI_x64.dll" Alias "InitializePrinter" () As Boolean
    Public Declare Function CutPaper_x64 Lib "BXLPAPI_x64.dll" Alias "CutPaper" () As Boolean

    Public Declare Function OpenDrawer_x64 Lib "BXLPAPI_x64.dll" Alias "OpenDrawer" (ByVal value As Integer) As Integer

    Public Declare Function SetCharacterSet_x64 Lib "BXLPAPI_x64.dll" Alias "SetCharacterSet" (ByVal value As Integer) As Integer
    Public Declare Function SetInterChrSet_x64 Lib "BXLPAPI_x64.dll" Alias "SetInterChrSet" (ByVal value As Integer) As Integer

    Public Declare Function GetPrinterCurrentStatus_x64 Lib "BXLPAPI_x64.dll" Alias "GetPrinterCurrentStatus" () As Integer
    Public Declare Function GetCharacterSet_x64 Lib "BXLPAPI_x64.dll" Alias "GetCharacterSet" () As Integer
    Public Declare Function GetInterChrSet_x64 Lib "BXLPAPI_x64.dll" Alias "GetInterChrSet" () As Integer

    Public Declare Function BidiSetCallBack_x64 Lib "BXLPAPI_x64.dll" Alias "BidiSetCallBack" (ByVal callbackFunc As clsBXLAPI.StatusCallBackDelegate) As Integer

    Public Declare Function BidiCancelCallBack_x64 Lib "BXLPAPI_x64.dll" Alias "BidiCancelCallBack" () As Integer


    Public Declare Function DirectIO_x64 Lib "BXLPAPI_x64.dll" Alias "DirectIO" ( _
                        ByVal Data As Byte(), _
                        ByVal DataLength As UInt32, _
                        ByVal ReadByte As Byte(), _
                        ByRef ReadByteLength As UInt32) As Integer

    Public Declare Function WriteBuff_x64 Lib "BXLPAPI_x64.dll" Alias "WriteBuff" ( _
                    ByVal Data As Byte(), _
                    ByVal DataLength As Integer, _
                    ByRef ReadByteLength As Integer) As Integer

    Public Declare Function ReadBuff_x64 Lib "BXLPAPI_x64.dll" Alias "ReadBuff" ( _
                ByVal ReadBuffer As Byte(), _
                ByVal ReadBufferLength As Integer, _
                ByRef LengthRead As Integer) As Integer


    Public Declare Function PrintText_x64 Lib "BXLPAPI_x64.dll" Alias "PrintText" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String, _
                            ByVal Alignment As Integer, _
                            ByVal Attribute As Integer, _
                            ByVal TextSize As Integer) As Integer

    Public Declare Function PrintTextInImage_x64 Lib "BXLPAPI_x64.dll" Alias "PrintTextInImage" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String, _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal FontName As String, _
                            ByVal Italic As Boolean, _
                            ByVal Bold As Boolean, _
                            ByVal Underline As Boolean, _
                            ByVal FontSize As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function PrintTextInImageW_x64 Lib "BXLPAPI_x64.dll" Alias "PrintTextInImageW" ( _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal Data As String, _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal FontName As String, _
                            ByVal Italic As Boolean, _
                            ByVal Bold As Boolean, _
                            ByVal Underline As Boolean, _
                            ByVal FontSize As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function PrintTextW_x64 Lib "BXLPAPI_x64.dll" Alias "PrintTextW" ( _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal Data As String, _
                            ByVal Alignment As Integer, _
                            ByVal Attribute As Integer, _
                            ByVal TextSize As Integer, _
                            ByVal CodePage As Integer) As Integer

    Public Declare Function SetPagemode_x64 Lib "BXLPAPI_x64.dll" Alias "SetPagemode" (ByVal pageMode As Integer) As Integer
    Public Declare Function SetPagemodeDirection_x64 Lib "BXLPAPI_x64.dll" Alias "SetPagemodeDirection" (ByVal direction As Integer) As Integer
    Public Declare Function SetPagemodePrintArea_x64 Lib "BXLPAPI_x64.dll" Alias "SetPagemodePrintArea" (ByVal x As Integer, _
                                                                                                         ByVal y As Integer, _
                                                                                                         ByVal width As Integer, _
                                                                                                         ByVal height As Integer) As Integer
    Public Declare Function SetPagemodePosition_x64 Lib "BXLPAPI_x64.dll" Alias "SetPagemodePosition" (ByVal x As Integer, _
                                                                                                       ByVal y As Integer) As Integer

    Public Declare Function LineFeed_x64 Lib "BXLPAPI_x64.dll" Alias "LineFeed" (ByVal nFeed As Integer) As Integer

    Public Declare Function PrintBitmap_x64 Lib "BXLPAPI_x64.dll" Alias "PrintBitmap" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal FileName As String, _
                            ByVal Width As Integer, _
                            ByVal Alignment As Integer, _
                            ByVal Level As Integer, _
                            ByVal bDithering As Boolean) As Integer

    Public Declare Function PrintBitmapLZMA_x64 Lib "BXLPAPI_x64.dll" Alias "PrintBitmapLZMA" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal FileName As String, _
                            ByVal Width As Integer, _
                            ByVal Alignment As Integer, _
                            ByVal Level As Integer, _
                            ByVal bDithering As Boolean) As Integer

    Public Declare Function PrintBarcode_x64 Lib "BXLPAPI_x64.dll" Alias "PrintBarcode" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String, _
                            ByVal Symbology As Integer, _
                            ByVal Height As Integer, _
                            ByVal Width As Integer, _
                            ByVal Alignment As Integer, _
                            ByVal TextPosition As Integer) As Integer

    Public Declare Function PrintPDF417_x64 Lib "BXLPAPI_x64.dll" Alias "PrintPDF417" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String, _
                            ByVal Type As Integer, _
                            ByVal ColumnNumber As Integer, _
                            ByVal RowNumber As Integer, _
                            ByVal ModuleWidth As Integer, _
                            ByVal ModuleHeight As Integer, _
                            ByVal EccLevel As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function PrintQRCode_x64 Lib "BXLPAPI_x64.dll" Alias "PrintQRCode" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String, _
                            ByVal Model As Integer, _
                            ByVal ModuleSize As Integer, _
                            ByVal EccLevel As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function PrintQRCodeW_x64 Lib "BXLPAPI_x64.dll" Alias "PrintQRCodeW" ( _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal Data As String, _
                            ByVal Model As Integer, _
                            ByVal ModuleSize As Integer, _
                            ByVal EccLevel As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function PrintDataMatrix_x64 Lib "BXLPAPI_x64.dll" Alias "PrintDataMatrix" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String, _
                            ByVal ModuleSize As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function PrintGS1DataBar_x64 Lib "BXLPAPI_x64.dll" Alias "PrintGS1DataBar" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String, _
                            ByVal Symbology As Integer, _
                            ByVal ModuleSize As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function PrintCompositeSymbology_x64 Lib "BXLPAPI_x64.dll" Alias "PrintCompositeSymbology" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data1d As String, _
                            ByVal Symbol1d As Integer, _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data2d As String, _
                            ByVal Symbol2d As Integer, _
                            ByVal ModuleSize As Integer, _
                            ByVal Alignment As Integer) As Integer

    Public Declare Function TransactionStart_x64 Lib "BXLPAPI_x64.dll" Alias "TransactionStart" () As Integer

    Public Declare Function TransactionEnd_x64 Lib "BXLPAPI_x64.dll" Alias "TransactionEnd" ( _
                            ByVal SendCompletedCheck As Boolean, _
                            ByVal Timeout As Integer) As Integer

    Public Declare Function ClearScreen_x64 Lib "BXLPAPI_x64.dll" Alias "ClearScreen" () As Integer

    Public Declare Function ClearLine_x64 Lib "BXLPAPI_x64.dll" Alias "ClearLine" (ByVal Line As Integer) As Integer

    Public Declare Function DisplayString_x64 Lib "BXLPAPI_x64.dll" Alias "DisplayString" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String) As Integer

    Public Declare Function DisplayStringW_x64 Lib "BXLPAPI_x64.dll" Alias "DisplayStringW" ( _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal Data As String, _
                            ByVal Codepage As Integer) As Integer

    Public Declare Function DisplayStringAtLine_x64 Lib "BXLPAPI_x64.dll" Alias "DisplayStringAtLine" ( _
                            ByVal Line As Integer, _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal Data As String) As Integer

    Public Declare Function DisplayStringAtLineW_x64 Lib "BXLPAPI_x64.dll" Alias "DisplayStringAtLineW" ( _
                            ByVal Line As Integer, _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal Data As String, _
                            ByVal Codepage As Integer) As Integer

    Public Declare Function DisplayImage_x64 Lib "BXLPAPI_x64.dll" Alias "DisplayImage" ( _
                            ByVal index As Integer, _
                            ByVal x As Integer, _
                            ByVal y As Integer) As Integer

    Public Declare Function StoreImageFile_x64 Lib "BXLPAPI_x64.dll" Alias "StoreImageFile" ( _
                            <MarshalAs(UnmanagedType.LPStr)> ByVal fileName As String, _
                            ByVal index As Integer) As Integer

    Public Declare Function StoreImageFileW_x64 Lib "BXLPAPI_x64.dll" Alias "StoreImageFileW" ( _
                            <MarshalAs(UnmanagedType.LPWStr)> ByVal fileName As String, _
                            ByVal index As Integer) As Integer

    Public Declare Function SetInternationalCharacterset_x64 Lib "BXLPAPI_x64.dll" Alias "SetInternationalCharacterset" ( _
                            ByVal characterSet As Integer) As Integer

    Public Declare Function PaperEject_x64 Lib "BXLPAPI_x64.dll" Alias "PaperEject" ( _
                            ByVal nOption As Integer) As Integer
    Public Declare Function GetPresenterStatus_x64 Lib "BXLPAPI_x64.dll" Alias "GetPresenterStatus" () As Integer

#End Region


End Module
