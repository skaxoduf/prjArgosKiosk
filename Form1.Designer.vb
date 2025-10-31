<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        WebView21 = New Microsoft.Web.WebView2.WinForms.WebView2()
        pnlCSMain = New Panel()
        ListBox1 = New ListBox()
        CType(WebView21, ComponentModel.ISupportInitialize).BeginInit()
        pnlCSMain.SuspendLayout()
        SuspendLayout()
        ' 
        ' WebView21
        ' 
        WebView21.AllowExternalDrop = True
        WebView21.CreationProperties = Nothing
        WebView21.DefaultBackgroundColor = Color.White
        WebView21.Dock = DockStyle.Fill
        WebView21.Location = New Point(0, 0)
        WebView21.Name = "WebView21"
        WebView21.Size = New Size(1115, 760)
        WebView21.TabIndex = 0
        WebView21.ZoomFactor = 1R
        ' 
        ' pnlCSMain
        ' 
        pnlCSMain.BackColor = Color.White
        pnlCSMain.Controls.Add(ListBox1)
        pnlCSMain.Location = New Point(69, 107)
        pnlCSMain.Name = "pnlCSMain"
        pnlCSMain.Size = New Size(741, 476)
        pnlCSMain.TabIndex = 1
        ' 
        ' ListBox1
        ' 
        ListBox1.FormattingEnabled = True
        ListBox1.ItemHeight = 15
        ListBox1.Location = New Point(85, 73)
        ListBox1.Name = "ListBox1"
        ListBox1.Size = New Size(359, 274)
        ListBox1.TabIndex = 0
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1115, 760)
        Controls.Add(pnlCSMain)
        Controls.Add(WebView21)
        Name = "Form1"
        Text = "Form1"
        CType(WebView21, ComponentModel.ISupportInitialize).EndInit()
        pnlCSMain.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents WebView21 As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents pnlCSMain As Panel
    Friend WithEvents ListBox1 As ListBox

End Class
