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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        WebView21 = New Microsoft.Web.WebView2.WinForms.WebView2()
        pnlCSMain = New Panel()
        Label1 = New Label()
        txtCompanyCode = New TextBox()
        Label2 = New Label()
        txtPosNo = New TextBox()
        btnSave = New Button()
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
        WebView21.Size = New Size(1064, 1061)
        WebView21.TabIndex = 0
        WebView21.ZoomFactor = 1R
        ' 
        ' pnlCSMain
        ' 
        pnlCSMain.BackColor = Color.White
        pnlCSMain.BackgroundImage = CType(resources.GetObject("pnlCSMain.BackgroundImage"), Image)
        pnlCSMain.BackgroundImageLayout = ImageLayout.Zoom
        pnlCSMain.Controls.Add(btnSave)
        pnlCSMain.Controls.Add(txtPosNo)
        pnlCSMain.Controls.Add(txtCompanyCode)
        pnlCSMain.Controls.Add(Label2)
        pnlCSMain.Controls.Add(Label1)
        pnlCSMain.Location = New Point(22, 30)
        pnlCSMain.Name = "pnlCSMain"
        pnlCSMain.Size = New Size(866, 595)
        pnlCSMain.TabIndex = 1
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.BackColor = Color.Black
        Label1.Font = New Font("맑은 고딕", 24F, FontStyle.Bold, GraphicsUnit.Point, CByte(129))
        Label1.ForeColor = Color.SeaShell
        Label1.ImageAlign = ContentAlignment.BottomLeft
        Label1.Location = New Point(37, 26)
        Label1.Name = "Label1"
        Label1.Size = New Size(148, 45)
        Label1.TabIndex = 0
        Label1.Text = "업체코드"
        ' 
        ' txtCompanyCode
        ' 
        txtCompanyCode.BorderStyle = BorderStyle.FixedSingle
        txtCompanyCode.Font = New Font("맑은 고딕", 24F, FontStyle.Bold)
        txtCompanyCode.Location = New Point(191, 26)
        txtCompanyCode.Name = "txtCompanyCode"
        txtCompanyCode.Size = New Size(441, 50)
        txtCompanyCode.TabIndex = 1
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.BackColor = Color.Black
        Label2.Font = New Font("맑은 고딕", 24F, FontStyle.Bold)
        Label2.ForeColor = Color.SeaShell
        Label2.Location = New Point(37, 101)
        Label2.Name = "Label2"
        Label2.Size = New Size(148, 45)
        Label2.TabIndex = 0
        Label2.Text = "포스번호"
        ' 
        ' txtPosNo
        ' 
        txtPosNo.BorderStyle = BorderStyle.FixedSingle
        txtPosNo.Font = New Font("맑은 고딕", 24F, FontStyle.Bold)
        txtPosNo.Location = New Point(191, 101)
        txtPosNo.Name = "txtPosNo"
        txtPosNo.Size = New Size(105, 50)
        txtPosNo.TabIndex = 1
        ' 
        ' btnSave
        ' 
        btnSave.BackColor = Color.Black
        btnSave.BackgroundImageLayout = ImageLayout.None
        btnSave.Font = New Font("맑은 고딕", 24F, FontStyle.Bold)
        btnSave.ForeColor = Color.White
        btnSave.Location = New Point(342, 465)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(185, 57)
        btnSave.TabIndex = 2
        btnSave.Text = "저장하기"
        btnSave.UseVisualStyleBackColor = False
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1064, 1061)
        Controls.Add(pnlCSMain)
        Controls.Add(WebView21)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "Form1"
        Text = "ArgosAPT Kiosk"
        CType(WebView21, ComponentModel.ISupportInitialize).EndInit()
        pnlCSMain.ResumeLayout(False)
        pnlCSMain.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents WebView21 As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents pnlCSMain As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents txtCompanyCode As TextBox
    Friend WithEvents txtPosNo As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents btnSave As Button

End Class
