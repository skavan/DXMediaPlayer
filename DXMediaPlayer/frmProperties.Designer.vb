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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmProperties))
        Me.PG1 = New DevExpress.XtraVerticalGrid.PropertyGridControl()
        Me.PanelHeader = New DevExpress.XtraEditors.PanelControl()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.ButtonLPH_R = New DevExpress.XtraEditors.SimpleButton()
        Me.ButtonLPH_L = New DevExpress.XtraEditors.SimpleButton()
        CType(Me.PG1,System.ComponentModel.ISupportInitialize).BeginInit
        CType(Me.PanelHeader,System.ComponentModel.ISupportInitialize).BeginInit
        Me.PanelHeader.SuspendLayout
        Me.SuspendLayout
        '
        'PG1
        '
        Me.PG1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PG1.Location = New System.Drawing.Point(0, 54)
        Me.PG1.Name = "PG1"
        Me.PG1.Size = New System.Drawing.Size(666, 763)
        Me.PG1.TabIndex = 0
        '
        'PanelHeader
        '
        Me.PanelHeader.Controls.Add(Me.LabelControl1)
        Me.PanelHeader.Controls.Add(Me.ButtonLPH_R)
        Me.PanelHeader.Controls.Add(Me.ButtonLPH_L)
        Me.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelHeader.Location = New System.Drawing.Point(0, 0)
        Me.PanelHeader.Name = "PanelHeader"
        Me.PanelHeader.Size = New System.Drawing.Size(666, 54)
        Me.PanelHeader.TabIndex = 5
        '
        'LabelControl1
        '
        Me.LabelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.LabelControl1.AutoEllipsis = true
        Me.LabelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.LabelControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LabelControl1.Location = New System.Drawing.Point(51, 3)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Padding = New System.Windows.Forms.Padding(6, 0, 6, 6)
        Me.LabelControl1.Size = New System.Drawing.Size(564, 48)
        Me.LabelControl1.TabIndex = 2
        Me.LabelControl1.Text = "Properties"
        '
        'ButtonLPH_R
        '
        Me.ButtonLPH_R.AllowFocus = false
        Me.ButtonLPH_R.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.[True]
        Me.ButtonLPH_R.Dock = System.Windows.Forms.DockStyle.Right
        Me.ButtonLPH_R.Image = CType(resources.GetObject("ButtonLPH_R.Image"),System.Drawing.Image)
        Me.ButtonLPH_R.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.ButtonLPH_R.Location = New System.Drawing.Point(615, 3)
        Me.ButtonLPH_R.Margin = New System.Windows.Forms.Padding(6)
        Me.ButtonLPH_R.Name = "ButtonLPH_R"
        Me.ButtonLPH_R.Padding = New System.Windows.Forms.Padding(6)
        Me.ButtonLPH_R.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.[False]
        Me.ButtonLPH_R.Size = New System.Drawing.Size(48, 48)
        Me.ButtonLPH_R.TabIndex = 1
        Me.ButtonLPH_R.Tag = "UsePadding"
        '
        'ButtonLPH_L
        '
        Me.ButtonLPH_L.AllowFocus = false
        Me.ButtonLPH_L.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.[True]
        Me.ButtonLPH_L.Dock = System.Windows.Forms.DockStyle.Left
        Me.ButtonLPH_L.Image = CType(resources.GetObject("ButtonLPH_L.Image"),System.Drawing.Image)
        Me.ButtonLPH_L.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter
        Me.ButtonLPH_L.Location = New System.Drawing.Point(3, 3)
        Me.ButtonLPH_L.Margin = New System.Windows.Forms.Padding(6)
        Me.ButtonLPH_L.Name = "ButtonLPH_L"
        Me.ButtonLPH_L.Padding = New System.Windows.Forms.Padding(6)
        Me.ButtonLPH_L.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.[False]
        Me.ButtonLPH_L.Size = New System.Drawing.Size(48, 48)
        Me.ButtonLPH_L.TabIndex = 0
        Me.ButtonLPH_L.Tag = "UsePadding"
        '
        'frmProperties
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9!, 20!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(666, 817)
        Me.Controls.Add(Me.PG1)
        Me.Controls.Add(Me.PanelHeader)
        Me.Name = "frmProperties"
        Me.Text = "frmProperties"
        CType(Me.PG1,System.ComponentModel.ISupportInitialize).EndInit
        CType(Me.PanelHeader,System.ComponentModel.ISupportInitialize).EndInit
        Me.PanelHeader.ResumeLayout(false)
        Me.ResumeLayout(false)

End Sub

    Friend WithEvents PG1 As DevExpress.XtraVerticalGrid.PropertyGridControl
    Friend WithEvents PanelHeader As DevExpress.XtraEditors.PanelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ButtonLPH_R As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents ButtonLPH_L As DevExpress.XtraEditors.SimpleButton
End Class
