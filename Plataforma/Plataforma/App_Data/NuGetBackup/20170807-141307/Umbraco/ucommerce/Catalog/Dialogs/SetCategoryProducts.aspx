﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../../masterpages/Dialog.master" CodeBehind="SetCategoryProducts.aspx.cs" Inherits="UCommerce.Web.UI.UCommerce.Catalog.Dialogs.SetCategoryProducts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderLabel" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentArea" runat="server">
    <div class="propertyPane dialog-header">
        <h3>
		    <span>
			    <asp:Localize id="Localize2" meta:resourcekey="Header" runat="server" />
		    </span>
	    </h3>
	    <div>
		    <p class="guiDialogTiny">
			    <asp:Localize id="Localize3" runat="server" meta:resourcekey="SubHeader" />
		    </p>
            <a class="modal-close" onclick="UCommerceClientMgr.closeModalWindow();" data-dismiss="modal" aria-hidden="true"></a>
	    </div>
        <a class="modal-close" onclick="UCommerceClientMgr.closeModalWindow();" data-dismiss="modal" aria-hidden="true"></a>
   	</div>
    <div class="propertyPane contentPane set-category-products">
        <asp:PlaceHolder ID="ProductsPlaceHolder" runat="server"></asp:PlaceHolder>
    </div>
    
    <div class="propertyPane dialog-actions">
	    <div class="footerOkCancel">
			<asp:Button id="SaveButton" CssClass="dialog-saveButton" runat="server" meta:resourcekey="SaveButton" onclick="SaveButton_Clicked"></asp:Button>
			<em>
			    <asp:Localize id="Localize5" runat="server" meta:resourcekey="Or" />
			</em>
            <a href="#" Class="dialog-cancelButton" onclick="UCommerceClientMgr.closeModalWindow()">
				<asp:Localize id="Localize1" runat="server" meta:resourcekey="CancelButton" />
			</a>
		</div>
	</div>
</asp:Content>
