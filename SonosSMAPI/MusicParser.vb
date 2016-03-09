Imports System.IO
Imports System.Net
Imports System.ServiceModel
Imports System.Text
Imports System.Xml
Imports Sonos.Smapi.Wsdl

Namespace Smapi

Public Module MusicParser
    Public Const ROOT_ITEM_STRING = "SONOSHOME"
    Const HOUSEHOLDID As String = "Sonos_vQYSeT8nVVkancwy4oz0NXvbWv"
    Const DEVICE As String = "00-0E-58-A7-2F-96:A"
    Const GOOGLEKEY As String = "Ci0xL0VhYVYzT0Q0NDg3OWJrLTdhZ1YzckVXWUN2c2dTVXpRZGw2VDhRS2FhcmcQjIrR4ZIq"
    Const GOOGLETOKEN As String = "ya29.agIaSos8nlWXLZroJ9GsujnjPF2orTtQLZb86CDg_bbEWo1ynnCyoYqbOPT-qz18TOVHBG8"
    Const AMAZONTOKEN As String = "{enc:LkRahCP0owJPF/CKCOnmb9hH0vS/0i9/Y9YdBKK5L/nkK8tAKLTbNxJjr6A6M9Z3r+25E3rZpRJS+/DNbPw6icU7j4Ahyo1AzUhFXqtMsYpQcfX2TyND7GBLNqnnb7Q8TGquY8Q2wFkXGcyPQWRn4PxvZvCnVnm4XUPjAp25JDYxeXAD5xq8grGjyvI9xBTGx6PJgPPXYxghSxiNKB6NY1ArN9gDFd+1p2wgqZddrR/FEbtunEBz20QEvsIOy8rjvYAG17CWSczZ4zZl0RlgyUHlAdQxG1QFWEiFpH6/IcehU4dPznhdCAhV/czG0iJUgrs4jw5+C6YtlRydPWVMYaj23P5fyKlt5wAqVYr7SuY+Mq0hwg5UDa/hHWuwHP5FGyq+jiUU8ZYF5nSHX+sj587lGeJilBicTqdCG2LtgAfk/npp4T3vfct+bowYs8TwcTGP6gLIG6vAE28TJkkMhShfSjSXSFFJAmInTc2dm+lBv0GxnnhfLGjO0capH2B8LOcP2Xm0LnwgPkfKBls8gtYt7BsVmVWf2lLeEBfLz26LrMo7Hsp6mWoRwZWCzQuH4C1tkF+8Dj+b5muUQbrooSeiJjcWmrGVwYPkG8aGFhD3gSSB+bgE2VUHYws75fnHHSKcbAjtlF6cWcM2c48GNOybNGbIcy8IJ04ohjz4PyQz1EgP7rdCfgU2ApTNi2L9jhrkHOibKwY2+FT0H/mE1WZMqqc7MlgrZcOL84YskChlNxaQcqDOoRq7UfORVH93oRSCNSq9to1yLjVGlOiDARKQOF/fj3yflYcXmV7qrZE//HR6tzYPeVRtjDy5dWT2dXe28FVGw/4eRLvtKFBNuFAHeub2jiOL0I1P8P8XrcUuG7afAcv1gOMpgjLNrZivPlLyguScN0a42bk4QVVZOJJ8OCJ9eWBofH3j2ub75ThunRXNTj+6OsvEYOXQSBomVCrskM/5AwIcfa0AEWRSP7e+ah7Yh3aJhYsaKBmWSq8IiIaNKllLkyipsxD7q4Hg69wcDUvVmeUdTfWUizDtPtbJ8MkaObeL6eoftvr04aA=}{key:lhShBbSTThFHfI4KvxB5Mjn7L5750ovXiuSHt+5bdxZYDq3UzoohpUquQA8Vc9rtHBABvsrVSVE9PlpTUNBcp8I7O3Kdz0k9cotJieug2ujn6IHq9iCrS2h/0meJqL/TTPC65gYlgnfMu1G/DL64Pr4qoJU4Cuss7y6w16bgJIBQ0uREcRNVj0hrecFpVbKKz1KbSqIOukne1ULgh/tWgsu3Tv1GwRRrOukeB9eOPudZrZBPvEdsCFCYvKep+kQr8CWMZRcSYL63BASDZdizTWlZ/T0DL6hy185mngXsMNaf/UgwjZWWauXH6WbV7tPmYluIg6k93yH2kmWdXW7KhA==}{iv:MTUcgcmbkhf4/FQgV8klHw==}{name:QURQVG9rZW5FbmNyeXB0aW9uS2V5}{serial:Mg==}"
    Const AMAZONKEY As String = "MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC6B2/iET4WydHvQpjV2+hb6LfHA7nVz08m2z3wPvWFuywaHg4YlHKaCNZov2eCNQ0iwp/zPOCSYMMKdIO4SVM2QFX7QoXPXAZCCxGOX//cKJaahIMuVgmlpYkZ4Dk3/pRcU1crTbqIujnhSgR0cdagBpJa1bC/ssa/soNsRZq3eaUgPVmIe/7d91LhWQ5S8NPMKZKgRpiV2gvwXK1uUcr1U9/6p/zUZEObr3R0VhbbU6nDPfs5jbHDP/qXZNEst0V0frXZxHKvQiDynEtA/fFWrWtCYzuxlU7u+cpZBVGRXce0L2quZaddRu1mwG++saIH75JWZS9PFXjyqsMQLF5bAgMBAAECggEBAKDk0cWquBKplyLibKF6wyrKyjgcZtwfowuuT5G8QknZqrkkCXkE22GSBy8tbG7XR3cq8YBExM6VVdtzDoGTGZ8wYa0gye6gXuE8BYEoUnituiQGPBcVXt6fn0Z8AcEGNHbttdTEM3MR7gGGLPgxhTl+tZJOg8Io83DVConYYYoQ9+WCe8HcQt/HHHk4wZK6Q1CsGSzMXIGYmR1Zyoe21O/WdPSQyYNQhUY38DpOAnbWaaRcqh52zHsAoqTnIhBe/rRU5FImvRY5OugdQe4NmbZ4tCNfp06hfZxShDyDzjijJ4NFO0bQXSL19lhxiI7yNNdAKXm5GS7YyfGUNz9JXHECgYEA5VeRUfVzCXIq15SJvg9ZBAasOkog2RJQr8FQba/f2BK3aZWxQrbXnqET+W+XHCO0N63aD8jwlnNFUVrv0GW+5DEUaTDjNWE0Ko7cNIjXbI7Nw8kVVFZpWmmXlNNouqHhtGhKj1OFnHELgal80SN1fZK4YHq/dBw9Sv0ai8etVnUCgYEAz6cF3VXPbNXO7/whwwUKkJLKd927jVGGlXPIo370wnKyaKH7Wfp05Pkst7x7baEeODKS9em0NV0yvRVa1uvWaVwc5N4Wa1dLMOhMMnQLflJq6S/+oNzMbDK247c861rAqT+23j7LLVBbNeklcgAAf90drsYXqJK4IJ21iTRQZ48CgYAz/bMC+nq2tHwD3TeQr5gFcqHOoQlrTFygS1m5U/qR2EReGkJSFnMxEeEeVe7SwUFUsgPSOkJYSQ94zv24p6grwPiYlC2d50hVMYe4HSGBCaqZ7Nb6BJDjnzZu1bJTROmO5WmprkyG15W9MjgKjVhQPFex/4Gxh8lZW6GN+JlSFQKBgG20kCSfcTbOvsWLL0sZHrvC0bUCMFQ4/iF3SkkWibkxX5BoA6NF7vwJqNZpfcwtkwmdroa9Mbf/xQ6geZ2Se3SudZV0v+N1+wObtqXxKSFT6afI832JKXcAVB96b3ToPWEtiC9ifXUzPvz8cAwKdDkZWNU9UsJ8wUp6B661NMWBAoGAczNqzv1fVaS7VCZTK2SaG1bsZWpw907/1iTa4YROdCVJCSy06H7J4FJ2HZNZv/l70wYqoOm99SMXgriPCgsnYeO7Jso0gcYz8Mpw/0O4BHrU3cSAgXCHL2GrSbd0M24ksNWQaiEOyOoVMIfiTZscaFHzTVl0PO3RzeHOzrYS0bI="
        

