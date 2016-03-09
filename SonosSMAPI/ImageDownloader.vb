' Add an Imports directive and a reference for System.Net.Http.
Imports System.Drawing
Imports System.Net.Http

' Add the following Imports directive for System.Threading.
Imports System.Threading
Imports Sonos.Smapi

Namespace XWSDL

Public Class ImageDownloader

    ' Declare a System.Threading.CancellationTokenSource.
    Dim cts As CancellationTokenSource
    Dim urlList As List(Of String)
    Public Event ArtWorkReceived(url As String, image As Image)

    Public Async Sub GetImages(items As Dictionary(Of String, MusicItem), imageCache As Dictionary(Of String, Image))
        ' Instantiate the CancellationTokenSource.
        cts = New CancellationTokenSource()
        urlList = New List(Of String)
        For each musicItem As MusicItem In items.Values

            If musicItem?.ArtURL<>"" Then
                If Not imageCache.ContainsKey(musicItem?.ArtURL) Then
                    urlList.Add(musicItem.ArtURL)
                'Else
                '    RaiseEvent ArtWorkReceived(musicItem.ArtURL, imageCache(musicItem.ArtURL))
                End If
            End If
        Next
         Try
            Await AccessTheWebAsync(cts.Token)
            Debug.Print( "Downloads complete.")

        Catch ex As OperationCanceledException
            Debug.Print("Downloads canceled.")

        Catch ex As Exception
            Debug.Print("Downloads failed.")
        End Try

        ' Set the CancellationTokenSource to Nothing when the download is complete.
        cts = Nothing
    End Sub
    
    ' Provide a parameter for the CancellationToken.
    ' Change the return type to Task because the method has no return statement.
    Async Function AccessTheWebAsync(ct As CancellationToken) As Task

        Dim client As HttpClient = New HttpClient()

        ' Call SetUpURLList to make a list of web addresses.
        'Dim urlList As List(Of String) = SetUpURLList()

        ' ***Create a query that, when executed, returns a collection of tasks.
        Dim downloadTasksQuery As IEnumerable(Of Task(Of Integer)) =
            From url In urlList Select ProcessURLAsync(url, client, ct)

        ' ***Use ToList to execute the query and start the download tasks. 
        Dim downloadTasks As List(Of Task(Of Integer)) = downloadTasksQuery.ToList()

        ' ***Add a loop to process the tasks one at a time until none remain.
        While downloadTasks.Count > 0
            ' ***Identify the first task that completes.
            Dim firstFinishedTask As Task(Of Integer) = Await Task.WhenAny(downloadTasks)

            ' ***Remove the selected task from the list so that you don't
            ' process it more than once.
            downloadTasks.Remove(firstFinishedTask)

            ' ***Await the first completed task and display the results.
            Dim length = Await firstFinishedTask
            '//Debug.Print("Length of the downloaded website:  {0}", length)
        End While

    End Function


        ' Bundle the processing steps for a website into one async method.
    Async Function ProcessURLAsync(url As String, client As HttpClient, ct As CancellationToken) As Task(Of Integer)

        ' GetAsync returns a Task(Of HttpResponseMessage). 
        Dim response As HttpResponseMessage = Await client.GetAsync(url, ct)

        ' Retrieve the website contents from the HttpResponseMessage.
        Dim urlContents As Byte() = Await response.Content.ReadAsByteArrayAsync()
        Dim image As Image = Image.FromStream(New IO.MemoryStream(urlContents))
        RaiseEvent ArtWorkReceived(url, image)
        Return urlContents.Length
    End Function
End Class

End Namespace
