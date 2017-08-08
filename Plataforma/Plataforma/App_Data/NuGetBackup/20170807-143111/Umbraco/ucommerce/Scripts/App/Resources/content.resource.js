var uc_content_resource = function($http) {
	var serviceUrl = '/ucommerceapi/';

	return {
		getImageUrl: function(id) {
		    return $http.get(serviceUrl + 'Content/ImageUrl/' + id + top.location.search).then(function (response) {
				return response.data;
			});
		},
		
		getRootNode: function(contentType) {
		    return $http.get(serviceUrl + 'Content/' + contentType + '/RootNode' + top.location.search).then(function (response) {
		    	var data = response.data;
		    	return data;
			});
		},
		
		getChildren: function (contentType, nodeType, nodeId) {
		    return $http.get(serviceUrl + 'Content/' + contentType + '/Children/' + nodeType + '/' + nodeId + top.location.search).then(function (response) {
		    	var data = response.data;
		    	return data;
			});
		},

		getNodes: function (nodeTypes, nodeIds)
		{
			var formData = {
				'PreSelectedValues': nodeIds,
				'NodeTypes': nodeTypes
			}

			return $http.post(
				serviceUrl + 'Content/Nodes',
				formData,
				{
					dataType: "application/json"
				}
			);
		}
	};

}