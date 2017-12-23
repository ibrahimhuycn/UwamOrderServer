Namespace ASTM

                'Common ASTM records structure required for UWAM 

    Public Class Records
        'Record Template with chars < 240: 
        '[STX] [F#] [Text] [ETX] [CHK1] [CHK2] [CR] [LF]


        'Record Template with chars > 240:
        '[STX] [F#] [Text] [ETB] [CHK1] [CHK2] [CR] [LF]
        '[STX] [F#] [Text] [ETB] [CHK1] [CHK2] [CR] [LF]
        '……
        '[STX] [F#] [Text] [ETX] [CHK1] [CHK2] [CR] [LF]


        'Max characters in ASTM record cannot exceed 240 including the overhead of
        'control characters.
        Const MaxCharSPerRecord As Integer = 240
        Const MaxRetriesSendingRecord As Integer  = 06  'max number of retries after consecutive NAKs from recever.
        Enum Fields
            FieldDelimiter  = 124       ' ASCII for Vertical bar(|) is 124
            RepeatDelimiter1 = 92        ' ASCII for Backslash (\) is 92
            RepeatDelimiter2 = 165      ' ASCII for Yen (¥) is 165
            ComponentDelimiter = 94     ' ASCII for Caret(^) is 94
        End Enum
        Enum TimeOuts
            'Timeouts expresseed in milliseconds
            'Establishment phase
            ReplyWindowAfterENQ = 15000        'reply with ACK, NAK or EOT
            MinimumENQWaitAfterNAK = 10000     'Have to wait for min 10 sec before another ENQ
            ENQWaitAfterENQClash = 20000       'Server have to wait min 20 before sending ENQ after an ENQ Clash

        End Enum


    
        Function GenerateHeader()
        'Header Record Example: H|\^&|||||||||||E139→4-97|20100822100525<CR> 
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  #  | ASTM Field # | ASTM Name                       | VB alias          |
            ' +=====+==============+=================================+===================+
            ' |   1 |        7.1.1 |             ASTM Record Type ID |              type |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   2 |        7.1.2 |            Delimiter Definition |         delimeter |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   3 |        7.1.3 |              Message Control ID |        message_id |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   4 |        7.1.4 |                 Access Password |          password |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   5 |        7.1.5 |               Sender Name or ID |            sender |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   6 |        7.1.6 |           Sender Street Address |           address |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   7 |        7.1.7 |                  Reserved Field |          reserved |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   8 |        7.1.8 |         Sender Telephone Number |             phone |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   9 |        7.1.9 |       Characteristics of Sender |              caps |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  10 |       7.1.10 |                     Receiver ID |          receiver |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  11 |       7.1.11 |                        Comments |          comments |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  12 |       7.1.12 |                   Processing ID |     processing_id |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  13 |       7.1.13 |                  Version Number |           version |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  14 |       7.1.14 |            Date/Time of Message |         timestamp |
            ' +-----+--------------+---------------------------------+-------------------+
                                                        'Usage Status in Order Record Header.



            Const type As String = "H"
            Const delimiter0 As String = "\^&"
            Const delimiter1 As String = "|¥^&"
            Dim message_id As Integer = Nothing         'Not Used
            Dim password As String = Nothing            'Not Used
            Dim sender As String = Nothing              'Host Name in case of orderRecord
            Dim address As String = Nothing             'Not Used
            Dim phone As Integer = Nothing              'Not Used
            Dim caps As String = Nothing                'Not Used
            Dim receiver As String = Nothing            'Not Used
            Dim comments As String = Nothing            'Not Used
            Dim processing_id As Integer = Nothing      'Not Used
            Const version0 As String = "E1394-97"       'Either Used, Not Sure
            Const version1 As String = "LIS2-A2"        'Either Used, Not Sure
            Dim timestamp As DateTime = Nothing         'Date and time of message format is fixed with “YYYYMMDDHHMMSS”

            return 0
        End Function

        Function GeneratePatient()
        'Patient Record Example: 
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  #  | ASTM Field # | ASTM Name                       | VB  alias         |
            ' +=====+==============+=================================+===================+
            ' |   1 |        8.1.1 |                  Record Type ID |              type |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   2 |        8.1.2 |                 Sequence Number |               seq |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   3 |        8.1.3 |    Practice Assigned Patient ID |       practice_id |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   4 |        8.1.4 |  Laboratory Assigned Patient ID |     laboratory_id |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   5 |        8.1.5 |                      Patient ID |                id |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   6 |        8.1.6 |                    Patient Name |              name |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   7 |        8.1.7 |            Mother’s Maiden Name |       maiden_name |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   8 |        8.1.8 |                       Birthdate |         birthdate |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   9 |        8.1.9 |                     Patient Sex |               sex |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  10 |       8.1.10 |      Patient Race-Ethnic Origin |              race |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  11 |       8.1.11 |                 Patient Address |           address |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  12 |       8.1.12 |                  Reserved Field |          reserved |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  13 |       8.1.13 |        Patient Telephone Number |             phone |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  14 |       8.1.14 |          Attending Physician ID |      physician_id |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  15 |       8.1.15 |                Special Field #1 |         special_1 |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  16 |       8.1.16 |                Special Field #2 |         special_2 |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  17 |       8.1.17 |                  Patient Height |            height |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  18 |       8.1.18 |                  Patient Weight |            weight |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  19 |       8.1.19 |       Patient’s Known Diagnosis |         diagnosis |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  20 |       8.1.20 |     Patient’s Active Medication |        medication |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  21 |       8.1.21 |                  Patient’s Diet |              diet |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  22 |       8.1.22 |            Practice Field No. 1 |  practice_field_1 |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  23 |       8.1.23 |            Practice Field No. 2 |  practice_field_2 |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  24 |       8.1.24 |       Admission/Discharge Dates |    admission_date |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  25 |       8.1.25 |                Admission Status |  admission_status |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  26 |       8.1.26 |                        Location |          location |
            ' +-----+--------------+---------------------------------+-------------------+
            Const type as string = "P"                      
            Dim seq as Integer = Nothing                        'seq
            Dim practice_id As Integer = Nothing                'Not Used
            Dim laboratory_id As Integer = Nothing              'max 16 characters long    
            Dim id As Integer = Nothing                         'Not Used

            'Names separated by caret (^)
            'LastName^MiddleName^FirstName^
            Dim name As String = Nothing                                  
            Dim maiden_name As String = Nothing                 'Not Used
            Dim birthdate As Date = Nothing                     'B'DAY in AD    
            Dim sex As String = Nothing                         'Gender
            Dim race As String = Nothing                        'Not Used
            Dim address As String = Nothing                     'Not Used
            Dim reserved As String = Nothing                    'Not Used
            Dim phone As Integer = Nothing                      'Not Used
            Dim physician_id As Integer = Nothing               'Not Used
            Dim special_1 As String = Nothing                   'Blood Group & Rh 
            Dim special_2 As String = Nothing                   'Not Used
            Dim height As Integer = Nothing                     'Not Used
            Dim weight As Integer = Nothing                     'Not Used
            Dim diagnosis As String = Nothing                   'Disease Code       
            Dim medication As String = Nothing                  'Not Used
            Dim diet As String = Nothing                        'Not Used
            Dim practice_field_1 As String = Nothing            'Not Used
            Dim practice_field_2 As String = Nothing            'Not Used
            Dim admission_date As date = Nothing                'Not Used
            Dim admission_status As String = Nothing            'OP: Out-patient  IP: In-patient  (Blank): Unknown
            Dim location As String = Nothing             

            Return 0
        End Function

        Function GenerateOrder()
        'Order Record Example: O|1|123456^05^1234567890123456789012^B^O||^^^^CHM\^^^^UF||20040807101000|||||N||||||| |||||||Q<CR> 
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  #  | ASTM Field # | ASTM Name                      | VB  alias          |
            ' +=====+==============+================================+====================+
            ' |   1 |        9.4.1 |                 Record Type ID |               type |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |   2 |        9.4.2 |                Sequence Number |                seq |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |   3 |        9.4.3 |                    Specimen ID |          sample_id |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |   4 |        9.4.4 |         Instrument Specimen ID |         instrument |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |   5 |        9.4.5 |              Universal Test ID |               test |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |   6 |        9.4.6 |                       Priority |           priority |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |   7 |        9.4.7 |    Requested/Ordered Date/Time |         created_at |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |   8 |        9.4.8 |  Specimen Collection Date/Time |         sampled_at |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |   9 |        9.4.9 |            Collection End Time |       collected_at |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  10 |       9.4.10 |              Collection Volume |             volume |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  11 |       9.4.11 |                   Collector ID |          collector |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  12 |       9.4.12 |                    Action Code |        action_code |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  13 |       9.4.13 |                    Danger Code |        danger_code |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  14 |       9.4.14 |           Relevant Information |      clinical_info |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  15 |       9.4.15 |    Date/Time Specimen Received |       delivered_at |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  16 |       9.4.16 |            Specimen Descriptor |        biomaterial |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  17 |       9.4.17 |             Ordering Physician |          physician |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  18 |       9.4.18 |        Physician’s Telephone # |    physician_phone |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  19 |       9.4.19 |               User Field No. 1 |       user_field_1 |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  20 |       9.4.20 |               User Field No. 2 |       user_field_2 |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  21 |       9.4.21 |         Laboratory Field No. 1 | laboratory_field_1 |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  22 |       9.4.22 |         Laboratory Field No. 2 | laboratory_field_2 |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  23 |       9.4.23 |             Date/Time Reported |        modified_at |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  24 |       9.4.24 |              Instrument Charge |  instrument_charge |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  25 |       9.4.25 |          Instrument Section ID | instrument_section |
            ' +-----+--------------+--------------------------------+--------------------+
            ' |  26 |       9.4.26 |                    Report Type |        report_type |
            ' +-----+--------------+--------------------------------+--------------------+
            Const type as string = "O"
            'The sequence number starts with 1 in a maximum of 4 digits, 
            'and indicates the sequence position in which the record appeared in the message. 
            'This number is reset to 1 when a higher-level record appears in the message.
            Dim seq as Integer = Nothing 

            'sample_id: Sub-fields of Rack No., tube position., sample No., and sample No. attribute are placed by separating with a delimiter “^”.
            'Rack No.^ 
            'Tube Position No.^ 
            'Sample ID number^ 
            'Sample No. attribute^
            'If only sample_id is specified the field should look like ^^sample_id^^  with a max of 22 characters
            Dim sample_id as string = Nothing       
            Dim instrument as string = Nothing

            '^^^^Parameter 
            'When the host computer replies order, please set the Analyzer which it is necessary to measure.   
            'Example: “^^^^CHM\^^^^UF\^^^^UD”
            Dim test as Integer = Nothing

            Dim priority as string = Nothing      'R: Routine  A: Urgent   S: Emergency
            Dim created_at as DateTime = Nothing  ''YYYYMMDDHHMMSS 
            Dim sampled_at as DateTime  = Nothing
            Dim collected_at as DateTime  = Nothing
            Dim volume as Integer = Nothing
            Dim collector as Integer = Nothing

            ' [Host computer → U-WAM]

            'Code indicating the type of order information to be sent.
            'C:          Cancellation of a parameter
            'A:          Addition of a parameter to an existing order
            'N: New order
             Const action_code As String = "N" '

            Dim danger_code as string = Nothing
            Dim clinical_info as string = Nothing
            Dim delivered_at as DateTime = Nothing
            Dim biomaterial as Integer = Nothing   'UrineSampleType: Urine, Urine-EarlyMorning, Urine-Pooled, Urine-Postprandial, Urine-Catheter, Blank
            'Physician Code ^
            'Physician Last Name ^
            'Physician First Name ^^^
            'Physician Title
            'Therefore: If no physician info is provided it should look like ^ ^ ^^^ or ^^^^^
            Dim physician as string = Nothing
            Dim physician_phone as Integer = Nothing
            Dim user_field_1 as String   = Nothing
            Dim user_field_2 as string = Nothing
            Dim laboratory_field_1 as string = Nothing
            Dim laboratory_field_2 as string = Nothing
            Dim modified_at as DateTime  = Nothing
            Dim instrument_charge as String  = Nothing
            Dim instrument_section as String  = Nothing
            
            'F: Final result (Fixed: U-WAMwill always output the final results.) 
            'Y: No test order exists. (Use this when no order exists for the inquiry.)
            'Q: Response to the inquiry (Use this when an order exists for the inquiry.)
            Dim report_type as string = Nothing

            Return 0
        End Function

        Function Termination()
        'Termination Record Example: L|1|N<CR>
            ' +-----+--------------+---------------------------------+-------------------+
            ' |  #  | ASTM Field # | ASTM Name                       | VB  alias         |
            ' +=====+==============+=================================+===================+
            ' |   1 |       13.1.1 |                  Record Type ID |              type |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   2 |       13.1.2 |                 Sequence Number |               seq |
            ' +-----+--------------+---------------------------------+-------------------+
            ' |   3 |       13.1.3 |                Termination code |              code |
            ' +-----+--------------+---------------------------------+-------------------+

            Const type as String  = "L"
            Dim seq as Integer = 1          'Always 1 in the case of U-WAM
            Dim code as String = "N"        '“N” is usually used as a character string. Normal Termination



            Return 0
        End Function
    End Class
End Namespace

