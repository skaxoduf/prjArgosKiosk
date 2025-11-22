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
        btnExit = New Button()
        btnSave = New Button()
        txtTimer = New TextBox()
        txtPosNo = New TextBox()
        txtCompanyCode = New TextBox()
        Label3 = New Label()
        Label2 = New Label()
        Label1 = New Label()
        Button1 = New Button()
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
        pnlCSMain.Controls.Add(btnExit)
        pnlCSMain.Controls.Add(btnSave)
        pnlCSMain.Controls.Add(txtTimer)
        pnlCSMain.Controls.Add(txtPosNo)
        pnlCSMain.Controls.Add(txtCompanyCode)
        pnlCSMain.Controls.Add(Label3)
        pnlCSMain.Controls.Add(Label2)
        pnlCSMain.Controls.Add(Label1)
        pnlCSMain.Location = New Point(22, 30)
        pnlCSMain.Name = "pnlCSMain"
        pnlCSMain.Size = New Size(866, 595)
        pnlCSMain.TabIndex = 1
        ' 
        ' btnExit
        ' 
        btnExit.BackColor = Color.Black
        btnExit.BackgroundImageLayout = ImageLayout.None
        btnExit.Font = New Font("맑은 고딕", 24F, FontStyle.Bold)
        btnExit.ForeColor = Color.White
        btnExit.Location = New Point(37, 466)
        btnExit.Name = "btnExit"
        btnExit.Size = New Size(185, 57)
        btnExit.TabIndex = 2
        btnExit.Text = "종료하기"
        btnExit.UseVisualStyleBackColor = False
        ' 
        ' btnSave
        ' 
        btnSave.BackColor = Color.Black
        btnSave.BackgroundImageLayout = ImageLayout.None
        btnSave.Font = New Font("맑은 고딕", 24F, FontStyle.Bold)
        btnSave.ForeColor = Color.White
        btnSave.Location = New Point(637, 466)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(185, 57)
        btnSave.TabIndex = 2
        btnSave.Text = "저장하기"
        btnSave.UseVisualStyleBackColor = False
        ' 
        ' txtTimer
        ' 
        txtTimer.BorderStyle = BorderStyle.FixedSingle
        txtTimer.Font = New Font("맑은 고딕", 24F, FontStyle.Bold)
        txtTimer.Location = New Point(191, 165)
        txtTimer.Name = "txtTimer"
        txtTimer.Size = New Size(105, 50)
        txtTimer.TabIndex = 1
        ' 
        ' txtPosNo
        ' 
        txtPosNo.BorderStyle = BorderStyle.FixedSingle
        txtPosNo.Font = New Font("맑은 고딕", 24F, FontStyle.Bold)
        txtPosNo.Location = New Point(191, 95)
        txtPosNo.Name = "txtPosNo"
        txtPosNo.Size = New Size(105, 50)
        txtPosNo.TabIndex = 1
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
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.BackColor = Color.Black
        Label3.Font = New Font("맑은 고딕", 24F, FontStyle.Bold)
        Label3.ForeColor = Color.SeaShell
        Label3.Location = New Point(69, 165)
        Label3.Name = "Label3"
        Label3.Size = New Size(116, 45)
        Label3.TabIndex = 0
        Label3.Text = "타이머"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.BackColor = Color.Black
        Label2.Font = New Font("맑은 고딕", 24F, FontStyle.Bold)
        Label2.ForeColor = Color.SeaShell
        Label2.Location = New Point(37, 95)
        Label2.Name = "Label2"
        Label2.Size = New Size(148, 45)
        Label2.TabIndex = 0
        Label2.Text = "포스번호"
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
        ' Button1
        ' 
        Button1.Location = New Point(894, 12)
        Button1.Name = "Button1"
        Button1.Size = New Size(111, 37)
        Button1.TabIndex = 3
        Button1.Text = "유니락 발권"
        Button1.UseVisualStyleBackColor = True
        Button1.Visible = False
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1064, 1061)
        Controls.Add(Button1)
        Controls.Add(pnlCSMain)
        Controls.Add(WebView21)
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "ArgosAPT Kiosk"
        WindowState = FormWindowState.Maximized
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
    Friend WithEvents txtTimer As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents btnExit As Button
    Friend WithEvents Button1 As Button

End Class
