<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditProductRelations.ascx.cs" Inherits="UCommerce.Web.UI.Catalog.EditProductRelations" %>
<%@ Register tagPrefix="commerce" tagName="ValidationSummary" src="../Controls/ValidationSummaryDisplay.ascx" %>

<commerce:ValidationSummary runat="server" />
<script type="text/javascript">
	function confirm_delete()
	{
		return (confirm('<%# GetLocalResourceObject("ConfirmRelationDelete").ToString() %>') == true);
	}

	function openProductView(productId, parentCategoryId) {
		this.document.location.href = '<%# GetProductEditorUrl() %>?id=' + productId + '&parentCategoryId=' + parentCategoryId
	}
    $(function () {
        $("#product-relations").dataTable(
            {
                "bPaginate": false,
                // disable initial sort
                "aaSorting": [],
                // disable sorting on first column
                "aoColumnDefs": [
					{ 'bSortable': false, 'aTargets': [0] }
                ]
            });
    });

</script>

<div class="propertyPane">
    <h2 class="propertyPaneTitel"><asp:Localize runat="server" meta:resourcekey="ProductRelations"></asp:Localize></h2>
	<asp:Repeater runat="server" DataSource="<%# RelatedProducts %>" EnableViewState="false">
		<HeaderTemplate>
			<table id="product-relations" class="dataList border-collapse">
			<thead>
				<tr>
					<th></th>
					<th><asp:Localize ID="Localize1" runat="server" meta:resourceKey="RelationTypeHeader" /></th>
					<th><asp:Localize ID="Localize2" runat="server" meta:resourceKey="SkuHeader" /></th>
					<th><asp:Localize ID="Localize3" runat="server" meta:resourceKey="NameHeader" /></th>
				</tr>
				</thead>
				<tbody>
		</HeaderTemplate>
			<ItemTemplate>
			<tr>
				<td class="imageButton">
					<asp:ImageButton ID="DeleteButton" OnClick="DeleteButton_Clicked"  runat="server" ImageUrl="../images/ui/cross.png" CommandArgument="<%# ((ProductRelationViewObject)Container.DataItem).ProductRelationId %>" OnClientClick="return confirm_delete();" />
				</td> 
				<td style="width: 60px;padding-right: 15px">
					<%# ((ProductRelationViewObject)Container.DataItem).ProductRelationTypeName %>
				</td>
				<td>
					<asp:HiddenField runat="server" ID="ProductRelationIdHiddenField" Value="<%# ((ProductRelationViewObject)Container.DataItem).ProductRelationId %>" />
					<%# GetSku(((ProductRelationViewObject)Container.DataItem).Product) %>
				</td>
				<td>
					<a style="cursor:pointer;"onclick='openProductView(<%# GetProductFamilyId(((ProductRelationViewObject)Container.DataItem).Product) %>, <%# ((ProductRelationViewObject)Container.DataItem).ParentCategoryId %>);return false;'><%# ((ProductRelationViewObject)Container.DataItem).Product.Name%></a>
				</td>
			</tr>
	</ItemTemplate>
		<FooterTemplate>
		</tbody>
			</table>
		</FooterTemplate>
	</asp:Repeater>
	<div class="propertyPaneFooter"></div>
</div>