#Region "MetaData Retrieval methods"
     '// the main data music library data retrieval routine
    Public Function getMetaData2(ByRef musicServiceInfo As MusicServiceInfo, ByRef musicItem As MusicItem, id As String, index As Integer, count As Integer) As Integer
        If musicItem.ItemType=itemType.track Then Return -1
        Dim sonosSoap As  Wsdl.SonosSoapClient
        Dim getMetadataRequest As New Wsdl.getMetadataRequest
        Dim credentials As New Wsdl.credentials
        Dim binding As New BasicHttpBinding

        Dim address As New EndpointAddress(musicServiceInfo.Url)
        
        Dim HUAP As New HttpUserAgentEndpointBehavior("Linux UPnP/1.0 Sonos/26.1-76230 (WDCR:Microsoft Windows NT 6.2.9200.0)")

        'Dim eab As New EndpointAddressBuilder(address)
        'eab.Headers.Add(AddressHeader.CreateAddressHeader("user-agent",String.Empty, "Linux UPnP/1.0 Sonos/26.1-76230 (WDCR:Microsoft Windows NT 6.2.9200.0)"))
        'eab.Headers.Remove(AddressHeader.CreateAddressHeader("VsDebuggerCausalityData"))
        'address = eab.ToEndpointAddress
        
        '// HERE'S THE IMPORTANT BIT FOR SSL
        If musicServiceInfo.Url.ToLower.StartsWith("https") Then
            binding.Security.Mode = BasicHttpSecurityMode.Transport      
        End If
        sonosSoap =new Wsdl.SonosSoapClient(binding,address)
        
        '// credentials
        Dim lt As loginToken = New Wsdl.loginToken
        lt.householdId = musicServiceInfo.householdID
        lt.token = musicServiceInfo.token
        lt.key = musicServiceInfo.key
        
        credentials.deviceId = musicServiceInfo.deviceID
        '// If we don't have a token or a key and we do have a sessionID, use the sessionID
        If (musicServiceInfo.Token & musicServiceInfo.Key)="" And musicServiceInfo.SessionId<>"" Then
            credentials.Item = musicServiceInfo.SessionId
        Else
            credentials.Item = lt
        End If
        
        

        getMetadataRequest.id = id
        getMetadataRequest.index = index
        getMetadataRequest.count = count
        getMetadataRequest.credentials=credentials

        Dim getMdr As getMetadataResponse = Nothing
        Try
            sonosSoap.Endpoint.EndpointBehaviors.Add(HUAP)
            getMDR = sonosSoap.getMetadata(getMetadataRequest)
        Catch ex As ProtocolException
            Debug.Print("CUSTOM" & ex.ToString)
        Catch ex As FaultException(Of customFaultDetail)
            Debug.Print("CUSTOM" & ex.Detail.ToString)
        Catch ex As FaultException(Of String)
            Debug.Print("STRING" & ex.Detail.ToString)
        Catch ex As FaultException
            'Debug.Print(HUAP.inspector.newToken)
            If HUAP.inspector?.faultCode = "soap:Client.TokenRefreshRequired" Then
                If HUAP.inspector?.newToken<> musicServiceInfo.token Then
                    musicServiceInfo.token = HUAP.inspector.newToken
