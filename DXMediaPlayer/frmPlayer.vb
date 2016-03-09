
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Tile
Imports Sonos.Smapi
Imports Sonos.Smapi.Wsdl

Public Class frmPlayer

    '// used for bound and unbound libraryData management
    Dim libraryData As Dictionary(Of String, XmlMusicItem)
    Dim queueData As Dictionary(Of String, XmlMusicItem)

    Dim libraryShadowList As String()
    Dim queueShadowList As String()

    Dim musicServices As New Dictionary(Of String, MusicServiceInfo)            'A collection of MusicServices
    Dim rootItem As MusicItem                                                   'This is the top-most node
    Dim currentItem As MusicItem

    Const ITEMCOUNT = 50

    Private Sub frmPlayer_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Grid1.DataSource = GetInitialDataset(librarydata, false)
        SetupMusicServices
        Grid2.DataSource = GetInitialDataset(queuedata,true)
        LabelTPH_L.Text = "Family Room"
        Me.Text = "Sonos Media Player"
    End Sub

    Private Sub frmPlayer_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        
    End Sub

#Region "Data Handling"

    '// get a short (no scroll bar) or long (w scrollbar) dataset
    Private Function GetInitialDataset(ByRef dataDic As Dictionary(Of String, XmlMusicItem), isSmallDataset As Boolean) As String()
        If isSmallDataset Then
            dataDic = DeSerializeMusicItemLibrary(My.Resources.ShortAlbumTrackList, True)
        Else
            dataDic = DeSerializeMusicItemLibrary(My.Resources.AlbumTrackList, True)
        End If
        
        Dim shadowList(dataDic.Count - 1) As String
        For i As Integer = 0 To dataDic.Count - 1
            shadowList(i) = dataDic.Values(i).ID
        Next
        Return shadowList
    End Function

    '// replace strings with enum
    Private Sub UpdateDisplayStyle(dataset As String, tileStyle As eTileStyle)
        If libraryTileStyle<> tileStyle Then SetTileStyle(dataset, tileStyle)
    End Sub


    Public Overrides Function GetLibraryData(colName As String, rowIndex As Integer) As Object
        'Return MyBase.GetLibraryData(colName)
        'If libraryData Is Nothing Then Return ""
        Dim musicItem As MusicItem = currentItem
        If musicItem Is Nothing Then Return ""
        Dim key As String = musicItem.ItemIds(rowIndex)
        If key IsNot Nothing Then
            If musicItem.Items.ContainsKey(key) Then
                '// probably a more efficient way to do this!
                Select Case musicItem.Items(key).itemType
                    Case itemType.container, itemType.artist
                        UpdateDisplayStyle("Library", eTileStyle.SingleLine)
                    Case itemType.track
                        UpdateDisplayStyle("Library", eTileStyle.MultiLineFull)
                    Case itemType.album
                        UpdateDisplayStyle("Library", eTileStyle.MultiLineBasic)
                    Case Else
                        UpdateDisplayStyle("Library", eTileStyle.SingleLine)
                End Select



                If musicItem.Items(key).ID="root" Then 
                    
                End If
                Select Case colName
                    Case "colArt"
                        Return musicItem.Items(key).ArtWork
                        'Return libraryData.Values(rowIndex).ArtWork
                    Case "colTitle"
                        Return musicItem.Items(key).Title
                        'Return libraryData.Values(rowIndex).Title
                    Case "colLine2"
                        Return musicItem.Items(key)?.Album & "|" & musicItem.Items(key)?.Artist
                        'e.Value = "<color=ActiveCaption>" & libraryData(e.Row.ToString).Album & "</color>"
                        'Return libraryData.Values(rowIndex).Album & "|" & libraryData.Values(rowIndex).Artist
                    Case Else
                        Return ""
                End Select
            End If
        End If
        Return ""
    End Function

    Public Overrides Function GetQueueData(colName As String, rowIndex As Integer) As Object
        'Return MyBase.GetLibraryData(colName)
        If queueData Is Nothing Then Return ""
        Select Case colName
            Case "colQArt"
                Return queueData.Values(rowIndex).ArtWork
            Case "colQTitle"
                Return queueData.Values(rowIndex).Title
            Case "colQLine2"
                'e.Value = "<color=ActiveCaption>" & libraryData(e.Row.ToString).Album & "</color>"
                Return queueData.Values(rowIndex).Album & "|" & queueData.Values(rowIndex).Artist
            Case Else
                Return ""
        End Select
    End Function

    Overrides Function GetLabelText(ctl As Control) As String
        Select Case ctl.Name
            Case "LabelXtraItem"
                Return "The Sisters of Mercy" & vbCrLf & "A slight case of overbombing gone crazy with a really long bit of text"
        End Select
        Return "OUICH"
    End Function

    Public Overrides Function NavigateDataset(dataset As String, direction As String) As Boolean
        currentItem = FindParentMusicItem(rootItem, currentItem.SonosServiceID, currentItem.ParentID)
        If currentItem IsNot Nothing Then getData(currentItem)
        'Return MyBase.NavigateDataset(dataset, direction)
        Return True
    End Function
