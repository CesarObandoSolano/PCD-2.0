<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditPaymentMethodAvailability.ascx.cs" Inherits="UCommerce.Web.UI.Settings.Orders.EditPaymentMethodAvailability" %>
<%@ Register tagPrefix="commerce" tagName="ValidationSummary" src="../../Controls/ValidationSummaryDisplay.ascx" %>

<commerce:ValidationSummary runat="server" />
<div class="propertyPane leftAligned" >
    <div>
        <div class="propertyItem">
            <div class="propertyItemHeader"><asp:Localize runat="server" meta:resourceKey="ProductCatalogGroups" /></div>
            <div class="propertyItemContent">
                <asp:CheckBoxList runat="server" ID="ProductCatalogGroupsCheckBoxList" DataSource="<%# View.ProductCatalogGroups %>" DataValueField="ProductCatalogGroupId" DataTextField="Name" />
            </div>
        </div>
            
        <div class="propertyItem">
            <div class="propertyItemHeader"><asp:Localize runat="server" meta:resourceKey="Countries" /></div>
            <div class="propertyItemContent">
                <asp:CheckBoxList runat="server" ID="CountryCheckBoxList" DataSource="<%# View.Countries %>" DataValueField="CountryId" DataTextField="Name" />
            </div>
        </div>
    </div>
    <div class="propertyPaneFooter">-</div>
</div>