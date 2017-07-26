<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.OpenCSVDialog = New System.Windows.Forms.OpenFileDialog()
        Me.SavePACHDialog = New System.Windows.Forms.SaveFileDialog()
        Me.Num_String_Count = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Num_Max_Length = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Num_Start_Num = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SaveCSVDialog = New System.Windows.Forms.SaveFileDialog()
        Me.OpenPACHDialog = New System.Windows.Forms.OpenFileDialog()
        CType(Me.Num_String_Count, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Num_Max_Length, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Num_Start_Num, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(12, 12)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(84, 23)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Build Pach"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'OpenCSVDialog
        '
        Me.OpenCSVDialog.FileName = "*.csv"
        Me.OpenCSVDialog.Filter = "Comma Seperated Values|*.csv|All Files|*.*"
        '
        'SavePACHDialog
        '
        Me.SavePACHDialog.FileName = "0001.pach"
        Me.SavePACHDialog.Filter = "Pach File|*.pac;*.pach;*.dat|All Files|*.*"
        '
        'Num_String_Count
        '
        Me.Num_String_Count.Location = New System.Drawing.Point(13, 42)
        Me.Num_String_Count.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.Num_String_Count.Name = "Num_String_Count"
        Me.Num_String_Count.Size = New System.Drawing.Size(83, 20)
        Me.Num_String_Count.TabIndex = 2
        Me.Num_String_Count.Value = New Decimal(New Integer() {4249, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(102, 44)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "New Strings"
        '
        'Num_Max_Length
        '
        Me.Num_Max_Length.Location = New System.Drawing.Point(13, 69)
        Me.Num_Max_Length.Name = "Num_Max_Length"
        Me.Num_Max_Length.Size = New System.Drawing.Size(83, 20)
        Me.Num_Max_Length.TabIndex = 4
        Me.Num_Max_Length.Value = New Decimal(New Integer() {30, 0, 0, 0})
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(102, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "String Length"
        '
        'Num_Start_Num
        '
        Me.Num_Start_Num.Location = New System.Drawing.Point(13, 95)
        Me.Num_Start_Num.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
        Me.Num_Start_Num.Name = "Num_Start_Num"
        Me.Num_Start_Num.Size = New System.Drawing.Size(83, 20)
        Me.Num_Start_Num.TabIndex = 6
        Me.Num_Start_Num.Value = New Decimal(New Integer() {13734, 0, 0, 0})
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(102, 97)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(81, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "First New String"
        '
        'TextBox1
        '
        Me.TextBox1.Enabled = False
        Me.TextBox1.Location = New System.Drawing.Point(13, 121)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(83, 20)
        Me.TextBox1.TabIndex = 8
        Me.TextBox1.Text = "35A6"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(102, 124)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(90, 13)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Hex of First String"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(102, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 10
        Me.Button1.Text = "Extract csv"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Enabled = False
        Me.TextBox2.Location = New System.Drawing.Point(13, 147)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(83, 20)
        Me.TextBox2.TabIndex = 11
        Me.TextBox2.Text = "4640"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(102, 150)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(91, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Hex of Last String"
        '
        'SaveCSVDialog
        '
        Me.SaveCSVDialog.FileName = "*.csv"
        Me.SaveCSVDialog.Filter = "Comma Seperated Values|*.csv|All Files|*.*"
        '
        'OpenPACHDialog
        '
        Me.OpenPACHDialog.FileName = "0001.pach"
        Me.OpenPACHDialog.Filter = "Pach File|*.pac;*.pach;*.dat|All Files|*.*"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(208, 181)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Num_Start_Num)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Num_Max_Length)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Num_String_Count)
        Me.Controls.Add(Me.Button2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form1"
        Me.Text = "String Converter"
        CType(Me.Num_String_Count, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Num_Max_Length, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Num_Start_Num, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button2 As Button
    Friend WithEvents OpenCSVDialog As OpenFileDialog
    Friend WithEvents SavePACHDialog As SaveFileDialog
    Friend WithEvents Num_String_Count As NumericUpDown
    Friend WithEvents Label1 As Label
    Friend WithEvents Num_Max_Length As NumericUpDown
    Friend WithEvents Label2 As Label
    Friend WithEvents Num_Start_Num As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents SaveCSVDialog As SaveFileDialog
    Friend WithEvents OpenPACHDialog As OpenFileDialog
End Class
