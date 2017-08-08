<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateProductCatalogGroup.ascx.cs" Inherits="UCommerce.Web.UI.Catalog.CreateProductCatalogGroup" %>
<asp:Localize meta:resourcekey="NameLabel" runat="server">Name:</asp:Localize>
<asp:TextBox runat="server" ID="NameTextBox" CssClass="bigInput fontSize" Width="350px"/>
<asp:RequiredFieldValidator runat="server" ControlToValidate="NameTextBox" CssClass="validationMessage" Display="Dynamic" ErrorMessage="Please enter a name" Width="100%" meta:ResourceKey="RequiredNameValidator"/>
<asp:CustomValidator runat="server" 
        ID="UniqueNameValidator" 
        Display="Dynamic"
        CssClass="validationMessage"
        ErrorMessage="The selected name is already in use" 
        Width="100%" 
        OnServerValidate="UniqueNameValidator_ServerValidate"
		meta:ResourceKey="UniqueNameValidator"/>
    
    
<asp:CustomValidator runat="server" ID="CustomValidator1"
    CssClass="validationMessage"
    Display="Dynamic"
    ErrorMessage="Your license does not allow you to create any more"
    Width="100%"
    OnServerValidate="LicenseCheckValidator_ServerValidate"
	meta:ResourceKey="LicenseValidator"></asp:CustomValidator>
        
<div style="padding-top: 25px">
    <asp:Button runat="server" ID="CreateButton" Width="90px" Text="Create" meta:ResourceKey="CreateButton" OnClick="CreateButton_Clicked" />
    <em><asp:Localize meta:resourcekey="OrLabel" runat="server">or</asp:Localize></em>
    <a href="#" onclick="UCommerceClientMgr.closeModalWindow();" style="color: blue;"><asp:Localize meta:resourcekey="CancelLabel" runat="server">Cancel</asp:Localize></a>    
</div>