<%@ Import Namespace="UCommerce.Presentation.Web"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditProductRelationTypeBaseProperties.ascx.cs" Inherits="UCommerce.Web.UI.Settings.Catalog.EditProductRelationTypeBaseProperties" %>
<%@ Register tagPrefix="commerce" tagName="ValidationSummary" src="../../Controls/ValidationSummaryDisplay.ascx" %>

<commerce:ValidationSummary ID="ValidationSummary1" runat="server" />
<div class="propertyPane leftAligned">

    <div class="propertyItem">
	    <div class="propertyItemHeader"><asp:Localize ID="Localize2" runat="server" meta:resourceKey="Description" /></div>
        <div class="propertyItemContent">
            <asp:TextBox runat="server" ID="DescriptionTextBox" Text="<%# View.ProductRelationType.Description %>" TextMode="MultiLine" CssClass="mediumWidth smallHeight" />
            			<asp:CustomValidator runat="server" ControlToValidate="DescriptionTextBox" Display="Dynamic" id="LicenseValidator" ErrorMessage="Your license does not support creating any more." CssClass="validator" OnServerValidate="LicenseCheckValidator_ServerValidate"/>
        </div>
    </div>        
    <div class="propertyItem">
	    <div class="propertyItemHeader"><asp:Localize ID="Localize3" runat="server" meta:resourceKey="LastModifiedOn" /></div>
        <div class="propertyItemContent">
            <asp:Label ID="Label1" runat="server" Text="<%# View.IsNew ? String.Empty : View.ProductRelationType.ModifiedOn.ToString() %>" />
        </div>
    </div>        
    <div class="propertyItem">
	    <div class="propertyItemHeader"><asp:Localize ID="Localize4" runat="server" meta:resourceKey="LastModifiedBy" /></div>
        <div class="propertyItemContent">
            <%# View.ProductRelationType.ModifiedBy %>
        </div>
    </div>
    <div class="propertyPaneFooter">-</div>
</div>