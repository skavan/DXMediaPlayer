Imports System.IO
Imports System.Xml.Serialization
Imports Sonos.Smapi

Module DataUtilities

    '// serialize a XmlMusicItem List
    Public Sub SerializeMusicItemLibrary(mList As List(Of XmlMusicItem), filepath As String)

        Dim ser As XmlSerializer = New XmlSerializer(GetType(List(Of XmlMusicItem)))
        Using writer As TextWriter = New StreamWriter(filepath)
            ser.Serialize(writer, mList)
        End Using
    End Sub

    '// deserialize a XmlMusicItem List
    Public Function DeSerializeMusicItemLibrary(filepath As String) As List(Of XmlMusicItem)

        Dim ser As XmlSerializer = New XmlSerializer(GetType(List(Of XmlMusicItem)))
        Dim items As New List(Of XmlMusicItem)
        Try
            Using reader As TextReader = New StreamReader(filepath)
                items = ser.Deserialize(reader)
            End Using
        Catch ex As Exception

        End Try

        Return items
    End Function

    '// deserialize a XmlMusicItem List
    Public Function DeSerializeMusicItemLibrary(data As String, isResource As Boolean) As Dictionary(Of String, XmlMusicItem)
        Dim ser As XmlSerializer = New XmlSerializer(GetType(List(Of XmlMusicItem)))
        Dim items As New List(Of XmlMusicItem)
        Try
            Using reader As TextReader = New StringReader(data)
                items = ser.Deserialize(reader)
            End Using
        Catch ex As Exception

        End Try
        Dim dic As New Dictionary(Of String, XmlMusicItem)

        For Each item As XmlMusicItem In items
            dic.Add(item.ID, item)
        Next

        Return dic
    End Function

#Region "MusicItem related"

    '// Given a rootMusic Item and an ID (and a ServiceID to narrow scope), go find the target MusicItem and return it.
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

#End Region
End Module
