<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchProducts.ascx.cs" Inherits="UCommerce.Web.UI.Catalog.SearchProducts" %>

<script type="text/javascript">
	function openProductView(url) {
		this.document.location.href = url;
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
<div class="propertyPane">

	<commerce:PropertyPanel runat="server" meta:resourceKey="SkuNumber">
		<asp:TextBox runat="server" ID="ProductSkuNumber" CssClass="mediumWidth" />        
	</commerce:PropertyPanel>
	<commerce:PropertyPanel runat="server" meta:resourceKey="VariantSkuNumber">
		<asp:TextBox runat="server" ID="ProductVariantSkuNumber" CssClass="mediumWidth" />        
	</commerce:PropertyPanel>
	<commerce:PropertyPanel runat="server" meta:resourceKey="DisplayName">
		<asp:TextBox runat="server" ID="ProductName" CssClass="mediumWidth" />        
	</commerce:PropertyPanel>
	<commerce:PropertyPanel runat="server" meta:resourceKey="Description">
		<asp:TextBox runat="server" ID="ProductDescription" CssClass="mediumWidth" />        
	</commerce:PropertyPanel>
	<commerce:PropertyPanel runat="server" meta:resourceKey="ProductDefinition">
		<asp:DropDownList runat="server" ID="ProductDefinitions" DataTextField="Name" DataValueField="ProductDefinitionId" CssClass="mediumWidth"  />
	</commerce:PropertyPanel>
	<commerce:PropertyPanel runat="server" meta:resourceKey="ProductCatalogGroup">
		<asp:DropDownList runat="server" ID="ProductCatalogGroups" DataTextField="Name" DataValueField="ProductCatalogGroupId" CssClass="mediumWidth"  />
	</commerce:PropertyPanel>
	<commerce:PropertyPanel runat="server" meta:resourceKey="ProductCatalog">
		<asp:DropDownList runat="server" ID="ProductCatalogs" DataTextField="Name" DataValueField="ProductCatalogId" CssClass="mediumWidth"  />
	</commerce:PropertyPanel>
	<commerce:PropertyPanel runat="server" Text="&nbsp;" WithoutHeader="True">
		<asp:Button runat="server" ID="SearchButton" CssClass="mediumButton" OnClick="SearchButton_Clicked" meta:resourceKey="Search" />
	</commerce:PropertyPanel>
	<div class="propertyPaneFooter">-</div>
</div>
<asp:Panel CssClass="propertyPane" ID="SearchResults" runat="server">
	<asp:Repeater runat="server" ID="ProductRepeater" EnableViewState="false">
		<HeaderTemplate>
			<table class="dataList" cellspacing="0" width="100%" id="productTable">
				<thead>
					<tr>
						<th><span><asp:Localize runat="server" meta:resourceKey="SkuNumber" /></span></th>
						<th><span><asp:Localize runat="server" meta:resourceKey="Name" /></span></th>
						<th><span><asp:Localize runat="server" meta:resourceKey="DisplayName" /></span></th>
						<th><span><asp:Localize runat="server" meta:resourceKey="DisplayOnSite" /></span></th>
						<th><span><asp:Localize runat="server" meta:resourceKey="Category" /></span></th>
						<th><span><asp:Localize runat="server" meta:resourceKey="ProductDefinition" /></span></th>
						<th><span><asp:Localize runat="server" meta:resourceKey="Currency" /></span></th>
						<th class="rightAligned" ><span><asp:Localize runat="server" meta:resourceKey="Price" /></span></th>
					</tr>
				</thead>
				<tbody>
		</HeaderTemplate>
		<ItemTemplate>
					<tr class="clickable" onclick="openProductView('<%# GetProductLink(Container.DataItem) %>');">
						<td><%# GetSkuAndVariantNumber(Container.DataItem) %></td>
						<td><%# GetName(Container.DataItem) %></td>
						<td><%# GetDisplayName(Container.DataItem) %></td>
						<td><%# DataBinder.Eval(Container.DataItem, "DisplayOnSite") %></td>
						<td><%# GetCategory(Container.DataItem) %></td>
						<td>
							<a href='<%# GetProductDefinitionLink(Container.DataItem) %>'>
								<%# GetProductDefinitionName(Container.DataItem) %>
							</a>
						</td>
						<td>
							<%# GetCurrency(Container.DataItem) %>
						</td>
						<td class="rightAligned"><%# GetPrice(Container.DataItem) %></td>
					</tr>
		</ItemTemplate>
		<FooterTemplate>
				</tbody>
			</table>
		</FooterTemplate>
	</asp:Repeater>
	<div class="propertyPaneFooter">-</div>
</asp:Panel>