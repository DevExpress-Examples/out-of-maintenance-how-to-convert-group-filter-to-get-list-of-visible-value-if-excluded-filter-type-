<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v21.2, Version=21.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v21.2, Version=21.2.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
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
                    <dx:ASPxPivotGrid ID="ASPxPivotGrid2" runat="server" 
                        ClientIDMode="AutoID" DataSourceID="SqlDataSource1"
                        OnGroupFilterChanged="ASPxPivotGrid2_GroupFilterChanged" IsMaterialDesign="False">
                        <Fields>
                            <dx:PivotGridField ID="fieldProductName" Area="RowArea" AreaIndex="2"
                                GroupIndex="0" InnerGroupIndex="2">
                                <DataBindingSerializable>
                                    <dx:DataSourceColumnBinding ColumnName="ProductName" />
                                </DataBindingSerializable>
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldCompanyName" Area="RowArea" AreaIndex="1"
                                GroupIndex="0" InnerGroupIndex="1">
                                <DataBindingSerializable>
                                    <dx:DataSourceColumnBinding ColumnName="CompanyName" />
                                </DataBindingSerializable>
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldOrderDate" Area="ColumnArea" AreaIndex="0" UnboundFieldName="fieldOrderDate">
                                <DataBindingSerializable>
                                    <dx:DataSourceColumnBinding ColumnName="OrderDate" GroupInterval="DateMonth" />
                                </DataBindingSerializable>
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldProductAmount" Area="DataArea" AreaIndex="0">
                                <DataBindingSerializable>
                                    <dx:DataSourceColumnBinding ColumnName="ProductAmount" />
                                </DataBindingSerializable>
                            </dx:PivotGridField>
                            <dx:PivotGridField ID="fieldCompanyGroup" Area="RowArea" AreaIndex="0" Caption="Company Group"
                                FieldName="CompanyName" GroupIndex="0" InnerGroupIndex="0">
                                <DataBindingSerializable>
                                    <dx:ExpressionDataBinding Expression="Iif(Substring([fieldCompanyName], 0, 1) &lt; 'F', 'A-E', Substring([fieldCompanyName], 0, 1) &lt; 'T', 'F-S', 'T-Z')" />
                                </DataBindingSerializable>
                            </dx:PivotGridField>
                        </Fields>
                        <ClientSideEvents EndCallback="function(s, e) {
	if( s.cpFilter != null )
	{        
        filterMemo.SetText(s.cpFilter);
	}
}" />
                        <OptionsData DataProcessingEngine="Optimized" />
                        <Groups>
                            <dx:PivotGridWebGroup Caption="Companies" ShowNewValues="True" />
                        </Groups>
                    </dx:ASPxPivotGrid>

                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
            ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" 
            SelectCommand="SELECT [ProductName], [CompanyName], [OrderDate], [ProductAmount] FROM [CustomerReports]"></asp:SqlDataSource>

                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
