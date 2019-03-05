Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxPivotGrid
Imports DevExpress.XtraPivotGrid

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        If Not IsPostBack Then
            fieldCompanyName.CollapseAll()
        End If
        If ASPxPivotGrid2.JSProperties.ContainsKey("cpFilter") Then
            ASPxPivotGrid2.JSProperties.Remove("cpFilter")
        End If
    End Sub
    Protected Sub ASPxPivotGrid2_GroupFilterChanged(ByVal sender As Object, ByVal e As PivotGroupEventArgs)
        Dim fi = FilterInfo.GetFilterInfo(e.Group, PivotFilterType.Included)
        Dim result As String = FilterInfo.WriterHierachyToString(fi, "")
        ASPxPivotGrid2.JSProperties.Add("cpFilter", result)
    End Sub

    Protected Sub ASPxPivotGrid2_CustomGroupInterval(ByVal sender As Object, ByVal e As PivotCustomGroupIntervalEventArgs)
        If e.Field.UnboundFieldName <> "fieldCompanyGroup" Then
            Return
        End If
        If Convert.ToChar(e.Value.ToString().Chars(0)) < "F"c Then
            e.GroupValue = "A-E"
            Return
        End If
        If Convert.ToChar(e.Value.ToString().Chars(0)) > "E"c AndAlso Convert.ToChar(e.Value.ToString().Chars(0)) < "P"c Then
            e.GroupValue = "F-O"
            Return
        End If
        If Convert.ToChar(e.Value.ToString().Chars(0)) > "O"c AndAlso Convert.ToChar(e.Value.ToString().Chars(0)) < "T"c Then
            e.GroupValue = "P-S"
            Return
        End If
        If Convert.ToChar(e.Value.ToString().Chars(0)) > "S"c Then
            e.GroupValue = "T-Z"
        End If
    End Sub




End Class

Public Class FilterInfo
    Private Property Field() As PivotGridFieldBase
    Private Property Value() As Object
    Private Property ChildValues() As FilterInfo()

    Public Sub New()
        ChildValues = New FilterInfo(){}
    End Sub
    Public Sub New(ByVal v As PivotGroupFilterValue)
        Me.Field = v.Field
        Me.Value = v.Value
        If v.ChildValues Is Nothing Then
            Me.ChildValues = New FilterInfo(){}
        Else
            Me.ChildValues = v.ChildValues.Select(Function(cv) New FilterInfo(cv)).ToArray()
        End If
    End Sub
    Public Overrides Function ToString() As String
        Return String.Format("{0}: {1}({2})", Me.Field, Me.Value, ChildValues.Length)
    End Function
    Public Shared Function GetFilterInfo(ByVal group As PivotGridGroup, ByVal targetType As PivotFilterType) As FilterInfo()
        If group.FilterValues.HasFilter Then
            If group.FilterValues.FilterType = targetType Then
                Return group.FilterValues.Values.Select(Function(v) New FilterInfo(v)).ToArray()
            Else
                Return GetConvertedFilerInfo(group, group.FilterValues.Values, Nothing)
            End If
        Else
            Return New FilterInfo(){}
        End If
    End Function

    Private Shared Function GetConvertedFilerInfo(ByVal group As PivotGridGroup, ByVal filterValues As PivotGroupFilterValuesCollection, ByVal parentValues() As Object) As FilterInfo()
        If (Not group.FilterValues.HasFilter) OrElse filterValues Is Nothing OrElse filterValues.Count= 0 Then
            Return New FilterInfo(){}
        End If

        Dim currentField = filterValues.Items(0).Field
        Dim uniqueValues = group.GetUniqueValues(parentValues)
        Dim invertedFilterInfo = New List(Of FilterInfo)()
        For Each v As Object In uniqueValues
            Dim filterValue As PivotGroupFilterValue = filterValues.FirstOrDefault(Function(fv) Object.Equals(fv.Value, v))
            If filterValue Is Nothing Then
                invertedFilterInfo.Add(New FilterInfo() With {.Field = currentField, .Value = v})
            Else
                If filterValue.ChildValues IsNot Nothing AndAlso filterValue.ChildValues.Count > 0 Then
                    invertedFilterInfo.Add(New FilterInfo() With {.Field = currentField, .Value = v, .ChildValues = GetConvertedFilerInfo(group, filterValue.ChildValues, (If(parentValues, New Object(){})).Union(New Object() { v }).ToArray())})
                End If

            End If
        Next v
        Return invertedFilterInfo.ToArray()
    End Function

    Public Shared Function WriterHierachyToString(ByVal info() As FilterInfo, ByVal prefix As String) As String
        Dim sb As New StringBuilder()
        info.ToList().ForEach(Sub(fi)
            sb.AppendLine(prefix & fi.Field.GetDisplayText(fi.Value))
            Dim nestedValues As String = WriterHierachyToString(fi.ChildValues, prefix & "...")
            If Not String.IsNullOrEmpty(nestedValues) Then
                sb.Append(nestedValues)
            End If
        End Sub)
        Return sb.ToString()
    End Function

End Class

