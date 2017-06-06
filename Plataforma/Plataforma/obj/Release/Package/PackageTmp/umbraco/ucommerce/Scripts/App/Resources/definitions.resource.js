var uc_definitions_Resource = function($http) {
	var serviceUrl = '/ucommerceapi/';

    return {
        getDefinitionGraph: function () {
        	return $http.get(serviceUrl + 'Definitions/Definitiongraph').then(function (response) {
                return response.data;
            });
        },

		saveDefinitionGraph: function(definitionGraph) {
			var url = serviceUrl + 'Definitions/UpdateDefinitionGraph';
			$http.post(
        		url,
		        {
		        	definitionGraph: definitionGraph
		        },
		        {
		        	dataType: "application/json"
		        }
	        );
		}
    };
}