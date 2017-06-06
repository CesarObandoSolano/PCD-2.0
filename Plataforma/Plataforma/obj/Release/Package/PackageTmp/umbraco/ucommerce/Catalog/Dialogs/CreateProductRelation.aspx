<%@ page language="C#" autoeventwireup="true" codebehind="CreateProductRelation.aspx.cs"
	inherits="UCommerce.Web.UI.Catalog.Dialogs.CreateProductRelation" masterpagefile="../../MasterPages/Dialog.Master" %>

<%@ import namespace="UCommerce.Web.UI.Catalog.Dialogs" %>
<%@ register tagprefix="uc" tagname="CatalogItemSelector" src="CatalogItemSelector.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="HeaderLabel" runat="server"><asp:Localize ID="Localize5" runat="server" meta:resourceKey="Header" /></asp:content>
<asp:content id="Content3" contentplaceholderid="ContentArea" runat="server">
   	<div class="propertyPane dialog-header">
        <h3>
		    <span><asp:Localize id="Localize2" runat="server" meta:resourcekey="Header" /></span>
	    </h3>
	    <div>
		    <p class="guiDialogTiny"><asp:Localize id="Localize3" runat="server" meta:resourcekey="SubHeader" /></p>
	    </div>
        <a class="modal-close" onclick="UCommerceClientMgr.closeModalWindow();" data-dismiss="modal" aria-hidden="true"></a>
   	</div>
	<div class="propertyPane contentPane create-product-relation">
		
		<div class="propertyContainer propertyItems">
			<div class="propertyItem">
				<div class="propertyItemHeader" ><asp:Localize id="Localize4" runat="server" meta:resourcekey="RelationType" /></div>
				<div class="propertyItemContent" >
					<asp:DropDownList CssClass="editorDropDownBox" runat="server" id="RelationTypeSelector" datasource="<%# RelationTypes %>"
						datatextfield="Name" datavaluefield="ProductRelationTypeId" AutoPostBack="True" />
				</div>
			</div>
			<div class="propertyItem">
				<div class="propertyItemHeader" ><asp:Localize id="Localize6" runat="server" meta:resourcekey="TwoWayRelation" /></div>
				<div class="propertyItemContent" >
					<asp:CheckBox id="TwoWayRelationshipCheckBox" runat="server" />
				</div>
			</div> 
		
			<asp:PlaceHolder runat="server" ID="ProductRelationsPlaceHolder"></asp:PlaceHolder>
		</div>
		<div class="propertyPaneFooter"></div>
	</div>

    <div class="propertyPane dialog-actions">
	    <div class="footerOkCancel">
			<asp:Button id="SaveButton" CssClass="dialog-saveButton" runat="server" meta:resourcekey="SaveButton" onclick="SaveButton_Clicked" />
			<em>or&nbsp;</em>
			<a href="#" Class="dialog-cancelButton" onclick="UCommerceClientMgr.closeModalWindow()"><asp:Localize id="Localize1" runat="server" meta:resourcekey="CancelButton" /></a>
		</div>
	</div>
</asp:content>
