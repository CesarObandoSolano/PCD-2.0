<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AmountOffOrderLinesUi.ascx.cs" Inherits="UCommerce.Web.UI.UCommerce.Marketing.Awards.AmountOffOrderLinesUi" %>

<table cellpadding="0" cellspacing="0" style="width:100%;">
	<tr>
		<td>
		    <asp:PlaceHolder runat="server" id="NonEditModePlaceHolder">
				<asp:Label runat="server" id="AmountOffOrderLinesLabel" />
			</asp:PlaceHolder>

			<asp:PlaceHolder runat="server" id="EditModePlaceHolder"  visible="false">
				<asp:TextBox runat="server" id="AmountOffOrderLinesText" CssClass="width100" /><br/>
				<asp:RegularExpressionValidator id="DecimalValidator" runat="server" ControlToValidate="AmountOffOrderLinesText" 
								Style="color:Red;" ValidationExpression="^(\d|-)?(\d|,)*\.?\d*$" ValidationGroup="AmountOffOrderLines"
								Display="Dynamic" />
				<asp:RequiredFieldValidator id="RequiredValidator" runat="server" ValidationGroup="AmountOffOrderLines"
								ControlToValidate="AmountOffOrderLinesText" Display="Dynamic" />
			</asp:PlaceHolder>
		</td>
		<td style="width:50px; text-align:right; vertical-align:top;">
			<asp:ImageButton id="EditButton" runat="server" imageurl="../../Images/ui/pencil.png" meta:resourcekey="Edit" onclick="EditButton_Click" />
			<asp:ImageButton id="SaveButton" runat="server" imageurl="../../Images/ui/save.gif" meta:resourcekey="Save" visible="false" onclick="SaveButton_Click" ValidationGroup="AmountOffOrderLines" />
			<asp:ImageButton id="DeleteButton" runat="server" imageurl="../../Images/ui/cross.png" meta:resourcekey="Delete" onclick="DeleteButton_Click" /><br />
		</td>
	</tr>
</table>