<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateEmailProfile.ascx.cs" Inherits="UCommerce.Web.UI.Settings.Email.CreateEmailProfile" %>
Name:
<asp:TextBox runat="server" ID="NameTextBox" CssClass="bigInput fontSize" Width="350px"/>
<asp:RequiredFieldValidator runat="server" ControlToValidate="NameTextBox" CssClass="validationMessage" Display="Static" ErrorMessage="Please enter a name" Width="100%" />
<asp:CustomValidator runat="server" ErrorMessage="The name already exist." CssClass="validationMessage" OnServerValidate="NameValidator_ServerValidate" />
    
<div style="padding-top: 25px">
    <asp:Button runat="server" ID="CreateButton" Width="90px" Text="Create" OnClick="CreateButton_Clicked" />
    <em>or</em>
    <a href="#" onclick="UCommerceClientMgr.closeModalWindow();" style="color: blue; cursor: hand">Cancel</a>    
</div>