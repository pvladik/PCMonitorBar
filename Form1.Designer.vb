<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        components = New ComponentModel.Container()
        label1 = New Label()
        Label2 = New Label()
        Label3 = New Label()
        Timer1 = New Timer(components)
        cboUnit1 = New ComboBox()
        cboUnit2 = New ComboBox()
        cboUnit3 = New ComboBox()
        txtSensorName1 = New TextBox()
        txtSensorName2 = New TextBox()
        txtSensorName3 = New TextBox()
        btnApplyConfig = New Button()
        btnSaveConfig = New Button()
        txtOutputLog = New TextBox()
        Label6 = New Label()
        cboComPort = New ComboBox()
        btnConnect = New Button()
        lblConnectionStatus = New Label()
        NotifyIcon1 = New NotifyIcon(components)
        SuspendLayout()
        ' 
        ' label1
        ' 
        label1.AutoSize = True
        label1.Location = New Point(12, 14)
        label1.Name = "label1"
        label1.Size = New Size(54, 15)
        label1.TabIndex = 0
        label1.Text = "Display 1"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(12, 43)
        Label2.Name = "Label2"
        Label2.Size = New Size(54, 15)
        Label2.TabIndex = 1
        Label2.Text = "Display 2"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(12, 72)
        Label3.Name = "Label3"
        Label3.Size = New Size(54, 15)
        Label3.TabIndex = 2
        Label3.Text = "Display 3"
        ' 
        ' Timer1
        ' 
        ' 
        ' cboUnit1
        ' 
        cboUnit1.AutoCompleteCustomSource.AddRange(New String() {"CPU", "GPU", "RAM", "Network", "Mainboard"})
        cboUnit1.FormattingEnabled = True
        cboUnit1.Items.AddRange(New Object() {"%", "°C", "MHz", "MB", "W ", "V ", "x", "B/s", "KB/s", "MB/s", "oC", "`` "})
        cboUnit1.Location = New Point(179, 6)
        cboUnit1.Name = "cboUnit1"
        cboUnit1.Size = New Size(121, 23)
        cboUnit1.TabIndex = 4
        cboUnit1.Text = "11"
        ' 
        ' cboUnit2
        ' 
        cboUnit2.AutoCompleteCustomSource.AddRange(New String() {"CPU", "GPU", "RAM", "Network", "Mainboard"})
        cboUnit2.FormattingEnabled = True
        cboUnit2.Items.AddRange(New Object() {"%", "°C", "MHz", "MB", "W ", "V ", "x", "B/s", "KB/s", "MB/s", "`` "})
        cboUnit2.Location = New Point(179, 35)
        cboUnit2.Name = "cboUnit2"
        cboUnit2.Size = New Size(121, 23)
        cboUnit2.TabIndex = 5
        cboUnit2.Text = "11"
        ' 
        ' cboUnit3
        ' 
        cboUnit3.AutoCompleteCustomSource.AddRange(New String() {"CPU", "GPU", "RAM", "Network", "Mainboard"})
        cboUnit3.FormattingEnabled = True
        cboUnit3.Items.AddRange(New Object() {"%", "°C", "MHz", "MB", "W ", "V ", "x", "B/s", "KB/s", "MB/s", "oC", "`` "})
        cboUnit3.Location = New Point(179, 64)
        cboUnit3.Name = "cboUnit3"
        cboUnit3.Size = New Size(121, 23)
        cboUnit3.TabIndex = 6
        cboUnit3.Text = "11"
        ' 
        ' txtSensorName1
        ' 
        txtSensorName1.Location = New Point(73, 6)
        txtSensorName1.Name = "txtSensorName1"
        txtSensorName1.Size = New Size(100, 23)
        txtSensorName1.TabIndex = 14
        ' 
        ' txtSensorName2
        ' 
        txtSensorName2.Location = New Point(73, 35)
        txtSensorName2.Name = "txtSensorName2"
        txtSensorName2.Size = New Size(100, 23)
        txtSensorName2.TabIndex = 15
        ' 
        ' txtSensorName3
        ' 
        txtSensorName3.Location = New Point(73, 64)
        txtSensorName3.Name = "txtSensorName3"
        txtSensorName3.Size = New Size(100, 23)
        txtSensorName3.TabIndex = 16
        ' 
        ' btnApplyConfig
        ' 
        btnApplyConfig.Location = New Point(330, 6)
        btnApplyConfig.Name = "btnApplyConfig"
        btnApplyConfig.Size = New Size(124, 44)
        btnApplyConfig.TabIndex = 20
        btnApplyConfig.Text = "Activate"
        btnApplyConfig.UseVisualStyleBackColor = True
        ' 
        ' btnSaveConfig
        ' 
        btnSaveConfig.Location = New Point(330, 56)
        btnSaveConfig.Name = "btnSaveConfig"
        btnSaveConfig.Size = New Size(124, 42)
        btnSaveConfig.TabIndex = 21
        btnSaveConfig.Text = "Save config"
        btnSaveConfig.UseVisualStyleBackColor = True
        ' 
        ' txtOutputLog
        ' 
        txtOutputLog.Location = New Point(12, 104)
        txtOutputLog.Multiline = True
        txtOutputLog.Name = "txtOutputLog"
        txtOutputLog.ScrollBars = ScrollBars.Vertical
        txtOutputLog.Size = New Size(442, 146)
        txtOutputLog.TabIndex = 22
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(12, 291)
        Label6.Name = "Label6"
        Label6.Size = New Size(63, 15)
        Label6.TabIndex = 23
        Label6.Text = "Serial Port:"
        ' 
        ' cboComPort
        ' 
        cboComPort.FormattingEnabled = True
        cboComPort.Location = New Point(84, 283)
        cboComPort.Name = "cboComPort"
        cboComPort.Size = New Size(121, 23)
        cboComPort.TabIndex = 24
        ' 
        ' btnConnect
        ' 
        btnConnect.Location = New Point(221, 287)
        btnConnect.Name = "btnConnect"
        btnConnect.Size = New Size(123, 23)
        btnConnect.TabIndex = 25
        btnConnect.Text = "Connect"
        btnConnect.UseVisualStyleBackColor = True
        ' 
        ' lblConnectionStatus
        ' 
        lblConnectionStatus.AutoSize = True
        lblConnectionStatus.Location = New Point(84, 309)
        lblConnectionStatus.Name = "lblConnectionStatus"
        lblConnectionStatus.Size = New Size(41, 15)
        lblConnectionStatus.TabIndex = 26
        lblConnectionStatus.Text = "Label4"
        ' 
        ' NotifyIcon1
        ' 
        NotifyIcon1.Text = "NotifyIcon1"
        NotifyIcon1.Visible = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(518, 348)
        Controls.Add(lblConnectionStatus)
        Controls.Add(btnConnect)
        Controls.Add(cboComPort)
        Controls.Add(Label6)
        Controls.Add(txtOutputLog)
        Controls.Add(btnSaveConfig)
        Controls.Add(btnApplyConfig)
        Controls.Add(txtSensorName3)
        Controls.Add(txtSensorName2)
        Controls.Add(txtSensorName1)
        Controls.Add(cboUnit3)
        Controls.Add(cboUnit2)
        Controls.Add(cboUnit1)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(label1)
        Name = "Form1"
        Text = "CPU bar"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents cboUnit1 As ComboBox
    Friend WithEvents cboUnit2 As ComboBox
    Friend WithEvents cboUnit3 As ComboBox
    Friend WithEvents txtSensorName1 As TextBox
    Friend WithEvents txtSensorName2 As TextBox
    Friend WithEvents txtSensorName3 As TextBox
    Friend WithEvents btnApplyConfig As Button
    Friend WithEvents btnSaveConfig As Button
    Friend WithEvents txtOutputLog As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents cboComPort As ComboBox
    Friend WithEvents btnConnect As Button
    Friend WithEvents lblConnectionStatus As Label
    Friend WithEvents NotifyIcon1 As NotifyIcon

End Class
