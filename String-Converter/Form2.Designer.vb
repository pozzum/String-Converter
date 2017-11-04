<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        Me.TextBoxActiveFile = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.NumericMain = New System.Windows.Forms.NumericUpDown()
        Me.TextBoxMain = New System.Windows.Forms.TextBox()
        Me.ButtonDelete = New System.Windows.Forms.Button()
        Me.ButtonEdit = New System.Windows.Forms.Button()
        Me.ButtonAdd = New System.Windows.Forms.Button()
        Me.ButtonMerge = New System.Windows.Forms.Button()
        Me.NumericSecondary = New System.Windows.Forms.NumericUpDown()
        Me.TextBoxSecondary = New System.Windows.Forms.TextBox()
        Me.LabelLength = New System.Windows.Forms.Label()
        CType(Me.NumericMain, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumericSecondary, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextBoxActiveFile
        '
        Me.TextBoxActiveFile.Location = New System.Drawing.Point(74, 12)
        Me.TextBoxActiveFile.Name = "TextBoxActiveFile"
        Me.TextBoxActiveFile.ReadOnly = True
        Me.TextBoxActiveFile.Size = New System.Drawing.Size(94, 20)
        Me.TextBoxActiveFile.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Active File"
        '
        'NumericMain
        '
        Me.NumericMain.Hexadecimal = True
        Me.NumericMain.Location = New System.Drawing.Point(12, 38)
        Me.NumericMain.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.NumericMain.Name = "NumericMain"
        Me.NumericMain.Size = New System.Drawing.Size(156, 20)
        Me.NumericMain.TabIndex = 2
        '
        'TextBoxMain
        '
        Me.TextBoxMain.Enabled = False
        Me.TextBoxMain.Location = New System.Drawing.Point(12, 64)
        Me.TextBoxMain.Name = "TextBoxMain"
        Me.TextBoxMain.ReadOnly = True
        Me.TextBoxMain.Size = New System.Drawing.Size(156, 20)
        Me.TextBoxMain.TabIndex = 3
        '
        'ButtonDelete
        '
        Me.ButtonDelete.Location = New System.Drawing.Point(12, 90)
        Me.ButtonDelete.Name = "ButtonDelete"
        Me.ButtonDelete.Size = New System.Drawing.Size(75, 23)
        Me.ButtonDelete.TabIndex = 4
        Me.ButtonDelete.Text = "Delete"
        Me.ButtonDelete.UseVisualStyleBackColor = True
        '
        'ButtonEdit
        '
        Me.ButtonEdit.Location = New System.Drawing.Point(93, 90)
        Me.ButtonEdit.Name = "ButtonEdit"
        Me.ButtonEdit.Size = New System.Drawing.Size(75, 23)
        Me.ButtonEdit.TabIndex = 5
        Me.ButtonEdit.Text = "Edit"
        Me.ButtonEdit.UseVisualStyleBackColor = True
        '
        'ButtonAdd
        '
        Me.ButtonAdd.Location = New System.Drawing.Point(12, 119)
        Me.ButtonAdd.Name = "ButtonAdd"
        Me.ButtonAdd.Size = New System.Drawing.Size(75, 23)
        Me.ButtonAdd.TabIndex = 6
        Me.ButtonAdd.Text = "Add"
        Me.ButtonAdd.UseVisualStyleBackColor = True
        '
        'ButtonMerge
        '
        Me.ButtonMerge.Location = New System.Drawing.Point(93, 119)
        Me.ButtonMerge.Name = "ButtonMerge"
        Me.ButtonMerge.Size = New System.Drawing.Size(75, 23)
        Me.ButtonMerge.TabIndex = 7
        Me.ButtonMerge.Text = "Merge"
        Me.ButtonMerge.UseVisualStyleBackColor = True
        '
        'NumericSecondary
        '
        Me.NumericSecondary.Enabled = False
        Me.NumericSecondary.Hexadecimal = True
        Me.NumericSecondary.Location = New System.Drawing.Point(12, 148)
        Me.NumericSecondary.Maximum = New Decimal(New Integer() {1000000, 0, 0, 0})
        Me.NumericSecondary.Name = "NumericSecondary"
        Me.NumericSecondary.ReadOnly = True
        Me.NumericSecondary.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.NumericSecondary.Size = New System.Drawing.Size(156, 20)
        Me.NumericSecondary.TabIndex = 8
        '
        'TextBoxSecondary
        '
        Me.TextBoxSecondary.Enabled = False
        Me.TextBoxSecondary.Location = New System.Drawing.Point(12, 174)
        Me.TextBoxSecondary.Name = "TextBoxSecondary"
        Me.TextBoxSecondary.ReadOnly = True
        Me.TextBoxSecondary.Size = New System.Drawing.Size(156, 20)
        Me.TextBoxSecondary.TabIndex = 9
        '
        'LabelLength
        '
        Me.LabelLength.AutoSize = True
        Me.LabelLength.Location = New System.Drawing.Point(9, 197)
        Me.LabelLength.Name = "LabelLength"
        Me.LabelLength.Size = New System.Drawing.Size(113, 13)
        Me.LabelLength.TabIndex = 10
        Me.LabelLength.Text = "805194/805194 Bytes"
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(180, 218)
        Me.Controls.Add(Me.LabelLength)
        Me.Controls.Add(Me.TextBoxSecondary)
        Me.Controls.Add(Me.NumericSecondary)
        Me.Controls.Add(Me.ButtonMerge)
        Me.Controls.Add(Me.ButtonAdd)
        Me.Controls.Add(Me.ButtonEdit)
        Me.Controls.Add(Me.ButtonDelete)
        Me.Controls.Add(Me.TextBoxMain)
        Me.Controls.Add(Me.NumericMain)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextBoxActiveFile)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form2"
        Me.Text = "String Editor"
        CType(Me.NumericMain, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumericSecondary, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextBoxActiveFile As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents NumericMain As NumericUpDown
    Friend WithEvents TextBoxMain As TextBox
    Friend WithEvents ButtonDelete As Button
    Friend WithEvents ButtonEdit As Button
    Friend WithEvents ButtonAdd As Button
    Friend WithEvents ButtonMerge As Button
    Friend WithEvents NumericSecondary As NumericUpDown
    Friend WithEvents TextBoxSecondary As TextBox
    Friend WithEvents LabelLength As Label
End Class
