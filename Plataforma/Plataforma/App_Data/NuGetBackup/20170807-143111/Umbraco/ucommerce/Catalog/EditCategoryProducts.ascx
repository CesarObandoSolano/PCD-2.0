<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditCategoryProducts.ascx.cs" Inherits="UCommerce.Web.UI.Catalog.EditCategoryProducts" %>

<div class="propertyPane leftAligned">
    <h2 class="propertyPaneTitel"><asp:Localize runat="server" meta:resourcekey="ProductsInCategory"></asp:Localize></h2>
    <asp:Repeater runat="server" ID="ProductRepeater" EnableViewState="false">
        <HeaderTemplate>
            <table class="dataList" cellspacing="0" width="100%" id="productTable">
                <thead>
					<tr>
						<th><span><asp:Localize runat="server" meta:resourceKey="Sku" /></span></th>
						<th><span><asp:Localize runat="server" meta:resourceKey="Name" /></span></th>
						<th><span><asp:Localize runat="server" meta:resourceKey="ProductType" /></span></th>
						<th><span><asp:Localize runat="server" meta:resourceKey="Currency" /></span></th>
						<th class="rightAligned"><span><asp:Localize runat="server" meta:resourceKey="Price" /></span></th>
					</tr>
				</thead>
				<tbody>
        </HeaderTemplate>
        <ItemTemplate>
                <tr class="clickable" onclick='openProductView(<%# DataBinder.Eval(Container.DataItem, "ProductId") %>, <%# View.Category.CategoryId %>);return false;'>
                    <td style="padding-right: 15px"><%# DataBinder.Eval(Container.DataItem, "Sku") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "ProductDefinitionDisplayName") %></td>
					<td><%# Currency %></td>
                    <td class="rightAligned" ><%# DataBinder.Eval(Container.DataItem, "PriceAmount", "{0:n}")%></td>
                </tr>
        </ItemTemplate>
        <FooterTemplate>
				</tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>
	<div class="propertyPaneFooter"></div>
</div>     

<script type="text/javascript">
    function openProductView(productId, parentCategoryId) {
        this.document.location.href = 'editproduct.aspx?id=' + productId + '&parentcategoryId=' + parentCategoryId;
    }

    $(function () {
        $("#productTable").dataTable(
			{
			    "bPaginate": false,
			    // disable initial sort
			    "aaSorting": []
			}
		);
    });
</script>