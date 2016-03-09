Imports System.ServiceModel
Imports System.ServiceModel.Channels
Imports System.ServiceModel.Dispatcher
Imports System.ServiceModel.Description
Imports System.ComponentModel
Imports System.Xml

Namespace Smapi

Public Class HttpUserAgentMessageInspector

    Implements IClientMessageInspector
    Private Const USER_AGENT_HTTP_HEADER As String = "user-agent"

    Private m_userAgent As String

    Public newToken As String=""
    Public newKey As String=""
    Public faultCode As String=""

    Public Sub New(ByVal userAgent As String)
        Me.m_userAgent = userAgent
        'newToken = NT
    End Sub

#Region "IClientMessageInspector Members"

    Public Sub AfterReceiveReply(ByRef reply As System.ServiceModel.Channels.Message, ByVal correlationState As Object) Implements IClientMessageInspector.AfterReceiveReply
        'Debug.Print("AFTER RECEIVE" & reply.ToString)
        If reply.IsFault
            'Debug.Print("IS FAULT")
        
        
            Dim messageBuffer As MessageBuffer = reply.CreateBufferedCopy(Integer.MaxValue)
            Dim message As Message = messageBuffer.CreateMessage()
            Dim contents As XmlDictionaryReader = message.GetReaderAtBodyContents()
            Dim doc As New XmlDocument()
            doc.Load(contents)
        
        
            'inspect contents and do what you want, throw a custom              
            'exception for example...                 
            'We need to recreate the reply to resume the deserialisation as it             
            'can only be read once.            
            reply = messageBuffer.CreateMessage()
            messageBuffer.Close()

            faultCode =  doc?.GetElementsByTagName("faultcode")(0)?.InnerText
            newToken =  doc?.GetElementsByTagName("authToken")(0)?.InnerText
            newKey =  doc?.GetElementsByTagName("privateKey")(0)?.InnerText
        End If

        'Try
        '    Dim fault As String = doc.FirstChild("faultcode").InnerText
        'Catch ex As Exception
        '    Exit Sub        '//unhandled error case
        'End Try
        
        'Dim detail As XmlNode = doc?.LastChild
        'Dim token As String=""
        'Dim key As String=""
        'If detail IsNot Nothing Then
        '    Dim refreshTokenResult = detail("refreshAuthTokenResult")
        '    If refreshTokenResult IsNot Nothing Then
        '        token = refreshTokenResult("authToken").InnerText
        '        key = refreshTokenResult("privateKey").Innertext
        '    End If

        'End If

        'Dim token2 As String = doc.FirstChild.SelectSingleNode("detail").SelectSingleNode("refreshAuthTokenResult").SelectSingleNode("authToken").InnerText
        'newToken = Token
        
        
        'Throw New Exception("SGSHJSGHJ")
        'Dim faultDetail As Object = token
        'Dim exception As Exception = TryCast(faultDetail, Exception)
        
        'If exception IsNot Nothing Then
        '    Throw exception
        'End If
        
        'Throw New FaultException(Of String)(token, New FaultReason("Reason!"))
    End Sub

    Public Function BeforeSendRequest(ByRef request As System.ServiceModel.Channels.Message, ByVal channel As System.ServiceModel.IClientChannel) As Object Implements IClientMessageInspector.BeforeSendRequest
        Dim httpRequestMessage As HttpRequestMessageProperty
        Dim httpRequestMessageObject As Object = Nothing
        If request.Properties.TryGetValue(HttpRequestMessageProperty.Name, httpRequestMessageObject) Then
            httpRequestMessage = TryCast(httpRequestMessageObject, HttpRequestMessageProperty)
            If String.IsNullOrEmpty(httpRequestMessage.Headers(USER_AGENT_HTTP_HEADER)) Then
                httpRequestMessage.Headers(USER_AGENT_HTTP_HEADER) = Me.m_userAgent
            End If
        Else
            httpRequestMessage = New HttpRequestMessageProperty()
            httpRequestMessage.Headers.Add(USER_AGENT_HTTP_HEADER, Me.m_userAgent)
            request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage)
        End If
        Return Nothing
    End Function

#End Region
End Class

Public Class HttpUserAgentEndpointBehavior
    Implements IEndpointBehavior
    Private m_userAgent As String
    
    Public inspector As HttpUserAgentMessageInspector
    'Property refreshedToken As String
    '    Get
    '        Return inspector.newToken
    '    End Get
    '    Set
    '        _refreshedToken = value
    '    End Set
    'End Property

    Public Sub New(ByVal userAgent As String)
        Me.m_userAgent = userAgent
    End Sub

#Region "IEndpointBehavior Members"

    Public Sub AddBindingParameters(ByVal endpoint As ServiceEndpoint, ByVal bindingParameters As System.ServiceModel.Channels.BindingParameterCollection) Implements IEndpointBehavior.AddBindingParameters
    End Sub

    Public Sub ApplyClientBehavior(ByVal endpoint As ServiceEndpoint, ByVal clientRuntime As System.ServiceModel.Dispatcher.ClientRuntime) Implements IEndpointBehavior.ApplyClientBehavior
        inspector = New HttpUserAgentMessageInspector(Me.m_userAgent)
        
        clientRuntime.MessageInspectors.Add(inspector)
    End Sub

    Public Sub ApplyDispatchBehavior(ByVal endpoint As ServiceEndpoint, ByVal endpointDispatcher As System.ServiceModel.Dispatcher.EndpointDispatcher) Implements IEndpointBehavior.ApplyDispatchBehavior
    End Sub

    Public Sub Validate(ByVal endpoint As ServiceEndpoint) Implements IEndpointBehavior.Validate
    End Sub

#End Region
End Class

End Namespace
