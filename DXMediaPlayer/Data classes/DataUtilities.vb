Imports System.IO
Imports System.Xml.Serialization

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
End Module
