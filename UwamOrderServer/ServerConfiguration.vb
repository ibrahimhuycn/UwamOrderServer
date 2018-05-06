Imports System.Text.RegularExpressions

Public Class ServerConfiguration

    Public Sub New()
        InitializeComponent()

        'Read current IP and Port
        TextBoxIPAddress.Text = My.Settings.ServerIP
        TextBoxPort.Text = My.Settings.ServerPort
    End Sub

    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        Dim validatedPort As Integer
        Select Case True
            Case ValidateIPAddress(TextBoxIPAddress.Text) And UInt16.TryParse(TextBoxPort.Text, validatedPort)
                My.Settings.ServerIP = TextBoxIPAddress.Text
                My.Settings.ServerPort = validatedPort
                MsgBox("Settings saved. Please restart the application!")

                Me.Close()
                Me.Dispose()
            Case Else
                MsgBox("Invalid IP or port entered.")
        End Select
    End Sub

    Private Function ValidateIPAddress(ByVal IP As String) As Boolean
        If Not IP = "" Then
            Return Regex.IsMatch(IP, "^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$")
        Else
            Return False
        End If
    End Function

End Class