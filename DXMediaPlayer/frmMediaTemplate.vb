﻿Imports System.Runtime.InteropServices
Imports DevExpress.MyExtensions
Imports DevExpress.Skins
Imports DevExpress.Utils
Imports DevExpress.XtraBars.Docking2010
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Tile
Imports DevExpress.XtraGrid.Views.Tile.ViewInfo
Imports MediaPlayer.Utilities

Public Class frmMediaTemplate

#Region "Variables and Classes"

    '// styling related
    Dim dicSkins As New Dictionary(Of String, ScaleManager.SkinStyler)           'Just convenience, so I can manage reskinning easily

    '// setup skins and set DevExpress Base Font
    Dim commonSkins As New DevExpress.Skins.CommonSkins
    Dim ribbonSkins As New DevExpress.Skins.RibbonSkins
    Dim dashboardSkins As New DevExpress.Skins.DashboardSkins
    Dim AppBasefont As Font = Me.Font
    
    '// capture Vertical ScrollBar
    Dim WithEvents tileScrollBar_Left As DevExpress.XtraGrid.Scrolling.VCrkScrollBar 'The grid/TileView scrollbar

    '// get the resources manager
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMediaTemplate))

    '// scale related
    Dim currentScaleFactor As Single = 1
    Dim btnImageScaleFactor As SizeF = New SizeF(0.85, 0.85)        'The scale factor applied to button images
    Dim dxScaler As ScaleManager

    '// used to manage user initiated panel show hide functions
    Enum eForceShowState
        Normal
        ForceShow
        ForceHide
    End Enum

    Public Enum eTileStyle
        SingleLine
        SingleLineNoArt
        MultiLineFull
        MultiLineBasic
        MultiLineNoArt
    End Enum

    Dim libraryTileStyle As eTileStyle = eTileStyle.MultiLineFull

    Dim forceLeft As eForceShowState=eForceShowState.Normal
    Dim forceRight As eForceShowState=eForceShowState.Normal

    '// static to try and limit unnecessary panel resizing
    Dim lastFormSize As New Size(Me.Width, Me.Height)

    Const TILEITEMHEIGHT = 64
    
    Dim TVIE_colArt As DevExpress.XtraGrid.Views.Tile.TileViewItemElement 
    Dim TVIE_colTitle As DevExpress.XtraGrid.Views.Tile.TileViewItemElement 
    Dim TVIE_colLine2 As DevExpress.XtraGrid.Views.Tile.TileViewItemElement 

    Dim WithEvents cMenu As New ContextMenuStrip
    Dim WithEvents menuRenderer As New MyMenuRenderer

#End Region

#Region "Form Load & Initialisation Methods"

    '// when we load the form, we need to set basic styling, initialize the scaler and hook up events
    Private Sub frmMediaTemplate_Load(sender As Object, e As EventArgs) Handles Me.Load
        
        Me.Visible = False

        TVIE_colArt = Me.TileView1.TileTemplate.Item(0)                        
        TVIE_colTitle = Me.TileView1.TileTemplate.Item(1)
        TVIE_colLine2 = Me.TileView1.TileTemplate.Item(2)
        
        
        cMenu.Name = "LeftPopupMenu"
        cMenu.Renderer = menuRenderer
        cMenu.Items.Clear
        cMenu.LayoutStyle = ToolStripLayoutStyle.Table

        BuildMenu

        ClearImmediateWindow
        LogMeTraceLevel(TraceLevel.Info, eTraceCategories.All)
        VerboseMode = False
        SetModeDPIAware
        ''// set afterwards (at cost of extra refresh, but form font is now set
        ''// this impacts devexpress controls that haven't had their fonts set in the initialization code
        ''// such as the TileViewItems
        DevExpress.Utils.AppearanceObject.DefaultFont = Me.Font        '// set to form font
        AppBasefont = DevExpress.Utils.AppearanceObject.DefaultFont
        dxScaler = New ScaleManager(Me)
        SetSkinStylingOverrides
        dxScaler.SetPanelPaintHandlers(Me, dicSkins, AddressOf PanelBackGround_Paint)
        AddHandler cMenu.Paint, AddressOf PanelBackGround_Paint
        Me.Visible=True
        
    End Sub

    '// when the form is actually shown, we resize the panels
    Private Sub frmMediaTemplate_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        ResizePanels(true)
        Dim tileItemElement As TileItemElement = TileView1.SpringTileItemElementWidth(colTitle, True)
    End Sub

