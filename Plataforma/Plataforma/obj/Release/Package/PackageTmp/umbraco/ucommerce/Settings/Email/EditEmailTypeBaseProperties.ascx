<%@ Import Namespace="UCommerce.Presentation.Web"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditEmailTypeBaseProperties.ascx.cs" Inherits="UCommerce.Web.UI.Settings.Email.EditEmailTypeBaseProperties" %>
<%@ Register tagPrefix="commerce" tagName="ValidationSummary" src="../../Controls/ValidationSummaryDisplay.ascx" %>

<commerce:ValidationSummary ID="ValidationSummary1" runat="server" />
<div class="propertyPane leftAligned" >
    <div>

        <div class="propertyItem">
	        <div class="propertyItemHeader"><asp:Localize runat="server" meta:resourceKey="Description" /></div>
            <div class="propertyItemContent">
                <asp:TextBox runat="server" ID="DescriptionTextBox" Text="<%# View.EmailType.Description %>" TextMode="MultiLine" CssClass="mediumWidth smallHeight" />
            </div>
        </div>        
        <div class="propertyItem">
	        <div class="propertyItemHeader"><asp:Localize runat="server" meta:resourceKey="ModifiedOn" /></div>
            <div class="propertyItemContent">
                <%# View.EmailType.ModifiedOn %>
            </div>
        </div>
        <div class="propertyItem">
	        <div class="propertyItemHeader"><asp:Localize runat="server" meta:resourceKey="ModifiedBy" /></div>
            <div class="propertyItemContent">
                <%# View.EmailType.ModifiedBy %>
            </div>
        </div>        
    </div>
    <div class="propertyPaneFooter">-</div>
</div>