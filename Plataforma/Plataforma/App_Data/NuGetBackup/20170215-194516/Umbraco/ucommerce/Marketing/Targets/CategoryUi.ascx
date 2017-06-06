<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryUi.ascx.cs" Inherits="UCommerce.Web.UI.UCommerce.Marketing.Targets.CategoryUi" %>

<table cellpadding="0" cellspacing="0" style="width:100%;">
	<tr>
		<td>
			<asp:PlaceHolder runat="server" id="NoCategorySelectedPlaceHolder">
				<asp:Localize id="NoCategorySelectedLabel" runat="server" meta:resourcekey="NoCategorySelected" />
			</asp:PlaceHolder>

			<asp:PlaceHolder runat="server" id="CategoryInfoPlaceHolder" visible="false">
				<asp:Localize id="NameLabel" runat="server" meta:resourcekey="Name" />: <asp:Label runat="server" id="CategoryNameLabel" /><br />
			</asp:PlaceHolder>
		</td>
		<td style="width:50px; text-align:right; vertical-align:top;">
			<asp:ImageButton id="SelectCategoryButton" runat="server" ImageUrl="../../Images/ui/pencil.png" meta:resourcekey="Edit" />
			<asp:ImageButton id="DeleteTargetButton" runat="server" ImageUrl="../../Images/ui/cross.png" meta:resourcekey="Delete" onclick="DeleteTargetButton_Click" /><br />
		</td>
	</tr>
</table>