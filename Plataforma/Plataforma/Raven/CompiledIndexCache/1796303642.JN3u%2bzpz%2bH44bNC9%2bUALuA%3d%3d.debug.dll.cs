using Raven.Abstractions;
using Raven.Database.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Raven.Database.Linq.PrivateExtensions;
using Lucene.Net.Documents;
using System.Globalization;
using System.Text.RegularExpressions;
using Raven.Database.Indexing;


public class Index_Products_2fByFacetsRecurse_2fen_US : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_Products_2fByFacetsRecurse_2fen_US()
	{
		this.ViewText = @"docs.Products.Select(product => new {
	                                    product.ProductDefinition,
	                                    product.ThumbnailImageUrl,
	                                    product.PrimaryImageUrl,
	                                    product.Sku,
	                                    product.VariantSku,
	                                    product.Name,
	                                    product.DisplayOnSite,
	                                    product.Weight,
	                                    product.AllowOrdering,
	                                    product.ModifiedBy,
	                                    product.ModifiedOn,
	                                    product.CreatedOn,
	                                    product.CreatedBy,
                                        product.Variants,
	                                    Rating = this.Recurse(product, x => x.Variants).Select(x => new[] {x.Rating}),
	                                    product.CategoryIds,
                                        _ = this.Recurse(product, x => x.Variants).Select(x => x.Properties[""en-US""].Select(z => (z.Value is string ? (IEnumerable<object>) new [] { z.Value } : (IEnumerable<object>) z.Value).Select(v => this.CreateField(LoadDocument(product.ProductDefinition).Fields.SingleOrDefault(field => field.Name == z.Key).Properties[""en-US""][""DisplayName""] ?? z.Key, LoadDocument(LoadDocument(product.ProductDefinition).Fields.SingleOrDefault(field => field.Name == z.Key).DataTypeId).PreValues.SingleOrDefault(preValue => preValue.Value == v).Properties[""en-US""][""DisplayName""] ?? v)).Where(vv => LoadDocument(product.ProductDefinition).Fields.SingleOrDefault(ff => ff.Name == z.Key).Properties[""invariant""][""Facet""]))),
                                        __ = this.Recurse(product, x => x.Variants).Select(x => x.Properties[""invariant""].Select(z => (z.Value is string ? (IEnumerable<object>) new [] { z.Value } : (IEnumerable<object>) z.Value).Select(v => this.CreateField(LoadDocument(product.ProductDefinition).Fields.SingleOrDefault(field => field.Name == z.Key).Properties[""en-US""][""DisplayName""] ?? z.Key, LoadDocument(LoadDocument(product.ProductDefinition).Fields.SingleOrDefault(field => field.Name == z.Key).DataTypeId).PreValues.SingleOrDefault(preValue => preValue.Value == v).Properties[""en-US""][""DisplayName""] ?? v)).Where(vv => LoadDocument(product.ProductDefinition).Fields.SingleOrDefault(ff => ff.Name == z.Key).Properties[""invariant""][""Facet""]))),
                                        ___ = this.Recurse(product, x => x.Variants).Select(x => x.Prices.Select(y => this.CreateField(y.Key, y.Value)))
                                    })";
		this.ForEntityNames.Add("Products");
		this.AddMapDefinition(docs => docs.Where(__document => string.Equals(__document["@metadata"]["Raven-Entity-Name"], "Products", System.StringComparison.InvariantCultureIgnoreCase)).Select((Func<dynamic, dynamic>)(product => new {
			product.ProductDefinition,
			product.ThumbnailImageUrl,
			product.PrimaryImageUrl,
			product.Sku,
			product.VariantSku,
			product.Name,
			product.DisplayOnSite,
			product.Weight,
			product.AllowOrdering,
			product.ModifiedBy,
			product.ModifiedOn,
			product.CreatedOn,
			product.CreatedBy,
			product.Variants,
			Rating = this.Recurse(product, (Func<dynamic, dynamic>)(x => x.Variants)).Select((Func<dynamic, dynamic>)(x => new[] {
				x.Rating
			})),
			product.CategoryIds,
			_ = this.Recurse(product, (Func<dynamic, dynamic>)(x => x.Variants)).Select((Func<dynamic, dynamic>)(x => x.Properties["en-US"].Select((Func<dynamic, dynamic>)(z => (z.Value is string ? (IEnumerable<object>)new[] {
				z.Value
			} : (IEnumerable<object>)z.Value).Select((Func<dynamic, dynamic>)(v => this.CreateField(this.__dynamic_null != LoadDocument(product.ProductDefinition).Fields.SingleOrDefault((Func<dynamic, bool>)(field => field.Name == z.Key)).Properties["en-US"]["DisplayName"] ? LoadDocument(product.ProductDefinition).Fields.SingleOrDefault((Func<dynamic, bool>)(field => field.Name == z.Key)).Properties["en-US"]["DisplayName"] : z.Key, this.__dynamic_null != LoadDocument(LoadDocument(product.ProductDefinition).Fields.SingleOrDefault((Func<dynamic, bool>)(field => field.Name == z.Key)).DataTypeId).PreValues.SingleOrDefault((Func<dynamic, bool>)(preValue => preValue.Value == v)).Properties["en-US"]["DisplayName"] ? LoadDocument(LoadDocument(product.ProductDefinition).Fields.SingleOrDefault((Func<dynamic, bool>)(field => field.Name == z.Key)).DataTypeId).PreValues.SingleOrDefault((Func<dynamic, bool>)(preValue => preValue.Value == v)).Properties["en-US"]["DisplayName"] : v))).Where((Func<dynamic, bool>)(vv => LoadDocument(product.ProductDefinition).Fields.SingleOrDefault((Func<dynamic, bool>)(ff => ff.Name == z.Key)).Properties["invariant"]["Facet"])))))),
			__ = this.Recurse(product, (Func<dynamic, dynamic>)(x => x.Variants)).Select((Func<dynamic, dynamic>)(x => x.Properties["invariant"].Select((Func<dynamic, dynamic>)(z => (z.Value is string ? (IEnumerable<object>)new[] {
				z.Value
			} : (IEnumerable<object>)z.Value).Select((Func<dynamic, dynamic>)(v => this.CreateField(this.__dynamic_null != LoadDocument(product.ProductDefinition).Fields.SingleOrDefault((Func<dynamic, bool>)(field => field.Name == z.Key)).Properties["en-US"]["DisplayName"] ? LoadDocument(product.ProductDefinition).Fields.SingleOrDefault((Func<dynamic, bool>)(field => field.Name == z.Key)).Properties["en-US"]["DisplayName"] : z.Key, this.__dynamic_null != LoadDocument(LoadDocument(product.ProductDefinition).Fields.SingleOrDefault((Func<dynamic, bool>)(field => field.Name == z.Key)).DataTypeId).PreValues.SingleOrDefault((Func<dynamic, bool>)(preValue => preValue.Value == v)).Properties["en-US"]["DisplayName"] ? LoadDocument(LoadDocument(product.ProductDefinition).Fields.SingleOrDefault((Func<dynamic, bool>)(field => field.Name == z.Key)).DataTypeId).PreValues.SingleOrDefault((Func<dynamic, bool>)(preValue => preValue.Value == v)).Properties["en-US"]["DisplayName"] : v))).Where((Func<dynamic, bool>)(vv => LoadDocument(product.ProductDefinition).Fields.SingleOrDefault((Func<dynamic, bool>)(ff => ff.Name == z.Key)).Properties["invariant"]["Facet"])))))),
			___ = this.Recurse(product, (Func<dynamic, dynamic>)(x => x.Variants)).Select((Func<dynamic, dynamic>)(x => x.Prices.Select((Func<dynamic, dynamic>)(y => this.CreateField(y.Key, y.Value))))),
			__document_id = product.__document_id
		})));
		this.AddField("Rating");
		this.AddField("_");
		this.AddField("__");
		this.AddField("___");
		this.AddField("__document_id");
		this.AddField("ProductDefinition");
		this.AddField("ThumbnailImageUrl");
		this.AddField("PrimaryImageUrl");
		this.AddField("Sku");
		this.AddField("VariantSku");
		this.AddField("Name");
		this.AddField("DisplayOnSite");
		this.AddField("Weight");
		this.AddField("AllowOrdering");
		this.AddField("ModifiedBy");
		this.AddField("ModifiedOn");
		this.AddField("CreatedOn");
		this.AddField("CreatedBy");
		this.AddField("Variants");
		this.AddField("CategoryIds");
	}
}
