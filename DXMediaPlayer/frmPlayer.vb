
Imports DevExpress.XtraGrid.Views.Base

Public Class frmPlayer

    '// used for bound and unbound data management
    Dim data As Dictionary(Of String, MusicItem)
    Dim shadowList As String()

    Private Sub frmPlayer_Load(sender As Object, e As EventArgs) Handles Me.Load
        GetInitialDataset(false)
        LabelTPH_L.Text = "Sonos Media Player"
    End Sub

    Private Sub frmPlayer_Shown(sender As Object, e As EventArgs) Handles Me.Shown

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

    Public Overrides Function GetLibraryData(colName As String, rowIndex As Integer) As Object
        'Return MyBase.GetLibraryData(colName)
        If data Is Nothing Then Return ""
        Select Case colName
            Case "colArt"
                Return data.Values(rowIndex).ArtWork
            Case "colTitle"
                Return data.Values(rowIndex).Title
            Case "colLine2"
                'e.Value = "<color=ActiveCaption>" & data(e.Row.ToString).Album & "</color>"
                Return data.Values(rowIndex).Album & "|" & data.Values(rowIndex).Artist
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