'                    txtToken.Text = token
                    If HUAP.inspector?.newKey <> musicServiceInfo.key Then musicServiceInfo.key = HUAP.inspector.newKey
                    Return getMetaData2(musicServiceInfo, musicItem, id,index,count)
                    
                End If
            End If
            
        Catch ex As Exception
            Debug.Print(ex.ToString)
'            sonosSoap.InnerChannel.
        End Try
        
        If getMDR IsNot Nothing Then
            Debug.Print($"We have {getMdr.getMetadataResult.count} of a total of {getMdr.getMetadataResult.total} results")
            Return FillInData(getMdr.getMetadataResult,musicItem,musicServiceInfo )
        End If
        '//we don't have anything to do!
        Return -1
    End Function

    '// the routine used to take sonos provided metadata and put it into a musicItem structure
    Private Function FillInData(mediaList As Wsdl.mediaList, ByRef parentItem As MusicItem, musicServiceInfo As MusicServiceInfo) As Integer
        '// we need to tell the parent, how many children it has.
        If parentItem?.itemCount>0 Then
            
        Else
            '// redim and reset the array
            parentItem.itemCount = mediaList.total
            Dim temp(parentItem.itemCount-1) As String
            parentItem.itemids = temp
        End If
    
        '// now add the items
        Dim ctr As Integer = mediaList.index
        For each item As Object In mediaList.Items  'mediacollection or mediaMetaData
            Dim musicItem As New MusicItem
            
            '// do we already have this item in the items dictionary? if not, add it to the dictionary and update the master array
            If Not parentItem.Items.ContainsKey(item.id) Then
                musicItem.Smapi = item
                '// the common items
                musicItem.ID = item.id
                musicItem.Title = item.title
                musicItem.SonosServiceID = parentItem.SonosServiceID
                musicItem.ItemType=item.itemType
                musicItem.ParentID = parentItem.Id
                '// now we have to fill in stuff based on the item type.
                If item.GetType Is GetType(mediaCollection) Then
                    Dim medColItem As mediaCollection = item
                    
                    musicItem.ArtURL=item?.albumArtURI?.Value
                    musicItem.AuthRequired=item.authRequired
                    musicItem.Artist = item.artist
                    musicItem.ArtistID = item.artistId
                    musicItem.CanPlay=item.canPlay
                    
                ElseIf item.GetType Is GetType(mediaMetadata) Then
                    
                    If item.Item.GetType Is GetType(streamMetadata) Then
                        Dim streamItem As streamMetadata = item.Item
                        musicItem.ArtURL = streamItem.logo.Value
                        musicItem.CanPlay=True
                        
                    Else
                        Dim trackData As trackMetadata = item.Item
                        musicItem.Album = trackData.album
                        
                        musicItem.AlbumID = trackData.albumId
                        musicItem.Artist = trackData.artist
                        musicItem.ArtistID = trackData.artistId
                        musicItem.Rating = trackData.rating
                        musicItem.Tracknumber = trackData.trackNumber
                        musicItem.CanPlay = trackData.canPlay
                        musicItem.ArtURL = trackData.albumArtURI.Value
                        musicItem.AuthRequired = trackData.albumArtURI.requiresAuthentication
                    End If
                    
                             
                Else
                    Debug.Print("ANOTHER CASE:" & item.GetType.ToString)
                End If

                '// we need to update the flags
                parentItem.itemIds(ctr)=musicItem.ID
                parentItem.Items.Add(musicItem.ID, musicItem)
            End If
            ctr += 1
        Next
        '// return the starting point of the entry!
        Return mediaList.Total
    End Function

