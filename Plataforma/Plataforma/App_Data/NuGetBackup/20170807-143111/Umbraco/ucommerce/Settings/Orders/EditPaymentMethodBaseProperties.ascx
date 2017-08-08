<%@ Import Namespace="UCommerce.Presentation.Web" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditPaymentMethodBaseProperties.ascx.cs" inherits="UCommerce.Web.UI.Settings.Orders.EditPaymentMethodBaseProperties" %>
<%@ Register tagPrefix="commerce" tagName="ValidationSummary" src="../../Controls/ValidationSummaryDisplay.ascx" %>

<commerce:ValidationSummary runat="server" />
<asp:Panel runat="server" id="FixedPropertyPanel" CssClass="propertyPane">
	<div class="propertyItem">
		<div class="propertyItemHeader"><asp:Localize ID="Localize6" runat="server" meta:resourceKey="PaymentMethodService" /></div>
		<div class="propertyItemContent">
			<asp:DropDownList runat="server" ID="PaymentMethodServiceDropDownList" DataSource="<%# View.PaymentMethodServices %>" />
		</div>
	</div>
	<div class="propertyItem">
		<div class="propertyItemHeader"><asp:Localize ID="Localize7" runat="server" meta:resourceKey="Pipeline" /></div>
		<div class="propertyItemContent">
			<asp:DropDownList runat="server" ID="PipelineDropDownList" DataSource="<%# Pipelines %>" DataTextField="Text" DataValueField="Value" />
		</div>
	</div>
	<div class="propertyItem">
		<div class="propertyItemHeader"><asp:Localize id="Localize1" runat="server" meta:resourceKey="Image" /></div>
		<div class="propertyItemContent">
			<asp:PlaceHolder ID="ImagePicker" runat="server"></asp:PlaceHolder>        
		</div>
	</div>
	<div class="propertyItem">
		<div class="propertyItemHeader"><asp:Localize ID="Localize2" runat="server" meta:resourceKey="Enabled" /></div>
		<div class="propertyItemContent">
			<asp:CheckBox runat="server" ID="EnabledCheckBox" Checked="<%# View.PaymentMethod.Enabled %>" />
		</div>
	</div>
	<div class="propertyPaneFooter">-</div>
</asp:Panel>
<asp:Panel runat="server" id="PropertyPanel" cssclass="propertyPane"></asp:Panel>
<asp:Panel runat="server" id="AuditPropertyPanel" CssClass="propertyPane">
	<div class="propertyItem">
		<div class="propertyItemHeader"><asp:Localize ID="Localize3" runat="server" meta:resourceKey="LastModified" /></div>
		<div class="propertyItemContent">
			<%= View.PaymentMethod.ModifiedOn %>
		</div>
	</div>
	<div class="propertyItem">
		<div class="propertyItemHeader"><asp:Localize ID="Localize4" runat="server" meta:resourceKey="LastModifiedBy" /></div>
		<div class="propertyItemContent">
			<%= View.PaymentMethod.ModifiedBy %>
		</div>
	</div>
    <div class="propertyPaneFooter">-</div>
</asp:Panel>