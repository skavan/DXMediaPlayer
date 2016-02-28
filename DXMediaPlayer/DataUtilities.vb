Imports System.IO
Imports System.Xml.Serialization

Module DataUtilities
    '// serialize a MusicItem List
    Public Sub SerializeMusicItemLibrary(mList As List(Of MusicItem), filepath As String)

        Dim ser As XmlSerializer = New XmlSerializer(GetType(List(Of MusicItem)))
        Using writer As TextWriter = New StreamWriter(filepath)
            ser.Serialize(writer, mList)
        End Using
    End Sub

    '// deserialize a MusicItem List
    Public Function DeSerializeMusicItemLibrary(filepath As String) As List(Of MusicItem)

        Dim ser As XmlSerializer = New XmlSerializer(GetType(List(Of MusicItem)))
        Dim items As New List(Of MusicItem)
        Try
            Using reader As TextReader = New StreamReader(filepath)
                items = ser.Deserialize(reader)
            End Using
        Catch ex As Exception

        End Try

        Return items
    End Function

     '// deserialize a MusicItem List
    Public Function DeSerializeMusicItemLibrary(data As String, isResource As Boolean) As Dictionary(Of String, MusicItem)
        Dim ser As XmlSerializer = New XmlSerializer(GetType(List(Of MusicItem)))
        Dim items As New List(Of MusicItem)
        Try
            Using reader As TextReader = New StringReader(data)
                items = ser.Deserialize(reader)
            End Using
        Catch ex As Exception

        End Try
        Dim dic As New Dictionary(Of String, MusicItem)

        For Each item As MusicItem In items
            dic.Add(item.ID, item)
        Next

        Return dic
    End Function
End Module