#End Region

    '// Get the initial load of music services and put it in a root musicItem. Should be rebuilt when ported back into the main project
    Private Sub SetupMusicServices
        '// Setup the main (root) MusicServicesLibrary
        rootItem = InitializeRootMusicServices(musicServices)
        currentItem = rootItem
        '// Assign it to the Grid, stuffing the root MusicItem into the Tag for easy retrieval.

        Grid1.DataSource=currentItem.ItemIds


        'Grid1.Tag = rootItem
        'Grid1.DataSource = rootItem.ItemIds
    End Sub

    Private Sub TileView1_ItemRightClick(sender As Object, e As TileViewItemClickEventArgs) Handles TileView1.ItemRightClick
        frmProperties.Show
        Dim rowHandle = e.Item.RowHandle
        Dim key As String = currentItem.ItemIds(rowHandle)
        If currentItem.Items(key).Smapi IsNot Nothing Then
                frmProperties.SetDataSource(currentItem.Items(key).Smapi)
            Else
                frmProperties.SetDataSource(currentItem.Items(key))
        End If

        

    End Sub

    Private Sub TileView1_ItemClick(sender As Object, e As TileViewItemClickEventArgs) Handles TileView1.ItemClick
        Debug.Print("Item Clicked")
        Dim tv As TileView = sender

        '// The Parent musicItem is stored in the Tag. Get it.
        Dim parentItem As MusicItem = currentItem

        '// Get the rowIndex of the clicked Item
        Dim rowNum = e.Item.RowHandle
        Dim selectedItemKey As String = parentItem.ItemIds(rowNum)
        If selectedItemKey?.ToString <> "" Then
            '// now let's get the musicitem
            Dim musicItem As MusicItem = parentItem.Items(selectedItemKey)
            

            '// Special case to fill in Now Playing Window
            Select Case musicItem.ItemType
                Case itemType.track, itemType.stream, itemType.show
                    'FillInNowPlaying(musicItem)
                    'PG1.SelectedObject = musicItem
                    'Label3.Text = "ROW: " & rowNum
                    Exit Sub
            End Select
            getData(musicItem)
            
            'SetGridDataSource(musicItem, True)
            'PG1.SelectedObject = musicItem.Items.Values(0)
        End If

    End Sub

    Private Sub getData(musicItem As MusicItem)
        
        '// If it's empty, we better go fetch the data
            If musicItem.Items.Count = 0 Then
            Dim musicServiceInfo As MusicServiceInfo = musicServices(musicItem.SonosServiceID)
                '// Go Get the Data
                Dim numItems As Integer = getMetaData(musicServiceInfo, musicItem, 0, ITEMCOUNT)
                If numItems = 0 Then Exit Sub
            End If
            currentItem = musicItem
            Grid1.DataSource = musicItem.ItemIds

    End Sub

        '// The main data fetching routine. Need to rework this OUT of the form.
    Private Function getMetaData(musicServiceInfo As MusicServiceInfo, musicItem As MusicItem, startPosition As Integer, requestedCount As Integer) As Integer
        Dim index = startPosition
        Dim numItems As Integer = 0
        If musicItem IsNot Nothing Then
            If musicServiceInfo.Url.Contains("pandora") Then
                startPosition = GetPandoraList(musicServiceInfo, musicItem)
                numItems = musicItem.ItemCount
            Else
                '// if its sirius, then we need to fetch a sessionId
                If musicServiceInfo.Url.Contains("siriusxm") Then
                    musicServiceInfo.SessionId = GetSonosSessionID("http://192.168.1.57:1400", musicServiceInfo)
                End If
                numItems = getMetaData2(musicServiceInfo, musicItem, musicItem.ID, index, requestedCount)
            End If
        End If

        'imageLoader.GetImages(musicItem.Items, imageCache)
        'pg1.SelectedObject = musicItem
        Return numItems
    End Function


    Public Function FindParentMusicItem(rootItem As MusicItem, SonosServiceID As String, ID As String) As MusicItem
        '// start with the root
        If rootItem.ID = ID Then Return rootItem
        '// Then try each child in items.values
        For Each item As MusicItem In rootItem.Items.Values
            '// we are only interested in items (and their children that match the ServiceID)
            If item.SonosServiceID = SonosServiceID Then
                '// if we get a match return it.
                If item.ID = ID Then Return item
                '// Otherwise, if this item has children, recurse into them
                If item.Items.Count > 0 Then
                    Dim subItem As MusicItem = FindParentMusicItem(item, SonosServiceID, ID)
                    If subItem IsNot Nothing Then Return subItem
                End If
            End If

        Next
        Return Nothing
    End Function

End Class
