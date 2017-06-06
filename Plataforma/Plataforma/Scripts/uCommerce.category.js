﻿(function () {
    var categoryId = $('#category-id').val();
    var catalogId = $('#catalog-id').val();
    var i = 0;
    var prices = document.querySelectorAll('[data-productsku]');

    for (i ; i < prices.length; i++) {
        var currentPriceElement = prices[i];
        var currentSku = currentPriceElement.dataset.productsku;
        var currentJQelement = $(prices[i]);

        currentJQelement.prepend('<img src="/img/loader.gif" class="product-spinner">');

        $.uCommerce.getProductInformation({
            CatalogId: parseInt(catalogId),
            CategoryId: parseInt(categoryId),
            Sku: currentSku
        },
            function (data) {
                if (data.PriceCalculation.IsDiscounted) {
                    document.querySelector("p[data-productsku='" + data.Sku + "']").innerHTML = data.PriceCalculation.ListPrice.Amount.DisplayValue;
                    $('#' + data.Sku).find('.item-price').addClass('strike-through-price');
                    $('<p class="item-discounted-price">' + data.PriceCalculation.YourPrice.Amount.DisplayValue + '</p>').insertAfter($('#' + data.Sku).find('.item-price'));
                } else {
                    document.querySelector("p[data-productsku='" + data.Sku + "']").innerHTML = data.PriceCalculation.YourPrice.Amount.DisplayValue;
                }

                var product = $('#' + data.Sku);

                var productimage = product.find('.product-image');
                var productName = product.find('.item-name').find('a');
                var productButton = product.find('.view-details').find('a');

                productimage.click(function () {
                    window.location = data.NiceUrl;
                });

                productimage.css('cursor', 'pointer');
                productName.attr('href', data.NiceUrl);
                productButton.attr('href', data.NiceUrl);
            });
    }
})()