Imports System.Runtime.CompilerServices
Imports System.Threading

Namespace Utilities
Public Module TraceLogger
<flags>
    Public Enum eTraceCategories
        None = 0
        UPnPEvents = 1
        DataParsing = 2
        ImageHandling = 4
        Gui = 8
        ProcessFlow=16
        Debugging=32
        All = 64
    End Enum


    Public MyTraceSwitch As New TraceSwitch("Myapp", "my desc")
    Public MyTraceCategory As eTraceCategories = eTraceCategories.All
    Public Property VerboseMode As Boolean = True

    '//TODO: Switch Order
    Public Sub LogMeTraceLevel(traceLevel As TraceLevel, category As eTraceCategories)
        MyTraceSwitch.Level = traceLevel
        MyTraceCategory = category
    End Sub
    '//TODO: check the logic
    '// the <CellerMemberName> and related attributes inject the appropriate values automatically. Do not fill in on calls.
    Public Sub LogMe(category As eTraceCategories, message As String, level As TraceLevel, <CallerMemberName> Optional method As String = Nothing, <CallerFilePath> Optional sourcefilePath As String = Nothing,
        <CallerLineNumber()> Optional sourceLineNumber As Integer = 0)

    '// tracing is off.
        If MyTraceCategory=eTraceCategories.None Then Exit Sub      
        Dim st As New StackTrace()

        '// if the flag is set.
        If MyTraceCategory Or category Then
            If level <=MyTraceSwitch.Level
                If VerboseMode Then
                        Trace.WriteLine(String.Format("{5}|{0}: {1}, category={2}, file={3}, line={4}",method, message, category,sourcefilePath,sourceLineNumber, Thread.CurrentThread.ManagedThreadId))
                    Else
                        Trace.WriteLine(String.Format("{5}|{0} from {3} : {1}, category={2}, line={4}",method, message, category,st.GetFrame(2).GetMethod.Name,sourceLineNumber, Thread.CurrentThread.ManagedThreadId))
                End If
                
                'Trace.WriteLine(String.Format("{0}:{1} from {2} in file {3} at line number {4}", category, message, method, sourcefilePath, sourceLineNumber))
            End If
        End If

        'System.Diagnostics.Trace.TraceInformation("{0} from {1}", Message, method)
        'TraceSwitch.Level = TraceLevel.Info
        'Dim msg As String = "{0} from {1}", Message, method
        'Trace.WriteLineIf((level >= MyTraceSwitch.Level), Message & " from " & method)
        'Trace.WriteLineIf((level <= MyTraceSwitch.Level) And (category <= MyTraceCategory), String.Format("{0}:{1} from {2} in file {3} at line number {4}", category, message, method, sourcefilePath, sourceLineNumber))
    End Sub

End Module
End Namespace

