Imports DevExpress.XtraVerticalGrid.Events

Public Class frmProperties
    Dim parentObj As Object
    Public Sub SetDataSource(obj As Object)
        PG1.SelectedObject = obj
        parentObj = Obj
    End Sub

    Private Sub PG1_DoubleClick(sender As Object, e As EventArgs) Handles PG1.DoubleClick
        'Sonos.Smapi.Wsdl.trackMetadata
        Dim v = PG1.FocusedRow.Properties.Value
        If v.GetType = GetType(Sonos.Smapi.Wsdl.trackMetadata) Then
            PG1.SelectedObject = v

        End If
        'Debug.Print(PG1.FocusedRow.Properties.Value.GetType.ToString)
    End Sub

    Private Sub PG1_RowChanged(sender As Object, e As RowChangedEventArgs) Handles PG1.RowChanged
        'Debug.Print(e.Row.Name)
    End Sub

    Private Sub ButtonLPH_L_Click(sender As Object, e As EventArgs) Handles ButtonLPH_L.Click
        PG1.SelectedObject = parentObj
    End Sub
End Class