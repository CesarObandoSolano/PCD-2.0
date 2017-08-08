<%@ page language="C#" autoeventwireup="true" codebehind="AddProduct.aspx.cs"
	inherits="UCommerce.Web.UI.Orders.AddProduct" MasterPageFile="../MasterPages/Dialog.Master" %>
<%@ import namespace="UCommerce.Web.UI.Catalog.Dialogs" %>
<%@ register tagprefix="uc" tagname="CatalogItemSelector" src="../Catalog/Dialogs/CatalogItemSelector.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderLabel" runat="server"><asp:Localize ID="Localize5" runat="server" meta:resourceKey="Header" /></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentArea" runat="server">
   	<div class="propertyPane dialog-header">
        <h3>
		    <span><asp:Localize id="Localize2" runat="server" meta:resourcekey="Header" /></span>
	    </h3>
	    <div>
		    <p class="guiDialogTiny"><asp:Localize id="Localize3" runat="server" meta:resourcekey="SubHeader" /></p>
	    </div>
        <a class="modal-close" onclick="UCommerceClientMgr.closeModalWindow();" data-dismiss="modal" aria-hidden="true"></a>
   	</div>
	<div class="contentPane propertyPane add-product-on-order">
		<div class="propertyContainer propertyItems">
			<div class="propertyItem">
				<div class="propertyItemHeader"><asp:Localize ID="Localize4" runat="server" meta:resourceKey="PriceGroup" /></div>
				<div class="propertyItemContent propertyItemContentSmall">
					<asp:DropDownList runat="server" ID="PriceGroupDropDown" CssClass="mediumWidth" Style="margin-top: 0px" />
				</div>
			</div>
			

			<div class="multiProductPicker">
				<asp:PlaceHolder runat="server" ID="MultiProductSelectionPlaceHolder"></asp:PlaceHolder>
				<div class="propertyPaneFooter"></div>
			</div>
		</div>
	</div>
	
    <div class="propertyPane dialog-actions">
		<div class="footerOkCancel">
			<asp:Button id="SaveButton" CssClass="dialog-saveButton" runat="server" meta:resourcekey="SaveButton" onclick="SaveButton_Clicked" />
			<em>or </em><a href="#" class="dialog-cancelButton" onclick="UCommerceClientMgr.closeModalWindow()">
				<asp:Localize id="Localize1" runat="server" meta:resourcekey="CancelButton" />
			</a>
		</div>
	</div>
 
</asp:Content>