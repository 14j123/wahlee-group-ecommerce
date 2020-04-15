using Grand.Core.Domain.FlashSales;
using Grand.Framework.Components;
using Grand.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Grand.Web.Components
{
    public class HomePageCurrentFlashSaleProductsViewComponent : BaseViewComponent
    {
        #region Fields
        private readonly IProductViewModelService _productViewModelService;
        //private readonly FlashSale _flashSale;
        #endregion

        #region Constructors

        public HomePageCurrentFlashSaleProductsViewComponent(
            IProductViewModelService productViewModelService
            /*FlashSale flashSale*/)
        {
            this._productViewModelService = productViewModelService;
            //this._flashSale = flashSale;
        }

        #endregion

        #region Invoker

        public async Task<IViewComponentResult> InvokeAsync()
        {
           
            var model = await Task.Run(() => _productViewModelService.PrepareHomePageCurrentFlashSale());
            if (!model.Any())
                return Content("");

            return View("default",model);
        }
 
        #endregion

    }
}
