<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderAmountUi.ascx.cs" Inherits="UCommerce.Web.UI.UCommerce.Marketing.Targets.OrderAmountUi" %>

<table cellpadding="0" cellspacing="0" style="width:100%;">
	<tr>
		<td>
		    <asp:Localize ID="Localize1" runat="server" meta:resourcekey="MinOrderAmount" />: 
			<asp:PlaceHolder runat="server" id="NonEditModePlaceHolder">
				<asp:Label runat="server" id="MinOrderAmountLabel" />
			</asp:PlaceHolder>

			<asp:PlaceHolder runat="server" id="EditModePlaceHolder"  visible="false">
				<asp:TextBox runat="server" id="MinOrderAmountText" /><br />
				<asp:RegularExpressionValidator id="DecimalValidator" runat="server" ControlToValidate="MinOrderAmountText" 
								Style="color:Red;" ValidationExpression="^(\d|-)?(\d|,)*\.?\d*$" ValidationGroup="MinOrderAmountGroup"
								Display="Dynamic" />
				<asp:RequiredFieldValidator id="RequiredValidator" runat="server" ValidationGroup="MinOrderAmountGroup"
								ControlToValidate="MinOrderAmountText" Display="Dynamic" />
			</asp:PlaceHolder>
		</td>
		<td style="width:50px; text-align:right; vertical-align:top;">
			<asp:ImageButton id="EditButton" runat="server" imageurl="../../Images/ui/pencil.png" meta:resourcekey="Edit" onclick="EditButton_Click" />
			<asp:ImageButton id="SaveButton" runat="server" imageurl="../../Images/ui/save.gif" meta:resourcekey="Save" visible="false" onclick="SaveButton_Click" ValidationGroup="MinOrderAmountGroup" />
			<asp:ImageButton id="DeleteButton" runat="server" imageurl="../../Images/ui/cross.png" meta:resourcekey="Delete" onclick="DeleteButton_Click" /><br />
		</td>
	</tr>
</table>