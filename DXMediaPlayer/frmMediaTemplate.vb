Imports DevExpress.MyExtensions
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Tile
Imports DevExpress.XtraGrid.Views.Tile.ViewInfo

Public Class frmMediaTemplate

#Region "Variables and Classes"

    '// styling related
    Dim dicSkins As New Dictionary(Of String, SkinStyler)           'Just convenience, so I can manage reskinning easily
    Dim commonSkins As New DevExpress.Skins.CommonSkins
    Dim ribbonSkins As New DevExpress.Skins.RibbonSkins
    Dim dashboardSkins As New DevExpress.Skins.DashboardSkins
    Dim AppBasefont As Font = Me.Font
    
    Dim WithEvents tileScrollBar As DevExpress.XtraGrid.Scrolling.VCrkScrollBar 'The grid/TileView scrollbar
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMediaTemplate))

    '// scale related
    Dim currentScaleFactor As Single = 1
    Dim btnImageScaleFactor As SizeF = New SizeF(0.85, 0.85)        'The scale factor applied to button images
    Dim dxScaler As ScaleManager

    Class SkinStyler
        Property IsImage As Boolean
        Property Skins As Object
        Property ElementName As String
        Property ImageIndex As Integer
    End Class

    Enum eForceShowState
        Normal
        ForceShow
        ForceHide
    End Enum

    Dim forceLeft As eForceShowState=eForceShowState.Normal
    Dim forceRight As eForceShowState=eForceShowState.Normal
    Dim lastFormSize As New Size(Me.Width, Me.Height)
#End Region

#Region "Form Load & Initialisation Methods"

    Private Sub frmMediaTemplate_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetModeDPIAware
        '// set afterwards (at cost of extra refresh, but form font is now set
        '// this impacts devexpress controls that haven't had their fonts set in the initialization code
        '// such as the TileViewItems
        'Dim nf As Font = New System.Drawing.Font("Courier New", 12!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        DevExpress.Utils.AppearanceObject.DefaultFont = Me.Font        '// set to form font
        SetSkinStylingOverrides
        AppBasefont = DevExpress.Utils.AppearanceObject.DefaultFont
        dxScaler = New ScaleManager(Me)
        tileScrollBar = TileView1.VScrollBar            '// uses MyTileView, otherwise have to peek inside the Grid or the TileView
    End Sub

    Private Sub frmMediaTemplate_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Dim tileItemElement As TileItemElement = TileView1.SpringTileItemElementWidth(colTitle, True)
    End Sub

#End Region

