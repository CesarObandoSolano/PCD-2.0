<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateProductRelationType.ascx.cs" Inherits="UCommerce.Web.UI.Settings.Catalog.CreateProductRelationType" %>
Name:
<asp:TextBox runat="server" ID="NameTextBox" CssClass="bigInput fontSize" Width="350px"/>
<asp:RequiredFieldValidator ID="NameRequiredFieldValidator" runat="server" ControlToValidate="NameTextBox" CssClass="validationMessage" Display="Dynamic" meta:ResourceKey="NameRequiredFieldValidator" ErrorMessage="Please enter a name(*)" Width="100%" />
<asp:CustomValidator ID="NameCustomValidator" runat="server" ErrorMessage="The name is already in use(*)" CssClass="validationMessage" Display="Dynamic" meta:resourceKey="NameCustomValidator" OnServerValidate="NameValidator_ServerValidate" />
<asp:CustomValidator ID="LicenseCustomValidator" runat="server" CssClass="validationMessage" ErrorMessage="Your license does not allow you to create any more(*)" Display="Dynamic" meta:resourceKey="LicenseCustomValidator" OnServerValidate="LicenseValidator_ServerValidate" />
    
<div style="padding-top: 25px">
    <asp:Button runat="server" ID="CreateButton" Width="90px" Text="Create" OnClick="CreateButton_Clicked" />
    <em>or</em>
    <a href="#" onclick="UCommerceClientMgr.closeModalWindow();" style="color: blue; cursor: hand">Cancel</a>
</div>