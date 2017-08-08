<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditEmailProfileTypeInformation.ascx.cs" Inherits="UCommerce.Web.UI.Settings.Email.EditEmailProfileTypeInformation" %>
<%@ Register tagPrefix="commerce" tagName="ValidationSummary" src="../../Controls/ValidationSummaryDisplay.ascx" %>

<commerce:ValidationSummary runat="server" />
<div class="propertyPane">
	<div class="propertyItem">
		<div class="propertyItemHeader"><asp:Localize runat="server" meta:resourceKey="FromName" /></div>
		<div class="propertyItemContent">
			<asp:TextBox runat="server" ID="FromNameTextBox" Text="<%# View.EmailProfileInformation.FromName %>" CssClass="mediumWidth" />
			<asp:RequiredFieldValidator runat="server" ControlToValidate="FromNameTextBox" Text="*" ErrorMessage='<%# GetLocalResourceObject("FromName.Text") %>' CssClass="validator" />
		</div>
	</div>
	<div class="propertyItem">
		<div class="propertyItemHeader"><asp:Localize runat="server" meta:resourceKey="FromAddress" /></div>
		<div class="propertyItemContent">
			<asp:TextBox runat="server" ID="FromAddressTextBox" Text="<%# View.EmailProfileInformation.FromAddress %>" CssClass="mediumWidth" />
			<asp:RequiredFieldValidator runat="server" ControlToValidate="FromAddressTextBox" Text="*" ErrorMessage='<%# GetLocalResourceObject("FromAddress.Text") %>' CssClass="validator" />
		</div>
	</div>
	<div class="propertyItem">
		<div class="propertyItemHeader"><asp:Localize runat="server" meta:resourceKey="CcAddress" /></div>
		<div class="propertyItemContent">
			<asp:TextBox runat="server" ID="CcAddressTextBox" Text="<%# View.EmailProfileInformation.CcAddress %>" CssClass="mediumWidth" />
		</div>
	</div>
	<div class="propertyItem">
		<div class="propertyItemHeader"><asp:Localize runat="server" meta:resourceKey="BccAddress" /></div>
		<div class="propertyItemContent">
			<asp:TextBox runat="server" ID="BccAddressTextBox" Text="<%# View.EmailProfileInformation.BccAddress %>" CssClass="mediumWidth" />
		</div>
	</div>
    <div class="propertyPaneFooter">-</div>
</div>