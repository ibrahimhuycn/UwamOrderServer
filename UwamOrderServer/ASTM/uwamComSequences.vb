Imports System.Text
Namespace ASTM

    Public Class uwamComSequences
        Private TemplateFramesASTM(6) as String
        Public Sub New()
            'Maximum characters per frame: 240
            'ASTM templates for U-WAM
            'CHEMISTRY
            'TemplateFramesASTM(0) = "<ENQ>"
           ' TemplateFramesASTM(1) = "<STX>{FNo}H|\^&|||PATH.CLINICAL||||||||LIS2-A2<CR><ETX><CHK1><CHK2><CR><LF>"
            'TemplateFramesASTM(2) = "<STX>{FNo}P|{seq}||{patient_id}||||{DOB}|U||||||||||||||||<CR><ETX><CHK1><CHK2><CR><LF>"
            'TemplateFramesASTM(3) = "<STX>{FNo}O|{seq}|{SampleNumber}||^^^URO\^^^BLD\^^^BIL\^^^KET\^^^GLU\^^^PRO\^^^PH\^^^NIT\^^^LEU\^^^CRE\^^^ALB\^^^S.G.(Ref)\^^^COLOR\^^^ColorRANK\^^^CLOUD\|R||{YYYYMMDDHHmmss}||||N||||Urine|||{OrderNo}||<ETB><CHK1><CHK2><CR><LF>"
            'TemplateFramesASTM(4) = "<STX>{FNo}|||||O|||||C00001^PATH.CLINICAL<CR><ETX><CHK1><CHK2><CR><LF>"
           ' TemplateFramesASTM(5) = "<STX>{FNo}L|1|N<CR><ETX><CHK1><CHK2><CR><LF>"
            'TemplateFramesASTM(6) = "<EOT>"
            'DEPOSIT
            TemplateFramesASTM(0) = "<ENQ>"
            TemplateFramesASTM(1) = "<STX>{FNo}H|\^&|||PATH.CLINICAL||||||||LIS2-A2<CR><ETX><CHK1><CHK2><CR><LF>"
            TemplateFramesASTM(2) = "<STX>{FNo}P|{seq}||{patient_id}||||{DOB}|U<CR><ETX><CHK1><CHK2><CR><LF>"
            TemplateFramesASTM(3) = "<STX>{FNo}O|{seq}|{SampleNumber}||^^^RBC\^^^WBC\^^^WBC Clumps\^^^EC\^^^Squa.EC\^^^NonSEC\^^^CAST\^^^Hy.CAST\^^^Path.CAST\^^^BACT\^^^X'TAL\^^^YLC\^^^SPERM\^^^MUCUS\|R||{YYYYMMDDHHmmss}||||N||||Urine||<ETB><CHK1><CHK2><CR><LF>"
            TemplateFramesASTM(4) = "<STX>{FNo}|{OrderNo}|||||||O|||||C00001^PATH.CLINICAL<CR><ETX><CHK1><CHK2><CR><LF>"
            TemplateFramesASTM(5) = "<STX>{FNo}L|1|N<CR><ETX><CHK1><CHK2><CR><LF>"
            TemplateFramesASTM(6) = "<EOT>"
        End Sub

        ''' <summary>
        ''' Generates an array of ASTM frames for provided sample number,
        ''' </summary>
        ''' <param name="SampleNumber">SampleNumber as integer to generate ASTM frames</param>
        ''' <returns>string ASTM complete message from ENQ to EOT</returns>
        Public Function GenMessageASTM(byVal SampleNumber As String)
            Dim LoopCounter As Integer = 0
            Dim seq As Integer = 1 
            Dim ReplacedMessageASTM(6) as String 
            Dim FrameNumber As Integer  = 1
            Dim DOB As String = vbNull
            Dim EmulatedPatientID As String = Date.Now.ToString("HHmmssfff")
            Dim CurrentDateTime As String = Date.Now.ToString("yyyyMMddHHmmss")
            Dim OrderNumber As Double = EmulatedPatientID & right(SampleNumber,3)

            For each frame In TemplateFramesASTM
                If Left(frame, 5) = "<STX>" Then
                    If FrameNumber = 7 Then
                        FrameNumber = 1
                        ReplacedMessageASTM(LoopCounter) = ReplaceTemplateValues(TemplateFramesASTM(LoopCounter),SampleNumber, FrameNumber,seq,DOB,EmulatedPatientID,CurrentDateTime,OrderNumber)
                    Else
                        ReplacedMessageASTM(LoopCounter) = ReplaceTemplateValues(TemplateFramesASTM(LoopCounter), SampleNumber, FrameNumber, seq, DOB, EmulatedPatientID, CurrentDateTime, OrderNumber)
                        FrameNumber += 1
                    End If

                    'seq +=1
                    Else
                    'seq = 1

                    If FrameNumber = 7 Then
                        FrameNumber = 1
                        ReplacedMessageASTM(LoopCounter) = ReplaceTemplateValues(TemplateFramesASTM(LoopCounter), SampleNumber, FrameNumber, seq, DOB, EmulatedPatientID, CurrentDateTime, OrderNumber)
                    Else
                        ReplacedMessageASTM(LoopCounter) = ReplaceTemplateValues(TemplateFramesASTM(LoopCounter), SampleNumber, FrameNumber, seq, DOB, EmulatedPatientID, CurrentDateTime, OrderNumber)
                        
                        'frame number should not increment if it's ENQ
                        If Not frame = "<ENQ>" Then
                            FrameNumber += 1
                        End If
                       
                    End If
                End If
                LoopCounter +=1
            Next


            Return ReplacedMessageASTM
        End Function
        Private Function ReplaceTemplateValues(ByVal TemplateASTM As String,ByVal SNo As Integer, ByVal FNo As Integer, ByVal Seq As Integer, byVal DOB As String, ByVal PatientId As String, ByVal CollectedDateTime As String, byVal OrderNumber As Double)
        
            Dim ValueReplacedASTM As StringBuilder = New StringBuilder(TemplateASTM)

            ValueReplacedASTM.Replace("{FNo}",FNo.ToString )
            ValueReplacedASTM.Replace("{seq}",Seq.ToString )
            ValueReplacedASTM.Replace("{patient_id}", PatientId)
            ValueReplacedASTM.Replace("{DOB}",DOB )
            ValueReplacedASTM.Replace("{SampleNumber}", SNo.ToString )
            ValueReplacedASTM.Replace("{YYYYMMDDHHmmss}", CollectedDateTime )
            ValueReplacedASTM.Replace("{OrderNo}",OrderNumber.ToString )
            ValueReplacedASTM.Replace("<CHK1><CHK2>", GetCheckSumValue(ValueReplacedASTM.ToString))

            Return ValueReplacedASTM.ToString
        End Function
        ''' <summary>
        ''' Reads checksum of an ASTM frame, calculates characters after STX,
        ''' up to and including the ETX or ETB. Method assumes the frame contains an ETX or ETB.
        ''' </summary>
        ''' <param name="frame">frame of ASTM data to evaluate</param>
        ''' <returns>string containing checksum</returns>

        Public Function GetCheckSumValue(ByVal frame As String) As String
            frame = ReplaceControlCharacters(frame)
            Dim checksum As String = "00"
            Dim byteVal As Integer = 0
            Dim sumOfChars As Integer = 0
            Dim complete As Boolean = False
            For idx As Integer = 0 To frame.Length - 1
                byteVal = Convert.ToInt32(frame(idx))
                Select Case byteVal
                    Case astmConstants.STX
                        sumOfChars = 0
                    Case astmConstants.ETX, astmConstants.ETB
                        sumOfChars += byteVal
                        complete = True
                    Case Else
                        sumOfChars += byteVal
                End Select

                If complete Then Exit For
            Next

            If sumOfChars > 0 Then
                checksum = Convert.ToString(sumOfChars Mod 256, 16).ToUpper()
            End If

            Return CStr((If(checksum.Length = 1, "0" & checksum, checksum)))
        End Function
        ''' <summary>
        ''' Replaces display ASTM control characters (i.e., STX, ETX etc) with their ASCII values 
        ''' for calculation of checksum.
        ''' </summary>
        ''' <param name="astmFrame">frame of ASTM data to evaluate</param>
        ''' <returns>ASTM frame with actual ASCII control characters</returns>
        Public Function ReplaceControlCharacters(astmFrame As string) As String
            Dim ReplcedAstmFrame As StringBuilder = New StringBuilder(astmFrame)

            ReplcedAstmFrame.Replace("<STX>", ChrW(astmConstants.STX))
            ReplcedAstmFrame.Replace("<ETX>", ChrW(astmConstants.ETX))
            ReplcedAstmFrame.Replace("<EOT>", ChrW(astmConstants.EOT))
            ReplcedAstmFrame.Replace("<ENQ>", ChrW(astmConstants.ENQ))
            ReplcedAstmFrame.Replace("<ACK>", ChrW(astmConstants.ACK))
            ReplcedAstmFrame.Replace("<LF>", ChrW(astmConstants.LF))
            ReplcedAstmFrame.Replace("<CR>", ChrW(astmConstants.CR))
            ReplcedAstmFrame.Replace("<NAK>", ChrW(astmConstants.NAK))
            ReplcedAstmFrame.Replace("<ETB>", ChrW(astmConstants.ETB))

            Return ReplcedAstmFrame.ToString
        End Function
    End Class
End Namespace

