using Grand.Core;
using Grand.Core.Caching;
using Grand.Core.Domain.CouponsModule;
using Grand.Services.CouponsModule;
using Grand.Services.Localization;
using Grand.Services.Security;
using Grand.Web.Interfaces;
using Grand.Web.Models.Coupons;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Web.Services
{
    public partial class CouponViewModelService : ICouponViewModelService
    {
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ILocalizationService _localizationService;
        private readonly ICacheManager _cacheManager;
        private readonly ICouponService _couponService;
  

        public CouponViewModelService(
            IPermissionService permissionService, 
            IWorkContext workContext, 
            IStoreContext storeContext,
            ILocalizationService localizationService, 
            ICacheManager cacheManager, 
            ICouponService couponService)
        {
            this._permissionService = permissionService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._localizationService = localizationService;
            this._cacheManager = cacheManager;
            this._couponService = couponService;
        }

        #region coupon

        public virtual CouponModel PrepareCoupon(Coupon coupon)
        {
            var model = new CouponModel
            {
                Id = coupon.Id,
                Name = coupon.Name,
                StartTime = coupon.StartDateUtc,
                EndTime = coupon.EndDateUtc,
                CouponPercentage = coupon.CouponPercentage
            };

            return model;
        }

        public virtual List<CouponModel> PrepareHomePageCoupon()
        {
            CouponType? couponType = null;

            var AllCoupon = _couponService.GetAllCoupons(couponType)
                .Select(x => PrepareCoupon(x))
                .ToList();

            var model = new List<CouponModel>();
            
            foreach (var mod in AllCoupon)
            {
                var EachCoupon = (CouponModel)mod.Clone();
              
                model.Add(EachCoupon);
            }

            return model;
        }

        #endregion

    }
}