Imports System.Drawing

Namespace Smapi
<System.ComponentModel.TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter)), System.Serializable()>
Public Class MusicServiceInfo
        Property Key As String = ""
        Property Token As String = ""
        Property SessionId As String =""
        Property Url As String = ""
        Property Name As String = ""
        Property Id As String =""
        Property Index As Integer
        Property ArtURL As String
        Property itemType As Wsdl.itemType
        Property SonosServiceID As Integer
        Property HouseholdID As String
        Property DeviceID As String
        Property Username As String
        Property Password As String
        Property PartnerUsername As String
        Property PartnerPassword As String
    End Class
<System.ComponentModel.TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter)), System.Serializable()>
    Public Class MusicItemX
        Property Title As String
        Property Artist As String
        Property Album As String
        Property Year As String
        Property Rating As Integer
        Property Tracknumber As Integer
        Property AlbumID As String
        Property ArtistID As String
        Property ID As String
        Property CanPlay As Boolean
        Property SonosServiceID As Integer
    '// this is an attempt to be frugal with system resources.
    '// we add only found items to the dictionary
    '// but we create an array of ID's that is the total size
    '// of this category of elements.
    '// We only fill in ItemIds when we have a set of metadata in the dictionary
    '// else the value is Empty/"".
    '// the itemID array may, for example, be 1000 elements demonstrating the
    '// total number of elements in this category.
    '// the itemID order is the order sent to us by the music service.
    '// so until we get ALL the items into the dictionary, we can't do anything
    '// fancy vis-a-vis sorting or searching.
        Property ItemIds() As String()
        Property ItemType As Wsdl.itemType
        Property ItemCount As Integer
        Property Items As New Dictionary(Of String, musicItem)
        Property ArtURL As String
        Property AuthRequired As Boolean=False
        Property ParentID As String
    End Class
    Public Class MusicItem
        Inherits MusicItem2
        'Delegate Function GetArtWorkDelegate(url As String) As Image
        Property Items As New Dictionary(Of String, musicItem)
        Property Smapi As Object
        'Public GetArtMethod As GetArtWorkDelegate
        'Public Function GetArt As Image
        '    If GetArtMethod IsNot Nothing Then
        '        Return GetArtMethod(ArtUrl)
        '    End If
        '    Return Nothing
        'End Function
    End Class
<System.ComponentModel.TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter)), System.Serializable()>
    Public Class MusicItem2
        Property Title As String=""
        Property Artist As String=""
        Property Album As String=""
        Property Year As String=""
        Property Rating As Integer=0
        Property Tracknumber As Integer=0
        Property AlbumID As String=""
        Property ArtistID As String=""
        Property ID As String=""
        Property CanPlay As Boolean=False
        Property SonosServiceID As Integer=0
        Property ItemIds() As String()
        Property ItemType As Wsdl.itemType
        Property ItemCount As Integer
        'Property Items As New Dictionary(Of String, musicItem)
        Property ArtURL As String=""
        Property ArtLoaded As Boolean = False
        Property ArtWork As Image = Nothing
        Property AuthRequired As Boolean=False
        Property ParentID As String=""
    End Class

    End Namespace