#Region "Styling"   
    '// a central place to setup skinning overrides.
    Private Sub SetSkinStylingOverrides
        dicSkins.Add(PanelTop.Name, New SkinStyler With {.IsImage = "True",
                                  .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        dicSkins.Add(PanelLeftHeader.Name, New SkinStyler With {.IsImage = "True",
                                  .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        dicSkins.Add(PanelRightHeader.Name, New SkinStyler With {.IsImage = "True",
                                  .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        dicSkins.Add(PanelCenterHeader.Name, New SkinStyler With {.IsImage = "True",
                                  .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        dicSkins.Add(PanelLeftFooter.Name, New SkinStyler With {.IsImage = "True",
                                  .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        'dicSkins.Add(PanelRightHeader.Name, New SkinStyler With {.IsImage = "True",
        '                          .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        dicSkins.Add(PanelCenterFooter.Name, New SkinStyler With {.IsImage = "True",
                                  .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        dicSkins.Add(PanelLeftMin.Name, New SkinStyler With {.IsImage = "True",
                                  .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        dicSkins.Add(PanelRightMin.Name, New SkinStyler With {.IsImage = "True",
                                  .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        dicSkins.Add(PanelLeftXtraHeader.Name, New SkinStyler With {.IsImage = "True",
                                  .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 0})
        dicSkins.Add(TileView1.Name, New SkinStyler With {.IsImage = "True",
                                  .Skins = commonSkins, .ElementName = "HighlightedItem", .ImageIndex = 0})
        dicSkins.Add(PanelCenter.Name, New SkinStyler With {.IsImage = "True",
                                  .Skins = dashboardSkins, .ElementName = "DashboardItemBackground", .ImageIndex = 2})
        'PanelCenter.BackColor = PE1.BackColor
        '// use this tag to flag buttons where we want to rescale the images 
        ButtonLPH_L.Tag = "UsePadding"
        ButtonLPH_R.Tag = "UsePadding"
        TileView1.Appearance.ItemNormal.TextOptions.Trimming=Trimming.EllipsisCharacter
        TileView1.Appearance.ItemNormal.TextOptions.WordWrap= WordWrap.NoWrap
                
        'TileView1.Appearance.ItemFocused.BorderColor = Color.FromArgb(164, 192, 244)
        'TileView1.BorderStyle=DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat
    End Sub
#End Region

#Region "Panel Painting and Button, Label Resizing"

    '// demonstrating overriding paint method and using a skin image 
    Private Sub PanelBackGround_Paint(sender As Object, e As PaintEventArgs) Handles PanelTop.Paint, 
            PanelLeftHeader.Paint, PanelCenterHeader.Paint, PanelRightHeader.Paint, 
            PanelLeftFooter.Paint, PanelCenterFooter.Paint, PanelLeftXtraHeader.Paint,
            PanelLeftMin.Paint, PanelRightMin.Paint , PanelCenter.Paint
        Dim skinStyler As SkinStyler = dicSkins(sender.Name)
        e.Graphics.DrawImage(DrawButtonSkinGraphic(dxScaler.activeLookAndFeel, sender.Bounds, skinStyler.Skins, skinStyler.ElementName, skinStyler.ImageIndex), 0, 0)
    End Sub

    '// adjust size of button image based on the button Padding.
    '// this method is connected to Button SizeChanged Events 
    Private Sub Button_SizeChanged(sender As Object, e As EventArgs)
        Dim btnCtl = sender
        btnCtl.Width = btnCtl.Height
        btnCtl.Image = RescaleImageByPadding(resources.GetObject(btnCtl.Name & ".Image"),
                                            btnCtl.Size, btnCtl.Padding)
    End Sub

    '// not used, but an alternate approach to scaling button images using a scalingfactor
    Private Sub Button_SizeChangedByFactor(sender As Object, e As EventArgs)
        Dim btnCtl = sender
        btnCtl.Width = btnCtl.Height
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form))
        'If resources.GetObject(btnCtl.Name & ".Image") Is Nothing Then Exit Sub
        btnCtl.Image = RescaleImageByScaleFactor(resources.GetObject(btnCtl.Name & ".Image"),
                                                                     btnCtl.Size, btnImageScaleFactor)

    End Sub

    '// when the Grid is resized, change the TileViewItem Size
    Private Sub Grid1_SizeChanged(sender As Object, e As EventArgs) Handles Grid1.SizeChanged
        Dim view As MyTileView = sender.DefaultView
        view.SetHScaledTileViewItemSize(PanelLeftXtraHeader.Height * 1.1)
    End Sub

    '// when the scrollbar is visibly changed, recalc the TileViewItem Size
    Private Sub tileScrollBar_VisibleChanged(sender As Object, e As EventArgs) Handles tileScrollBar.VisibleChanged
        Dim grid As MyGridControl = tileScrollBar.Parent
        Dim view As MyTileView = grid.DefaultView
        view.SetHScaledTileViewItemSize(PanelLeftXtraHeader.Height * 1.1)
    End Sub

    Private Sub PanelLeft_SizeChanged(sender As Object, e As EventArgs) Handles PanelLeft.SizeChanged
        Dim tileItemElement As TileItemElement = TileView1.SpringTileItemElementWidth(colTitle, True)
        RefreshlabelText(LabelXtraItem)
        
    End Sub

    Private Sub PanelCenter_SizeChanged(sender As Object, e As EventArgs) Handles PanelCenter.SizeChanged
        ResizeArtwork
        
    End Sub

    Private Sub RefreshLabelText(ctl As LabelControl)
        Select Case ctl.Name
            Case "LabelXtraItem"
                Dim data As String = GetLabelText(ctl)
                Dim l1 As String = data
                Dim l2 As String =""
                If data.Contains(vbcrlf) Then
                    l1 = data.Split(vbCr)(0)
                    l2 = data.Split(vblf)(1)
                End If
                l1 = TruncateString(l1, ctl, "...")
                l2 = TruncateString(l2, ctl, "...")
                If l2<>"" Then
                    LabelXtraItem.Text = String.Format("<b>{0}</b><br>{1}", l1, l2)
                Else
                    LabelXtraItem.Text = String.Format("<b>{0}</b>",l1)
                End If
        End Select
        
    End Sub
    Private Sub ResizeArtwork
        Dim vPadding As Single = 12
        Dim y As Single = PanelCenter.Height - PanelCenterHeader.Height - PanelCenterFooter.Height - 12
        Dim picHeight As Single = (y-vPadding)/2
        PE1.Height = picHeight
        Dim x As Single = PanelCenter.Width
        Dim xPad As Single = (PanelCenter.Width-picHeight) / 2
        If xPad<0 Then xPad=0
        PE1.Properties.Padding = New Padding(xPad,vPadding,xPad,vPadding)
        'PE1.Refresh
        'Debug.Print("Resized. Width: {0}, Adj Width: {1} ,Height: {2}",PE1.Width,x-picHeight,  picHeight)
    End Sub

    Private Sub ResizePanels
        If Not Me.Visible Then Exit Sub
        If Me.Size = lastFormSize Then Exit Sub
        PanelLayoutSuspendResume(false)
        'PL
        'PL PC
        'PL PC PR
        'PC
        'PC PR
        'PR
        Dim availWidth As Single = Me.ClientSize.Width
        Dim plp As Single = 0.36
        Dim pcp aS Single = 0.36
        Dim prp As Single = 0.28

        '// If either of the mini panes are shown, subtract their width from the available width
        If PanelLeftMin.Visible Then availWidth=availWidth-PanelLeftMin.Width
        If PanelRightMin.Visible Then availWidth=availWidth-PanelRightMin.Width

        '// calculate a denominator based on the visible panels
        Dim denom = (plp*Convert.ToInt32(PanelLeft.Visible))+
                (pcp*Convert.ToInt32(PanelCenter.Visible))+ (prp*Convert.ToInt32(PanelRight.Visible))

        Dim combo As Integer = Convert.ToInt32(PanelLeft.Visible) +
                                (Convert.ToInt32(PanelCenter.Visible)*2) + 
                                (Convert.ToInt32(PanelRight.Visible)*4)
        Debug.Print("Available Width:" & availWidth)
        Select Case availWidth
            Case < (PanelLeft.MinimumSize.Width + PanelCenter.MinimumSize.Width)          '// show one
                
                    
                    PanelRight.Hide
                    PanelRightMin.Show
                    PanelLeftMin.Show
                    PanelLeft.Hide
                    PanelCenter.Show

                If forceLeft = eForceShowState.ForceShow Then
                    PanelCenter.Hide
                    PanelLeftMin.Hide
                    PanelLeft.Show
                    forceRight = eForceShowState.Normal
                ElseIf forceRight = eForceShowState.ForceShow Then
                    PanelCenter.Hide
                    PanelLeftMin.Show
                    PanelLeft.Hide
                    PanelRight.Show
                    PanelRightMin.Hide
                    forceLeft = eForceShowState.Normal
                End If



                '// normally we would show center but
                '// if forceL then c=0, r=0
                '// if forceR then c=0, L=0
                '// if forceL and forceR then forceR off and c=0 l=0
            Case (PanelLeft.MinimumSize.Width + PanelCenter.MinimumSize.Width) To (PanelLeft.MinimumSize.Width + PanelCenter.MinimumSize.Width + PanelRight.MinimumSize.Width - 1)         '// show two
                
                    PanelRight.Hide
                    PanelRightMin.Show
                    PanelLeftMin.Hide
                    PanelLeft.Show
                    PanelCenter.Show
                
                    If forceLeft = forceRight = eForceShowState.ForceShow Then
                        PanelCenter.Hide
                        PanelLeftMin.Hide
                        PanelLeft.Show
                        PanelRight.Show
                        PanelRightMin.Hide
                    ElseIf forceRight=eForceShowState.ForceShow Or forceLeft= eForceShowState.ForceHide Then
                        If forceRight=eForceShowState.ForceHide And forceLeft= eForceShowState.ForceHide Then
                            PanelRight.Hide
                            PanelRightMin.Show
                            PanelLeftMin.Show
                            PanelLeft.Hide
                            PanelCenter.Show
                        Else
                            PanelRight.Show
                            PanelRightMin.Hide
                            PanelLeftMin.Show
                            PanelLeft.Hide
                            PanelCenter.Show
                            If forceLeft = eForceShowState.ForceShow Then forceLeft = eForceShowState.Normal
                        End If
                        
                    End If
                
            Case Else
                
                    PanelRightMin.Hide
                    PanelRight.Show
                    PanelLeftMin.Hide
                    PanelLeft.Show
                    PanelCenter.Show
                    If forceLeft = eForceShowState.ForceHide And forceRight = eForceShowState.ForceHide Then
                        PanelRight.Hide
                        PanelRightMin.Show
                        PanelLeftMin.Show
                        PanelLeft.Hide
                        PanelCenter.Show
                    ElseIf forceLeft = eForceShowState.ForceHide Then
                        PanelRight.Show
                        PanelRightMin.Hide
                        PanelLeftMin.Show
                        PanelLeft.Hide
                        PanelCenter.Show
                    ElseIf forceRight = eForceShowState.ForceHide
                        PanelRight.Hide
                        PanelRightMin.Show
                        PanelLeftMin.Hide
                        PanelLeft.Show
                        PanelCenter.Show
                    End If

                '// if 3 then good
                '// if 2 then show one subject to manual override
                '// if 1 then show 2 -- but check for manual hide

        End Select

        Dim clientWidth As Single = Me.ClientSize.Width - (Convert.ToInt32(PanelLeftMin.Visible) * PanelLeftMin.Width) - (Convert.ToInt32(PanelRightMin.Visible) * PanelRightMin.Width)
        availWidth = clientWidth

        '// first calculate the center panel
        Debug.Print("CALCULATED Width: {0}, LeftPanel #/%: {1}/{2}, RightPanel #/%: {3}, {4}, CenterPanel #/%: {5}, {6}", 
                    clientWidth, clientWidth*plp, plp, clientWidth*prp, prp,
                    clientWidth*pcp, pcp)

        '// recalc the denominator
        denom = (plp*Convert.ToInt32(PanelLeft.Visible))+
                (pcp*Convert.ToInt32(PanelCenter.Visible))+ (prp*Convert.ToInt32(PanelRight.Visible))

        '// now reclac the denominator so that we use relative percentages
        availWidth = AdjustPanelWidth(PanelCenter, availWidth, pcp, denom, true)
        Dim denom2 As Single = (plp*Convert.ToInt32(PanelLeft.Visible))+(prp*Convert.ToInt32(PanelRight.Visible))
        availWidth = AdjustPanelWidth(PanelLeft, availWidth, plp, denom2, true)
        denom2 = (prp*Convert.ToInt32(PanelRight.Visible))
        availWidth = AdjustPanelWidth(PanelRight, availWidth, prp, denom2, true)
        PanelLayoutSuspendResume(true)
        lastFormSize = Me.Size
        

        Debug.Print("Avail Width: {0}, LeftPanel #/%: {1}/{2}, RightPanel #/%: {3}, {4}, CenterPanel #/%: {5}, {6}", 
                    clientWidth, PanelLeft.Width, PanelLeft.Width/clientWidth, PanelRight.Width, PanelRight.Width/clientWidth,
                    PanelCenter.Width, PanelCenter.Width/clientWidth)
        Debug.Print("Avail Width Left: {0}, LeftPanel #/%: {1}/{2}, RightPanel #/%: {3}, {4}, CenterPanel #/%: {5}, {6}", 
                    availWidth, PanelLeft.Width, PanelLeft.Width/clientWidth/denom, PanelRight.Width, PanelRight.Width/clientWidth/denom,
                    PanelCenter.Width, PanelCenter.Width/clientWidth/denom)
    End Sub

    Private Function AdjustPanelWidth(panel As PanelControl, availWidth As Single, pct As single, denominator As Single, doXForm As Boolean) As Single
        Dim panelWidth As Single = 0
        If panel.Visible Then
            panelWidth = availWidth * (pct/denominator)
            If panelWidth < Panel.MinimumSize.Width Then panelWidth = panel.MinimumSize.Width
            if doXForm Then 
                panel.Width = panelWidth
                Debug.Print("XFORM {0} to {1}", panel.Name, panelWidth)
            End If
            Return availWidth-panelWidth
        Else
            Return availWidth
        End If

    End Function

    Private Sub PanelLayoutSuspendResume(doResume As Boolean)
        If doResume Then
            PanelLeft.ResumeLayout
            PanelRight.ResumeLayout
            PanelCenter.ResumeLayout
            PanelLeft_SizeChanged(PanelLeft, New EventArgs)
        Else
            PanelCenter.SuspendLayout
            PanelLeft.SuspendLayout
            PanelRight.SuspendLayout
        End If
    End Sub

    Private Function GetDesiredPictureWidth(vPadding As Single) As Single
        Dim y As Single = PanelCenter.Height - PanelCenterHeader.Height - PanelCenterFooter.Height - 12
        Dim picHeight As Single = (y-vPadding)/2
        Return picHeight
    End Function

    Overridable Function GetLabelText(ctl As Control) As String
        Return ""
    End Function

#End Region

#Region "Mouse & HotTracking"
    Private Sub TileView1_MouseMove(sender As Object, e As MouseEventArgs) Handles TileView1.MouseMove
       
        Dim view As MyTileView = sender
        Dim info As TileViewHitInfo = view.CalcHitInfo(New Point(e.X, e.Y))
        If info.InItem Then
            Dim item = info.Item
            If Not (view.HotTrackRow = item.RowHandle) Then
                '// new row
                Dim pRow As Integer = view.HotTrackRow
                view.HotTrackRow = item.RowHandle
                '// refresh previous to normal
                If pRow <> GridControl.InvalidRowHandle Then view.RefreshRow(pRow)
                '// refresh the new hotTrackrow
                view.RefreshRow(view.HotTrackRow)
            End If
        Else
            'view.HotTrackRow = GridControl.InvalidRowHandle
        End If
    End Sub

    Private Sub TileView1_FocusedRowChanged(sender As Object, e As FocusedRowChangedEventArgs) Handles TileView1.FocusedRowChanged
        'Debug.Print("Prev: {0}, New: {1}", e.PrevFocusedRowHandle, e.FocusedRowHandle)
        'TileView1.RefreshRow(e.FocusedRowHandle)
    End Sub

    Private Sub Grid1_MouseLeave(sender As Object, e As EventArgs) Handles Grid1.MouseLeave
        Debug.Print("Mouse Left Grid")
        '// invalidate previous hottrackrow if any once we leave the grid
        Dim view As MyTileView = Ctype(Grid1.DefaultView,MyTileView)
        Dim pRow As Integer = view.HotTrackRow
        view.HotTrackRow=GridControl.InvalidRowHandle
        If pRow<> GridControl.InvalidRowHandle Then TileView1.RefreshRow(pRow)
    End Sub

    Private Sub TileView1_ItemCustomize(sender As Object, e As TileViewItemCustomizeEventArgs) Handles TileView1.ItemCustomize
        'Debug.Print("TileView Customize: {0}, row: {1}", sender.GetType.Name, e.RowHandle)
        Dim item As TileViewItem = e.Item
        Dim view As MyTileView = e.Item.View
        '// this is to handle the Hover Management. If we're hovering, set the background image. If not, set to nothing.
        '// need to handle the crazy Grid/TileView scalefactor thing.
        If view.HotTrackRow = item.RowHandle Then
            Dim skinStyler As SkinStyler = dicSkins(sender.Name)
            Item.BackgroundImage = DrawButtonSkinGraphic(dxScaler.activeLookAndFeel, view.GetTileViewItemBackgroundBounds, skinStyler.Skins, skinStyler.ElementName, skinStyler.ImageIndex)
        Else
            item.BackgroundImage=Nothing
        End If
        
    End Sub


#End Region

#Region "Main Demo Button Methods"

    '// set all DevExpress fonts to specific size or auto-size
    Private Sub ButtonResizeFonts_Click(sender As Object, e As EventArgs) Handles ButtonResizeFonts.Click
        Dim s As String = ComboFontSize.SelectedItem
        If s = "Auto" Or s = "" Then
            dxScaler.ScaleFonts(Me, appBaseFont, currentScaleFactor)
        Else
            dxScaler.ScaleFonts(Me, appBaseFont, (s / appBaseFont.Size))
        End If
    End Sub

    '// Scale Form by a factor
    Private Sub ButtonScaleForm_Click(sender As Object, e As EventArgs) Handles ButtonScaleForm.Click
        Dim s As String = ComboScaleForm.SelectedItem
        If s > 0 Then
            currentScaleFactor = dxScaler.ScaleForm(Me, s, currentScaleFactor)
        End If
    End Sub

    '// Create resizing handlers for relevant buttons
    Private Sub ButtonResizeBtns_Click(sender As Object, e As EventArgs) Handles ButtonResizeBtns.Click
        dxScaler.SetButtonSizeChangedHandlers(Me, AddressOf Button_SizeChanged)
    End Sub

    Private Sub ButtonFontMetrics_Click(sender As Object, e As EventArgs) Handles ButtonFontMetrics.Click
        Debug.Print("Font Size:" & LabelLPH_C.Font.SizeInPoints)
        Debug.Print("Ctl Size:" & LabelLPH_C.Height)
        Debug.Print("Line Spacing:" & LabelLPH_C.Font.GetHeight)
        Debug.Print("em-height:" & LabelLPH_C.Font.FontFamily.GetEmHeight(LabelLPH_C.Font.Style))
        Debug.Print("DPIY:" & DevExpress.XtraBars.DPISettings.DpiY)
        Debug.Print("ScaleFactor" & currentScaleFactor)
        Debug.Print("GridScaleFactor:" & Grid1.ScaleFactor.Width)
        Debug.Print("Form Width: {0}, Form Height: {1}, Panel Width: {2}", Me.Width, Me.Height, PanelLeft.Width)
    End Sub



    Private Sub DoScaling(ScaleChange As Single)
        Debug.Print("================START SCALING=================")
        Dim desiredScaleFactor As Single 
        'Debug.Print("Current Desired/Current [{0},{1}]", desiredScaleFactor, currentScaleFactor)
        'Debug.Print("Current Form x,y & Font Size[{0},{1} : {2}]", Me.Width, Me.Height, DevExpress.Utils.AppearanceObject.DefaultFont.Size)
        desiredScaleFactor = currentScaleFactor + ScaleChange
        If desiredScaleFactor < 0.5 Then desiredScaleFactor = 0.5
        If desiredScaleFactor > 2 Then desiredScaleFactor = 2

        currentScaleFactor = dxScaler.ScaleForm(Me, desiredScaleFactor, currentScaleFactor)
        dxScaler.ScaleFonts(Me, appBaseFont, currentScaleFactor)
        'Debug.Print("+++++++++++++TRANSFORMED+++++++++++++++")
        'Debug.Print("New Desired/Current [{0},{1}]", desiredScaleFactor, currentScaleFactor)
        'Debug.Print("New Form x,y & Font Size[{0},{1} : {2}]", Me.Width, Me.Height, DevExpress.Utils.AppearanceObject.DefaultFont.Size)
    End Sub

    Private Sub ButtonScaleTiles_Click(sender As Object, e As EventArgs) Handles ButtonScaleTiles.Click
        'TileView1.SetHScaledTileViewItemSize(PanelLeftXtraHeader.Height)
        ResizeArtwork
    End Sub

#End Region

    Private Sub frmMediaTemplate_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        Debug.Print("FRM RESIZE END")
        ResizePanels
    End Sub

    
    Private Sub ButtonCPH_L_Click(sender As Object, e As EventArgs) Handles ButtonCPH_L.Click
        ShowHidePanel(PanelLeft)
    End Sub

    Private Sub ButtonCPH_R_Click(sender As Object, e As EventArgs) Handles ButtonCPH_R.Click
        ShowHidePanel(PanelRight)
    End Sub

    Private Function ShowHidePanel(panel As PanelControl) As Boolean
        If panel.Visible Then
            panel.Hide
            panel.Tag = True    '//user hidden/unhidden
            Return True
        Else
            panel.Show
            panel.Tag = False
            Return False
        End If
    End Function

    Private Sub ButtonLPH_R_Click_1(sender As Object, e As EventArgs) Handles ButtonLPH_R.Click
        PanelLeft.Hide
        PanelLeftMin.Show
        forceLeft = eForceShowState.ForceHide
        ResizePanels
    End Sub

    Private Sub SimpleButton6_Click(sender As Object, e As EventArgs) Handles SimpleButton6.Click
        PanelLeft.Show
        PanelLeftMin.Hide
        forceLeft = eForceShowState.ForceShow
        forceRight = eForceShowState.Normal
        ResizePanels
    End Sub

    Private Sub SBtnQExpand_Click(sender As Object, e As EventArgs) Handles SBtnQExpand.Click
        
        PanelRightMin.Hide
        PanelRight.Show
        forceRight = eForceShowState.ForceShow
        forceLeft = eForceShowState.Normal
        ResizePanels
    End Sub

    Private Sub ButtonRPH_L_Click(sender As Object, e As EventArgs) Handles ButtonRPH_L.Click
        PanelRight.Hide
        PanelRightMin.Show
        forceRight = eForceShowState.ForceHide
        ResizePanels
    End Sub

    Private Sub ButtonRPF_L_Click(sender As Object, e As EventArgs) Handles ButtonRPF_L.Click
        DoScaling(-0.25)
    End Sub

    Private Sub ButtonRPF_R_Click(sender As Object, e As EventArgs) Handles ButtonRPF_R.Click
        DoScaling(+0.25)
    End Sub

    Private Sub LabelRPH_C_Click(sender As Object, e As EventArgs) Handles LabelRPH_C.Click
        
    End Sub

    Private Sub LabelRightMin_Click(sender As Object, e As EventArgs) Handles LabelRightMin.Click

    End Sub

    Private Sub LabelRightMin_Paint(sender As Object, e As PaintEventArgs) Handles LabelRightMin.Paint

        PaintVerticalText2(sender, e.Graphics, 90, "PLAYER QUEUE", Alignment.Center)

        
    End Sub

    Private Sub PaintVerticalText(control As Control, g As Graphics, lbl As String, alignment As Alignment)
        Dim format As New StringFormat
        format.Alignment = StringAlignment.Center
        format.FormatFlags = StringFormatFlags.DirectionVertical
        Dim sz As SizeF = g.MeasureString(lbl, Me.Font)
        Dim x = (control.Width-sz.Height)/2
        Dim y = (control.Height/2) - (sz.Width/2)
        g.DrawString(lbl, DevExpress.Utils.AppearanceObject.DefaultFont, New SolidBrush(control.ForeColor), x, y, format)
    End Sub

    Private Sub PaintVerticalText2(control As Control, g As Graphics, angle As Integer, lbl As String, alignment As Alignment)
        Dim format As New StringFormat
        format.Alignment = alignment
        format.LineAlignment = alignment
        g.TranslateTransform(control.Width/2,control.Height/2)
        g.RotateTransform(angle)
        Dim sz As SizeF = g.MeasureString(lbl, Me.Font)
        Dim x = (control.Width-sz.Height)/2
        Dim y = (control.Height/2) - (sz.Width/2)
        g.DrawString(lbl, DevExpress.Utils.AppearanceObject.DefaultFont, New SolidBrush(control.ForeColor), -sz.Width/2, -sz.Height/2)
        g.ResetTransform
    End Sub

    Private Sub PanelLeftMin_Paint(sender As Object, e As PaintEventArgs) Handles PanelLeftMin.Paint
        PaintVerticalText2(sender, e.Graphics, -90, "MUSIC LIBRARY", Alignment.Center)
    End Sub
End Class