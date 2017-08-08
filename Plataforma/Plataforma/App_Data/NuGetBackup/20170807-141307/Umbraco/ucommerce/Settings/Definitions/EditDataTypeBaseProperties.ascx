<%@ Import Namespace="UCommerce.Presentation.Web"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditDataTypeBaseProperties.ascx.cs" Inherits="UCommerce.Web.UI.Settings.Definitions.EditDataTypeBaseProperties" %>
<%@ Register tagPrefix="commerce" tagName="ValidationSummary" src="../../Controls/ValidationSummaryDisplay.ascx" %>

<commerce:ValidationSummary runat="server" />
<div class="propertyPane leftAligned">
    <div>
     
        <div class="propertyItem">
            <div class="propertyItemHeader"><asp:Localize runat="server" meta:resourceKey="Definition" /></div>
            <div class="propertyItemContent">
                <asp:DropDownList runat="server" ID="DefinitionDropDown" CssClass="mediumWidth" Enabled="<%# !View.DataType.BuiltIn %>"></asp:DropDownList>
            </div>
        </div>              
        <div class="propertyItem">
            <div class="propertyItemHeader"><asp:Localize runat="server" meta:resourceKey="Nullable" /></div>
            <div class="propertyItemContent">
                <asp:CheckBox runat="server" ID="NullableCheckBox" Checked="<%# View.DataType.Nullable %>" Enabled="<%# !View.DataType.BuiltIn %>" />
            </div>
        </div>            
        <div class="propertyItem">
            <div class="propertyItemHeader"><asp:Localize runat="server" meta:resourceKey="ValidationExpression" /></div>
            <div class="propertyItemContent">
                <asp:TextBox runat="server" ID="ValidationExpressionTextBox" Text="<%# View.DataType.ValidationExpression %>" CssClass="mediumWidth" Enabled="<%# !View.DataType.BuiltIn %>" />                    
            </div>
        </div>                    
    </div>
    <div class="propertyPaneFooter">-</div>
</div>