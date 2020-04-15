using FluentValidation.Attributes;
using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using Grand.Web.Areas.Admin.Validators.Coupons;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grand.Web.Areas.Admin.Models.CouponsModule
{
    [Validator(typeof(CouponValidator))]
    public partial class CouponModel : BaseGrandEntityModel
    {
        public CouponModel()
        {
            AvailableCouponRequirementRules = new List<SelectListItem>();
            CouponRequirementMetaInfos = new List<CouponRequirementMetaInfo>();
            AvailableCouponAmountProviders = new List<SelectListItem>();
        }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.Name")]
        public string Name { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.CouponType")]
        public int CouponTypeId { get; set; }
        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.CouponType")]
        public string CouponTypeName { get; set; }

        //used for the list page
        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.TimesUsed")]
        public int TimesUsed { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.UsePercentage")]
        public bool UsePercentage { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.CouponPercentage")]
        public decimal CouponPercentage { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.CouponAmount")]
        public decimal CouponAmount { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.CalculateByPlugin")]
        public bool CalculateByPlugin { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.CouponPluginName")]
        public string CouponPluginName { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.MaximumCouponAmount")]
        [UIHint("DecimalNullable")]
        public decimal? MaximumCouponAmount { get; set; }

        public string PrimaryStoreCurrencyCode { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.StartDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDate { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.EndDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDate { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.RequiresCouponCode")]
        public bool RequiresCouponCode { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.Reused")]
        public bool Reused { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.IsCumulative")]
        public bool IsCumulative { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.CouponLimitation")]
        public int CouponLimitationId { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.LimitationTimes")]
        public int LimitationTimes { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.MaximumCouponedQuantity")]
        [UIHint("Int32Nullable")]
        public int? MaximumCouponedQuantity { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Requirements.CouponRequirementType")]
        public string AddCouponRequirement { get; set; }
        public IList<SelectListItem> AvailableCouponRequirementRules { get; set; }
        public IList<CouponRequirementMetaInfo> CouponRequirementMetaInfos { get; set; }
        public IList<SelectListItem> AvailableCouponAmountProviders { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.Fields.IsEnabled")]
        public bool IsEnabled { get; set; }

        #region Nested classes

        public partial class CouponRequirementMetaInfo : BaseGrandModel
        {
            public string CouponRequirementId { get; set; }
            public string RuleName { get; set; }
            public string ConfigurationUrl { get; set; }
        }

        public partial class CouponUsageHistoryModel : BaseGrandEntityModel
        {
            public string CouponId { get; set; }

            [GrandResourceDisplayName("Admin.Promotions.Coupons.History.Order")]
            public string OrderId { get; set; }
            public int OrderNumber { get; set; }

            [GrandResourceDisplayName("Admin.Promotions.Coupons.History.OrderTotal")]
            public string OrderTotal { get; set; }

            [GrandResourceDisplayName("Admin.Promotions.Coupons.History.CreatedOn")]
            public DateTime CreatedOn { get; set; }
        }

        public partial class AppliedToCategoryModel : BaseGrandModel
        {
            public string CategoryId { get; set; }

            public string CategoryName { get; set; }
        }
        public partial class AddCategoryToCouponModel : BaseGrandModel
        {
            [GrandResourceDisplayName("Admin.Catalog.Categories.List.SearchCategoryName")]
            
            public string SearchCategoryName { get; set; }

            public string CouponId { get; set; }

            public string[] SelectedCategoryIds { get; set; }
        }


        public partial class AppliedToManufacturerModel : BaseGrandModel
        {
            public string ManufacturerId { get; set; }

            public string ManufacturerName { get; set; }
        }
        public partial class AddManufacturerToCouponModel : BaseGrandModel
        {
            [GrandResourceDisplayName("Admin.Catalog.Manufacturers.List.SearchManufacturerName")]
            
            public string SearchManufacturerName { get; set; }

            public string CouponId { get; set; }

            public string[] SelectedManufacturerIds { get; set; }
        }


        public partial class AppliedToProductModel : BaseGrandModel
        {
            public string ProductId { get; set; }

            public string ProductName { get; set; }
        }
        public partial class AddProductToCouponModel : BaseGrandModel
        {
            public AddProductToCouponModel()
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

            public string CouponId { get; set; }

            public string[] SelectedProductIds { get; set; }
        }

        public partial class AppliedToVendorModel : BaseGrandModel
        {
            public string VendorId { get; set; }

            public string VendorName { get; set; }
        }
        public partial class AddVendorToCouponModel : BaseGrandModel
        {
            [GrandResourceDisplayName("Admin.Catalog.Vendors.List.SearchVendorName")]
            
            public string SearchVendorName { get; set; }

            [GrandResourceDisplayName("Admin.Catalog.Vendors.List.SearchVendorEmail")]
            
            public string SearchVendorEmail { get; set; }

            public string CouponId { get; set; }

            public string[] SelectedVendorIds { get; set; }
        }


        public partial class AppliedToStoreModel : BaseGrandModel
        {
            public string StoreId { get; set; }

            public string StoreName { get; set; }
        }
        public partial class AddStoreToCouponModel : BaseGrandModel
        {
            [GrandResourceDisplayName("Admin.Catalog.Stores.List.SearchStoreName")]
            
            public string SearchStoreName { get; set; }

            public string CouponId { get; set; }

            public string[] SelectedStoreIds { get; set; }
        }

        #endregion
    }
}