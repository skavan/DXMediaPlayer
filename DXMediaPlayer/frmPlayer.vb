
Imports DevExpress.XtraGrid.Views.Base

Public Class frmPlayer

    '// used for bound and unbound data management
    Dim data As Dictionary(Of String, MusicItem)
    Dim shadowList As String()

    Private Sub frmPlayer_Load(sender As Object, e As EventArgs) Handles Me.Load
        GetInitialDataset(false)
        LabelTPH_L.Text = "Sonos Media Player"
    End Sub

#Region "Data Handling"
    
    '// get a short (no scroll bar) or long (w scrollbar) dataset
    Private Sub GetInitialDataset(isSmallDataset As Boolean)
        If isSmallDataset Then
            data = DeSerializeMusicItemLibrary(My.Resources.ShortAlbumTrackList, True)
        Else
            data = DeSerializeMusicItemLibrary(My.Resources.AlbumTrackList, True)
        End If
        
        ReDim shadowList(data.Count - 1)
        For i As Integer = 0 To data.Count - 1
            shadowList(i) = data.Values(i).ID
        Next
        Grid1.DataSource = shadowList
    End Sub

    '// the main method to fill in the TileView1 Tiles
    Private Sub TileView1_CustomUnboundColumnData(sender As Object, e As CustomColumnDataEventArgs) Handles TileView1.CustomUnboundColumnData
        If e.IsGetData Then
            Select Case e.Column.Name
                Case "colArt"
                    e.Value = data(e.Row.ToString).ArtWork
                Case "colTitle"
                    e.Value = data(e.Row.ToString).Title
            End Select
        End If
    End Sub

    Overrides Function GetLabelText(ctl As Control) As String
        Select Case ctl.Name
            Case "LabelXtraItem"
                Return "The Sisters of Mercy" & vbCrLf & "A slight case of overbombing gone crazy with a really long bit of text"
        End Select
        Return "OUICH"
    End Function

#End Region
End Class