#End Region

#Region "Data and XML Utilities"

    '// convert a musicservice structure into a music item --
    Public Function musicServiceToItem(musicServiceInfo As MusicServiceInfo) As MusicItem
        Dim musicItem As New MusicItem
        With musicServiceInfo
            musicItem.Title = .Name
            musicItem.ID = .Id
            musicItem.SonosServiceID = .SonosServiceId
            musicItem.ArtURL = .ArtURL
            musicItem.canPlay = False
            musicItem.itemType = Wsdl.itemType.container
        End With
    Return musicItem
    End Function

     '// XML Element finder utility
    Public Function getXMLElementByChildFieldNameValue(el As XmlElement, fieldName As String, subfieldName As String, subfieldValue As String) As XmlElement
        If el Is Nothing Then Return Nothing

        '// get all the elements with the fieldname. i.e. <struct>
        Dim memberEl As XmlNodeList = el.GetElementsByTagName(fieldName)
        '// iterate through them
        For Each childEl As XmlElement In memberEl
            '// get the first child element matching the fieldname
            Dim subEl = childEl(subfieldName)
            If subEl IsNot Nothing Then
                '// look for a value match
                If subEl(subfieldName)?.InnerText = subfieldValue Then
                    Return subEl
                End If
            End If
        Next
        Return Nothing
    End Function

    '// Setup the root nodes. This must be autodone when transplanted into SonosMetadata
    '// it fills in a "Master Music Services"" Dictionary and returns the same data in the Items Dictionary of a root MusicItem
    Public Function InitializeRootMusicServices(ByRef musicServices As Dictionary(Of String, MusicServiceInfo),Optional repeat As Boolean=False) As MusicItem
        Dim musicServiceInfo As MusicServiceInfo

        musicServiceInfo = New MusicServiceInfo With {.SonosServiceID = 151, .HouseholdID = HOUSEHOLDID, 
            .DeviceID = DEVICE, .Name = "Google Play", .Key = GOOGLEKEY, .Token = GOOGLETOKEN, 
            .Url = "https://mclients.googleapis.com/music/sonos/wsf/smapi", 
            .Id = "root", .Index = 0, .itemType = itemType.container}
        musicServices.Add(musicServiceInfo.SonosServiceID, musicServiceInfo)

        musicServiceInfo = New MusicServiceInfo With {.SonosServiceID = 201, .HouseholdID = HOUSEHOLDID, 
            .DeviceID = DEVICE, .Name = "Amazon Player", .Key = AMAZONKEY, .Token = AMAZONTOKEN, 
            .Url = "https://cloudplayer.ws.sonos.com/smapi", .Id = "root", .Index = 0, .itemType = itemType.container}
        musicServices.Add(musicServiceInfo.SonosServiceID, musicServiceInfo)

        musicServiceInfo = New MusicServiceInfo With {.SonosServiceID = 6, .HouseholdID = HOUSEHOLDID,
            .DeviceID = DEVICE, .Name = "iHeart Radio", .Key = "", .Token = "58429589",
            .Url = "http://sonos.iheart.com/soap", .Id = "root", .Index = 0, .itemType = itemType.container}
        musicServices.Add(musicServiceInfo.SonosServiceID, musicServiceInfo)

        musicServiceInfo = New MusicServiceInfo With {.SonosServiceID = 3, .HouseholdID = HOUSEHOLDID,
            .DeviceID = DEVICE, .Name = "Pandora", .Key = "", .Token = "",
            .Url = "https://tuner.pandora.com/services/xmlrpc/?method=auth.partnerLogin",
            .Id = "root", .Index = 0, .itemType = itemType.container,
            .PartnerUsername = "sonos", .PartnerPassword = "e15d5838a85c70e526a0b97fab6acfa7",
            .Username = "suresh.kavan@gmail.com", .Password = "f124mld"}
        musicServices.Add(musicServiceInfo.SonosServiceID, musicServiceInfo)

        musicServiceInfo = New MusicServiceInfo With {.SonosServiceID = 37, .HouseholdID = HOUSEHOLDID,
            .DeviceID = DEVICE, .Name = "Sirius XM", .Key = "", .Token = "", .SessionId = "",
            .Url = "http://siriusxm.ws.sonos.com/smapi",
            .Id = "root", .Index = 0, .itemType = itemType.container,
            .PartnerUsername = "sonos",
            .Username = "sureshkavan"}
        musicServices.Add(musicServiceInfo.SonosServiceID, musicServiceInfo)

        Dim rootItem As MusicItem= New MusicItem
        With rootItem
            .Title="My Music Library"
            .ItemCount = musicServices.Count
            .ID = ROOT_ITEM_STRING
            ReDim .ItemIds(.ItemCount-1)
        End With
        Dim ctr As Integer=0
        For Each item As MusicServiceInfo In musicServices.Values
            Dim musicItem As MusicItem = musicServiceToItem(item)
            musicItem.ParentID = ROOT_ITEM_STRING
            rootItem.Items.Add(musicItem.SonosServiceID,musicItem)
            rootItem.ItemIds(ctr) = musicItem.SonosServiceID
            ctr +=1
        Next
        '// temporary structure
        If repeat Then
            rootItem.ItemCount = rootItem.ItemCount*3
             ReDim Preserve rootItem.ItemIds((rootItem.ItemCount)-1)
            For Each item As MusicServiceInfo In musicServices.Values
                Dim musicItem As MusicItem = musicServiceToItem(item)
                rootItem.Items.Add(musicItem.SonosServiceID & ctr,musicItem)
                rootItem.ItemIds(ctr) = musicItem.SonosServiceID
                ctr +=1
            Next
            For Each item As MusicServiceInfo In musicServices.Values
                Dim musicItem As MusicItem = musicServiceToItem(item)
                rootItem.Items.Add(musicItem.SonosServiceID & ctr,musicItem)
                rootItem.ItemIds(ctr) = musicItem.SonosServiceID
                ctr +=1
            Next
            End If
        Return rootItem
    End Function
