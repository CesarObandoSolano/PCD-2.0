<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateOrderNumber.ascx.cs" Inherits="UCommerce.Web.UI.Settings.Orders.CreateOrderNumber" %>
Name:
<asp:TextBox runat="server" ID="NameTextBox" CssClass="bigInput fontSize" Width="350px"/>
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="NameTextBox" CssClass="validationMessage" Display="Static" ErrorMessage="Please enter a name" Width="100%" />
<asp:CustomValidator runat="server" ErrorMessage="The name already exist." OnServerValidate="NameValidator_ServerValidate" CssClass="validationMessage" />
    
<div style="padding-top: 25px">
    <asp:Button runat="server" ID="CreateButton" Width="90px" Text="Create" OnClick="CreateButton_Clicked" />
    <em>or</em>
    <a href="#" onclick="UCommerceClientMgr.closeModalWindow();" style="color: blue; cursor: hand">Cancel</a>    
</div>