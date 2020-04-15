using Grand.Framework.Components;
using Grand.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Grand.Web.Components
{
    public class HomePageTrendingProductsViewComponent : BaseViewComponent
    {
        #region Fields
        private readonly IProductViewModelService _productViewModelService;
        #endregion

        #region Constructors

        public HomePageTrendingProductsViewComponent(
            IProductViewModelService productViewModelService)
        {
            this._productViewModelService = productViewModelService;
        }

        #endregion

        #region Invoker

        public async Task<IViewComponentResult> InvokeAsync(int? productThumbPictureSize)
        {
            var model = await Task.Run(() => _productViewModelService.PrepareProductsDisplayedOnHomePageTrending(productThumbPictureSize));
            if (!model.Any())
                return Content("");

            return View("default",model);
        }

        #endregion

    }
}
