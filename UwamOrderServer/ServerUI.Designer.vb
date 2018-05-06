<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ServerUI
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ServerUI))
        Me.btnSendOrders = New System.Windows.Forms.Button()
        Me.BtnImportOrders = New System.Windows.Forms.Button()
        Me.OpenOrdersFile = New System.Windows.Forms.OpenFileDialog()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.ColSerial = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDateTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colLog = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.gbServerLogs = New System.Windows.Forms.GroupBox()
        Me.btnStartServer = New System.Windows.Forms.Button()
        Me.GroupBoxServerStatus = New System.Windows.Forms.GroupBox()
        Me.EditServerConfig = New System.Windows.Forms.PictureBox()
        Me.lblNoClients = New System.Windows.Forms.Label()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblServerPort = New System.Windows.Forms.Label()
        Me.lblServerIP = New System.Windows.Forms.Label()
        Me.ProgressBarVisualProgress = New System.Windows.Forms.ProgressBar()
        Me.PictureBoxNotConected = New System.Windows.Forms.PictureBox()
        Me.PictureBoxConnected = New System.Windows.Forms.PictureBox()
        Me.PictureBoxServerOffline = New System.Windows.Forms.PictureBox()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbServerLogs.SuspendLayout()
        Me.GroupBoxServerStatus.SuspendLayout()
        CType(Me.EditServerConfig, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxNotConected, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxConnected, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxServerOffline, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnSendOrders
        '
        Me.btnSendOrders.Location = New System.Drawing.Point(187, 63)
        Me.btnSendOrders.Name = "btnSendOrders"
        Me.btnSendOrders.Size = New System.Drawing.Size(87, 24)
        Me.btnSendOrders.TabIndex = 0
        Me.btnSendOrders.Text = "Send &Orders"
        Me.btnSendOrders.UseVisualStyleBackColor = True
        '
        'BtnImportOrders
        '
        Me.BtnImportOrders.Location = New System.Drawing.Point(94, 63)
        Me.BtnImportOrders.Name = "BtnImportOrders"
        Me.BtnImportOrders.Size = New System.Drawing.Size(87, 24)
        Me.BtnImportOrders.TabIndex = 2
        Me.BtnImportOrders.Text = "Import"
        Me.BtnImportOrders.UseVisualStyleBackColor = True
        '
        'OpenOrdersFile
        '
        Me.OpenOrdersFile.FileName = "Orders.txt"
        Me.OpenOrdersFile.InitialDirectory = "D:\"
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(5, 93)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(65, 14)
        Me.lblProgress.TabIndex = 4
        Me.lblProgress.Text = "lblProgress"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToOrderColumns = True
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ColSerial, Me.colDateTime, Me.colLog})
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(3, 18)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(649, 162)
        Me.DataGridView1.TabIndex = 5
        '
        'ColSerial
        '
        Me.ColSerial.HeaderText = "#"
        Me.ColSerial.Name = "ColSerial"
        Me.ColSerial.ReadOnly = True
        Me.ColSerial.Width = 50
        '
        'colDateTime
        '
        Me.colDateTime.HeaderText = "Date"
        Me.colDateTime.Name = "colDateTime"
        Me.colDateTime.ReadOnly = True
        Me.colDateTime.Width = 130
        '
        'colLog
        '
        Me.colLog.HeaderText = "Log"
        Me.colLog.Name = "colLog"
        Me.colLog.ReadOnly = True
        Me.colLog.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colLog.Width = 400
        '
        'gbServerLogs
        '
        Me.gbServerLogs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbServerLogs.Controls.Add(Me.DataGridView1)
        Me.gbServerLogs.Location = New System.Drawing.Point(0, 140)
        Me.gbServerLogs.Name = "gbServerLogs"
        Me.gbServerLogs.Size = New System.Drawing.Size(655, 183)
        Me.gbServerLogs.TabIndex = 6
        Me.gbServerLogs.TabStop = False
        Me.gbServerLogs.Text = "Server Logs"
        '
        'btnStartServer
        '
        Me.btnStartServer.Location = New System.Drawing.Point(94, 35)
        Me.btnStartServer.Name = "btnStartServer"
        Me.btnStartServer.Size = New System.Drawing.Size(87, 24)
        Me.btnStartServer.TabIndex = 8
        Me.btnStartServer.Text = "Start Server"
        Me.btnStartServer.UseVisualStyleBackColor = True
        '
        'GroupBoxServerStatus
        '
        Me.GroupBoxServerStatus.Controls.Add(Me.EditServerConfig)
        Me.GroupBoxServerStatus.Controls.Add(Me.lblNoClients)
        Me.GroupBoxServerStatus.Controls.Add(Me.lblStatus)
        Me.GroupBoxServerStatus.Controls.Add(Me.lblServerPort)
        Me.GroupBoxServerStatus.Controls.Add(Me.lblServerIP)
        Me.GroupBoxServerStatus.Location = New System.Drawing.Point(287, 35)
        Me.GroupBoxServerStatus.Name = "GroupBoxServerStatus"
        Me.GroupBoxServerStatus.Size = New System.Drawing.Size(302, 99)
        Me.GroupBoxServerStatus.TabIndex = 10
        Me.GroupBoxServerStatus.TabStop = False
        Me.GroupBoxServerStatus.Text = "Server Status"
        '
        'EditServerConfig
        '
        Me.EditServerConfig.BackColor = System.Drawing.SystemColors.Control
        Me.EditServerConfig.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.EditServerConfig.Image = CType(resources.GetObject("EditServerConfig.Image"), System.Drawing.Image)
        Me.EditServerConfig.Location = New System.Drawing.Point(248, 13)
        Me.EditServerConfig.Name = "EditServerConfig"
        Me.EditServerConfig.Size = New System.Drawing.Size(48, 48)
        Me.EditServerConfig.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.EditServerConfig.TabIndex = 15
        Me.EditServerConfig.TabStop = False
        '
        'lblNoClients
        '
        Me.lblNoClients.AutoSize = True
        Me.lblNoClients.Location = New System.Drawing.Point(6, 81)
        Me.lblNoClients.Name = "lblNoClients"
        Me.lblNoClients.Size = New System.Drawing.Size(69, 14)
        Me.lblNoClients.TabIndex = 3
        Me.lblNoClients.Text = "lblNoClients"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(6, 55)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(52, 14)
        Me.lblStatus.TabIndex = 2
        Me.lblStatus.Text = "lblStatus"
        '
        'lblServerPort
        '
        Me.lblServerPort.AutoSize = True
        Me.lblServerPort.Location = New System.Drawing.Point(6, 41)
        Me.lblServerPort.Name = "lblServerPort"
        Me.lblServerPort.Size = New System.Drawing.Size(76, 14)
        Me.lblServerPort.TabIndex = 1
        Me.lblServerPort.Text = "lblServerPort"
        '
        'lblServerIP
        '
        Me.lblServerIP.AutoSize = True
        Me.lblServerIP.Location = New System.Drawing.Point(6, 27)
        Me.lblServerIP.Name = "lblServerIP"
        Me.lblServerIP.Size = New System.Drawing.Size(65, 14)
        Me.lblServerIP.TabIndex = 0
        Me.lblServerIP.Text = "lblServerIP"
        '
        'ProgressBarVisualProgress
        '
        Me.ProgressBarVisualProgress.Location = New System.Drawing.Point(8, 110)
        Me.ProgressBarVisualProgress.Name = "ProgressBarVisualProgress"
        Me.ProgressBarVisualProgress.Size = New System.Drawing.Size(273, 24)
        Me.ProgressBarVisualProgress.TabIndex = 11
        '
        'PictureBoxNotConected
        '
        Me.PictureBoxNotConected.Image = CType(resources.GetObject("PictureBoxNotConected.Image"), System.Drawing.Image)
        Me.PictureBoxNotConected.Location = New System.Drawing.Point(8, 7)
        Me.PictureBoxNotConected.Name = "PictureBoxNotConected"
        Me.PictureBoxNotConected.Size = New System.Drawing.Size(80, 80)
        Me.PictureBoxNotConected.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBoxNotConected.TabIndex = 12
        Me.PictureBoxNotConected.TabStop = False
        '
        'PictureBoxConnected
        '
        Me.PictureBoxConnected.Image = CType(resources.GetObject("PictureBoxConnected.Image"), System.Drawing.Image)
        Me.PictureBoxConnected.Location = New System.Drawing.Point(8, 7)
        Me.PictureBoxConnected.Name = "PictureBoxConnected"
        Me.PictureBoxConnected.Size = New System.Drawing.Size(80, 80)
        Me.PictureBoxConnected.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBoxConnected.TabIndex = 13
        Me.PictureBoxConnected.TabStop = False
        '
        'PictureBoxServerOffline
        '
        Me.PictureBoxServerOffline.Image = CType(resources.GetObject("PictureBoxServerOffline.Image"), System.Drawing.Image)
        Me.PictureBoxServerOffline.Location = New System.Drawing.Point(8, 7)
        Me.PictureBoxServerOffline.Name = "PictureBoxServerOffline"
        Me.PictureBoxServerOffline.Size = New System.Drawing.Size(80, 80)
        Me.PictureBoxServerOffline.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBoxServerOffline.TabIndex = 14
        Me.PictureBoxServerOffline.TabStop = False
        '
        'ServerUI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(657, 323)
        Me.Controls.Add(Me.PictureBoxServerOffline)
        Me.Controls.Add(Me.PictureBoxConnected)
        Me.Controls.Add(Me.PictureBoxNotConected)
        Me.Controls.Add(Me.ProgressBarVisualProgress)
        Me.Controls.Add(Me.GroupBoxServerStatus)
        Me.Controls.Add(Me.btnStartServer)
        Me.Controls.Add(Me.gbServerLogs)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.BtnImportOrders)
        Me.Controls.Add(Me.btnSendOrders)
        Me.Font = New System.Drawing.Font("Cambria", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "ServerUI"
        Me.Text = "UWAM Analysis Server"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbServerLogs.ResumeLayout(False)
        Me.GroupBoxServerStatus.ResumeLayout(False)
        Me.GroupBoxServerStatus.PerformLayout()
        CType(Me.EditServerConfig, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxNotConected, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxConnected, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxServerOffline, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnSendOrders As Button
    Friend WithEvents BtnImportOrders As Button
    Friend WithEvents OpenOrdersFile As OpenFileDialog
    Friend WithEvents lblProgress As Label
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents gbServerLogs As GroupBox
    Private WithEvents btnStartServer As Button
    Friend WithEvents GroupBoxServerStatus As GroupBox
    Friend WithEvents lblNoClients As Label
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblServerPort As Label
    Friend WithEvents lblServerIP As Label
    Friend WithEvents ProgressBarVisualProgress As ProgressBar
    Friend WithEvents PictureBoxNotConected As PictureBox
    Friend WithEvents PictureBoxConnected As PictureBox
    Friend WithEvents PictureBoxServerOffline As PictureBox
    Friend WithEvents ColSerial As DataGridViewTextBoxColumn
    Friend WithEvents colDateTime As DataGridViewTextBoxColumn
    Friend WithEvents colLog As DataGridViewTextBoxColumn
    Friend WithEvents EditServerConfig As PictureBox
End Class