#End Region

#Region "Styling"   
    '// a central place to setup skinning overrides.
    Private Sub SetSkinStylingOverrides
        dicSkins.Add(PanelTop.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
                                  .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})

        'dicSkins.Add(PanelLeftHeader.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
        '                          .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        'dicSkins.Add(PanelRightHeader.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
        '                          .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        'dicSkins.Add(PanelCenterHeader.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
        '                          .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        'dicSkins.Add(PanelLeftFooter.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
        '                          .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        'dicSkins.Add(PanelRightFooter.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
        '                          .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        'dicSkins.Add(PanelCenterFooter.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
        '                          .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        'dicSkins.Add(PanelLeftMin.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
        '                          .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})
        'dicSkins.Add(PanelRightMin.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
        '                          .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 2})

        'dicSkins.Add(PanelLeftXtraHeader.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
        '                          .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 0})
        'dicSkins.Add(PanelCenterBody.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
        '                          .Skins = dashboardSkins, .ElementName = "DashboardItemBackground", .ImageIndex = 4})
        'dicSkins.Add(PanelCenterBody.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
        '                          .Skins = commonSkins, .ElementName = "Button", .ImageIndex = 4})
        dicSkins.Add(PanelCenterBody.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
                                  .Skins = ribbonSkins, .ElementName = "ContextTabCategory", .ImageIndex = 0})
        dicSkins.Add(TileView1.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
                                  .Skins = commonSkins, .ElementName = "HighlightedItem", .ImageIndex = 0})
        dicSkins.Add(cMenu.Name & "Highlighted", New ScaleManager.SkinStyler With {.IsImage = "True",
                                  .Skins = commonSkins, .ElementName = "LayoutItemBackground", .ImageIndex = 2})
        dicSkins.Add(cMenu.Name, New ScaleManager.SkinStyler With {.IsImage = "True",
                                  .Skins = commonSkins, .ElementName = "LayoutItemBackground", .ImageIndex = 1})

        '// use this tag to flag buttons where we want to rescale the images 
        ButtonLPH_L.Tag = "UsePadding"
        ButtonLPH_R.Tag = "UsePadding"
        TileView1.Appearance.ItemNormal.TextOptions.Trimming=Trimming.EllipsisCharacter
        TileView1.Appearance.ItemNormal.TextOptions.WordWrap= WordWrap.NoWrap
        
        For Each btn As WindowsUIButton In ButtonsTransport.Buttons
            btn.Image=  RescaleImageByPadding(btn.Image,
                                            btn.Image.Size, ButtonsTransport.Padding)
        Next
        
            
    End Sub
#End Region

#Region "Control/Form Painting and Resizing"
    '// the core triggers to force resizing or repainting are:
    '// --- the form resize event (could be moved to the resize end event if we liked)
    '// --- the datasourcechanged event (which triggers grid styling)
    '// --- the form scale up and scale down buttons
    '// --- the panel force hide and force show buttons
    '// --- the vertical scroll bar Visible Changed Event

    '// when the form resizes, resize the panels
    Private Sub frmMediaTemplate_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        LogMe(eTraceCategories.Gui,"Form Resize", TraceLevel.Info)
        PanelsSuspendLayout(False)
        ResizePanels(False)
        PanelsSuspendLayout(True)
    End Sub

    '// when the Grid is resized, change the TileViewItem Size
    Private Sub Grid1_SizeChanged(sender As Object, e As EventArgs) Handles Grid1.SizeChanged
        '// no longer used. handled by form resize or panel resize       
        '// LogMe(eTraceCategories.Gui,"Grid Size_Changed", TraceLevel.Info)
    End Sub

    '// when the scrollbar is visibly changed, recalc the TileViewItem Size
    Private Sub tileScrollBar_Left_VisibleChanged(sender As Object, e As EventArgs) Handles tileScrollBar_Left.VisibleChanged
        '// for some reason this event triggers all the blinking time, even when the parent control is hidden!
        '// this next line is to trap for and optimize for that.
        If PanelLeft.Visible = False Then Exit Sub
        LogMe(eTraceCategories.Gui,"vScrollBarChanged: " & tileScrollBar_Left.Visible & "<**>" & PanelLeft.Visible, TraceLevel.Info)
        TidyUpGridElements
        
    End Sub

    '// demonstrating overriding paint method and using a skin image 
    Private Sub PanelBackGround_Paint(sender As Object, e As PaintEventArgs) 
        'LogMe(eTraceCategories.Gui,"Panel Background", TraceLevel.Info)
        Dim skinStyler As ScaleManager.SkinStyler = dicSkins(sender.Name)
        e.Graphics.DrawImage(DrawButtonSkinGraphic(dxScaler.activeLookAndFeel, sender.Bounds, skinStyler.Skins, skinStyler.ElementName, skinStyler.ImageIndex), 0, 0)
    End Sub

    '// THIS SHOULD NOW BE FIXED BY DEVEXPRESS
    '// adjust size of button image based on the button Padding.
    '// this method is connected to Button SizeChanged Events 
    Private Sub Button_SizeChanged(sender As Object, e As EventArgs)
        LogMe(eTraceCategories.Gui,"Button_SizeChanged", TraceLevel.Info)
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


    Private Sub LabelRightMin_Paint(sender As Object, e As PaintEventArgs) Handles LabelRightMin.Paint
        PaintTextAtAngle(sender, e.Graphics, DevExpress.Utils.AppearanceObject.DefaultFont,  90, "PLAYER QUEUE", Alignment.Center)      
    End Sub

    Private Sub PanelLeftMin_Paint(sender As Object, e As PaintEventArgs) Handles PanelLeftMin.Paint
        PaintTextAtAngle(sender, e.Graphics, DevExpress.Utils.AppearanceObject.DefaultFont, -90, "MUSIC LIBRARY", Alignment.Center)
    End Sub


#End Region

#Region "Responsive Resize Panels Method"

    '// the main resizing method that handles responsive design and user forcing of panel display
    Private Sub ResizePanels(bForceResize As Boolean)
        If Not Me.Visible Then Exit Sub
        If (Me.Size = lastFormSize) And Not bForceResize Then Exit Sub
        LogMe(eTraceCategories.Gui,"RESIZE PANELS", TraceLevel.Info)
        'Me.SuspendLayout
        'PanelsSuspendLayout(false)
        
        '// relative sizes of each panel        
        Dim plp As Single = 0.36
        Dim pcp aS Single = 0.36
        Dim prp As Single = 0.28
        
        Dim availWidth As Single = Me.ClientSize.Width

        '// If either of the mini panes are shown, subtract their width from the available width
        If PanelLeftMin.Visible Then availWidth=availWidth-PanelLeftMin.Width
        If PanelRightMin.Visible Then availWidth=availWidth-PanelRightMin.Width

        '// calculate a denominator based on the visible panels
        Dim denom = (plp*Convert.ToInt32(PanelLeft.Visible))+
                (pcp*Convert.ToInt32(PanelCenter.Visible))+ (prp*Convert.ToInt32(PanelRight.Visible))

        Dim pr, prm, pl, plm, pc As Boolean
        Dim show As Boolean = True: Dim hide As Boolean = false

        Select Case availWidth
            Case < (PanelLeft.MinimumSize.Width + PanelCenter.MinimumSize.Width)          '// only room for one panel
                '// set the defaults
                pr = hide: prm = show
                pl = hide: plm = show
                pc = show
                If forceLeft = eForceShowState.ForceShow Then
                    '// if we have forced the left to show, show it and hide the center (the right was hidden above)
                    pc = hide
                    pl = show: plm = hide
                    forceRight = eForceShowState.Normal
                ElseIf forceRight = eForceShowState.ForceShow Then
                    '// similarly, if the right panel is force shown, hide the center and the left
                    pc = hide
                    pl = hide:plm=show
                    pr = show: prm = hide
                    forceLeft = eForceShowState.Normal
                End If

            Case (PanelLeft.MinimumSize.Width + PanelCenter.MinimumSize.Width) To (PanelLeft.MinimumSize.Width + PanelCenter.MinimumSize.Width + PanelRight.MinimumSize.Width - 1)         '// show two
                '// the default two panels
                pr = hide: prm = show
                pl = show: plm = hide
                pc = show
                
                If forceLeft = forceRight = eForceShowState.ForceShow Then
                    '// if both sides are force show, hide the center
                    pc = hide
                    pl=show: plm = hide
                    pr=show: prm = hide
                ElseIf forceRight=eForceShowState.ForceShow Or forceLeft= eForceShowState.ForceHide Then
                    If forceRight=eForceShowState.ForceHide And forceLeft= eForceShowState.ForceHide Then
                        'if both sides are force hide then only show the center
                        pc = show
                        pl = hide: plm = show
                        pr = hide: prm = show
                    Else
                        pr = show: prm = hide
                        pl = hide: plm = show
                        pc = show
                        If forceLeft = eForceShowState.ForceShow Then forceLeft = eForceShowState.Normal
                    End If
                End If               
            Case Else       '// room for 3 panels
                
                pl = show: plm = hide
                pr = show: prm = hide
                pc = show

                If forceLeft = eForceShowState.ForceHide And forceRight = eForceShowState.ForceHide Then
                    pc = show
                    pl = hide: plm =show
                    pr = hide: prm = show
                ElseIf forceLeft = eForceShowState.ForceHide Then
                    pr = show: prm = hide
                    pl = hide: plm = show
                    pc = show
                ElseIf forceRight = eForceShowState.ForceHide
                    pr = hide: prm = show
                    pl = show: plm = hide
                    pc = show
                End If

        End Select
        'PanelCenter.SuspendLayout
        PanelCenter.Visible = pc
        PanelLeftMin.Visible = plm
        PanelLeft.Visible = pl
        PanelRightMin.Visible = prm
        PanelRight.Visible = pr
        
        
        Dim clientWidth As Single = Me.ClientSize.Width - (Convert.ToInt32(PanelLeftMin.Visible) * PanelLeftMin.Width) - (Convert.ToInt32(PanelRightMin.Visible) * PanelRightMin.Width)
        availWidth = clientWidth

        '// first calculate the center panel
        
        '// recalc the denominator
        denom = (plp*Convert.ToInt32(PanelLeft.Visible))+
                (pcp*Convert.ToInt32(PanelCenter.Visible))+ (prp*Convert.ToInt32(PanelRight.Visible))
        availWidth = AdjustPanelWidth(PanelCenter, availWidth, pcp, denom, true)

        '// now recalc the denominator so that we use relative percentages
        Dim denom2 As Single = (plp*Convert.ToInt32(PanelLeft.Visible))+(prp*Convert.ToInt32(PanelRight.Visible))
        availWidth = AdjustPanelWidth(PanelLeft, availWidth, plp, denom2, true)

        '// now recalc the denominator so that we use relative percentages
        denom2 = (prp*Convert.ToInt32(PanelRight.Visible))
        availWidth = AdjustPanelWidth(PanelRight, availWidth, prp, denom2, true)

        ResizeArtwork
        
        lastFormSize = Me.Size
    
    End Sub 

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
            Dim skinStyler As ScaleManager.SkinStyler = dicSkins(sender.Name)
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
        Debug.Print("DevExpress Font Size:" & appBaseFont.Size)
        Debug.Print("TileViewItem:" & GetTileElement(TileView1, "colTitle").Appearance.Normal.Font.Size)
        Debug.Print("Ctl Size:" & LabelLPH_C.Height)
        Debug.Print("Line Spacing:" & LabelLPH_C.Font.GetHeight)
        Debug.Print("em-height:" & LabelLPH_C.Font.FontFamily.GetEmHeight(LabelLPH_C.Font.Style))
        Debug.Print("DPIY:" & DevExpress.XtraBars.DPISettings.DpiY)
        Debug.Print("ScaleFactor" & currentScaleFactor)
        Debug.Print("GridScaleFactor:" & Grid1.ScaleFactor.Width)
        Debug.Print("Form Width: {0}, Form Height: {1}, Panel Width: {2}", Me.Width, Me.Height, PanelLeft.Width)
    End Sub




    Private Sub ButtonScaleTiles_Click(sender As Object, e As EventArgs) Handles ButtonScaleTiles.Click
        'TileView1.SetHScaledTileViewItemSize(PanelLeftXtraHeader.Height)
        ResizeArtwork
    End Sub

#End Region

#Region "GUI Button Clicks"

    Private Sub ButtonLPH_R_Click_1(sender As Object, e As EventArgs) Handles ButtonLPH_R.Click
        PanelShowHide(PanelLeft, PanelLeftMin,False,forceLeft, forceRight)
    End Sub

    Private Sub ButtonLMinH_T_Click(sender As Object, e As EventArgs) Handles ButtonLMinH_T.Click
        PanelShowHide(PanelLeft, PanelLeftMin,True,forceLeft, forceRight)
    End Sub

    Private Sub ButtonRMinH_T_Click(sender As Object, e As EventArgs) Handles ButtonRMinH_T.Click
        PanelShowHide(PanelRight, PanelRightMin,True,forceRight, forceLeft)
    End Sub

    Private Sub ButtonRPH_L_Click(sender As Object, e As EventArgs) Handles ButtonRPH_L.Click
        PanelShowHide(PanelRight, PanelRightMin,False,forceRight, forceLeft)
    End Sub

    Private Sub ButtonRPF_L_Click(sender As Object, e As EventArgs) Handles ButtonRPF_L.Click
        DoScaling(-0.25)
    End Sub

    Private Sub ButtonRPF_R_Click(sender As Object, e As EventArgs) Handles ButtonRPF_R.Click
        DoScaling(+0.25)
    End Sub



#End Region

#Region "Overridable Methods"

    '// this is marked overridable so it can be handled by the inherited form that handles data stuff
    Overridable Function GetLabelText(ctl As Control) As String
        Return ""
    End Function

    Overridable Function GetLibraryData(colName As String, rowIndex As Integer) As Object
        Return ""
    End Function


#End Region

#Region "Important Supporting Methods"    

        '// method to rescale form by a factor. Max is 2x, Min is 0.5x
    Private Sub DoScaling(ScaleChange As Single)
        Debug.Print("================START SCALING=================")

        Dim desiredScaleFactor As Single 
        
        desiredScaleFactor = currentScaleFactor + ScaleChange
        If desiredScaleFactor < 0.5 Then desiredScaleFactor = 0.5
        If desiredScaleFactor > 2 Then desiredScaleFactor = 2
        Me.Hide
        '// some odd bug that causes the visible change event to be fired umpteen times needs to be worked around by detaching and then reattaching the scrollbar
        tileScrollBar_Left = Nothing
        currentScaleFactor = dxScaler.ScaleForm(Me, desiredScaleFactor, currentScaleFactor)
        dxScaler.ScaleFonts(Me, appBaseFont, currentScaleFactor)
        ResizePanels(True)
        tileScrollBar_Left = TileView1.VScrollBar
        Me.Show
        
        SetTileStyle(TileView1, libraryTileStyle)
        
    End Sub

    '// calculates panel 
    Private Function AdjustPanelWidth(panel As PanelControl, availWidth As Single, pct As single, denominator As Single, doXForm As Boolean) As Single
        Dim panelWidth As Single = 0
        If panel.Visible Then
            panelWidth = availWidth * (pct/denominator)
            If panelWidth < Panel.MinimumSize.Width Then panelWidth = panel.MinimumSize.Width
            if doXForm Then 
                panel.Width = panelWidth
                'Debug.Print("XFORM {0} to {1}", panel.Name, panelWidth)
            End If
            Return availWidth-panelWidth
        Else
            Return availWidth
        End If

    End Function
    
    '// a method to suspend layout updating for the main panels -- and to run a tidyup method when Layout is resumed.
    Private Sub PanelsSuspendLayout(doResume As Boolean)
        If doResume Then
            PanelLeft.ResumeLayout
            PanelRight.ResumeLayout
            PanelCenter.ResumeLayout
            PanelCenterBody.ResumeLayout
            TidyUpGridElements
        Else
            PanelCenter.SuspendLayout
            PanelLeft.SuspendLayout
            PanelRight.SuspendLayout
            PanelCenterBody.SuspendLayout
        End If
    End Sub

    '// Sets the status of the side panels, along with their "mini" siblings. It also forces a PanelResize event
    Private Sub PanelShowHide(panel As PanelControl, panelMin As PanelControl, Show As Boolean, ByRef forceThis As eForceShowState, ByRef forceThat As eForceShowState)
        PanelsSuspendLayout(false)
        If Show Then
            panel.Show
            panelMin.Hide
            forceThis = eForceShowState.ForceShow
            forceThat = eForceShowState.Normal
        Else
            panel.Hide
            panelMin.Show
            forceThis = eForceShowState.ForceHide
        End If
        ResizePanels(True)
        PanelsSuspendLayout(True)
    End Sub

    '// A method to ensure tiles are set to the right scale and text formatting is managed
    Private Sub TidyUpGridElements
        LogMe(eTraceCategories.Gui,"TIDY UP GRID", TraceLevel.Info)      
        TileView1.SetHScaledTileViewItemSize(TILEITEMHEIGHT * currentScaleFactor)
        TileView1.SpringTileItemElementWidth(colTitle, True)
        TileView1.SpringTileItemElementWidth(colLine2, True)
        RefreshlabelText(LabelXtraItem)     
    End Sub

    '// A method to create a two line truncated text display with HTML formatting
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
    
    '// Resize the artwork
    Private Sub ResizeArtwork
        Dim vPaddingT As Single = 24
        Dim vPaddingB As Single = 12
        Dim y As Single = PanelCenterBody.Height 
        Dim picHeight As Single = (y-(vPaddingT+vPaddingB))/2
        PE1.Height = picHeight
        Dim x As Single = PanelCenter.Width
        Dim xPad As Single = (PanelCenter.Width-picHeight) / 2
        If xPad<0 Then xPad=0
        PE1.Properties.Padding = New Padding(xPad,vPaddingT,xPad,vPaddingB)
        'Debug.Print("Resized. Width: {0}, Adj Width: {1} ,Height: {2}",PE1.Width,x-picHeight,  picHeight)
    End Sub

    Private Sub ClearImmediateWindow
        Dim dte = Marshal.GetActiveObject("VisualStudio.DTE.14.0")
        Try
            dte.Windows.Item("Immediate Window").Activate() 'Activate Immediate Window  
            dte.ExecuteCommand("Edit.SelectAll")
            dte.ExecuteCommand("Edit.ClearAll")
        Catch ex As Exception

        End Try
        Marshal.ReleaseComObject(dte)
    End Sub

#End Region

        '// the main method to fill in the TileView1 Tiles
    Private Sub TileView1_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles TileView1.CustomUnboundColumnData
        If e.IsGetData Then
            Dim value As Object = GetLibraryData(e.Column.Name, e.ListSourceRowIndex)
            Select Case e.Column.Name
                Case "colArt"
                    e.Value = value 
                Case "colTitle"
                    e.Value = "<color=HighlightText>" & value & "</color>"
                Case "colLine2"
                    Dim strVal As String = value
                    If strVal.Contains("|") Then
                        Dim part1 As String = strVal.Split("|")(0) 
                        Dim part2 As String = "<color=ActiveCaption>" & strVal.Split("|")(1) & "</color>"
                        e.Value = part2 & ", " & part1
                    Else
                        e.Value = value
                    End If
            End Select
        End If
    End Sub

    Private Function SetTileStyle(tileView As TileView, tileStyle As eTileStyle) As eTileStyle
        If tileView.DataSource Is Nothing Then Return tilestyle
        
        Dim artScaleFactor = currentScaleFactor * (currentScaleFactor / Grid1.ScaleFactor.Width)
        Dim availHeight = TileView1.GetTileViewItem(0).ItemInfo.ContentBounds.Height / Grid1.ScaleFactor.Width
        LogMe(eTraceCategories.Gui, "SetTileStyle:" & Grid1.ScaleFactor.Width & " availHeight:" & availheight, TraceLevel.Info)
        '// My crazy formula to get the spacing right!
        Dim vOffset1 = ((FontHelper.GetCellAscent(Me.Font)+ FontHelper.GetCellDescent(Me.Font)) * currentScaleFactor)
        Dim vOffset2 = (FontHelper.GetCellDescent(Me.Font)*2) * currentScaleFactor
        
        TVIE_colArt.ImageSize = New Size(availHeight, availHeight)
        Dim hOffset1 = TVIE_colArt.ImageSize.Width + 8
        Dim hOffset2 = 8
        Select Case tileStyle
            Case eTileStyle.SingleLine
                colLine2.Visible = False
                colTitle.Visible = True
                colArt.Visible = True
                TVIE_colTitle.TextLocation = New Point(hOffset1, -1)
            Case eTileStyle.SingleLineNoArt
                colLine2.Visible = False
                colTitle.Visible = True
                colArt.Visible = False
                TVIE_colTitle.TextLocation = New Point(hOffset2, -1)
            Case eTileStyle.MultiLineBasic
            Case eTileStyle.MultiLineFull
                colLine2.Visible = True
                colTitle.Visible = True
                colArt.Visible = True
                TVIE_colTitle.TextLocation = New Point(hOffset1, -vOffset1)
                TVIE_colLine2.AnchorOffset = New Point(0, -vOffset2)
            Case eTileStyle.MultiLineNoArt
                colLine2.Visible = True
                colTitle.Visible = True
                colArt.Visible = False
                TVIE_colTitle.TextLocation = New Point(hOffset2, -vOffset1)
                TVIE_colLine2.AnchorOffset = New Point(0, -vOffset2)

        End Select
        availHeight = TileView1.GetTileViewItem(0).ItemInfo.ContentBounds.Height
        Dim bigIconSize = New Size(availHeight*.75,availHeight*.75)
        Dim smIconSize= New Size(availHeight*.5,availHeight*.5)
        Dim adjust As Single = ((bigIconSize.Height-smIconSize.Height)/2) 

        Dim item = TileView1.ContextButtons.Item(0)
        item.Size = bigIconSize
        item.Glyph = RescaleImageByScaleFactor(My.Resources.Circled_Chevron_Right_64, bigIconSize)
        item = TileView1.ContextButtons.Item(1)
        item.Size = smIconSize
        item.Glyph = RescaleImageByScaleFactor(My.Resources.Sort_Down_64, smIconSize)
        item.AnchorOffset = New Point(0, adjust)


        TileView1.SpringTileItemElementWidth(colTitle, True)
        TileView1.SpringTileItemElementWidth(colLine2, True)
        Return tileStyle
    End Function

    
    Private Sub ButtonCPH_L_Click(sender As Object, e As EventArgs) Handles ButtonCPH_L.Click
        libraryTileStyle= SetTileStyle(TileView1, eTileStyle.SingleLine)
        TileView1.RefreshData
    End Sub

    Private Sub ButtonCPH_R_Click(sender As Object, e As EventArgs) Handles ButtonCPH_R.Click
        libraryTileStyle= SetTileStyle(TileView1, eTileStyle.MultiLineFull)
        TileView1.RefreshData       
    End Sub

    Private Sub ButtonCPF_R_Click(sender As Object, e As EventArgs) Handles ButtonCPF_R.Click
        libraryTileStyle= SetTileStyle(TileView1, eTileStyle.MultiLineNoArt)
        TileView1.RefreshData
    End Sub

    Private Sub ButtonCPF_L_Click(sender As Object, e As EventArgs) Handles ButtonCPF_L.Click
        libraryTileStyle= SetTileStyle(TileView1, eTileStyle.SingleLineNoArt)
        TileView1.RefreshData
    End Sub

    Private Sub TileView1_DataSourceChanged(sender As Object, e As EventArgs) Handles TileView1.DataSourceChanged
        If tileScrollBar_Left Is Nothing Then tileScrollBar_Left = TileView1.VScrollBar            '// uses MyTileView, otherwise have to peek inside the Grid or the TileView
        SetTileStyle(TileView1, libraryTileStyle)
    End Sub

    Private Sub frmMediaTemplate_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        
    End Sub

    Private Sub ButtonLPH_L_Click(sender As Object, e As EventArgs) Handles ButtonLPH_L.Click

        Dim currentSkin As DevExpress.Skins.Skin = CommonSkins.GetSkin(DefaultLookAndFeel1.LookAndFeel)
        
        
        'Dim c As Color = DXSystemColors.ScrollBar
        'Dim d As Color = color.Black 'currentSkin.TranslateColor(DXSystemColors.WindowFrame)
        'Dim f As Color = currentSkin.TranslateColor(SystemColors.ActiveCaption)
        'Debug.Print(c.GetBrightness)
        'cMenu.BackColor = c
        'cMenu.ForeColor = d
        
        'cMenu.Items.Clear
        'BuildMenu
        cMenu.Margin = New Padding(12)
        cMenu.Padding = New Padding(12)
        
        cMenu.Font = LabelLPH_C.Font
        cMenu.ShowImageMargin=False
        cMenu.ForeColor = LabelLPH_C.ForeColor
        'menuRenderer.BackColor = c
        'menuRenderer.HighlightBackColor = f

        cMenu.Show(sender, sender.Width, 0 )
    End Sub

    Private Sub cMenu_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles cMenu.ItemClicked
        Debug.Print("Clicked: " & e.ClickedItem.Text)
        

        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(e.ClickedItem.Text)
    End Sub
    Private Sub BuildMenu()
        
        Dim skins As SkinContainerCollection = SkinManager.[Default].Skins
        For Each s As SkinContainer In skins
	        Dim chitem As New ToolStripMenuItem
	        chitem.Text = s.SkinName
            chitem.Margin = New Padding(3)
	        'chitem.Font = AppBasefont
	        If s.SkinName = DevExpress.LookAndFeel.UserLookAndFeel.[Default].SkinName Then
		        chitem.Checked = True
	        End If
	        cMenu.Items.Add(chitem)
        Next
    End Sub

    Private Sub menuRenderer_RenderMenuItemBackground(sender As Object, e As ToolStripItemRenderEventArgs) Handles menuRenderer.RenderMenuItemBackground
        Dim skinStyler As ScaleManager.SkinStyler 
        If e.Item.Selected Then
            skinStyler = dicSkins(cMenu.Name & "Highlighted")
        Else
            skinStyler = dicSkins(cMenu.Name)
        End If
        e.Graphics.DrawImage(DrawButtonSkinGraphic(dxScaler.activeLookAndFeel,New Rectangle(0,0,e.Item.Size.Width-6, e.Item.Size.Height), skinStyler.Skins, skinStyler.ElementName, skinStyler.ImageIndex), 0, 0)
        '
    End Sub
End Class