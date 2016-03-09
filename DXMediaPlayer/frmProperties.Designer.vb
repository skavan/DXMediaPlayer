<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProperties
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
        Me.PG1 = New DevExpress.XtraVerticalGrid.PropertyGridControl()
        CType(Me.PG1,System.ComponentModel.ISupportInitialize).BeginInit
        Me.SuspendLayout
        '
        'PG1
        '
        Me.PG1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PG1.Location = New System.Drawing.Point(0, 0)
        Me.PG1.Name = "PG1"
        Me.PG1.Size = New System.Drawing.Size(666, 817)
        Me.PG1.TabIndex = 0
        '
        'frmProperties
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9!, 20!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(666, 817)
        Me.Controls.Add(Me.PG1)
        Me.Name = "frmProperties"
        Me.Text = "frmProperties"
        CType(Me.PG1,System.ComponentModel.ISupportInitialize).EndInit
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents PG1 As DevExpress.XtraVerticalGrid.PropertyGridControl
End Class
