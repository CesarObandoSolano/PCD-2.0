<%@ control language="C#" autoeventwireup="true" codebehind="EditDefinitionBaseProperties.ascx.cs" inherits="UCommerce.Web.UI.Settings.Definitions.EditDefinitionBaseProperties" %>
<%@ Register tagPrefix="commerce" tagName="ValidationSummary" src="../../Controls/ValidationSummaryDisplay.ascx" %>

<script type="text/javascript">
	function openCategory(id) {
		document.location.href = '../../catalog/editcategory.aspx?id=' + id + '&activeTab=EditCategoryBaseProperties.ascx';
	}
</script>
<commerce:ValidationSummary id="ValidationSummary1" runat="server" />
<div style="text-align: left;">
	<div class="propertyPane leftAligned">
        <div class="propertyItem">
	        <div class="propertyItemHeader"><asp:Localize runat="server" meta:resourceKey="Description" /></div>
            <div class="propertyItemContent"><asp:TextBox runat="server" ID="DescriptionTextBox" TextMode="MultiLine" CssClass="mediumWidth smallHeight" Text="<%# View.Definition.Description %>" /></div>
	    </div>
		<div class="propertyItem">
			<div class="propertyItemHeader"><asp:localize runat="server" meta:resourcekey="InheritedDefinitions" /></div>
			<div class="propertyItemContent">
				
	<asp:CheckboxList runat="server" id="ParentDefinitionCheckboxList" />
				<asp:customvalidator id="InheritanceServerValidator" onservervalidate="Inheritance_ServerValidator" runat="server" meta:resourcekey="UniqueSku" errormessage="SKU already exists*" cssclass="validationMessage" display="Dynamic" />
			</div>
		</div>
        <div class="propertyPaneFooter"></div>
	</div>
</div>