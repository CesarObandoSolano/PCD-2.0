<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PercentOffShippingTotalUi.ascx.cs" Inherits="UCommerce.Web.UI.UCommerce.Marketing.Awards.PercentOffShippingTotalUi" %>

<table cellpadding="0" cellspacing="0" style="width:100%;">
	<tr>
		<td>
			<asp:PlaceHolder runat="server" id="NonEditModePlaceHolder">	
				<asp:Label runat="server" id="PercentOffShippingTotalLabel" />
			</asp:PlaceHolder>

			<asp:PlaceHolder runat="server" id="EditModePlaceHolder"  visible="false">
				<asp:TextBox runat="server" id="PercentOffShippingTotalText" /><br />
				<asp:RangeValidator id="PercentOffRangeValidator" runat="server"
								  ControlToValidate="PercentOffShippingTotalText"
								  ValidationGroup="PercentOffShippingTotal"
								  Type="Double"
								  MinimumValue="-100"
								  MaximumValue="100" />
				<asp:RequiredFieldValidator id="RequiredValidator" runat="server" ValidationGroup="PercentOffShippingTotal"
								ControlToValidate="PercentOffShippingTotalText" Display="Dynamic" />
			</asp:PlaceHolder>
		</td>
		<td style="width:50px; text-align:right; vertical-align:top;">
			<asp:ImageButton id="EditButton" runat="server" imageurl="../../Images/ui/pencil.png" meta:resourcekey="Edit" onclick="EditButton_Click" />
			<asp:ImageButton id="SaveButton" runat="server" imageurl="../../Images/ui/save.gif" meta:resourcekey="Save" visible="false" onclick="SaveButton_Click" ValidationGroup="PercentOffShippingTotal" />
			<asp:ImageButton id="DeleteButton" runat="server" imageurl="../../Images/ui/cross.png" meta:resourcekey="Delete" onclick="DeleteButton_Click" /><br />
		</td>
	</tr>
</table>