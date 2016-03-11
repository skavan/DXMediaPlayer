Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Tile
Imports Sonos.Smapi
Imports Sonos.Smapi.Wsdl

Public Class frmPlayer

#Region "Variables & Enums"
    '// used for bound and unbound libraryData management
    Dim libraryData As Dictionary(Of String, XmlMusicItem)
    Dim queueData As Dictionary(Of String, XmlMusicItem)

    Dim libraryShadowList As String()
    Dim queueShadowList As String()

    Dim musicServices As New Dictionary(Of String, MusicServiceInfo)            'A collection of MusicServices
    Dim rootItem As MusicItem                                                   'This is the top-most node
    Dim currentItem As MusicItem
    Const ITEMCOUNT = 50
    '// a dictionary tohold images
    Dim imageCache As New Dictionary(Of String, Image)                          'A dictionary to hold cached images
    '// a class to download images async
    Dim WithEvents imageLoader As New ImageDownloader                           ' A class to load images Async

#End Region

#Region "Initialization"
    Private Sub frmPlayer_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetupMusicServices
        'Grid1.DataSource = GetInitialDataset(librarydata, false)
        Grid2.DataSource = GetInitialDataset(queuedata,true)
        LabelTPH_L.Text = "Family Room"
        Me.Text = "Sonos Media Player"
    End Sub

#End Region


#Region "Data Handling"

    '// get a short (no scroll bar) or long (w scrollbar) dataset from the resource XML files
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

    ''// replace strings with enum
    'Private Sub UpdateDisplayStyle(dataset As String, tileStyle As eTileStyle)
    '    If libraryTileStyle<> tileStyle Then SetTileStyle(dataset, tileStyle)
    'End Sub

    '// fill in Library Data Elements
    Public Overrides Function GetLibraryData(colName As String, rowIndex As Integer) As Object
        'Return MyBase.GetLibraryData(colName)
        'If libraryData Is Nothing Then Return ""
        Dim musicItem As MusicItem = currentItem
        If musicItem Is Nothing Then Return ""
        Dim key As String = musicItem.ItemIds(rowIndex)
        If key IsNot Nothing Then
            If musicItem.Items.ContainsKey(key) Then
                '// probably a more efficient way to do this!
                'Select Case musicItem.Items(key).itemType
                '    Case itemType.container, itemType.artist
                '        UpdateDisplayStyle("Library", eTileStyle.SingleLine)
                '    Case itemType.track
                '        UpdateDisplayStyle("Library", eTileStyle.MultiLineFull)
                '    Case itemType.album
                '        UpdateDisplayStyle("Library", eTileStyle.MultiLineBasic)
                '    Case Else
                '        UpdateDisplayStyle("Library", eTileStyle.SingleLine)
                'End Select

                Select Case colName
                    Case "colArt"
                        If musicItem.Items(key)?.ArtURL <> "" Then
                            If imageCache.ContainsKey(musicItem.Items(key)?.ArtURL) Then
                                '// this is an attempt to avoid repainting the image if we already have it.
                                'If TileView1.GetRowCellValue(rowIndex, colArt) <> (musicItem.Items(key).ArtURL)
                                    Return imageCache(musicItem.Items(key).ArtURL)
                                    'tileView1.SetRowCellValue(e.ListSourceRowIndex,colURL, (musicItem.Items(key).ArtURL))
                                'End If
                            End If
                        End If

                        Return musicItem.Items(key).ArtWork
                        
                    Case "colTitle"
                        Return musicItem.Items(key).Title
                        
                    Case "colLine2"
                        Return musicItem.Items(key)?.Album & "|" & musicItem.Items(key)?.Artist
                        
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
        Return True
    End Function
#End Region

    Public Overloads Sub SetDataSource(dataSet As eDataSet, musicItem As MusicItem)
        '// Look at the first item of the dataset. And figure out what to do WRT style
        '// not sure i like the data form knowing about single lines etc... but don't see a way around it.
        '// maybe change to (a) Has Artwork and (b) Has MultiLine and let the base form decide how to display the data.
        Dim style as eTileStyle = eTileStyle.SingleLine
        Select Case musicItem.Items.Values(0).ItemType
            Case itemType.container, itemType.artist
                style = eTileStyle.SingleLine
            Case itemType.track
                style = eTileStyle.MultiLineFull
            Case itemType.album
                style =  eTileStyle.MultiLineBasic
            Case Else
                style = eTileStyle.SingleLine
        End Select
        SetDataSource(dataSet, musicItem.ItemIds, style)

    End Sub

    '// Get the initial load of music services and put it in a root musicItem. Should be rebuilt when ported back into the main project
    Private Sub SetupMusicServices
        '// Setup the main (root) MusicServicesLibrary
        rootItem = InitializeRootMusicServices(musicServices)
        currentItem = rootItem
        '// Assign it to the Grid, stuffing the root MusicItem into the Tag for easy retrieval.
        SetDataSource(eDataSet.Library, currentItem)
        'Grid1.DataSource=currentItem.ItemIds
        
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

    '// go get the Music Library data we need
    Private Sub getData(musicItem As MusicItem)     
        '// If it's empty, we better go fetch the data
        If musicItem.Items.Count = 0 Then
            Dim musicServiceInfo As MusicServiceInfo = musicServices(musicItem.SonosServiceID)
            '// Go Get the Data
            Dim numItems As Integer = getMetaData(musicServiceInfo, musicItem, 0, ITEMCOUNT)
            If numItems = 0 Then Exit Sub
        End If
        currentItem = musicItem
        imageLoader.GetImages(currentItem.Items, imageCache)
        SetDataSource(eDataSet.Library, currentItem)
        'Grid1.DataSource = musicItem.ItemIds
    End Sub

    Private Sub imageLoader_ArtWorkReceived(url As String, image As Image) Handles imageLoader.ArtWorkReceived
        Dim parentItem As MusicItem = currentItem
        If Not imageCache.ContainsKey(url) Then
            Debug.Print("ADDING TO CACHE: ")
            imageCache.Add(url, image)
        End If

        Dim visibleRows As Dictionary(Of Integer, TileViewItem) = TileView1.GetVisibleRows
        For Each tileViewItem In visibleRows.Values
            Dim musicItem = GetMusicItemFromRow(TileView1, currentItem, tileViewItem.RowHandle)
            If musicItem.ArtURL = url Then
                TileView1.RefreshRow(tileViewItem.RowHandle)
                'TileView1.SetRowCellValue(tileViewItem.RowHandle, colArtWork, imageCache(url))
            End If
            musicItem.ArtWork = image
            'If musicItem IsNot Nothing Then

            '    '// we have a filled row
            '    If musicItem.ArtURL= url Then
            '        Dim Col As DevExpress.XtraGrid.Columns.GridColumn = TileView1.Columns.ColumnByFieldName("colArtWork")
            '        Debug.Print("Received Artwork:" & tileViewItem.RowHandle)
            '        TileView1.SetRowCellValue(tileViewItem.RowHandle, Col, imageCache(url))
            '        Exit For
            '    End If

            'End If
        Next
    End Sub

    Private Function GetMusicItemFromRow(View As TileView, parentItem As MusicItem, rowIndex As Integer) As MusicItem
        Dim itemKey As String = parentItem.ItemIds(rowIndex)
        '// this might be a filled or a blank item
        If itemKey?.ToString <> "" Then
            Return parentItem.Items(itemKey)
        Else
            Return Nothing
        End If
    End Function

End Class
