using Grand.Core;
using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Directory;
using Grand.Core.Domain.CouponsModule;
using Grand.Core.Domain.Stores;
using Grand.Core.Domain.Vendors;
using Grand.Framework.Extensions;
using Grand.Services.Catalog;
using Grand.Services.Directory;
using Grand.Services.CouponsModule;
using Grand.Services.Helpers;
using Grand.Services.Localization;
using Grand.Services.Logging;
using Grand.Services.Orders;
using Grand.Services.Stores;
using Grand.Services.Vendors;
using Grand.Web.Areas.Admin.Extensions;
using Grand.Web.Areas.Admin.Interfaces;
using Grand.Web.Areas.Admin.Models.Catalog;
using Grand.Web.Areas.Admin.Models.CouponsModule;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Web.Areas.Admin.Services
{
    public partial class CouponViewModelService: ICouponViewModelService
    {
        #region Fields

        private readonly ICouponService _couponService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ICurrencyService _currencyService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly CurrencySettings _currencySettings;
        private readonly IWorkContext _workContext;
        private readonly IManufacturerService _manufacturerService;
        private readonly IStoreService _storeService;
        private readonly IVendorService _vendorService;
        private readonly IOrderService _orderService;
        private readonly IPriceFormatter _priceFormatter;

        #endregion

        #region Constructors

        public CouponViewModelService(ICouponService couponService,
            ILocalizationService localizationService,
            ICurrencyService currencyService,
            ICategoryService categoryService,
            IProductService productService,
            IWebHelper webHelper,
            ICustomerActivityService customerActivityService,
            CurrencySettings currencySettings,
            IWorkContext workContext,
            IManufacturerService manufacturerService,
            IStoreService storeService,
            IVendorService vendorService,
            IOrderService orderService,
            IPriceFormatter priceFormatter)
        {
            _couponService = couponService;
            _localizationService = localizationService;
            _currencyService = currencyService;
            _categoryService = categoryService;
            _productService = productService;
            _webHelper = webHelper;
            _customerActivityService = customerActivityService;
            _currencySettings = currencySettings;
            _workContext = workContext;
            _manufacturerService = manufacturerService;
            _storeService = storeService;
            _vendorService = vendorService;
            _orderService = orderService;
            _priceFormatter = priceFormatter;
        }

        #endregion

        public virtual CouponListModel PrepareCouponListModel()
        {
            var model = new CouponListModel
            {
                AvailableCouponTypes = CouponType.AssignedToOrderTotal.ToSelectList(false).ToList()
            };
            model.AvailableCouponTypes.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = "" });
            return model;
        }
        public virtual (IEnumerable<CouponModel> couponModel, int totalCount) PrepareCouponModel(CouponListModel model, int pageIndex, int pageSize)
        {
            CouponType? couponType = null;
            if (model.SearchCouponTypeId > 0)
                couponType = (CouponType)model.SearchCouponTypeId;
            var coupons = _couponService.GetAllCoupons(couponType,
                model.SearchCouponCode,
                model.SearchCouponName,
                true);

            return (coupons.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x =>
                {
                    var couponModel = x.ToModel();
                    couponModel.CouponTypeName = x.CouponType.GetLocalizedEnum(_localizationService, _workContext);
                    couponModel.PrimaryStoreCurrencyCode = x.CalculateByPlugin ? "" : _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
                    couponModel.TimesUsed = _couponService.GetAllCouponUsageHistory(x.Id, pageSize: 1).TotalCount;
                    return couponModel;
                }), coupons.Count);
        }
        public virtual void PrepareCouponModel(CouponModel model, Coupon coupon)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
            model.AvailableCouponRequirementRules.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Promotions.Discounts.Requirements.DiscountRequirementType.Select"), Value = "" });
            var couponPlugins = _couponService.LoadAllCouponPlugins();
            foreach (var couponPlugin in couponPlugins)
                foreach (var couponRule in couponPlugin.GetRequirementRules())
                    model.AvailableCouponRequirementRules.Add(new SelectListItem { Text = couponRule.FriendlyName, Value = couponRule.SystemName });

            //discount amount providers
            foreach (var item in _couponService.LoadCouponAmountProviders())
            {
                model.AvailableCouponAmountProviders.Add(new SelectListItem() { Value = item.PluginDescriptor.SystemName, Text = item.PluginDescriptor.FriendlyName });
            }

            if (coupon != null)
            {
                //requirements
                foreach (var dr in coupon.CouponRequirements.OrderBy(dr => dr.Id))
                {
                    var couponPlugin = _couponService.LoadCouponPluginBySystemName(dr.CouponRequirementRuleSystemName);
                    var couponRequirement = couponPlugin.GetRequirementRules().Single(x => x.SystemName == dr.CouponRequirementRuleSystemName);
                    {
                        if (couponPlugin != null)
                        {
                            model.CouponRequirementMetaInfos.Add(new CouponModel.CouponRequirementMetaInfo
                            {
                                CouponRequirementId = dr.Id,
                                RuleName = couponRequirement.FriendlyName,
                                ConfigurationUrl = GetRequirementUrlInternal(couponRequirement, coupon, dr.Id)
                            });
                        }
                    }
                }
            }
            else
                model.IsEnabled = true;
        }
        public virtual Coupon InsertCouponModel(CouponModel model)
        {
            var coupon = model.ToEntity();
            _couponService.InsertCoupon(coupon);

            //activity log
            _customerActivityService.InsertActivity("AddNewDiscount", coupon.Id, _localizationService.GetResource("ActivityLog.AddNewCoupon"), coupon.Name);
            return coupon;
        }

        public virtual Coupon UpdateCouponModel(Coupon coupon, CouponModel model)
        {
            var prevCouponType = coupon.CouponType;
            coupon = model.ToEntity(coupon);
            _couponService.UpdateCoupon(coupon);

            //clean up old references (if changed) and update "HasDiscountsApplied" properties
            if (prevCouponType == CouponType.AssignedToCategories
                && coupon.CouponType != CouponType.AssignedToCategories)
            {
                //applied to categories
                //_categoryService.
                var categories = _categoryService.GetAllCategoriesByCoupon(coupon.Id);

                //update "HasDiscountsApplied" property
                foreach (var category in categories)
                {
                    var item = category.AppliedCoupons.Where(x => x == coupon.Id).FirstOrDefault();
                    category.AppliedCoupons.Remove(item);
                }
            }
            if (prevCouponType == CouponType.AssignedToManufacturers
                && coupon.CouponType != CouponType.AssignedToManufacturers)
            {
                //applied to manufacturers
                var manufacturers = _manufacturerService.GetAllManufacturersByCoupon(coupon.Id);
                foreach (var manufacturer in manufacturers)
                {
                    var item = manufacturer.AppliedCoupons.Where(x => x == coupon.Id).FirstOrDefault();
                    manufacturer.AppliedCoupons.Remove(item);
                }
            }
            if (prevCouponType == CouponType.AssignedToSkus
                && coupon.CouponType != CouponType.AssignedToSkus)
            {
                //applied to products
                var products = _productService.GetProductsByCoupon(coupon.Id);

                foreach (var p in products)
                {
                    var item = p.AppliedCoupons.Where(x => x == coupon.Id).FirstOrDefault();
                    p.AppliedCoupons.Remove(item);
                    _productService.DeleteCoupon(item, p.Id);
                }
            }

            //activity log
            _customerActivityService.InsertActivity("EditCoupon", coupon.Id, _localizationService.GetResource("ActivityLog.EditCoupon"), coupon.Name);
            return coupon;

        }
        public virtual void DeleteCoupon(Coupon coupon)
        {
            _couponService.DeleteCoupon(coupon);
            //activity log
            _customerActivityService.InsertActivity("DeleteCoupon", coupon.Id, _localizationService.GetResource("ActivityLog.DeleteCoupon"), coupon.Name);
        }
        public virtual void InsertCouponCode(string couponId, string couponCode)
        {
            var coupon = new CouponTicket
            {
                CouponCode = couponCode.ToUpper(),
                CouponId = couponId
            };
            _couponService.InsertCouponTicket(coupon);
        }
        public virtual string GetRequirementUrlInternal(ICouponRequirementRule couponRequirementRule, Coupon coupon, string couponRequirementId)
        {
            if (couponRequirementRule == null)
                throw new ArgumentNullException("couponRequirementRule");

            if (coupon == null)
                throw new ArgumentNullException("coupon");

            string url = string.Format("{0}{1}", _webHelper.GetStoreLocation(), couponRequirementRule.GetConfigurationUrl(coupon.Id, couponRequirementId));
            return url;
        }
        public void DeleteCouponRequirement(CouponRequirement couponRequirement, Coupon coupon)
        {
            _couponService.DeleteCouponRequirement(couponRequirement);
            coupon.CouponRequirements.Remove(couponRequirement);
            _couponService.UpdateCoupon(coupon);
        }
        public virtual CouponModel.AddProductToCouponModel PrepareProductToCouponModel()
        {
            var model = new CouponModel.AddProductToCouponModel();
            //categories
            model.AvailableCategories.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = " " });
            var categories = _categoryService.GetAllCategories(showHidden: true);
            foreach (var c in categories)
                model.AvailableCategories.Add(new SelectListItem { Text = c.GetFormattedBreadCrumb(categories), Value = c.Id.ToString() });

            //manufacturers
            model.AvailableManufacturers.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = " " });
            foreach (var m in _manufacturerService.GetAllManufacturers(showHidden: true))
                model.AvailableManufacturers.Add(new SelectListItem { Text = m.Name, Value = m.Id.ToString() });

            //stores
            model.AvailableStores.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = " " });
            foreach (var s in _storeService.GetAllStores())
                model.AvailableStores.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString() });

            //vendors
            model.AvailableVendors.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = " " });
            foreach (var v in _vendorService.GetAllVendors(showHidden: true))
                model.AvailableVendors.Add(new SelectListItem { Text = v.Name, Value = v.Id.ToString() });

            //product types
            model.AvailableProductTypes = ProductType.SimpleProduct.ToSelectList(false).ToList();
            model.AvailableProductTypes.Insert(0, new SelectListItem { Text = _localizationService.GetResource("Admin.Common.All"), Value = " " });
            return model;
        }
        public virtual (IList<ProductModel> products, int totalCount) PrepareProductModel(CouponModel.AddProductToCouponModel model, int pageIndex, int pageSize)
        {
            var products = _productService.PrepareProductList(model.SearchCategoryId, model.SearchManufacturerId, model.SearchStoreId, model.SearchVendorId, model.SearchProductTypeId, model.SearchProductName, pageIndex, pageSize);
            return (products.Select(x => x.ToModel()).ToList(), products.TotalCount);
        }
        public virtual void InsertProductToCouponModel(CouponModel.AddProductToCouponModel model)
        {
            foreach (string id in model.SelectedProductIds)
            {
                var product = _productService.GetProductById(id);
                if (product != null)
                {
                    if (product.AppliedCoupons.Count(d => d == model.CouponId) == 0)
                    {
                        product.AppliedCoupons.Add(model.CouponId);
                        _productService.InsertCoupon(model.CouponId, product.Id);
                    }
                }
            }
        }
        public virtual void DeleteProduct(Coupon coupon, Product product)
        {
            //remove discount
            if (product.AppliedCoupons.Count(d => d == coupon.Id) > 0)
            {
                product.AppliedCoupons.Remove(coupon.Id);
                _productService.DeleteCoupon(coupon.Id, product.Id);
            }
        }
        public virtual void DeleteCategory(Coupon coupon, Category category)
        {
            //remove discount
            if (category.AppliedCoupons.Count(d => d == coupon.Id) > 0)
                category.AppliedCoupons.Remove(coupon.Id);

            _categoryService.UpdateCategory(category);
        }
        public virtual void InsertCategoryToCouponModel(CouponModel.AddCategoryToCouponModel model)
        {
            foreach (string id in model.SelectedCategoryIds)
            {
                var category = _categoryService.GetCategoryById(id);
                if (category != null)
                {
                    if (category.AppliedCoupons.Count(d => d == model.CouponId) == 0)
                        category.AppliedCoupons.Add(model.CouponId);

                    _categoryService.UpdateCategory(category);
                }
            }
        }
        public virtual void DeleteManufacturer(Coupon coupon, Manufacturer manufacturer)
        {
            //remove discount
            if (manufacturer.AppliedCoupons.Count(d => d == coupon.Id) > 0)
                manufacturer.AppliedCoupons.Remove(coupon.Id);

            _manufacturerService.UpdateManufacturer(manufacturer);
        }
        public virtual void InsertManufacturerToCouponModel(CouponModel.AddManufacturerToCouponModel model)
        {
            foreach (string id in model.SelectedManufacturerIds)
            {
                var manufacturer = _manufacturerService.GetManufacturerById(id);
                if (manufacturer != null)
                {
                    if (manufacturer.AppliedCoupons.Count(d => d == model.CouponId) == 0)
                        manufacturer.AppliedCoupons.Add(model.CouponId);

                    _manufacturerService.UpdateManufacturer(manufacturer);
                }
            }
        }
        public virtual void DeleteVendor(Coupon coupon, Vendor vendor)
        {
            //remove discount
            if (vendor.AppliedCoupons.Count(d => d == coupon.Id) > 0)
                vendor.AppliedCoupons.Remove(coupon.Id);

            _vendorService.UpdateVendor(vendor);
        }
        public virtual void InsertVendorToCouponModel(CouponModel.AddVendorToCouponModel model)
        {
            foreach (string id in model.SelectedVendorIds)
            {
                var vendor = _vendorService.GetVendorById(id);
                if (vendor != null)
                {
                    if (vendor.AppliedCoupons.Count(d => d == model.CouponId) == 0)
                        vendor.AppliedCoupons.Add(model.CouponId);

                    _vendorService.UpdateVendor(vendor);
                }
            }
        }
        public virtual void DeleteStore(Coupon coupon, Store store)
        {
            //remove discount
            if (store.AppliedCoupons.Count(d => d == coupon.Id) > 0)
                store.AppliedCoupons.Remove(coupon.Id);

            _storeService.UpdateStore(store);
        }
        public virtual void InsertStoreToCouponModel(CouponModel.AddStoreToCouponModel model)
        {
            foreach (string id in model.SelectedStoreIds)
            {
                var store = _storeService.GetStoreById(id);
                if (store != null)
                {
                    if (store.AppliedCoupons.Count(d => d == model.CouponId) == 0)
                        store.AppliedCoupons.Add(model.CouponId);

                    _storeService.UpdateStore(store);
                }
            }
        }
        public (IEnumerable<CouponModel.CouponUsageHistoryModel> usageHistoryModels, int totalCount) PrepareCouponUsageHistoryModel(Coupon coupon, int pageIndex, int pageSize)
        {
            var dateTimeHelper = Grand.Core.Infrastructure.EngineContext.Current.Resolve<IDateTimeHelper>();
            var duh = _couponService.GetAllCouponUsageHistory(coupon.Id, null, null, null, pageIndex - 1, pageSize);
            return (duh.Select(x =>
            {
                var order = _orderService.GetOrderById(x.OrderId);
                var duhModel = new CouponModel.CouponUsageHistoryModel
                {
                    Id = x.Id,
                    CouponId = x.CouponId,
                    OrderId = x.OrderId,
                    OrderNumber = order != null ? order.OrderNumber : 0,
                    OrderTotal = order != null ? _priceFormatter.FormatPrice(order.OrderTotal, true, false) : "",
                    CreatedOn = dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc)
                };
                return duhModel;
            }), duh.TotalCount);
        }
    }
}
