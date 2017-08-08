function uc_localization_resource($http) {
	var serviceUrl = '/ucommerceApi/Localization/TranslatedStrings';// + top.location.search;

	return {
		getTranslatedStrings: function (ressource) {
			return $http.post(
				serviceUrl,
				{
					RessourceName: ressource
				},
				{
					dataType: "application/json"
				}
			);
		}
	};
}

