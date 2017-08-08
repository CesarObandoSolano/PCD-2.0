<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateProductDefinition.ascx.cs" Inherits="UCommerce.Web.UI.Settings.Definitions.CreateProductDefinition" %>
Name:
<asp:TextBox runat="server" ID="NameTextBox" CssClass="bigInput fontSize" Width="350px"/><br />
<asp:RequiredFieldValidator runat="server" CssClass="validationMessage" ControlToValidate="NameTextBox" ErrorMessage="Please enter a name.(*)" Width="100%" meta:ResourceKey="RequiredNameValidator" Display="Dynamic" />
<asp:CustomValidator ID="UniqueNameCustomValidator" CssClass="validationMessage" ControlToValidate="NameTextBox" ErrorMessage="The product definition already exists. Please select another name.(*)" meta:ResourceKey="UniqueNameValidator" OnServerValidate="UniqeNameCustomVallidator_ServerValidate" Display="Dynamic" runat="server"/>
<asp:CustomValidator ID="LicenseCustomValidator" CssClass="validationMessage" ControlToValidate="NameTextBox" ErrorMessage="You will exceed your license if you create any more.(*)" meta:ResourceKey="LicenseCustomValidator" OnServerValidate="LicenseCustomValidator_ServerValidate" Display="Dynamic" runat="server"/>
    
<div style="padding-top: 25px">
    <asp:Button runat="server" ID="CreateButton" Width="90px" Text="Create" OnClick="CreateButton_Clicked" />
    <em>or</em>
    <a href="#" onclick="UCommerceClientMgr.closeModalWindow();" style="color: blue; cursor: hand">Cancel</a>    
</div>