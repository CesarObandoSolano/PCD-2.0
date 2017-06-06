var uc_definitions_service = function (uCommerceDefinitionsResource) {
	return {
		getDefinitionGraph: function () {
			return uCommerceDefinitionsResource.getDefinitionGraph();
		},

		saveDefinitionGraph: function (definitionGraph) {
			return uCommerceDefinitionsResource.saveDefinitionGraph(definitionGraph);
		}
	};
}