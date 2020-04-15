using Grand.Core.Domain.CouponsModule;
using Grand.Web.Models.Coupons;
using System.Collections.Generic;

namespace Grand.Web.Interfaces
{
    public partial interface ICouponViewModelService
    {
        List<CouponModel> PrepareHomePageCoupon();

    }
}