#End Region
    
#Region "HTTP, SOAP and POST methods" 
    '// create an Http Sync Client
    Private Function CreateHttpSyncClient(ByVal useragent As String, targetUri As Uri) As HttpWebRequest
        Dim hc = CType(WebRequest.Create(targetURI), HttpWebRequest)
        hc.AllowAutoRedirect = True
        hc.AutomaticDecompression = DecompressionMethods.Deflate Or DecompressionMethods.GZip
        hc.Timeout = 15000
        hc.UserAgent = useragent
        Return hc
    End Function

    '// routine to send an XML request
    Public Function SendXMLRequest(url As String, postData As String, method As String, optional headers As Dictionary(Of String, String)=Nothing) As String
        Dim req As WebRequest = WebRequest.Create(New Uri(url))
        '// if we have passed in some custom headers, set them!
        If headers IsNot Nothing Then
            For Each key As String In headers.Keys
                req.Headers.Add(key, headers(key))
            Next
        End If
        'req.Headers.Add()
        Dim encoding As New System.Text.UTF8Encoding()
        Dim byteArray As [Byte]() = encoding.GetBytes(postData)
        req.ContentType = "text/xml"
        req.Method = method
        req.ContentLength = byteArray.Length


        Dim stream = req.GetRequestStream()
        stream.Write(byteArray, 0, byteArray.Length)
        stream.Close()

        Dim response = req.GetResponse().GetResponseStream()

        Dim reader As New StreamReader(response)
        Dim res = reader.ReadToEnd()
        reader.Close()
        response.Close()

        Return res
    End Function

    '// specific soap request to get a sonosSessionID for a given service.
    Public Function GetSonosSessionID(deviceURL As String, musicServiceInfo As MusicServiceInfo) As String
        Dim xml As String = $"<?xml version=""1.0"" encoding=""utf-8""?>
                            <s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"" 
                            s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
                            <s:Body><u:GetSessionId xmlns:u=""urn:schemas-upnp-org:service:MusicServices:1"">
                            <ServiceId>{musicServiceInfo.SonosServiceID}</ServiceId>
                            <Username>{musicServiceInfo.Username}</Username>
                            </u:GetSessionId></s:Body></s:Envelope>"
        Dim headers As New Dictionary(Of String, String)
        headers.Add("SOAPACTION","urn:schemas-upnp-org:service:MusicServices:1#GetSessionId")
        Dim res As String = SendXMLRequest($"{deviceURL}/MusicServices/Control",xml,"POST", headers)
        Dim xmlDoc As New XmlDocument
        xmlDoc.LoadXml(res)
        Return xmlDoc.GetElementsByTagName("SessionId")(0).InnerText
    End Function

     ' POST an XML string
    Private Function POST(url As String, jsonContent As String) As String
	    Dim request As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
	    request.Method = "POST"

	    Dim encoding As New System.Text.UTF8Encoding()
	    Dim byteArray As [Byte]() = encoding.GetBytes(jsonContent)

	    request.ContentLength = byteArray.Length
	    request.ContentType = "text/xml"

	    Using dataStream As Stream = request.GetRequestStream()
		    dataStream.Write(byteArray, 0, byteArray.Length)
	    End Using
	    Dim length As Long = 0
	    Try
		    Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
			    length = response.ContentLength
		    End Using
			    ' Log exception and throw as for GET example above
	    Catch ex As WebException
	    End Try
            Return ""
    End Function

