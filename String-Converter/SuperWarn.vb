Public Class SuperWarn
    Public Dontask As Boolean
    Public SavedResult As Boolean

    Shared Sub resetsettings()
        SuperWarn.Dontask = False
        SuperWarn.CheckBox1.Checked = False
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Dontask = CheckBox1.Checked
    End Sub

    Private Sub SuperWarn_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = Hex(Form2.NumericMain.Value)
        TextBox2.Text = Form2.OldText
    End Sub
End Class