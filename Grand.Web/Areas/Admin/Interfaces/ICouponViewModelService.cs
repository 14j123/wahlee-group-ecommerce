using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.CouponsModule;
using Grand.Core.Domain.Stores;
using Grand.Core.Domain.Vendors;
using Grand.Services.CouponsModule;
using Grand.Web.Areas.Admin.Models.Catalog;
using Grand.Web.Areas.Admin.Models.CouponsModule;
using System.Collections.Generic;

namespace Grand.Web.Areas.Admin.Interfaces
{
    public interface ICouponViewModelService
    {
        CouponListModel PrepareCouponListModel();
        (IEnumerable<CouponModel> couponModel, int totalCount) PrepareCouponModel(CouponListModel model, int pageIndex, int pageSize);
        void PrepareCouponModel(CouponModel model, Coupon coupon);
        Coupon InsertCouponModel(CouponModel model);
        Coupon UpdateCouponModel(Coupon coupon, CouponModel model);
        void DeleteCoupon(Coupon coupon);
        void InsertCouponCode(string couponId, string couponCode);
        string GetRequirementUrlInternal(ICouponRequirementRule couponRequirementRule, Coupon coupon, string couponRequirementId);
        void DeleteCouponRequirement(CouponRequirement couponRequirement, Coupon coupon);
        CouponModel.AddProductToCouponModel PrepareProductToCouponModel();
        (IList<ProductModel> products, int totalCount) PrepareProductModel(CouponModel.AddProductToCouponModel model, int pageIndex, int pageSize);
        void InsertProductToCouponModel(CouponModel.AddProductToCouponModel model);
        void DeleteProduct(Coupon coupon, Product product);
        void DeleteCategory(Coupon coupon, Category category);
        void DeleteVendor(Coupon coupon, Vendor vendor);
        void DeleteManufacturer(Coupon coupon, Manufacturer manufacturer);
        void DeleteStore(Coupon coupon, Store store);
        void InsertCategoryToCouponModel(CouponModel.AddCategoryToCouponModel model);
        void InsertManufacturerToCouponModel(CouponModel.AddManufacturerToCouponModel model);
        void InsertVendorToCouponModel(CouponModel.AddVendorToCouponModel model);
        void InsertStoreToCouponModel(CouponModel.AddStoreToCouponModel model);
        (IEnumerable<CouponModel.CouponUsageHistoryModel> usageHistoryModels, int totalCount) PrepareCouponUsageHistoryModel(Coupon coupon, int pageIndex, int pageSize);

    }
}
