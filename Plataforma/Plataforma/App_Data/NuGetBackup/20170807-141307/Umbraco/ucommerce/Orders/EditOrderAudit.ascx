<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditOrderAudit.ascx.cs" Inherits="UCommerce.Web.UI.Orders.EditOrderAudit" %>
<%@ Import Namespace="UCommerce.EntitiesV2"%>

<script type="text/javascript">
    $(function () {
        $("#order-audits").dataTable(
            {
                "bPaginate": false,
                // disable initial sort
                "aaSorting": [],
            });
    });
</script>
<div class="propertyPane orderAudit">
    <h2 class="propertyPaneTitel"><asp:Literal runat="server" meta:resourcekey="OrderAudits"></asp:Literal></h2>
    <asp:Repeater ID="Repeater1" runat="server" DataSource="<%# OrderAudits %>">
        <HeaderTemplate>
            <table class="dataList" id="order-audits" style="width: 100%; vertical-align: top;">
            <thead>
                <tr>
                    <th><span><asp:Localize ID="Localize1" runat="server" meta:resourceKey="CreatedOn" /></span></th>
                    <th><span><asp:Localize ID="Localize2" runat="server" meta:resourceKey="CreatedBy" /></span></th>
                    <th><span><asp:Localize ID="Localize3" runat="server" meta:resourceKey="Status" /></span></th>
                    <th><span><asp:Localize ID="Localize4" runat="server" meta:resourceKey="Message" /></span></th>
                </tr>  
            </thead> 
        <tbody>
        </HeaderTemplate>
        <ItemTemplate>         
            <tr>
                <td class="auditCreatedOn"><%# ((OrderStatusAudit)Container.DataItem).CreatedOn %></td>
                <td><%# ((OrderStatusAudit)Container.DataItem).CreatedBy %></td>
                <td><%# ((OrderStatusAudit)Container.DataItem).NewOrderStatus.Name %></td>
                <td class="auditMessage"><%# ((OrderStatusAudit)Container.DataItem).Message %></td>
            </tr>    
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table>  
        </FooterTemplate>
    </asp:Repeater>
    <div class="propertyPaneFooter"></div>
</div>