using Grand.Core.Domain.CouponsModule;
using Grand.Framework.Components;
using Grand.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Grand.Web.Components
{
    public class HomePageCouponListShowViewComponent : BaseViewComponent
    {
        #region Fields
        private readonly ICouponViewModelService _couponViewModelService;

        #endregion

        #region Constructors

        public HomePageCouponListShowViewComponent(

            ICouponViewModelService couponViewModelService

            )
        {
            this._couponViewModelService = couponViewModelService;
        }

        #endregion

        #region Invoker

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var model = await Task.Run(() => _couponViewModelService.PrepareHomePageCoupon());
            if (!model.Any())
                return Content("");

            return View("default", model);
        }

        #endregion

    }
}
