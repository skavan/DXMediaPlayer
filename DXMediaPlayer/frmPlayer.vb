
Imports DevExpress.XtraGrid.Views.Base

Public Class frmPlayer

    '// used for bound and unbound libraryData management
    Dim libraryData As Dictionary(Of String, MusicItem)
    Dim queueData As Dictionary(Of String, MusicItem)

    Dim libraryShadowList As String()
    Dim queueShadowList As String()

    Private Sub frmPlayer_Load(sender As Object, e As EventArgs) Handles Me.Load
        Grid1.DataSource = GetInitialDataset(librarydata, false)
        Grid2.DataSource = GetInitialDataset(queuedata,true)
        LabelTPH_L.Text = "Family Room"
        Me.Text = "Sonos Media Player"
    End Sub

    Private Sub frmPlayer_Shown(sender As Object, e As EventArgs) Handles Me.Shown

    End Sub

#Region "Data Handling"

    '// get a short (no scroll bar) or long (w scrollbar) dataset
    Private Function GetInitialDataset(ByRef dataDic As Dictionary(Of String, MusicItem), isSmallDataset As Boolean) As String()
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

    Public Overrides Function GetLibraryData(colName As String, rowIndex As Integer) As Object
        'Return MyBase.GetLibraryData(colName)
        If libraryData Is Nothing Then Return ""
        Select Case colName
            Case "colArt"
                Return libraryData.Values(rowIndex).ArtWork
            Case "colTitle"
                Return libraryData.Values(rowIndex).Title
            Case "colLine2"
                'e.Value = "<color=ActiveCaption>" & libraryData(e.Row.ToString).Album & "</color>"
                Return libraryData.Values(rowIndex).Album & "|" & libraryData.Values(rowIndex).Artist
            Case Else
                Return ""
        End Select
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

#End Region
End Class
