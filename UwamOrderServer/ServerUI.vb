Imports System.IO
Imports System.ComponentModel
Imports System.Timers

Public Class ServerUI

    Public Event TabulateSentOrders(Sender As tcpControl, SampleOrder As String)

    Public Event VisualProgress(sender As tcpControl, Progress As Integer)

    Public Event NoOrdersTransmitted(Sender As tcpControl, ByVal NoSampleOrdersTransmitted As Integer, ByVal TotalSampleOrderFrames As Integer)

    Dim ServerStatus As String
    Dim lastDetectedNoCnx As Integer

    Private Const DELIMITER As String = ":"
    Dim IPnPort(1) As String

    Dim ActualSampleNoCount As Integer   'ACTUAL LENGTH OF SAMPLE NUMBERS IN THE ORDERS FILE WITHOUT INCLUDING EMPTIES.

    Dim OrderReadySampleNumbers(100000) As Integer
    Private WithEvents Server As tcpControl
    Dim ASTMProcessor As New ASTM.uwamComSequences

    Dim DisplaySeq As Integer = 1

    Private Delegate Sub UpdateGridViewonACK(dgv As DataGridView, row As DataGridViewRow)

    Private Delegate Sub CnxNoMonitor(ssCnxNoDisplay As Label, CnxNo As String)

    Private Delegate Sub UpdateVisualProgress(pbar As ProgressBar, ProgressValue As Decimal)

    Private Delegate Sub NoSamplesSent(lbl As Label, NoTransmitted As Integer, TotalNumber As Integer)

    'STORE GENERATED ASTM FRAMES IN A GENERIC LIST
    ReadOnly SampleOrderFrames As New List(Of ASTM.astmFramesCurrentOrder)

    Dim LastFrameTransmitted As Integer = 0

    'TIMER TO INITIATE TRANSMIT OF NEXT SAMPLE IN THE GENERIC LIST HOLDING ASTM FRAMES.
    Dim WithEvents InititiateNewSampleTransmitTimer As New Timers.Timer

    'DEFAULT STATE SINCE <ACK> FROM HOST WILL INITIATE TRANSMIT. BUT AFTER TRANSMIT OF <EOT>, HOST WILL NOT REPLY BECK WITH <ACK>, SO THIS WILL BE SET TO
    'ReasonForTransmit.InitiateTransmitNextSample.  InstantiateNewSampleTransmitTimer WILL CALL Private sub TransmitFrame(Reason As ReasonForTransmit) IN THIS CASE
    'WHICH WILL START TRANSMIT OF THE NEXT SAMPLE IN THE LIST.
    Dim StartTransmit As ReasonForTransmit = ReasonForTransmit.DoNotInitiateTransmitNextSample

    Dim CountNAK As Integer = 0

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        InititiateNewSampleTransmitTimer.Interval = 10
        InititiateNewSampleTransmitTimer.Enabled = True
        Erase OrderReadySampleNumbers

        'VISUAL STATUS OF SERVER
        PictureBoxServerOffline.Show()
        PictureBoxNotConected.Hide()
        PictureBoxConnected.Hide()

        'SETTING SOME FALLBACK VALUES FOR LABELS
        lblServerIP.Text = "Server IP: "
        lblServerPort.Text = "Server Port: "
        lblStatus.Text = "Server Status"
        lblNoClients.Text = "Number of Connections: "

    End Sub

    Enum ReasonForTransmit
        NewFrame
        OnACK
        InitiateTransmitNextSample
        DoNotInitiateTransmitNextSample
        onNAK
        OnEOT
    End Enum

    Private Sub UpdateGridView(dgv As DataGridView, row As DataGridViewRow)
        If dgv.InvokeRequired = True Then
            dgv.Invoke(New UpdateGridViewonACK(AddressOf UpdateGridView), New Object() {dgv, row})
        Else
            dgv.Rows.Add(row)
        End If
    End Sub

    Private Sub UpdateCnxInfo(ssCnxNoDisplay As Label, CnxNo As String)
        If ssCnxNoDisplay.InvokeRequired Then
            lblServerIP.Invoke(New CnxNoMonitor(AddressOf UpdateCnxInfo), New Object() {ssCnxNoDisplay, IPnPort(0)})
            lblServerPort.Invoke(New CnxNoMonitor(AddressOf UpdateCnxInfo), New Object() {ssCnxNoDisplay, IPnPort(1)})
            lblStatus.Invoke(New CnxNoMonitor(AddressOf UpdateCnxInfo), New Object() {ssCnxNoDisplay, ServerStatus})
            lblNoClients.Invoke(New CnxNoMonitor(AddressOf UpdateCnxInfo), New Object() {ssCnxNoDisplay, CnxNo})
        Else
            lblServerIP.Text = "Server IP: " & IPnPort(0)
            lblServerPort.Text = "Server Port: " & IPnPort(1)
            lblStatus.Text = "Server Status: " & ServerStatus

            lblNoClients.Text = "Number of Connections: " & CnxNo

            'SHOWING COLOUR IMAGES DEPENDING ON SERVER STATUS
            If IPnPort(0) = "" Then
                PictureBoxServerOffline.Show()
                PictureBoxNotConected.Hide()
                PictureBoxConnected.Hide()
            ElseIf ServerStatus = "Listening" Then
                PictureBoxNotConected.Show()
                PictureBoxServerOffline.Hide()
                PictureBoxConnected.Hide()

            ElseIf ServerStatus = "Connected, Listening" Then
                PictureBoxNotConected.Hide()
                PictureBoxServerOffline.Hide()
                PictureBoxConnected.Show()
            End If
        End If
    End Sub

    Private Sub UpdateVProgress(pbar As ProgressBar, ProgressValue As Decimal)
        If pbar.InvokeRequired Then
            pbar.Invoke(New UpdateVisualProgress(AddressOf UpdateVProgress), New Object() {pbar, ProgressValue})
        Else
            pbar.Value = ProgressValue
            pbar.Update()
            pbar.Refresh()
            pbar.Invalidate()
        End If
    End Sub

    Private Sub NumberSamplesSent(lbl As Label, NoTransmitted As Integer, TotalNumber As Integer)
        If lbl.InvokeRequired Then
            lbl.Invoke(New NoSamplesSent(AddressOf NumberSamplesSent), New Object() {lbl, NoTransmitted, TotalNumber})
        Else
            lbl.Text = String.Format("Orders Sent: {0} of {1}", NoTransmitted, TotalNumber / 7)
        End If
    End Sub

    ''' <summary>
    ''' Initialize form on Load. Sets lblProgress.Text & lblNoClients.Text values to their defaults.
    ''' </summary>

    Private Sub ServerUI_Load(sender As Object, e As EventArgs) Handles Me.Load

        lblProgress.Text = String.Format("No. Analysis orders: {0}", 0)
        lblNoClients.Text = "Number of Connections: 0"
    End Sub

    ''' <summary>
    ''' Called to maintain an incremental count of valid sample numbers indicating total number of sample number read from Orders.text file
    ''' </summary>
    ''' <param name="IsSample">Variable specifying whether the string read was a valid sample number.</param>
    Public Sub MaintainActualRecordCount(IsSample As Boolean)

        Select Case IsSample
            Case True
                ActualSampleNoCount = ActualSampleNoCount + 1
            Case False
                If Not ActualSampleNoCount = 0 Then
                    ActualSampleNoCount = ActualSampleNoCount - 1
                End If
        End Select

    End Sub

    ''' <summary>
    ''' Reads episode numbers from disk, validates them, calls MaintainActualRecordCount to maintain actual sample count.
    ''' Adds sample Numbers to OrderReadySampleNumbers Array to be used to generate ASTM orders records
    ''' </summary>
    Private Sub BtnImportOrdes_Click(sender As Object, e As EventArgs) Handles BtnImportOrders.Click
        'ARRAY TO STORE ALL EPISODE NUMBERS FROM TEXT FILE.
        Dim EpisodeNo() As String = Nothing

        Try
            'OPEN DIALOG AND STORES ALL EPIDSODE NUMBERS IN ARRAY
            If OpenOrdersFile.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                EpisodeNo = File.ReadAllLines(OpenOrdersFile.FileName)
            ElseIf DialogResult.Cancel Then
                Exit Sub
            End If
        Catch ex As Exception
            PopulateLog(ex.Message)
        End Try

        'ARRAY TO STORE PROCESSED, VALIDATED EPISODE NUMBERS.
        'THE LENGTH OF THIS ARRAY IS LONGER THAN THE ACTUAL U R/E SAMPLES
        Dim SampleNumber(EpisodeNo.Length) As Integer

        Dim counter As Integer = 0          ' FOR THE LOOP
        ActualSampleNoCount = 0
        'TRYING TO VALIDATE EPISODE NUMBERS AND ASSIGN THEM TO AN INTERGER ARRAY FOR SENDING ORDERS.
        If Not EpisodeNo.Length = 0 Then        'ELIMINATING POSIBILITY OF BLANK ORDERS FILES READ FROM DISK

            For Each episode As String In EpisodeNo       'ELIMINATING POSSIBILITY OF BLANK LINES BEING PROCESSED.

                'GET RID OF TRAILING SPACES ON BOTH SIDE OF THE NUMBER.
                Dim episodeTrimmed As String = Trim(episode)
                Dim CheckIntStatus As Integer = 0

                Select Case episodeTrimmed.Length
                    Case 9        'CASE LENGTH 9: CU1234567
                        'EXTRACT THE INTEGER CASE 8: FROM CU123456
                        SampleNumber(ActualSampleNoCount) = Strings.Mid(episodeTrimmed, 3, 7)
                        MaintainActualRecordCount(True)
                    Case 11       'CASE LENGTH 11A: 1-CU1234567 | CASE 11B: CU253253-G

                        'CHECK WHETHER THE FIRST CHARACTER IN THE episodeTrimmed NUMBER IS A STRING OR INTEGER.
                        Dim HandlingCase10 As String = Strings.Left(episodeTrimmed, 1)
                        If Integer.TryParse(HandlingCase10, CheckIntStatus) Then
                            ' IF PASSED: episodeTrimmed NUMBER FORMAT: 1-CU123456
                            'GETTING THE INTEFER: 123456
                            SampleNumber(ActualSampleNoCount) = Strings.Mid(episodeTrimmed, 5, 7)
                            MaintainActualRecordCount(True)
                        Else
                            'LOGGING CASE 10B: CU1234567-G, VARIANT OUT TO SERVER LOG GRIDVIEW
                            PopulateLog(String.Format("Episode number: {0}, No order for Urine R/E", episodeTrimmed))
                        End If

                    Case Else       'CASE LENGTH >10: 1-CU1234567-P
                        PopulateLog(String.Format("Episode number: {0}, No order for Urine R/E", episodeTrimmed))
                End Select
                counter += 1

            Next

        End If

        PopulateLog(String.Format("Number Ep Nos: {0}{1}Urine R/E sample Count: {2}{1} No. Sample Numbers: {3}", EpisodeNo.Length, vbCrLf, ActualSampleNoCount, SampleNumber.Length - 1))

        'CLEAR PUBLIC ARRAY FOR SAMPLE NUMBERS.
        Erase OrderReadySampleNumbers
        Dim ReturnSampleNumbers As Integer = 0
        '
        Array.Resize(OrderReadySampleNumbers, ActualSampleNoCount)

        'ADING SAMPLE NUMBERS TO PUBLIC ARRAY
        For Each num In SampleNumber
            If Not num = 0 Then
                OrderReadySampleNumbers(ReturnSampleNumbers) = num
            End If
            ReturnSampleNumbers += 1
        Next
        'DISPLAY THE NUMBER OF SAMPLE NUMBERS IN ARRAY
        lblProgress.Text = String.Format("No. Analysis orders: {0}", ActualSampleNoCount)
    End Sub

    ''' <summary>
    ''' Updates and sorts with the dataField provided by using the Delegate UpdateGridView, gets DisplaySeq from variable and DateTime from system
    ''' DisplaySeq is the total number of rows in the DataGridView and is used to sort the UpdateGridView
    ''' </summary>
    ''' <param name="dataField">Information displayed in DataGridView, could be an error message, sent frames, received frames, etc...</param>
    Public Sub PopulateLog(ByVal dataField As String)
        Dim Row As New DataGridViewRow

        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = DisplaySeq})
        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = Date.Now().ToString})
        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = dataField.ToString})
        UpdateGridView(DataGridView1, Row)

        DisplaySeq += 1

        DataGridView1.Sort(ColSerial, ListSortDirection.Descending)
    End Sub

    Private Sub btnStartServer_Click(sender As Object, e As EventArgs) Handles btnStartServer.Click
        Try
            Server = New tcpControl(My.Settings.ServerIP, My.Settings.ServerPort)

            'Handlers for custom Events Raised by tcpControl.
            btnStartServer.Enabled = False
        Catch ex As Exception
            PopulateLog(ex.Message & sender.ToString)
        End Try
    End Sub

    Sub onACK(sender As tcpControl, receivedMsg As String) Handles Server.onACK
        ' MsgBox("[ACK]")
        Dim Row As New DataGridViewRow

        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = DisplaySeq})
        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = Date.Now().ToString})
        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = receivedMsg.ToString})
        DisplaySeq += 1
        UpdateGridView(DataGridView1, Row)

        'Send Next frame
        TransmitFrame(ReasonForTransmit.OnACK, "WAM.ACK")
    End Sub

    Private Sub DataGridView1_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles DataGridView1.RowsAdded
        Try
            DataGridView1.Sort(ColSerial, ListSortDirection.Descending)
        Catch ex As Exception
            PopulateLog(ex.Message)
            PopulateLog(ex.StackTrace)
            Exit Sub
        End Try

    End Sub

    Sub OnNAK(sender As tcpControl, receivedMsg As String) Handles Server.onNAK
        Dim Row As New DataGridViewRow

        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = DisplaySeq})
        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = Date.Now().ToString})
        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = receivedMsg.ToString})
        DisplaySeq += 1
        UpdateGridView(DataGridView1, Row)
        TransmitFrame(ReasonForTransmit.onNAK, "WAM.NAK")
    End Sub

    Sub OnENQ(sender As tcpControl, receivedMsg As String) Handles Server.onENQ
        ' MsgBox("[ENQ]")

        Dim Row As New DataGridViewRow

        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = DisplaySeq})
        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = Date.Now().ToString})
        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = receivedMsg.ToString})
        DisplaySeq += 1
        UpdateGridView(DataGridView1, Row)

    End Sub

    Sub OnMessage(sender As tcpControl, receivedMsg As String) Handles Server.onASTMMessage
        ' MsgBox("[ENQ]")

        Dim Row As New DataGridViewRow

        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = DisplaySeq})
        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = Date.Now().ToString})
        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = receivedMsg.ToString})
        DisplaySeq += 1
        UpdateGridView(DataGridView1, Row)
    End Sub

    Sub PopulateLog(sender As tcpControl, LogItem As String) Handles Server.PopulateLog, Me.TabulateSentOrders
        ' MsgBox ("PopulateLog raised")
        Dim Row As New DataGridViewRow

        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = DisplaySeq})
        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = Date.Now().ToString})
        Row.Cells.Add(New DataGridViewTextBoxCell With {.Value = LogItem.ToString})
        DisplaySeq += 1
        UpdateGridView(DataGridView1, Row)
    End Sub

    ''' <summary>
    ''' Generates ASTM messages with ASTMProcessor.GenMessageASTM and calls TransmitFrame to send orders to analyser.
    ''' </summary>
    Private Sub btnSendOrders_Click(sender As Object, e As EventArgs) Handles btnSendOrders.Click
        'CHECKING FOR PRESENCE OF NEWLY IMPORTED SAMPLE NUMBERS FOR WHICH ORDERS NEEDS TO BE SENT
        If OrderReadySampleNumbers IsNot Nothing Then
            'VERIFIYING THAT ANALYSER IS CONNECTED.
            If lastDetectedNoCnx > 0 Then
                'DISABLE THE BUTTON TO AVOID MULIPLE CLICKS
                btnSendOrders.Enabled = False

                'CLEAR SAMPLE ORDER CACHE FROM PREVIOUS IMPORTS, IF ANY.
                'RESET THE COUNTER COUNTING FRAMES SENT TO ANALYSER.
                'RESET SOME USER DISPLAYED VALUES
                SampleOrderFrames.Clear()
                LastFrameTransmitted = 0
                ActualSampleNoCount = 0

                DataGridView1.SuspendLayout()
                Dim msgCounter As Integer = 1
                Dim ASTM_MESSAGES(6) As String     'ARRAY TO HOLD THE COMPLETE ASTM MESSAGE FROM <ENQ> TO <EOT> FOR CHEMISTRY AND DEPOSIT.
                For Each samplenumber In OrderReadySampleNumbers
                    ASTM_MESSAGES = ASTMProcessor.GenMessageASTM(samplenumber)      'GENERATE CHEMISTRY AND DEPOSIT ORDER FRAMES BY PROVIDING SAMPLE NUMBER. ASSIGN THEM TO ASTM_MESSAGES ARRAY

                    'ITERATING THROUGH ASTM_MESSAGES ARRAY AND ADDING THE FRAMES TO A GENERIC LIST SO THAT ASTM_MESSAGES ARRAY IS AVAILABLE TO STORE A NEW SET OF FRAMES FOR THE NEXT SAMPLE NUMBER.
                    For Each astmmessage In ASTM_MESSAGES
                        'server.SendASTMFrame(astmmessage)
                        SampleOrderFrames.Add(New ASTM.astmFramesCurrentOrder(astmmessage))
                    Next

                    msgCounter += 1
                Next
                DataGridView1.ResumeLayout()
                Erase OrderReadySampleNumbers     'OrderReadySampleNumbers ARRAY BEING ERASED AFTER GENERATING ASTM MESSAGES FOR THE SAMPLES.

                'ENABLE THE BUTTON
                TransmitFrame(ReasonForTransmit.NewFrame, "ORD.NEW")    'CALLING THE FUNCTION TO TRANSMIT THE GENERATED ASTM FRAMES TO ANALYSER.
                btnSendOrders.Enabled = True
            Else
                PopulateLog("Order server is not connected to analyzer.")
            End If
        Else
            PopulateLog("No pending orders. Please import new orders first!")
        End If
    End Sub

    Private Sub TransmitFrame(Reason As ReasonForTransmit, RFDby As String)
        If Not LastFrameTransmitted = SampleOrderFrames.Count Then

            RaiseEvent NoOrdersTransmitted(Server, LastFrameTransmitted, SampleOrderFrames.Count)
            Select Case Reason
                Case ReasonForTransmit.NewFrame
                    Server.SendASTMFrame(SampleOrderFrames.Item(LastFrameTransmitted)._Frame, LastFrameTransmitted, "NEW", RFDby)
                    RaiseEvent TabulateSentOrders(Server, SampleOrderFrames.Item(LastFrameTransmitted)._Frame)
                    LastFrameTransmitted = LastFrameTransmitted + 1

                    'UPDATE THE PROGRESS BAR
                    RaiseEvent NoOrdersTransmitted(Server, LastFrameTransmitted, SampleOrderFrames.Count)

                Case ReasonForTransmit.OnACK, ReasonForTransmit.InitiateTransmitNextSample

                    If CountNAK > 0 Then
                        StartTransmit = ReasonForTransmit.InitiateTransmitNextSample
                        CountNAK = 0
                        Exit Sub
                    End If

                    If SampleOrderFrames.Item(LastFrameTransmitted)._Frame = "<EOT>" Then
                        Server.SendASTMFrame(SampleOrderFrames.Item(LastFrameTransmitted)._Frame, LastFrameTransmitted, "ACK", RFDby)
                        LastFrameTransmitted = LastFrameTransmitted + 1
                        RaiseEvent NoOrdersTransmitted(Server, LastFrameTransmitted, SampleOrderFrames.Count)
                        RaiseEvent TabulateSentOrders(Server, SampleOrderFrames.Item(LastFrameTransmitted - 1)._Frame)
                        StartTransmit = ReasonForTransmit.InitiateTransmitNextSample
                        Exit Sub
                    Else
                        Server.SendASTMFrame(SampleOrderFrames.Item(LastFrameTransmitted)._Frame, LastFrameTransmitted, "ACK", RFDby)
                        LastFrameTransmitted = LastFrameTransmitted + 1
                        RaiseEvent TabulateSentOrders(Server, SampleOrderFrames.Item(LastFrameTransmitted - 1)._Frame)
                        RaiseEvent NoOrdersTransmitted(Server, LastFrameTransmitted, SampleOrderFrames.Count)

                    End If

                Case ReasonForTransmit.onNAK
                    If CountNAK < 5 Then
                        Server.SendASTMFrame(SampleOrderFrames.Item(LastFrameTransmitted - 1)._Frame, LastFrameTransmitted, "NAK", RFDby)
                        RaiseEvent TabulateSentOrders(Server, SampleOrderFrames.Item(LastFrameTransmitted - 1)._Frame & CountNAK.ToString)
                        CountNAK = CountNAK + 1
                        RaiseEvent NoOrdersTransmitted(Server, LastFrameTransmitted, SampleOrderFrames.Count)
                    Else
                        Server.SendASTMFrame("<EOT>", LastFrameTransmitted, "NA1", RFDby)
                        'TODO: CREATE A SAVE POINT OF SOME SORT SO THAT FAILED ORDER FOR THE SAMPLE NUMBER WILL START TRANSMISSION FROM <ENQ>
                        RaiseEvent TabulateSentOrders(Server, "<EOT>")
                        CountNAK = 0
                        RaiseEvent NoOrdersTransmitted(Server, LastFrameTransmitted, SampleOrderFrames.Count)

                        Exit Sub
                    End If

                    'UPDATE THE PROGRESS BAR
                    RaiseEvent NoOrdersTransmitted(Server, LastFrameTransmitted, SampleOrderFrames.Count)

                Case ReasonForTransmit.OnEOT

            End Select
        Else
            LastFrameTransmitted = 0
            SampleOrderFrames.Clear()
            Exit Sub
        End If
    End Sub

    Private Sub InititiateNewSampleTransmitTimer_Elapsed(sender As Object, e As ElapsedEventArgs) Handles InititiateNewSampleTransmitTimer.Elapsed
        If StartTransmit = ReasonForTransmit.InitiateTransmitNextSample Then
            StartTransmit = ReasonForTransmit.DoNotInitiateTransmitNextSample
            TransmitFrame(ReasonForTransmit.InitiateTransmitNextSample, "TIM.NXT")
        End If
    End Sub

    Private Sub UpdateProgressBar(sender As tcpControl, Progress As Decimal) Handles Me.VisualProgress
        UpdateVProgress(ProgressBarVisualProgress, Progress)
    End Sub

    Sub MonitoringActiveCnx(Sender As tcpControl, localEndPoint As String, _ServerStatus As String, CnxNo As Integer) Handles Server.CnxMonitor
        ServerStatus = _ServerStatus
        IPnPort = localEndPoint.Split(DELIMITER)
        lastDetectedNoCnx = CnxNo

        UpdateCnxInfo(lblServerIP, "Server IP: " & IPnPort(0))
        UpdateCnxInfo(lblServerPort, "Server Port: " & IPnPort(1))
        UpdateCnxInfo(lblStatus, "Server Status: " & ServerStatus)
        UpdateCnxInfo(lblNoClients, CnxNo)

    End Sub

    Sub NoSamplesTransmitted(Sender As tcpControl, ByVal LastFrame As Integer, ByVal TotalFrames As Integer) Handles Me.NoOrdersTransmitted

        'COUNT NUMBER OF SAMPLES ORDERS SENT.
        Dim NumberSampleOrdersTransmitted As Integer = LastFrame \ 7
        NumberSamplesSent(lblProgress, NumberSampleOrdersTransmitted, TotalFrames)
        RaiseEvent VisualProgress(Server, (LastFrame / TotalFrames) * 100)

    End Sub

    Private Sub EditServerConfig_Click(sender As Object, e As EventArgs) Handles EditServerConfig.Click
        'change ServerIP and ServerPort from settings file and reload settings
        Dim ChangeConfiguration As New ServerConfiguration
        ChangeConfiguration.Show()
    End Sub

End Class