<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditDefinitionFieldBaseProperties.ascx.cs" Inherits="UCommerce.Web.UI.Settings.Definitions.EditDefinitionFieldBaseProperties" %>
<%@ Register tagPrefix="commerce" tagName="ValidationSummary" src="../../Controls/ValidationSummaryDisplay.ascx" %>

<commerce:ValidationSummary ID="ValidationSummary1" runat="server" />

<div class="propertyPane">

    <div class="propertyItem">
        <div class="propertyItemHeader"><asp:Localize ID="Localize2" runat="server" meta:resourceKey="DataType" /></div>
        <div class="propertyItemContent">
            <asp:DropDownList runat="server" Enabled="<%# !View.DefinitionField.BuiltIn %>" ID="DataTypeDropDown" DataSource="<%# View.DataTypes %>" DataValueField="DataTypeId" DataTextField="TypeName" OnDataBound="DataTypeDropDown_DataBound" CssClass="mediumWidth" />
        </div>
    </div>

    <asp:PlaceHolder runat="server" id="PropertyPanel"></asp:PlaceHolder>

    <div class="propertyPaneFooter"></div>

</div>
