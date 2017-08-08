<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditOrderNumberSerieBaseProperties.ascx.cs" Inherits="UCommerce.Web.UI.Settings.Orders.EditOrderNumberSerieBaseProperties" %>
<%@ Register tagPrefix="commerce" tagName="ValidationSummary" src="../../Controls/ValidationSummaryDisplay.ascx" %>

<commerce:ValidationSummary runat="server" />
<div class="propertyPane leftAligned">

    <commerce:PropertyPanel runat="server" meta:resourceKey="PrefixProperty">
        <asp:TextBox runat="server" ID="PrefixTextBox" Text="<%# View.OrderNumberSerie.Prefix %>" />
    </commerce:PropertyPanel>
    <commerce:PropertyPanel runat="server" meta:resourceKey="PostfixProperty">
        <asp:TextBox runat="server" ID="PostfixTextBox" Text="<%# View.OrderNumberSerie.Postfix %>" />
    </commerce:PropertyPanel>
    <commerce:PropertyPanel runat="server" meta:resourceKey="IncrementProperty">
        <asp:TextBox runat="server" ID="IncrementTextBox" Text="<%# View.OrderNumberSerie.Increment %>"  />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="IncrementTextBox" Display="Dynamic" Text="*" ErrorMessage='<%# GetLocalResourceObject("IncrementProperty.Text") %>' CssClass="validator" />
        <asp:RangeValidator runat="server" ControlToValidate="IncrementTextBox" MinimumValue="1" MaximumValue="<%# int.MaxValue %>" Type="Integer" Text="*" ErrorMessage='<%# GetLocalResourceObject("IncrementProperty.Text") %>' CssClass="validator" Display="Static"></asp:RangeValidator>
    </commerce:PropertyPanel>
    <commerce:PropertyPanel runat="server" meta:resourceKey="CurrentNumberProperty">
        <%# View.OrderNumberSerie.Prefix %><%# View.OrderNumberSerie.CurrentNumber + View.OrderNumberSerie.Increment %><%# View.OrderNumberSerie.Postfix %>
    </commerce:PropertyPanel>
	<div class="propertyPaneFooter">-</div>
</div>