Imports System.IO
Public Class Form2
#Region "Main Form Processes"
    Dim OffsetByHeader As Integer()
    Dim LengthByHeader As Integer()
    Dim ReferenceByHeader As Integer()
    Dim BytesByHeader(1000000)() As Byte
    Dim StringByReference As String()
    'Const MaxReadableLength = 805194 '804876
    Dim CurrentIndexMain = 0
    Dim CurrentIndexSecondary = 0
    Dim CurrentLength = 0


    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBoxActiveFile.Text = Form1.SentFile
        ProcessString(True)
        ReadMainReference()
        ReadSecondReference()
    End Sub
    Sub ProcessString(Optional Backup As Boolean = False)
        'Creating Origional Backup
        If Backup Then
            File.Copy(TextBoxActiveFile.Text,
          Path.GetDirectoryName(TextBoxActiveFile.Text) & "\" &
          Path.GetFileNameWithoutExtension(TextBoxActiveFile.Text) & ".org", True)
        End If
        'Reading Full File
        Dim FileBytes() As Byte = File.ReadAllBytes(TextBoxActiveFile.Text)
        'Making Sure File is Uncompressed
        If (BitConverter.ToInt32(FileBytes, 0) = 0) = False Then
            MessageBox.Show("File Must Be Uncompressed!", "Oh No")
            Me.Close()
        Else
            'Getting the Header String Count
            Dim StringCount As Integer = BitConverter.ToInt32(FileBytes, 4)
            'Get Data On the Pach parts
            ReDim OffsetByHeader(StringCount - 1)
            ReDim LengthByHeader(StringCount - 1)
            ReDim ReferenceByHeader(StringCount - 1)
            StringByReference = New String(50000000) {}
            For i As Integer = 0 To StringCount - 1
                OffsetByHeader(i) = BitConverter.ToInt32(FileBytes, 8 + i * 12 + 0)
                LengthByHeader(i) = BitConverter.ToInt32(FileBytes, 8 + i * 12 + 4)
                ReferenceByHeader(i) = BitConverter.ToInt32(FileBytes, 8 + i * 12 + 8)
                'Trim all 00 chars so the strings don't end abrubtly in future manipulation
                BytesByHeader(i) = New Byte(LengthByHeader(i) - 1) {}
                Array.Copy(FileBytes, OffsetByHeader(i), BytesByHeader(i), 0, LengthByHeader(i))
                'If ReferenceByHeader(i) > &HF0000 Then
                'Dim ByteString As System.Text.StringBuilder = New System.Text.StringBuilder
                'For j As Integer = 0 To BytesByHeader(i).Length - 1
                'ByteString.Append(Hex(BytesByHeader(i)(j)) + ", ")
                'Next
                'MessageBox.Show(ByteString.ToString)
                'End If
                StringByReference(ReferenceByHeader(i)) = System.Text.Encoding.ASCII.GetString(BytesByHeader(i),
                                                         0,
                                                         BytesByHeader(i).Length).TrimEnd(Chr(0))
            Next
            'MessageBox.Show(Hex(OffsetByHeader(StringCount - 1)))
            'MessageBox.Show(Hex(LengthByHeader(StringCount - 1)))
            CurrentLength = OffsetByHeader(StringCount - 1) + LengthByHeader(StringCount - 1)
            'LabelLength.Text = CurrentLength & "\" & MaxReadableLength & " Bytes"
            If ReferenceByHeader(0) <> 0 AndAlso NumericMain.Value = 0 Then
                NumericMain.Value = ReferenceByHeader(0)
                NumericSecondary.Value = ReferenceByHeader(0)
            End If
        End If
    End Sub
    Sub RebuildFile(Optional deletedindex As Integer = 1000000)
        'Moving Removed Index To Back And Removing
        Dim TempArray() As Byte = New Byte(CurrentLength + TextBoxMain.Text.Length - OldText.Length) {}
        'MessageBox.Show("Copy New String Count")
        'Copying Over The New String Count
        If deletedindex <> 1000000 Then
            Buffer.BlockCopy(BitConverter.GetBytes(OffsetByHeader.Length - 1), 0, TempArray, 4, 4)
        Else 'no deleted string
            Buffer.BlockCopy(BitConverter.GetBytes(OffsetByHeader.Length), 0, TempArray, 4, 4)
        End If

        'Copying Over Each String
        'MessageBox.Show("Building New File")
        For i As Integer = 0 To OffsetByHeader.Length - 1
            Try
                If i < deletedindex Then
                    Buffer.BlockCopy(BitConverter.GetBytes(OffsetByHeader(i)), 0, TempArray, 8 + i * 12 + 0, 4)
                    Buffer.BlockCopy(BitConverter.GetBytes(LengthByHeader(i)), 0, TempArray, 8 + i * 12 + 4, 4)
                    Buffer.BlockCopy(BitConverter.GetBytes(ReferenceByHeader(i)), 0, TempArray, 8 + i * 12 + 8, 4)
                    Buffer.BlockCopy(BytesByHeader(i), 0, TempArray, OffsetByHeader(i), LengthByHeader(i))
                ElseIf i = deletedindex Then
                ElseIf i > deletedindex Then
                    Buffer.BlockCopy(BitConverter.GetBytes(OffsetByHeader(i)), 0, TempArray, 8 + (i - 1) * 12 + 0, 4)
                    Buffer.BlockCopy(BitConverter.GetBytes(LengthByHeader(i)), 0, TempArray, 8 + (i - 1) * 12 + 4, 4)
                    Buffer.BlockCopy(BitConverter.GetBytes(ReferenceByHeader(i)), 0, TempArray, 8 + (i - 1) * 12 + 8, 4)
                    Buffer.BlockCopy(BytesByHeader(i), 0, TempArray, OffsetByHeader(i), LengthByHeader(i))
                End If
            Catch ex As Exception
                MessageBox.Show(i & vbNewLine & " " & BytesByHeader(i).Length & " " & LengthByHeader(i) &
            ex.Message)
            End Try
        Next
        File.Copy(TextBoxActiveFile.Text,
          Path.GetDirectoryName(TextBoxActiveFile.Text) & "\" &
          Path.GetFileNameWithoutExtension(TextBoxActiveFile.Text) & ".bak", True)
        File.WriteAllBytes(TextBoxActiveFile.Text, TempArray)
        ProcessString()
    End Sub
