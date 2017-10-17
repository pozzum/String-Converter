Imports System
Imports System.IO
Imports System.IO.Stream
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.FileIO
Imports System.ComponentModel 'Backgroundworker
Imports System.Environment 'appdata
Imports System.Text.RegularExpressions 'match collections
Public Class Form1
    Dim counter As Integer = 0

    Function Bytes2Dec(array() As Byte)
        Dim Number As Int32 = array(3) * 256 * 256 * 256 + array(2) * 256 * 256 + array(1) * 256 + array(0)
        'MessageBox.Show(Number.ToString & "Count" & counter)
        Return Number
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If OpenCSVDialog.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            If SavePACHDialog.ShowDialog = System.Windows.Forms.DialogResult.OK Then

                counter = 0
                'First How many new strings
                Dim NewStringCount As Integer = Num_String_Count.Value
                'How Big Are the strings
                Dim stringsize As Integer = Num_Max_Length.Value
                Dim newdataset(NewStringCount * 12 + NewStringCount * stringsize + 8 - 1) As Byte
                '12 to say string attirbutes then the string length
                Dim NewStart(3) As Byte 'a byteset for the location in the file
                'now to set the string reference
                Dim StringName(3) As Byte
                Dim StartString As Int32 = Num_Start_Num.Value ' 35A6 first new string to be 35A7
                StringName = BitConverter.GetBytes(StartString)

                'we need the first strings location - string length
                NewStart = BitConverter.GetBytes(8 + NewStringCount * 12 - stringsize)
                'now to build the file
                For i As Integer = 0 To 3
                    newdataset(i) = 0
                Next
                'we need the total string count for the start
                Dim Stringcount(3) As Byte
                Stringcount = BitConverter.GetBytes(NewStringCount)
                newdataset(4) = Stringcount(0)
                newdataset(5) = Stringcount(1)
                newdataset(6) = Stringcount(2)
                newdataset(7) = Stringcount(3)
                For i As Integer = 8 To 7 + NewStringCount * 12
                    If 0 <= counter AndAlso counter < 3 Then
                        counter = counter + 1
                    ElseIf counter = 3 Then 'Setting String Location
                        NewStart = BitConverter.GetBytes(Bytes2Dec(NewStart) + stringsize)
                        newdataset(i - 3) = NewStart(0)
                        newdataset(i - 2) = NewStart(1)
                        newdataset(i - 1) = NewStart(2)
                        newdataset(i) = NewStart(3)
                        counter = counter + 1
                    ElseIf 3 < counter AndAlso counter < 7 Then
                        counter = counter + 1
                    ElseIf counter = 7 Then 'Setting String Length
                        Dim temp() As Byte = BitConverter.GetBytes(stringsize)
                        newdataset(i - 3) = temp(0)
                        newdataset(i - 2) = temp(1)
                        newdataset(i - 1) = temp(2)
                        newdataset(i) = temp(3)
                        counter = counter + 1
                    ElseIf 7 < counter AndAlso counter < 11 Then
                        counter = counter + 1
                    ElseIf counter = 11 Then
                        StringName = BitConverter.GetBytes(Bytes2Dec(StringName) + 1)
                        newdataset(i - 3) = StringName(0)
                        newdataset(i - 2) = StringName(1)
                        newdataset(i - 1) = StringName(2)
                        newdataset(i) = StringName(3)
                        counter = 0
                    End If
                Next
                Dim StartByte As Integer = 8 + NewStringCount * 12
                Dim TextArray() As String ' NewStringCount - 1

                Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(OpenCSVDialog.FileName)
                    MyReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited
                    MyReader.SetDelimiters({vbTab, ","})
                    Dim counter As Integer = 0
                    Do While Not MyReader.EndOfData
                        Try
                            TextArray = MyReader.ReadFields
                            Dim bytearray() As Byte
                            'MessageBox.Show(TextArray.Length)
                            Try
                                bytearray = System.Text.Encoding.ASCII.GetBytes(TextArray(0))
                            Catch ex As Exception
                                MessageBox.Show(ex.Message & counter & TextArray(0)) '& TextArray(i) 
                            End Try
                            For j As Integer = 0 To 29
                                Try
                                    If j < bytearray.Length Then
                                        newdataset(StartByte + counter * 30 + j) = bytearray(j)
                                    End If
                                Catch ex As Exception
                                    MessageBox.Show("Error: " & counter & " " & TextArray(0) & " " & j & " " & Mid(TextArray(0), j - 1, j))
                                End Try
                            Next
                        Catch ex As FileIO.MalformedLineException
                            Stop
                        End Try
                        counter = counter + 1
                    Loop
                    'For i As Integer = 0 To NewStringCount - 1
                    'Try
                    'TextArray = MyReader.ReadFields
                    'My.Computer.FileSystem.WriteAllText(
                    '"C:\Users\Pozzum\Desktop\Book2.csv", TextArray(i), True) 'Book1.csv
                    ' Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException
                    'MsgBox("Line " & ex.Message & " is invalid.  Skipping")
                    '
                    'End Try
                    'Next
                End Using

                Try
                    File.WriteAllBytes(SavePACHDialog.FileName, newdataset)
                Catch ex As Exception
                    'MessageBox.Show(ex.Message)
                End Try
                MessageBox.Show("String Built")
            End If
        End If

    End Sub

    Private Sub NumericUpDown3_ValueChanged(sender As Object, e As EventArgs) Handles Num_Start_Num.ValueChanged
        TextBox1.Text = Hex(Num_Start_Num.Value)
        TextBox2.Text = Hex(Num_Start_Num.Value + Num_String_Count.Value)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If OpenPACHDialog.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            If SaveCSVDialog.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                Dim Source As Byte() = File.ReadAllBytes(OpenPACHDialog.FileName)
                Dim String_Count As Integer = Bytes2Dec({Source(4), Source(5), Source(6), Source(7)})
                Num_String_Count.Value = String_Count
                Dim Max_Length As Integer = 0
                Dim String_List(String_Count - 1) As String
                Dim Ref_Num(String_Count - 1) As Integer
                For i As Integer = 0 To String_Count - 1
                    Ref_Num(i) = Bytes2Dec({Source(&H10 + 12 * i), Source(&H11 + 12 * i), Source(&H12 + 12 * i), Source(&H13 + 12 * i)})
                    Dim Start_Num As Integer = Bytes2Dec({Source(&H8 + 12 * i), Source(&H9 + 12 * i), Source(&HA + 12 * i), Source(&HB + 12 * i)})
                    Dim Length As Integer = Bytes2Dec({Source(&HC + 12 * i), Source(&HD + 12 * i), Source(&HE + 12 * i), Source(&HF + 12 * i)})
                    If Length > Max_Length Then
                        Max_Length = Length
                    End If
                    Dim String_Bytes As Byte() = New Byte(Length - 1) {}
                    Buffer.BlockCopy(Source, Start_Num, String_Bytes, 0, Length)
                    String_List(i) = System.Text.Encoding.ASCII.GetString(String_Bytes)
                Next
                Dim CSV_Writer As System.IO.StreamWriter
                CSV_Writer = My.Computer.FileSystem.OpenTextFileWriter(SaveCSVDialog.FileName, False)
                For i As Integer = 0 To String_Count - 1
                    CSV_Writer.WriteLine(String_List(i) & "," & Hex(Ref_Num(i)))
                Next
                CSV_Writer.Close()
                MessageBox.Show("Writing Complete")
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        If OpenPACHDialog.ShowDialog = System.Windows.Forms.DialogResult.OK Then
            If OpenCSVDialog.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                If SavePACHDialog.ShowDialog = System.Windows.Forms.DialogResult.OK Then
                    Dim SourceStringFiles As Byte() = File.ReadAllBytes(OpenPACHDialog.FileName)
                    Dim String_Count As Integer = BitConverter.ToInt32(SourceStringFiles, 4)
                    MessageBox.Show(String_Count)
                    Dim Max_Length As Integer = 0
                    Dim String_List(String_Count - 1) As String
                    Dim Ref_Num(String_Count - 1) As Integer
                    Dim Start_Num(String_Count - 1) As Integer
                    Dim Length(String_Count - 1) As Integer
                    For i As Integer = 0 To String_Count - 1
                        Start_Num(i) = BitConverter.ToInt32(SourceStringFiles, &H8 + 12 * i)
                        Length(i) = BitConverter.ToInt32(SourceStringFiles, &HC + 12 * i)
                        Ref_Num(i) = BitConverter.ToInt32(SourceStringFiles, &H10 + 12 * i)
                        If Length(i) > Max_Length Then
                            Max_Length = Length(i)
                        End If
                        String_List(i) = System.Text.Encoding.ASCII.GetString(SourceStringFiles, Start_Num(i), Length(i)).TrimEnd(Chr(0))
                        Start_Num(i) = Start_Num(i) + Num_String_Count.Value * 12
                    Next
                    Dim NewStringFile As Byte() = New Byte(SourceStringFiles.Count + 12 * Num_String_Count.Value + Num_String_Count.Value * Num_Max_Length.Value - 1) {}
                    'we need the total string count for the start
                    Dim Stringcount As Byte()
                    Stringcount = BitConverter.GetBytes(CInt(String_Count + Num_String_Count.Value))
                    Buffer.BlockCopy(Stringcount, 0, NewStringFile, 4, 4)
                    For i As Integer = 0 To String_Count - 1
                        Dim TempArray As Byte() = BitConverter.GetBytes(Start_Num(i))
                        Buffer.BlockCopy(TempArray, 0, NewStringFile, &H8 + 12 * i, 4)
                        TempArray = BitConverter.GetBytes(Length(i))
                        Buffer.BlockCopy(TempArray, 0, NewStringFile, &HC + 12 * i, 4)
                        TempArray = BitConverter.GetBytes(Ref_Num(i))
                        Buffer.BlockCopy(TempArray, 0, NewStringFile, &H10 + 12 * i, 4)
                    Next
                    File.WriteAllBytes(SavePACHDialog.FileName, NewStringFile)
                    MessageBox.Show("Exist Header Saved")
                    Dim BaseLine As Integer = Start_Num(String_Count - 1) + Length(String_Count - 1)
                    For i As Integer = 0 To Num_String_Count.Value - 1
                        'starting Number first
                        Dim TempArray As Byte() = BitConverter.GetBytes(CInt(BaseLine + i * Num_Max_Length.Value))
                        Buffer.BlockCopy(TempArray, 0, NewStringFile, String_Count * 12 + &H8 + 12 * i, 4)
                        'then length
                        TempArray = BitConverter.GetBytes(CInt(Num_Max_Length.Value))
                        Buffer.BlockCopy(TempArray, 0, NewStringFile, String_Count * 12 + &HC + 12 * i, 4)
                        'then referece number
                        TempArray = BitConverter.GetBytes(CInt(Num_Start_Num.Value + i))
                        Buffer.BlockCopy(TempArray, 0, NewStringFile, String_Count * 12 + &H10 + 12 * i, 4)
                    Next
                    File.WriteAllBytes(SavePACHDialog.FileName, NewStringFile)
                    MessageBox.Show("New Header Saved")
                    MessageBox.Show(Hex(Start_Num(0)))
                    Buffer.BlockCopy(SourceStringFiles, Start_Num(0) - Num_String_Count.Value * 12, NewStringFile, 8 + String_Count * 12 + Num_String_Count.Value * 12, SourceStringFiles.Count - Start_Num(0) - 1)
                    File.WriteAllBytes(SavePACHDialog.FileName, NewStringFile)
                    MessageBox.Show("Exist Strings Saved")

                    Dim TextArray() As String
                    Using MyReader As New Microsoft.VisualBasic.FileIO.TextFieldParser(OpenCSVDialog.FileName)
                        MyReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited
                        MyReader.SetDelimiters({vbTab, ","})
                        Dim counter As Integer = 0
                        Do While Not MyReader.EndOfData
                            Try
                                TextArray = MyReader.ReadFields
                                Dim bytearray() As Byte
                                'MessageBox.Show(TextArray.Length)
                                Try
                                    bytearray = System.Text.Encoding.ASCII.GetBytes(TextArray(0))
                                Catch ex As Exception
                                    MessageBox.Show(ex.Message & counter & TextArray(0)) '& TextArray(i) 
                                End Try
                                For j As Integer = 0 To 29
                                    Try
                                        If j < bytearray.Length Then
                                            NewStringFile(BaseLine + counter * 30 + j) = bytearray(j)
                                        End If
                                    Catch ex As Exception
                                        MessageBox.Show("Error: " & counter & " " & TextArray(0) & " " & j & " " & Mid(TextArray(0), j - 1, j))
                                    End Try
                                Next
                            Catch ex As FileIO.MalformedLineException
                                Stop
                            End Try
                            counter = counter + 1
                        Loop
                    End Using
                    Try
                        File.WriteAllBytes(SavePACHDialog.FileName, NewStringFile)
                    Catch ex As Exception
                        'MessageBox.Show(ex.Message)
                    End Try
                    MessageBox.Show("String Built")
                End If
            End If
        End If
    End Sub
End Class
