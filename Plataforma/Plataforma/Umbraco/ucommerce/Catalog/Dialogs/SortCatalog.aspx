<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SortCatalog.aspx.cs" Inherits="UCommerce.Web.UI.Catalog.Dialogs.SortCatalog" MasterPageFile="../../../MasterPages/umbracoDialog.Master" %>
<asp:Content ContentPlaceHolderID="head" runat="server">

  <style type="text/css">
    #sortableFrame{height: 270px; overflow: auto; border: 1px solid #ccc;}
    #sortableNodes{padding: 4px; display: block}
    #sortableNodes thead tr th{border-bottom:1px solid #ccc; padding: 4px; padding-right: 25px;
                                background-image: url(/umbraco_client/tableSorting/img/bg.gif);     
                                cursor: pointer; 
                                font-weight: bold; 
                                background-repeat: no-repeat; 
                                background-position: center right; 
                               }
    
    #sortableNodes thead tr th.headerSortDown { 
      background-image: url(/umbraco_client/tableSorting/img/desc.gif); 
    }
     
    #sortableNodes thead tr th.headerSortUp { 
      background-image: url(/umbraco_client/tableSorting/img/asc.gif); 
    } 
    
    #sortableNodes tbody tr td{border-bottom:1px solid #efefef}
    #sortableNodes td{padding: 4px; cursor: move;}  
    tr.tDnD_whileDrag , tr.tDnD_whileDrag td{background:#dcecf3; border-color:  #a8d8eb !Important; margin-top: 20px;}
    #sortableNodes .nowrap{white-space: nowrap; } 
    </style>

  <script src="/umbraco_client/ui/jquery.js" type="text/javascript"></script>
	<link href="../../Scripts/UmbracoLegacy/submodal/subModal.css" rel="stylesheet" type="text/css"/>
</asp:Content>


<asp:Content ContentPlaceHolderID="body" runat="server">

<div id="sortArea">
<div class="propertyPane leftAligned">
  <p class="help">
    <asp:Localize runat="server" meta:resourceKey="Header" />
  </p>
  
  <div id="sortableFrame">
    <table id="sortableNodes" cellspacing="0">
      <colgroup>
	      <col/>
	      <col/>
	    </colgroup>
	    <thead>
	    <tr>
        <th style="width: 100%"><asp:Localize runat="server" meta:resourceKey="NameColumn" /></th>
        <th class="nowrap"><asp:Localize  runat="server" meta:resourceKey="SortOrderColumn" /></th>
      </tr>
      </thead>
      <tbody>
        <asp:Literal ID="lt_nodes" runat="server" />
      </tbody>
    </table>
  </div>
</div>

  <br />
  <p>
  
  
  <asp:TextBox id="NewSortOrder" CssClass="newSortOrder" runat="server"  />
  <asp:Button id="SaveButton" runat="server" OnClientClick="getNewSortOrder();" meta:resourceKey="SaveButton" onclick="SaveButton_Clicked" />
    <em> or </em><a href="#" style="color: blue"  onClick="UCommerceClientMgr.closeModalWindow();"><asp:Localize runat="server" meta:resourceKey="CancelButton" /></a>  
  </p>
</div>
   <script type="text/javascript" src="/umbraco_client/tablesorting/tableFilter.js"></script>
  <script type="text/javascript" src="/umbraco_client/tablesorting/tableDragAndDrop.js"></script>
  
   <script type="text/javascript">

       jQuery(document).ready(function() {
           jQuery('.newSortOrder').hide();
           jQuery("#sortableNodes").tablesorter();
           jQuery("#sortableNodes").tableDnD({containment: jQuery("#sortableFrame") } );
     });


     function getNewSortOrder() {
       var rows = jQuery('#sortableNodes tbody tr');
       var sortOrder = "";

       jQuery.each(rows, function() {
         sortOrder += jQuery(this).attr("id").replace("node_","") + ",";
       });
       
       jQuery('.newSortOrder').val(sortOrder);
       
      }       
  </script>
  
</asp:Content>
