<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateDataType.ascx.cs" Inherits="UCommerce.Web.UI.Settings.Definitions.CreateDataType" %>
<table>
	<tr>
		<td>
			<asp:Localize meta:resourcekey="DataTypeNameLabel" runat="server">Name:</asp:Localize>
		</td>
		<td>
			<asp:TextBox runat="server" ID="NameTextBox" CssClass="bigInput fontSize"/>
			<asp:RequiredFieldValidator meta:ResourceKey="RequiredNameValidator" runat="server" ControlToValidate="NameTextBox" CssClass="validationMessage" Display="Dynamic" ErrorMessage="Please enter a name(*)" Width="100%" />
			<asp:CustomValidator meta:ResourceKey="UniqueNameValidator" CssClass="validationMessage" ControlToValidate="NameTextBox" ErrorMessage="The name is already in use(*)" OnServerValidate="NameValidator_ServerValidate" Display="Dynamic" runat="server"  />
		</td>
	</tr>
	<tr>
		<td>
			<asp:Localize meta:resourcekey="DefinitionNameLabel" runat="server">Definition:</asp:Localize>
		</td>
		<td> 
			<asp:DropDownList runat="server" ID="DefinitionDropDownList" CssClass="bigInput fontSize"></asp:DropDownList>
		</td>
	</tr>
</table>



<div style="padding-top: 25px">
    <asp:Button runat="server" ID="CreateButton" Width="90px" Text="Create" OnClick="CreateButton_Clicked" />
    <em>or</em>
    <a href="#" onclick="UCommerceClientMgr.closeModalWindow();" style="color: blue; cursor: hand">Cancel</a>
</div>