using FluentValidation.Attributes;
using Grand.Framework.Localization;
using Grand.Framework.Mapping;
using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using Grand.Web.Areas.Admin.Validators.FlashSales;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Grand.Web.Areas.Admin.Models.FlashSales
{
    [Validator(typeof(FlashSaleValidator))]
    public class FlashSaleModel : BaseGrandEntityModel, ILocalizedModel<FlashSaleModel.FlashSaleLocalizedModel>, IAclMappingModel, IStoreMappingModel
    {
        public FlashSaleModel()
        {
            AvailableStores = new List<StoreModel>();
            Locales = new List<FlashSaleLocalizedModel>();
            AvailableCustomerRoles = new List<CustomerRoleModel>();
        }

        [GrandResourceDisplayName("Admin.Promotions.FlashSales.Fields.Name")]
        public string Name { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.FlashSales.Fields.StartDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDateUtc { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.FlashSales.Fields.EndDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDateUtc { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.FlashSales.Fields.IsEnabled")]
        public bool IsEnabled { get; set; }

        //Store mapping
        [GrandResourceDisplayName("Admin.ContentManagement.FlashSales.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [GrandResourceDisplayName("Admin.ContentManagement.FlashSales.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }
        public IList<FlashSaleLocalizedModel> Locales { get; set; }

        //ACL
        [GrandResourceDisplayName("Admin.ContentManagement.FlashSales.Fields.SubjectToAcl")]
        public bool SubjectToAcl { get; set; }
        [GrandResourceDisplayName("Admin.ContentManagement.FlashSales.Fields.AclCustomerRoles")]
        public List<CustomerRoleModel> AvailableCustomerRoles { get; set; }
        public string[] SelectedCustomerRoleIds { get; set; }
 
        //localized
        public partial class FlashSaleLocalizedModel : ILocalizedModelLocal
        {
            public string LanguageId { get; set; }

            [GrandResourceDisplayName("Admin.ContentManagement.FlashSales.Fields.Name")]
            public string Name { get; set; }

        }

        //Product
        public partial class SelectedToProductModel : BaseGrandModel
        {
            public string ProductId { get; set; }

            public string ProductName { get; set; }
        }

        //search list
        public partial class AddProductToFlashSaleModel : BaseGrandModel
        {
            public AddProductToFlashSaleModel()
            {
                AvailableCategories = new List<SelectListItem>();
                AvailableManufacturers = new List<SelectListItem>();
                AvailableStores = new List<SelectListItem>();
                AvailableVendors = new List<SelectListItem>();
                AvailableProductTypes = new List<SelectListItem>();
            }

            [GrandResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]

            public string SearchProductName { get; set; }
            [GrandResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
            public string SearchCategoryId { get; set; }
            [GrandResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
            public string SearchManufacturerId { get; set; }
            [GrandResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
            public string SearchStoreId { get; set; }
            [GrandResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
            public string SearchVendorId { get; set; }
            [GrandResourceDisplayName("Admin.Catalog.Products.List.SearchProductType")]
            public int SearchProductTypeId { get; set; }

            public IList<SelectListItem> AvailableCategories { get; set; }
            public IList<SelectListItem> AvailableManufacturers { get; set; }
            public IList<SelectListItem> AvailableStores { get; set; }
            public IList<SelectListItem> AvailableVendors { get; set; }
            public IList<SelectListItem> AvailableProductTypes { get; set; }

            public string FlashSaleId { get; set; }
            public string[] SelectedProductIds { get; set; }


        }
    }
}
