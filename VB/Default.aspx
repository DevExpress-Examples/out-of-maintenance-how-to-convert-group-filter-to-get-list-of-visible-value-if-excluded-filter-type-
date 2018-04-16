<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v15.2, Version=15.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <dx:ASPxMemo ID="filterMemo" runat="server" ClientInstanceName="filterMemo" Height="309px" Width="273px">
                    </dx:ASPxMemo>
                </td>
                <td>
                    <dx:ASPxPivotGrid ID="ASPxPivotGrid2" runat="server" OnCustomGroupInterval="ASPxPivotGrid2_CustomGroupInterval"
                        ClientIDMode="AutoID" DataSourceID="AccessDataSource1"  OnGroupFilterChanged="ASPxPivotGrid2_GroupFilterChanged">
                        <Fields>
                            <dx:PivotGridField ID="fieldProductName" Area="RowArea" AreaIndex="2" FieldName="ProductName"
                                GroupIndex="0" InnerGroupIndex="2">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldCompanyName" Area="RowArea" AreaIndex="1" FieldName="CompanyName"
                                GroupIndex="0" InnerGroupIndex="1">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldOrderDate" Area="ColumnArea" AreaIndex="0" FieldName="OrderDate"
                                GroupInterval="DateMonth" UnboundFieldName="fieldOrderDate">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldProductAmount" Area="DataArea" AreaIndex="0" FieldName="ProductAmount">
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldCompanyGroup" Area="RowArea" AreaIndex="0" Caption="Company Group"
                                FieldName="CompanyName" GroupIndex="0" GroupInterval="Custom" InnerGroupIndex="0"
                                UnboundFieldName="fieldCompanyGroup">
                            </dx:PivotGridField>
                        </Fields>
                        <ClientSideEvents EndCallback="function(s, e) {
    if( s.cpFilter != null )
    {        
        filterMemo.SetText(s.cpFilter);
    }
}" />
                        <Groups>
                            <dx:PivotGridWebGroup Caption="Companies" ShowNewValues="True" />
                        </Groups>
                    </dx:ASPxPivotGrid>
                    <asp:AccessDataSource ID="AccessDataSource1" runat="server" DataFile="~/App_Data/nwind.mdb"
                        SelectCommand="SELECT [ProductName], [CompanyName], [OrderDate], [ProductAmount] FROM [CustomerReports]">
                    </asp:AccessDataSource>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>