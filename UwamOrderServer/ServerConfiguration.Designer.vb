<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ServerConfiguration
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ServerConfiguration))
        Me.EditServerConfigIcon = New System.Windows.Forms.PictureBox()
        Me.lblServerSettingsDescription = New System.Windows.Forms.Label()
        Me.lblIP = New System.Windows.Forms.Label()
        Me.lblPort = New System.Windows.Forms.Label()
        Me.TextBoxIPAddress = New System.Windows.Forms.TextBox()
        Me.TextBoxPort = New System.Windows.Forms.TextBox()
        Me.ButtonSave = New System.Windows.Forms.Button()
        CType(Me.EditServerConfigIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'EditServerConfigIcon
        '
        Me.EditServerConfigIcon.BackColor = System.Drawing.SystemColors.Control
        Me.EditServerConfigIcon.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.EditServerConfigIcon.Image = CType(resources.GetObject("EditServerConfigIcon.Image"), System.Drawing.Image)
        Me.EditServerConfigIcon.Location = New System.Drawing.Point(-1, 0)
        Me.EditServerConfigIcon.Name = "EditServerConfigIcon"
        Me.EditServerConfigIcon.Size = New System.Drawing.Size(72, 72)
        Me.EditServerConfigIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.EditServerConfigIcon.TabIndex = 16
        Me.EditServerConfigIcon.TabStop = False
        '
        'lblServerSettingsDescription
        '
        Me.lblServerSettingsDescription.AutoSize = True
        Me.lblServerSettingsDescription.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblServerSettingsDescription.Location = New System.Drawing.Point(74, 3)
        Me.lblServerSettingsDescription.Name = "lblServerSettingsDescription"
        Me.lblServerSettingsDescription.Size = New System.Drawing.Size(395, 15)
        Me.lblServerSettingsDescription.TabIndex = 17
        Me.lblServerSettingsDescription.Text = "Change server IP and Port and restart server with new settings on save."
        '
        'lblIP
        '
        Me.lblIP.AutoSize = True
        Me.lblIP.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIP.Location = New System.Drawing.Point(92, 26)
        Me.lblIP.Name = "lblIP"
        Me.lblIP.Size = New System.Drawing.Size(62, 15)
        Me.lblIP.TabIndex = 18
        Me.lblIP.Text = "Server IP: "
        '
        'lblPort
        '
        Me.lblPort.AutoSize = True
        Me.lblPort.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPort.Location = New System.Drawing.Point(80, 52)
        Me.lblPort.Name = "lblPort"
        Me.lblPort.Size = New System.Drawing.Size(74, 15)
        Me.lblPort.TabIndex = 19
        Me.lblPort.Text = "Server Port: "
        '
        'TextBoxIPAddress
        '
        Me.TextBoxIPAddress.Location = New System.Drawing.Point(160, 24)
        Me.TextBoxIPAddress.Name = "TextBoxIPAddress"
        Me.TextBoxIPAddress.Size = New System.Drawing.Size(198, 20)
        Me.TextBoxIPAddress.TabIndex = 20
        '
        'TextBoxPort
        '
        Me.TextBoxPort.Location = New System.Drawing.Point(160, 52)
        Me.TextBoxPort.Name = "TextBoxPort"
        Me.TextBoxPort.Size = New System.Drawing.Size(198, 20)
        Me.TextBoxPort.TabIndex = 21
        '
        'ButtonSave
        '
        Me.ButtonSave.Location = New System.Drawing.Point(394, 48)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(75, 23)
        Me.ButtonSave.TabIndex = 22
        Me.ButtonSave.Text = "&Save"
        Me.ButtonSave.UseVisualStyleBackColor = True
        '
        'ServerConfiguration
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(479, 79)
        Me.Controls.Add(Me.ButtonSave)
        Me.Controls.Add(Me.TextBoxPort)
        Me.Controls.Add(Me.TextBoxIPAddress)
        Me.Controls.Add(Me.lblPort)
        Me.Controls.Add(Me.lblIP)
        Me.Controls.Add(Me.lblServerSettingsDescription)
        Me.Controls.Add(Me.EditServerConfigIcon)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "ServerConfiguration"
        Me.Text = "Server Configuration"
        CType(Me.EditServerConfigIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents EditServerConfigIcon As PictureBox
    Friend WithEvents lblServerSettingsDescription As Label
    Friend WithEvents lblIP As Label
    Friend WithEvents lblPort As Label
    Friend WithEvents TextBoxIPAddress As TextBox
    Friend WithEvents TextBoxPort As TextBox
    Friend WithEvents ButtonSave As Button
End Class
