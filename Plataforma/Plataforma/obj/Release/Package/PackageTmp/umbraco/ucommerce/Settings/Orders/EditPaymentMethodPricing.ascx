<%@ control language="C#" autoeventwireup="True" codebehind="EditPaymentMethodPricing.ascx.cs" inherits="UCommerce.Web.UI.Settings.Orders.EditPaymentMethodPricing" %>
<%@ Import Namespace="UCommerce.EntitiesV2" %>
<%@ Register tagPrefix="commerce" tagName="ValidationSummary" src="../../Controls/ValidationSummaryDisplay.ascx" %>

<commerce:ValidationSummary runat="server" />
<div class="propertyPane leftAligned">
	<div>
		<div class="propertyItem">
			<div class="propertyItemHeader"><asp:Localize runat="server" meta:resourcekey="FeePercentage" /> %</div>
			<div class="propertyItemContent">
				<asp:TextBox runat="server" style="text-align: right;" id="FeePercentageTextBox" text="<%# View.PaymentMethod.FeePercent %>"
					cssclass="smallWidth" />
				
				<asp:RequiredFieldValidator runat="server" controltovalidate="FeePercentageTextBox"
					errormessage="*" cssclass="validator" />
				<asp:RangeValidator runat="server" controltovalidate="FeePercentageTextBox" type="Double"
					minimumvalue="0" maximumvalue="100" errormessage="*" cssclass="validator" />
			</div>
		</div>
	</div>
	<div class="propertyPaneFooter">
		-</div>
</div>
<div class="propertyPane leftAligned">
	<div>
		<div class="">
			<div class=""><asp:Localize runat="server" meta:resourcekey="Description" /></div>
            <div class="" style="width: 100%;">
				<commerce:BulkEditGridView bulkedit="true" runat="server" id="PaymentMethodPricesGridView" CssClass="dataList customDataList paymentMethodPrices" Style="margin-top: 20px;"
					datasource="<%# PaymentMethodFees %>" showheader="true" datakeynames="PaymentMethodFeeId"
					autogeneratecolumns="false" gridlines="None">
                <columns>
                    <asp:TemplateField meta:resourceKey="EnabledHeader" ControlStyle-Width="50px">
                        <EditItemTemplate>
                            <asp:CheckBox runat="server" 
                                ID="EnabledCheckBox" 
								style="text-align: center;"
                                Checked="<%# ((PaymentMethodFee)Container.DataItem).Id > 0 %>" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField meta:resourceKey="PriceGroupHeader">
                        <EditItemTemplate>
                            <commerce:SafeDropDownList runat="server"
                                ID="PriceGroupDropDown"
                                AllowPrebindSet="true"                          
                                SelectedValue="<%# ((PaymentMethodFee)Container.DataItem).PriceGroup.PriceGroupId %>"
                                DataSource="<%# GetPriceGroupsForCurrency(((PaymentMethodFee)Container.DataItem).Currency) %>"
                                DataValueField="PriceGroupId"
                                DataTextField="Name"
                                OnDataBound="PriceGroupDropDown_DataBound"
                                CssClass="mediumWidth" />                            
                        </EditItemTemplate>                   
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourceKey="PriceHeader" ItemStyle-Wrap="false">
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="PriceTextBox" Text="<%# ((PaymentMethodFee)Container.DataItem).Fee %>" CssClass="amountInput"></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="PriceTextBox" ErrorMessage="*" CssClass="validator" />
                            <asp:RangeValidator runat="server" ControlToValidate="PriceTextBox" Type="Double" MinimumValue="0" MaximumValue="<%# decimal.MaxValue %>" ErrorMessage="*" CssClass="validator" />
                        </EditItemTemplate>
                    </asp:TemplateField>
					<asp:TemplateField meta:resourceKey="CurrencyHeader" ControlStyle-Width="100px">
                        <EditItemTemplate>  
                            <asp:HiddenField runat="server" ID="PaymentMethodFeeId" Value="<%# ((PaymentMethodFee)Container.DataItem).PaymentMethodFeeId %>" />
                            <asp:HiddenField runat="server" ID="CurrencyId" Value="<%# ((PaymentMethodFee)Container.DataItem).Currency.CurrencyId %>" />
                            <%# ((PaymentMethodFee)Container.DataItem).Currency.ISOCode %>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </commerce:BulkEditGridView>
			</div>
		</div>
	</div>
	<div class="propertyPaneFooter">
		-</div>
</div>
