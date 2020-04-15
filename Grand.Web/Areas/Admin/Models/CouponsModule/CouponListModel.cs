using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Grand.Web.Areas.Admin.Models.CouponsModule
{
    public partial class CouponListModel : BaseGrandModel
    {
        public CouponListModel()
        {
            AvailableCouponTypes = new List<SelectListItem>();
        }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.List.SearchDiscountCouponCode")]
        
        public string SearchCouponCode { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.List.SearchDiscountName")]
        
        public string SearchCouponName { get; set; }

        [GrandResourceDisplayName("Admin.Promotions.Coupons.List.SearchDiscountType")]
        public int SearchCouponTypeId { get; set; }
        public IList<SelectListItem> AvailableCouponTypes { get; set; }
    }
}