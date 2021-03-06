Imports System.Net
Imports System.Text
Imports System.Net.Sockets
Imports System.Threading
Imports UwamOrderServer.ASTM.astmConstants
Imports System.Timers

Public Class tcpControl

    Public Event onASTMMessage(sender As tcpControl, Data As String)

    Public Event onACK(sender As tcpControl, ReceivedMsg As String)

    Public Event onNAK(sender As tcpControl, ReceivedMsg As String)

    Public Event onENQ(sender As tcpControl, ReceivedMsg As String)

    Public Event onEOT(sender As tcpControl)

    'Public Event deltaClientCnxNo(sender As tcpControl, CnxNo As Integer)
    Public Event PopulateLog(Sender As tcpControl, LogItem As String)

    Public Event CnxMonitor(Sender As tcpControl, localEndPoint As String, ServerStatus As String, CnxNo As Integer)

    Public WithEvents ActiveCnxMonitorTimer As New Timers.Timer

    Dim FinalizeFrame As New ASTM.uwamComSequences
    Dim _server As TcpListener
    Dim _listOfClients As New List(Of TcpClient)

    Dim IsServerStarted As Boolean

    Public Sub New(ByVal localIP As String, ByVal ServerPort As Integer)
        StartServer(localIP, ServerPort)
        ActiveCnxMonitorTimer.Interval = 100
        ActiveCnxMonitorTimer.Enabled = True
    End Sub

    Function StartServer(ByVal localIP As String, ByVal ServerPort As Integer)
        Try
            _server = New TcpListener(IPAddress.Parse(localIP), ServerPort)
            _server.Start()

            MsgBox(String.Format("Server Started{0}Listening on {1}:{2}", vbCrLf, localIP, ServerPort))
            'RaiseEvent PopulateLog(Me, "Server Started")
            ThreadPool.QueueUserWorkItem(AddressOf NewClient)
            IsServerStarted = True
            Return 0
        Catch ex As Exception
            ' RaiseEvent PopulateLog(Me, "Server Started")
            MsgBox(ex.Message)
        End Try

    End Function

    Private Sub NewClient(state As Object)

        If _listOfClients.Count = 0 Then
            RaiseEvent PopulateLog(Me, "Listening on " & _server.LocalEndpoint.ToString)
        End If

        Dim client As TcpClient = _server.AcceptTcpClient()

        Try
            _listOfClients.Add(client)
            ThreadPool.QueueUserWorkItem(AddressOf NewClient)

            ActiveCnxMonitorTimer.Enabled = True

            ' Check to see if this NetworkStream is readable.

            While True
                Dim ns As NetworkStream = client.GetStream()

                If ns.CanRead Then
                    Dim myReadBuffer(1024) As Byte
                    Dim myCompleteMessage As StringBuilder = New StringBuilder()
                    Dim numberOfBytesRead As Integer = 0

                    ' Incoming message may be larger than the buffer size.
                    Do
                        numberOfBytesRead = ns.Read(myReadBuffer, 0, myReadBuffer.Length)
                        myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead))
                    Loop While ns.DataAvailable

                    'Get the message as string
                    Dim astmReply As String = myCompleteMessage.ToString()

                    If astmReply = ChrW(ACK) Then
                        RaiseEvent onACK(Me, "<ACK>")
                    ElseIf astmReply = ChrW(NAK) Then
                        RaiseEvent onNAK(Me, "<NAK>")
                    ElseIf astmReply = ChrW(ENQ) Then
                        RaiseEvent onENQ(Me, "<ENQ>")
                        SendASTMFrame("<ACK>", 0, "ENQ Received", "ASTM reply on ENQ")
                    ElseIf astmReply = ChrW(EOT) Then
                        RaiseEvent onEOT(Me)
                        MsgBox("EOT")
                    Else
                        'Sending an ACK on each message
                        SendASTMFrame("<ACK>", 0, "ENQ Received", "ASTM reply on ENQ")
                        RaiseEvent onASTMMessage(Me, astmReply)
                    End If

                End If

            End While
        Catch ex As Exception
            If _listOfClients.Contains(client) Then
                _listOfClients.Remove(client)

            End If
            RaiseEvent PopulateLog(Me, ex.Message)
        Finally
            ActiveCnxMonitorTimer.Enabled = True
        End Try
    End Sub

    Public Sub SendASTMFrame(ByVal astmFrame As String, ByVal FrameNumber As Integer, location As String, ByVal RFDby As String)
        Dim FinalizedAstmFrame As String = FinalizeFrame.ReplaceControlCharacters(astmFrame)

        'BYPASS LOGGING
        Const DEBUG As Boolean = True
        If DEBUG = True Then
            'Logging Final frames to Disk
            Dim file As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(Application.StartupPath() & "\astmDebug.txt", True)
            file.WriteLine(String.Format("{0}  {1}   {2}   {3}   {4}", DateTime.Now, RFDby, location, FrameNumber, astmFrame))
            file.Close()
        End If

        For Each c As TcpClient In _listOfClients
            Dim nns As NetworkStream = c.GetStream()
            'nns.Write(Encoding.ASCII.GetBytes(FinalizedAstmFrame), 0, FinalizedAstmFrame.Length)
            nns.Write(Encoding.GetEncoding("ISO-8859-1").GetBytes(FinalizedAstmFrame), 0, FinalizedAstmFrame.Length)
            'nns.Write(Encoding.GetEncoding("ISO-8859-1").GetBytes(astmFrame), 0, astmFrame.Length)
        Next
    End Sub

    Private Sub ActiveCnxMonitorTimer_Elapsed(sender As Object, e As ElapsedEventArgs) Handles ActiveCnxMonitorTimer.Elapsed
        ActiveCnxMonitorTimer.Enabled = False

        If _listOfClients.Count > 0 Then
            RaiseEvent CnxMonitor(Me, _server.LocalEndpoint.ToString, "Connected, Listening", _listOfClients.Count)
        Else
            RaiseEvent CnxMonitor(Me, _server.LocalEndpoint.ToString, "Listening", _listOfClients.Count)

        End If

    End Sub
    'TODO: Move the WriteApplicationLog Sub to another class. Add Log4Net to the project and handle logging better.
    Public Sub WriteApplicationLog(sender As Object,ByVal writeLine As string)
            Dim file As IO.StreamWriter = My.Computer.FileSystem.OpenTextFileWriter(Application.StartupPath() & "\astmDebug.txt", True)
            file.WriteLine(String.Format("{0}  {1}   {2}", DateTime.Now, Sender.ToString, writeLine))
            file.Close()
    End Sub
End Class
