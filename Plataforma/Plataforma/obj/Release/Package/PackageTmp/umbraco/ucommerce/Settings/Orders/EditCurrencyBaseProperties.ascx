<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCurrencyBaseProperties.ascx.cs" Inherits="UCommerce.Web.UI.Settings.Orders.EditCurrencyBaseProperties" %>
<%@ Register tagPrefix="commerce" tagName="ValidationSummary" src="../../Controls/ValidationSummaryDisplay.ascx" %>

<commerce:ValidationSummary runat="server" />
<div class="propertyPane leftAligned">
    
    <commerce:PropertyPanel runat="server" meta:resourceKey="ExchangeRateProperty">
        <asp:TextBox runat="server" ID="ExchangeRateTextBox" Text="<%# View.Currency.ExchangeRate %>"></asp:TextBox>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="ExchangeRateTextBox" Display="Dynamic" Text="*" ErrorMessage='<%# GetLocalResourceObject("ExchangeRateProperty.Text") %>' CssClass="validator" />
        <asp:RangeValidator runat="server" ControlToValidate="ExchangeRateTextBox" Display="Dynamic" Text="*" ErrorMessage='<%# GetLocalResourceObject("ExchangeRateProperty.Text") %>' CssClass="validator" MinimumValue="0" MaximumValue="<%# int.MaxValue %>" Type="Integer" ></asp:RangeValidator>
    </commerce:PropertyPanel>
	<div class="propertyPaneFooter">-</div>
</div>