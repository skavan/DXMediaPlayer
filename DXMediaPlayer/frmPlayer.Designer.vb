<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPlayer
    Inherits frmMediaTemplate

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.SuspendLayout
        '
        'frmPlayer
        '
        Me.Appearance.Options.UseFont = true
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144!, 144!)
        Me.ClientSize = New System.Drawing.Size(1200, 750)
        Me.Name = "frmPlayer"
        Me.Text = "frmPlayer"
        Me.ResumeLayout(false)

End Sub

End Class