#End Region

#Region "Pandora Specific"
    '// Special case to Handle Pandora item Requests
    Public Function GetPandoraList(ByRef musicServiceInfo As MusicServiceInfo, ByRef musicItem As MusicItem) As Integer
        
        Dim xml As String = $"<?xml version=""1.0"" encoding=""utf-8""?><methodCall><methodName>auth.partnerLogin</methodName><params><param><value><struct><member><name>version</name><value><string>4</string></value></member><member><name>deviceModel</name><value><string>DesktopControllerPC</string></value></member><member><name>username</name><value><string>{musicServiceInfo.partnerUserName}</string></value></member><member><name>password</name><value><string>{musicServiceInfo.partnerPassword}</string></value></member></struct></value></param></params></methodCall>"
        Dim res As String = SendXMLRequest("https://tuner.pandora.com/services/xmlrpc/?method=auth.partnerLogin",xml,"POST")
        Dim partnerAuthToken As String = getPandoraFieldValue(res, "partnerAuthToken")
        Dim partnerId As String = getPandoraFieldValue(res, "partnerId")
        Dim urlEncPartnerAuthToken As String = Uri.EscapeDataString(partnerAuthToken)
        xml = $"<?xml version=""1.0"" encoding=""utf-8""?><methodCall><methodName>auth.userLogin</methodName><params><param><value><struct><member><name>loginType</name><value><string>user</string></value></member><member><name>username</name><value><string>{musicServiceInfo.userName}</string></value></member><member><name>password</name><value><string>{musicServiceInfo.password}</string></value></member><member><name>partnerAuthToken</name><value><string>{partnerAuthToken}</string></value></member></struct></value></param></params></methodCall>"
        res = SendXMLRequest($"https://tuner.pandora.com/services/xmlrpc/?method=auth.userLogin&partner_id={partnerId}&auth_token={urlEncPartnerAuthToken}",xml,"POST")
        Dim userAuthToken = getPandoraFieldValue(res, "userAuthToken")
        Dim userId = getPandoraFieldValue(res, "userId")
        Dim urlEncToken As String = Uri.EscapeDataString(userAuthToken)

        xml = $"<?xml version=""1.0"" encoding=""utf-8""?><methodCall><methodName>user.getStationList</methodName><params><param><value><struct><member><name>userAuthToken</name><value><string>{userAuthToken}</string></value></member><member><name>sortField</name><value><string>alphabetical</string></value></member><member><name>sortOrder</name><value><string>asc</string></value></member><member><name>includeStationArtUrl</name><value><boolean>1</boolean></value></member><member><name>includeShuffleInsteadOfQuickMix</name><value><boolean>1</boolean></value></member></struct></value></param></params></methodCall>"
        res = SendXMLRequest($"http://tuner.pandora.com/services/xmlrpc/?method=user.getStationList&partner_id={partnerId}&auth_token={urlEncToken}&user_id={userId}",xml,"POST")
        musicServiceInfo.Token = userAuthToken
        musicServiceInfo.Key = userId
        return FillInPandoraData(musicItem, res)
    End Function

    '// Special case to take Pandora metadata and fill in a MusicItem
    Public Function FillInPandoraData(ByRef parentItem As MusicItem, xml As String) As Integer
        Dim xmlDoc As New XmlDocument
        If xml = "" Then
            MsgBox("Nothing to Parse [ParseAvailableMusic Services]")
            Return ""
        End If
        xmlDoc.LoadXml(xml)
        
        'Dim resultElement As xmlElement = getXMLElementByChildFieldNameValue(xmlDoc.LastChild,"member","name","result")
        Dim resultElement As xmlElement = xmlDoc.LastChild
        '// get the first "struct"
        resultElement = resultElement.GetElementsByTagName("struct")(0)
        Dim arrayElement As XmlElement = resultElement.GetElementsByTagName("array")(0)

        'Dim stationElement As XmlElement = getXMLElementByChildFieldNameValue(resultElement, "struct", "name", "stations")

        Dim stationNodes As xmlNodeList = arrayElement.GetElementsByTagName("struct")
        
        If parentItem?.itemCount>0 Then
            
        Else
            '// redim and reset the array
            parentItem.itemCount = stationNodes.Count
            Dim temp(parentItem.itemCount-1) As String
            parentItem.itemids = temp
        End If
        Dim ctr As Integer=0
        '// get each "struct" element in the stations array
        For each el As XMLElement In stationNodes
            Dim musicItem As New MusicItem
            '// get and then iterate through all the childnodes
            Dim memberEl As XmlNodeList = el.ChildNodes
                For Each xnode As XmlNode In memberEl
                '// this ishighly specific. the first child must be name and the last child must be value.
                    Select Case xnode.FirstChild.InnerText
                        Case "stationId"
                            musicItem.ID = xnode.LastChild.InnerText
                        Case "artUrl"
                            musicItem.ArtURL = xnode.LastChild.InnerText
                        Case "stationName"
                            musicItem.Title =  xnode.LastChild.InnerText
                     End Select
                Next
            musicItem.CanPlay=True
            musicItem.ItemType=itemType.show
            musicItem.SonosServiceID = parentItem.SonosServiceID
            parentItem.ItemIds(ctr) = musicItem.ID
            ctr +=1
            '// add the child to the collection
            parentItem.Items.Add(musicItem.ID, musicItem)
        Next
        Return 0 '// its always the entire  list.
    End Function
   
    '// A Pandora specific utility
    Public Function getPandoraFieldValue(xml As String, fieldName As String) As String
        Dim xmlDoc As New XmlDocument
        If xml = "" Then
            MsgBox("Nothing to Parse [ParseAvailableMusic Services]")
            Return ""
        End If
        xmlDoc.LoadXml(xml)
        Dim el As XmlElement = xmlDoc.GetElementsByTagName("struct")(0)
        Dim memberEl As XmlNodeList = el.GetElementsByTagName("member")
        For Each xnode As XmlNode In memberEl
            If xnode.FirstChild.InnerText = fieldName Then
                Return xnode.LastChild.InnerText
            End If
        Next
        Return ""
    End Function

#End Region
   
End Module

End Namespace