#End Region
#Region "Control Processes"
    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericMain.ValueChanged
        ReadMainReference()
    End Sub
    Sub ReadMainReference()
        Dim Tempnumber As Integer = NumericMain.Value
        If ReferenceByHeader.Contains(NumericMain.Value) Then
            TextBoxMain.Text = StringByReference(NumericMain.Value)
            CurrentIndexMain = NumericMain.Value
        ElseIf NumericMain.Value > CurrentIndexMain Then
            While Tempnumber < 50000001
                If ReferenceByHeader.Contains(Tempnumber) Then
                    Exit While
                End If
                Tempnumber = Tempnumber + 1
            End While
            If Tempnumber = 50000001 Then
                NumericMain.Value = CurrentIndexMain
            Else
                NumericMain.Value = Tempnumber
            End If
        ElseIf NumericMain.Value < CurrentIndexMain Then
            While Tempnumber > -1
                If ReferenceByHeader.Contains(Tempnumber) Then
                    Exit While
                End If
                Tempnumber = Tempnumber - 1
            End While
            If Tempnumber = -1 Then
                NumericMain.Value = CurrentIndexMain
            Else
                NumericMain.Value = Tempnumber
            End If
        Else
            NumericMain.Value = CurrentIndexMain
        End If
    End Sub
    Private Sub NumericSecondary_ValueChanged(sender As Object, e As EventArgs) Handles NumericSecondary.ValueChanged
        If ButtonAdd.Text = "Save" Then
            Dim Tempnumber As Integer = NumericSecondary.Value
            If ReferenceByHeader.Contains(NumericSecondary.Value) = False Then
                CurrentIndexSecondary = NumericSecondary.Value
            ElseIf NumericSecondary.Value > CurrentIndexSecondary Then
                While Tempnumber < 50000001
                    If ReferenceByHeader.Contains(Tempnumber) = False Then
                        Exit While
                    End If
                    Tempnumber = Tempnumber + 1
                End While
                If Tempnumber = 50000001 Then
                    NumericSecondary.Value = CurrentIndexSecondary
                Else
                    NumericSecondary.Value = Tempnumber
                End If
            ElseIf NumericSecondary.Value < CurrentIndexSecondary Then
                While Tempnumber > -1
                    If ReferenceByHeader.Contains(Tempnumber) = False Then
                        Exit While
                    End If
                    Tempnumber = Tempnumber - 1
                End While
                If Tempnumber = -1 Then
                    NumericSecondary.Value = CurrentIndexSecondary
                Else
                    NumericSecondary.Value = Tempnumber
                End If
            Else
                NumericSecondary.Value = CurrentIndexSecondary
            End If
        Else
            ReadSecondReference()
        End If
    End Sub
    Sub ReadSecondReference()
        Dim Tempnumber As Integer = NumericSecondary.Value
        If ReferenceByHeader.Contains(NumericSecondary.Value) Then
            TextBoxSecondary.Text = StringByReference(NumericSecondary.Value)
            CurrentIndexSecondary = NumericSecondary.Value
        ElseIf NumericSecondary.Value > CurrentIndexSecondary Then
            While Tempnumber < 1000001
                If ReferenceByHeader.Contains(Tempnumber) Then
                    Exit While
                End If
                Tempnumber = Tempnumber + 1
            End While
            If Tempnumber = 1000001 Then
                NumericSecondary.Value = CurrentIndexSecondary
            Else
                NumericSecondary.Value = Tempnumber
            End If
        ElseIf NumericSecondary.Value < CurrentIndexSecondary Then
            While Tempnumber > -1
                If ReferenceByHeader.Contains(Tempnumber) Then
                    Exit While
                End If
                Tempnumber = Tempnumber - 1
            End While
            If Tempnumber = -1 Then
                NumericSecondary.Value = CurrentIndexSecondary
            Else
                NumericSecondary.Value = Tempnumber
            End If
        Else
            NumericSecondary.Value = CurrentIndexSecondary
        End If
    End Sub
    Private Sub ButtonDelete_Click(sender As Object, e As EventArgs) Handles ButtonDelete.Click
        If ButtonDelete.Text = "Delete" Then
            'MessageBox.Show(NumericMain.Value)
            Dim DeletedIndex As Integer = Array.IndexOf(ReferenceByHeader, CInt(NumericMain.Value))
            'MessageBox.Show(DeletedIndex)
            Dim TempIndex As Integer = DeletedIndex + 1
            'MessageBox.Show("Removing Text Bytes")
            'Removing Text Bytes For All Later Arrays
            While TempIndex < OffsetByHeader.Length
                Try
                    OffsetByHeader(TempIndex) = OffsetByHeader(TempIndex) - LengthByHeader(DeletedIndex)
                    TempIndex = TempIndex + 1
                Catch ex As Exception
                    MessageBox.Show(TempIndex & vbNewLine &
                                ex.Message)
                End Try
            End While
            'MessageBox.Show("Removing Header Bytes")
            'Removing the 12 header bytes from all
            For i As Integer = 0 To OffsetByHeader.Length - 1
                OffsetByHeader(i) = OffsetByHeader(i) - 12
            Next
            RebuildFile(DeletedIndex)
        Else 'Text says cancel
            NumericMain.ReadOnly = False
            NumericMain.Enabled = True
            TextBoxMain.ReadOnly = True
            NumericSecondary.ReadOnly = True
            NumericSecondary.Enabled = False
            TextBoxSecondary.ReadOnly = True
            ButtonEdit.Show()
            ButtonAdd.Show()
            ButtonMerge.Show()
            OldText = ""
            ButtonDelete.Text = "Delete"
            ButtonEdit.Text = "Edit"
            ButtonAdd.Text = "Add"
            ButtonMerge.Text = "Merge"
        End If
    End Sub
    Dim OldText As String = ""
    Private Sub ButtonEdit_Click(sender As Object, e As EventArgs) Handles ButtonEdit.Click
        If ButtonEdit.Text = "Edit" Then
            NumericMain.ReadOnly = True
            NumericMain.Enabled = False
            TextBoxMain.ReadOnly = False
            ButtonDelete.Text = "Cancel"
            ButtonAdd.Hide()
            ButtonMerge.Hide()
            OldText = TextBoxMain.Text
            ButtonEdit.Text = "Save"
        Else ' if ButtonEdit.Text = "Save" then
            'Save the File First
            Dim Difference As Integer = TextBoxMain.Text.Length - OldText.Length ' Shorter is - Longer is +
            Dim EditedIndex As Integer = Array.IndexOf(ReferenceByHeader, CInt(NumericMain.Value))
            'Edit the Edited Index
            LengthByHeader(EditedIndex) = LengthByHeader(EditedIndex) + Difference
            Dim NewTextBytes As Byte() = New Byte(LengthByHeader(EditedIndex)) {}
            Buffer.BlockCopy(System.Text.Encoding.ASCII.GetBytes(TextBoxMain.Text), 0, NewTextBytes, 0, LengthByHeader(EditedIndex) - 1)
            BytesByHeader(EditedIndex) = NewTextBytes
            'Adjusting the offsets for the changed difference
            Dim TempIndex As Integer = EditedIndex + 1
            While TempIndex < OffsetByHeader.Length
                Try
                    OffsetByHeader(TempIndex) = OffsetByHeader(TempIndex) + Difference
                    TempIndex = TempIndex + 1
                Catch ex As Exception
                    MessageBox.Show(TempIndex & vbNewLine &
                                ex.Message)
                End Try
            End While
            'Rebuild The File
            RebuildFile()
            'Then reset the menu
            NumericMain.ReadOnly = False
            NumericMain.Enabled = True
            TextBoxMain.ReadOnly = True
            ButtonDelete.Text = "Delete"
            ButtonAdd.Show()
            ButtonMerge.Show()
            OldText = ""
            ButtonEdit.Text = "Edit"
        End If
    End Sub
    Private Sub ButtonAdd_Click(sender As Object, e As EventArgs) Handles ButtonAdd.Click
        If ButtonAdd.Text = "Add" Then
            NumericMain.ReadOnly = True
            NumericMain.Enabled = False
            NumericSecondary.ReadOnly = False
            NumericSecondary.Enabled = True
            TextBoxSecondary.ReadOnly = False
            ButtonDelete.Text = "Cancel"
            ButtonEdit.Hide()
            ButtonMerge.Hide()
            ButtonAdd.Text = "Save"
            For i As Integer = NumericSecondary.Value To 1000001
                If i < 1000001 Then
                    If ReferenceByHeader.Contains(i) = False Then
                        NumericSecondary.Value = i
                        Exit For
                    End If
                Else
                    NumericSecondary.Value = 0
                End If
            Next
            If NumericSecondary.Value = 0 Then
                For i As Integer = 0 To NumericSecondary.Value
                    If ReferenceByHeader.Contains(i) = False Then
                        NumericSecondary.Value = i
                        Exit For
                    End If
                Next
            End If
        Else ' if ButtonEdit.Text = "Save" then
            'Check if new string would pass max length
            'If (CurrentLength + 12 + TextBoxSecondary.Text.Length + 1) > MaxReadableLength Then
            'Dim result As Integer = MessageBox.Show("New string will not fit!", "To exit add hit cancel.", MessageBoxButtons.OKCancel)
            'If result = DialogResult.OK Then
            'Exit Sub
            'Else ' cancel
            NumericMain.ReadOnly = False
            NumericMain.Enabled = True
            NumericSecondary.ReadOnly = True
            NumericSecondary.Enabled = False
            TextBoxSecondary.ReadOnly = True
            ButtonDelete.Text = "Delete"
            ButtonEdit.Show()
            ButtonMerge.Show()
            ButtonAdd.Text = "Add"
            Exit Sub
            'End If
            'End If
            ' New String Fits
            'Adding the new string
            Dim OldOffsetByHeader As Integer() = OffsetByHeader
            ReDim OffsetByHeader(OffsetByHeader.Length)
            Dim OldLengthByHeader As Integer() = LengthByHeader
            ReDim LengthByHeader(LengthByHeader.Length)
            Dim OldReferenceByHeader As Integer() = ReferenceByHeader
            ReDim ReferenceByHeader(ReferenceByHeader.Length)
            'Dim OldBytesByHeader()() As Byte = BytesByHeader
            Dim WorkingIndex As Integer = 0
            Dim TempIndex As Integer = OldReferenceByHeader.Length
            Try
                Do While TempIndex > 0
                    TempIndex = TempIndex - 1
                    'Try
                    If OldReferenceByHeader(TempIndex) > NumericSecondary.Value Then
                        OffsetByHeader(TempIndex + 1) = OldOffsetByHeader(TempIndex) + TextBoxSecondary.Text.Length + 1
                        LengthByHeader(TempIndex + 1) = OldLengthByHeader(TempIndex)
                        ReferenceByHeader(TempIndex + 1) = OldReferenceByHeader(TempIndex)
                        'Dim temparray As Byte() = New Byte(OldBytesByHeader(TempIndex).Length - 1) {}
                        'BytesByHeader(TempIndex + 1) = temparray
                        BytesByHeader(TempIndex + 1) = BytesByHeader(TempIndex)
                    ElseIf OldReferenceByHeader(TempIndex) < NumericSecondary.Value Then
                        If WorkingIndex = 0 Then 'Insert new string and copy the last string
                            WorkingIndex = TempIndex
                            OffsetByHeader(TempIndex + 1) = OldOffsetByHeader(TempIndex) + OldLengthByHeader(TempIndex)
                            LengthByHeader(TempIndex + 1) = TextBoxSecondary.Text.Length + 1
                            ReferenceByHeader(TempIndex + 1) = NumericSecondary.Value
                            Dim NewTextBytes As Byte() = New Byte(LengthByHeader(TempIndex + 1) - 1) {}
                            Buffer.BlockCopy(System.Text.Encoding.ASCII.GetBytes(TextBoxSecondary.Text), 0, NewTextBytes, 0, LengthByHeader(TempIndex + 1) - 1)
                            BytesByHeader(TempIndex + 1) = New Byte(LengthByHeader(TempIndex + 1) - 1) {}
                            BytesByHeader(TempIndex + 1) = NewTextBytes
                            '--------------
                            OffsetByHeader(TempIndex) = OldOffsetByHeader(TempIndex)
                            LengthByHeader(TempIndex) = OldLengthByHeader(TempIndex)
                            ReferenceByHeader(TempIndex) = OldReferenceByHeader(TempIndex)
                            'BytesByHeader(i + 1) = OldBytesByHeader(i)
                            'Dim temparray As Byte() = New Byte(OldBytesByHeader(TempIndex).Length - 1) {}
                            'BytesByHeader(TempIndex + 1) = temparray
                            BytesByHeader(TempIndex) = BytesByHeader(TempIndex)
                            'Array.Copy(OldBytesByHeader(i), 0, BytesByHeader(i + 1), 0, OldBytesByHeader(i).Length - 1)
                        Else
                            OffsetByHeader(TempIndex) = OldOffsetByHeader(TempIndex)
                            LengthByHeader(TempIndex) = OldLengthByHeader(TempIndex)
                            ReferenceByHeader(TempIndex) = OldReferenceByHeader(TempIndex)
                            BytesByHeader(TempIndex) = BytesByHeader(TempIndex)
                            'Array.Copy(OldBytesByHeader(i), 0, BytesByHeader(i + 1), 0, OldBytesByHeader(i).Length - 1)
                        End If
                    End If
                Loop
            Catch ex As Exception
                MessageBox.Show(TempIndex & vbNewLine & ex.Message)
            End Try
            'For i As Integer = 0 To OldReferenceByHeader.Length - 1
            ''Try
            'If OldReferenceByHeader(i) < NumericSecondary.Value Then
            '    OffsetByHeader(i) = OldOffsetByHeader(i)
            '    LengthByHeader(i) = OldLengthByHeader(i)
            '    ReferenceByHeader(i) = OldReferenceByHeader(i)
            '    BytesByHeader(i) = OldBytesByHeader(i)
            'ElseIf OldReferenceByHeader(i) > NumericSecondary.Value Then
            '    If WorkingIndex = 0 Then 'Insert new string and copy the last string
            '        WorkingIndex = i
            '        OffsetByHeader(i) = OldOffsetByHeader(OldOffsetByHeader.Length - 1) + OldLengthByHeader(OldLengthByHeader.Length - 1)
            '        LengthByHeader(i) = TextBoxSecondary.Text.Length + 1
            '        ReferenceByHeader(i) = NumericSecondary.Value
            '        Dim NewTextBytes As Byte() = New Byte(LengthByHeader(i) - 1) {}
            '        Buffer.BlockCopy(System.Text.Encoding.ASCII.GetBytes(TextBoxSecondary.Text), 0, NewTextBytes, 0, LengthByHeader(i) - 1)
            '        BytesByHeader(i) = New Byte(LengthByHeader(i) - 1) {}
            '        BytesByHeader(i) = NewTextBytes
            '        '--------------
            '        OffsetByHeader(i + 1) = OldOffsetByHeader(i)
            '        LengthByHeader(i + 1) = OldLengthByHeader(i)
            '        ReferenceByHeader(i + 1) = OldReferenceByHeader(i)
            '        'BytesByHeader(i + 1) = OldBytesByHeader(i)
            '        Dim temparray As Byte() = New Byte(OldBytesByHeader(i).Length - 1) {}
            '        BytesByHeader(i + 1) = temparray
            '        BytesByHeader(i + 1) = OldBytesByHeader(i)
            '        'Array.Copy(OldBytesByHeader(i), 0, BytesByHeader(i + 1), 0, OldBytesByHeader(i).Length - 1)
            '    Else
            '        OffsetByHeader(i + 1) = OldOffsetByHeader(i)
            '        LengthByHeader(i + 1) = OldLengthByHeader(i)
            '        ReferenceByHeader(i + 1) = OldReferenceByHeader(i)
            '        Dim temparray As Byte() = New Byte(OldBytesByHeader(i).Length - 1) {}
            '        BytesByHeader(i + 1) = temparray
            '        BytesByHeader(i + 1) = OldBytesByHeader(i)
            '        MessageBox.Show(System.Text.Encoding.ASCII.GetString(OldBytesByHeader(i)))
            '        'Array.Copy(OldBytesByHeader(i), 0, BytesByHeader(i + 1), 0, OldBytesByHeader(i).Length - 1)
            '    End If
            'End If
            'Catch ex As Exception
            'MessageBox.Show(i & vbNewLine & ex.Message)
            'End Try
            'OffsetByHeader(OffsetByHeader.Length - 1) = OffsetByHeader(OffsetByHeader.Length - 2) + LengthByHeader(OffsetByHeader.Length - 2)
            'LengthByHeader(LengthByHeader.Length - 1) = TextBoxSecondary.Text.Length + 1
            'ReferenceByHeader(ReferenceByHeader.Length - 1) = NumericSecondary.Value
            'Dim NewTextBytes As Byte() = New Byte(LengthByHeader(LengthByHeader.Length - 1)) {}
            'Buffer.BlockCopy(System.Text.Encoding.ASCII.GetBytes(TextBoxSecondary.Text), 0, NewTextBytes, 0, LengthByHeader(LengthByHeader.Length - 1) - 1)
            'BytesByHeader(LengthByHeader.Length - 1) = NewTextBytes
            'Adding the 12 header bytes from all
            For i As Integer = 0 To OffsetByHeader.Length - 1
                OffsetByHeader(i) = OffsetByHeader(i) + 12
            Next

            'Rebuild The File
            RebuildFile()
            'reseting the menu
            NumericMain.ReadOnly = False
            NumericMain.Enabled = True
            NumericSecondary.ReadOnly = True
            NumericSecondary.Enabled = False
            TextBoxSecondary.ReadOnly = True
            ButtonDelete.Text = "Delete"
            ButtonEdit.Show()
            ButtonMerge.Show()
            ButtonAdd.Text = "Add"
        End If
    End Sub
    Private Sub ButtonMerge_Click(sender As Object, e As EventArgs) Handles ButtonMerge.Click
        If ButtonMerge.Text = "Merge" Then
            NumericMain.ReadOnly = True
            NumericMain.Enabled = False
            NumericSecondary.ReadOnly = False
            NumericSecondary.Enabled = True
            ButtonDelete.Text = "Cancel"
            ButtonEdit.Hide()
            ButtonAdd.Hide()
            ButtonMerge.Text = "Save"
            If ReferenceByHeader(0) <> 0 AndAlso NumericSecondary.Value = 0 Then
                NumericMain.Value = ReferenceByHeader(0)
            End If
        Else ' if ButtonEdit.Text = "Save" then
            Dim RemainingIndex As Integer = Array.IndexOf(ReferenceByHeader, CInt(NumericMain.Value))
            Dim RemovedIndex As Integer = Array.IndexOf(ReferenceByHeader, CInt(NumericSecondary.Value))
            Dim RemovedLength As Integer = LengthByHeader(RemovedIndex)
            If (LengthByHeader(RemainingIndex) = LengthByHeader(RemovedIndex)) = False Then
                Dim result = MessageBox.Show("String lengths do not match!" & vbNewLine &
                                "First string length: " & LengthByHeader(RemainingIndex) & vbNewLine &
                                "Second string length: " & LengthByHeader(RemovedIndex) & vbNewLine &
                                "Continue?", "Length mismatch.", MessageBoxButtons.YesNoCancel)
                If result = DialogResult.Yes Then
                    LengthByHeader(RemovedIndex) = LengthByHeader(RemainingIndex)
                ElseIf result = DialogResult.Cancel Then
                    'Reset Menu
                    NumericMain.ReadOnly = False
                    NumericMain.Enabled = True
                    NumericSecondary.ReadOnly = True
                    NumericSecondary.Enabled = False
                    ButtonDelete.Text = "Delete"
                    ButtonEdit.Show()
                    ButtonAdd.Show()
                    ButtonMerge.Text = "Merge"
                    Exit Sub
                Else 'no click
                    Exit Sub
                End If
            End If
            'Make File Changes
            'Making the new offsets merge
            OffsetByHeader(RemovedIndex) = OffsetByHeader(RemainingIndex)
            BytesByHeader(RemovedIndex) = BytesByHeader(RemainingIndex)
            TextBoxSecondary.Text = TextBoxMain.Text
            'Adjusting the offsets for the removed string
            Dim TempIndex As Integer = RemovedIndex + 1
            While TempIndex < OffsetByHeader.Length
                Try
                    OffsetByHeader(TempIndex) = OffsetByHeader(TempIndex) - RemovedLength
                    TempIndex = TempIndex + 1
                Catch ex As Exception
                    MessageBox.Show(TempIndex & vbNewLine &
                                ex.Message)
                End Try
            End While
            'Rebuild The File
            RebuildFile()
            'Reset Menu
            NumericMain.ReadOnly = False
            NumericMain.Enabled = True
            NumericSecondary.ReadOnly = True
            NumericSecondary.Enabled = False
            ButtonDelete.Text = "Delete"
            ButtonEdit.Show()
            ButtonAdd.Show()
            ButtonMerge.Text = "Merge"
        End If
    End Sub
#End Region
End Class