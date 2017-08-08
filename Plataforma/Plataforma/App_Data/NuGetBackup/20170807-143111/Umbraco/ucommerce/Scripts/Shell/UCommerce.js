﻿$(document).ready(function () {
	wireScript();
});

function wireScript() {
	window.queryString = queryString();
	window.isNewEntity = window.queryString['id'] == null;

	if (window.isNewEntity) {
		$('.entityName').keyup(function () {
			var val = $(this).val();
			$('.multiLingualDisplayName').val(val);
		});
	}
}

function queryString() {
	var result = {}, queryString = location.search.slice(1),
		re = /([^&=]+)=([^&]*)/g, m;

	while (m = re.exec(queryString)) {
		result[decodeURIComponent(m[1])] = decodeURIComponent(m[2]);
	}

	return result;
}


// Using "parent" object to reference the main page, that hosts this iframe.

var UCommerceClientMgr = {
    BaseUCommerceUrl: "/sitecore modules/Shell/ucommerce/",
    Shell: "Sitecore",

    generateRandom: function () {
        return Math.random() * Math.pow(100, 10);
    },

    openModal: function (url, name, width, height) {
        var r = this.generateRandom();
        if (url.indexOf("?") > -1) {
            url += "&rndo=" + r;
        }
        else {
            url += "?rndo=" + r;
        }
        parent.openModal(url, name, width, height);
    },

    getCurrentNodeId: function () {
        return parent.getCurrentNodeId();
    },

    contentFrame: function (url) {
        parent.setTimeout(function () {
            parent.redirectToUrl(url);
        }, 200);
    },

    refreshTree: function () {
        parent.updateSelectedNodeSiblings();
    },
    refreshChildren: function () {
        if (parent) {
            parent.updateSelectedNodeChildren();
        }
    },
    
    refreshChildrenFor: function (keyVal) {
		parent.reloadNodes(keyVal);
	},

    updateCurrentNodeText: function (nodeText) {
        parent.updateSelectedNodeText(nodeText);
    },

    childNodeCreated: function () {
        UmbClientMgr.mainTree().childNodeCreated();
    },

    // For instance nodeId can be Category_67
    findAndSelectNode: function (nodeId) {
        parent.findAndSelectNode(nodeId);
    },

    closeModalWindow: function (url) {
        parent.closeModal();
    },

    showSpeechBubble: function (icon, header, body) {
    	parent.addAlert(icon, header, body);
    	if (window.addAlert) {
    		window.addAlert(icon, header, body);
    	}
    },

    setFocusToTreeNode: function (treeNode) {
        UmbClientMgr.mainTree().selectNode(node, false, true);
    },

    showPopupWindow: function (url, width, height, saveFunction) {
        UmbClientMgr.openModalWindow(this._itemPickerUrl, this._label, this._showHeader, this._width, this._height, 60, 0, ['#cancelbutton'], function (e) { _this.SaveSelection(e); });
    }
};

function sortCategoryThis() {
    UCommerceClientMgr.openModal('ucommerce/catalog/dialogs/SortCatalog.aspx?id=' + UCommerceClientMgr.getCurrentNodeId(), 'Sort', 600, 450);
};

function sortProductsThis() {
    UCommerceClientMgr.openModal('ucommerce/catalog/dialogs/SortCatalog.aspx?id=' + UCommerceClientMgr.getCurrentNodeId() + '&products=1', 'Sort', 600, 450);
};