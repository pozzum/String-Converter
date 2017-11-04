Imports System.IO
Public Class Form2
#Region "Main Form Processes"
    Dim OffsetByHeader As Integer()
    Dim LengthByHeader As Integer()
    Dim ReferenceByHeader As Integer()
    Dim BytesByHeader(1000000)() As Byte
    Dim StringByReference As String()
    Const MaxReadableLength = 805194
    Dim CurrentIndexMain = 0
    Dim CurrentIndexSecondary = 0
    Dim CurrentLength = 0


    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBoxActiveFile.Text = Form1.SentFile
        ProcessString(True)
        ReadMainReference()
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
            StringByReference = New String(1000000) {}
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
            LabelLength.Text = CurrentLength & "\" & MaxReadableLength & " Bytes"
            If ReferenceByHeader(0) <> 0 AndAlso NumericMain.Value = 0 Then
                NumericMain.Value = ReferenceByHeader(0)
            End If
        End If
    End Sub
    Sub RebuildFile(Optional deletedindex As Integer = 1000000)
        'Moving Removed Index To Back And Removing
        Dim TempArray() As Byte = New Byte(MaxReadableLength - 1) {}
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
                MessageBox.Show(i & vbNewLine &
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
            While Tempnumber < 1000001
                If ReferenceByHeader.Contains(Tempnumber) Then
                    Exit While
                End If
                Tempnumber = Tempnumber + 1
            End While
            If Tempnumber = 1000001 Then
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
                While Tempnumber < 1000001
                    If ReferenceByHeader.Contains(Tempnumber) = False Then
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
        End If
    End Sub
    Private Sub ButtonDelete_Click(sender As Object, e As EventArgs) Handles ButtonDelete.Click
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
    End Sub
    Dim OldText As String = ""
    Private Sub ButtonEdit_Click(sender As Object, e As EventArgs) Handles ButtonEdit.Click
        If ButtonEdit.Text = "Edit" Then
            NumericMain.ReadOnly = True
            NumericMain.Enabled = False
            TextBoxMain.ReadOnly = False
            TextBoxMain.Enabled = True
            ButtonDelete.Hide()
            ButtonAdd.Hide()
            ButtonMerge.Hide()
            OldText = TextBoxMain.Text
            ButtonEdit.Text = "Save"
        Else ' if ButtonEdit.Text = "Save" then
            'Save the File First
            Dim Difference As Integer = TextBoxMain.Text.Length - OldText.Length ' Shorter is - Longer is +
            MessageBox.Show(Difference)
            Dim EditedIndex As Integer = Array.IndexOf(ReferenceByHeader, CInt(NumericMain.Value))
            'Edit the Edited Index
            LengthByHeader(EditedIndex) = LengthByHeader(EditedIndex) + Difference
            Dim NewTextBytes As Byte() = New Byte(LengthByHeader(EditedIndex)) {}
            Buffer.BlockCopy(System.Text.Encoding.ASCII.GetBytes(TextBoxMain.Text), 0, NewTextBytes, 0, LengthByHeader(EditedIndex) - 1)
            BytesByHeader(EditedIndex) = NewTextBytes
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
            TextBoxMain.Enabled = False
            ButtonDelete.Show()
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
            TextBoxSecondary.Enabled = True
            ButtonDelete.Hide()
            ButtonEdit.Hide()
            ButtonMerge.Hide()
            ButtonAdd.Text = "Save"
            For i As Integer = 0 To 1000001
                If ReferenceByHeader.Contains(i) = False Then
                    NumericSecondary.Value = i
                    Exit For
                End If
            Next
        Else ' if ButtonEdit.Text = "Save" then
            'Check if new string would pass max length
            If (CurrentLength + 12 + TextBoxSecondary.Text.Length + 1) > MaxReadableLength Then
                Dim result As Integer = MessageBox.Show("New string will not fit!", "To exit add hit cancel.", MessageBoxButtons.OKCancel)
                If result = DialogResult.OK Then
                    Exit Sub
                Else ' cancel
                    NumericMain.ReadOnly = False
                    NumericMain.Enabled = True
                    NumericSecondary.ReadOnly = True
                    NumericSecondary.Enabled = False
                    TextBoxSecondary.ReadOnly = True
                    TextBoxSecondary.Enabled = False
                    ButtonDelete.Show()
                    ButtonEdit.Show()
                    ButtonMerge.Show()
                    ButtonAdd.Text = "Add"
                    Exit Sub
                End If
            End If
            ' New String Fits
            'Adding the new string
            ReDim Preserve OffsetByHeader(OffsetByHeader.Length)
            ReDim Preserve LengthByHeader(LengthByHeader.Length)
            ReDim Preserve ReferenceByHeader(ReferenceByHeader.Length)
            OffsetByHeader(OffsetByHeader.Length - 1) = OffsetByHeader(OffsetByHeader.Length - 2) + LengthByHeader(OffsetByHeader.Length - 2)
            LengthByHeader(LengthByHeader.Length - 1) = TextBoxSecondary.Text.Length + 1
            ReferenceByHeader(ReferenceByHeader.Length - 1) = NumericSecondary.Value
            Dim NewTextBytes As Byte() = New Byte(LengthByHeader(LengthByHeader.Length - 1)) {}
            Buffer.BlockCopy(System.Text.Encoding.ASCII.GetBytes(TextBoxSecondary.Text), 0, NewTextBytes, 0, LengthByHeader(LengthByHeader.Length - 1) - 1)
            BytesByHeader(LengthByHeader.Length - 1) = NewTextBytes
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
            TextBoxSecondary.Enabled = False
            ButtonDelete.Show()
            ButtonEdit.Show()
            ButtonMerge.Show()
            ButtonAdd.Text = "Add"
        End If
    End Sub

#End Region
End Class