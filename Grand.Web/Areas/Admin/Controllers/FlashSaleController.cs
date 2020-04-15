using Grand.Core.Domain.Catalog;
using Grand.Core;
using Grand.Framework.Controllers;
using Grand.Framework.Kendoui;
using Grand.Framework.Mvc;
using Grand.Framework.Mvc.Filters;
using Grand.Framework.Security.Authorization;
using Grand.Services.Catalog;
using Grand.Services.Customers;
using Grand.Services.Localization;
using Grand.Services.Messages;
using Grand.Services.Security;
using Grand.Services.Stores;
using Grand.Services.Vendors;
using Grand.Services.Common;
using Grand.Web.Areas.Admin.Extensions;
using Grand.Web.Areas.Admin.Models.Customers;
using Grand.Web.Areas.Admin.Models.FlashSales;
using Grand.Web.Areas.Admin.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Grand.Services.FlashSales;

namespace Grand.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.FlashSales)]
    public partial class FlashSaleController : BaseAdminController
    {
        #region Fields
        
        private readonly IFlashSaleService _flashSaleService;
        private readonly IFlashSaleViewModelService _flashSaleViewModelService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Constructors

        public FlashSaleController(
           
            IFlashSaleService flashSaleService,
            IFlashSaleViewModelService flashSaleViewModelService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IPermissionService permissionService,
            IWorkContext workContext)
            
        {
            this._flashSaleService = flashSaleService;
            this._flashSaleViewModelService = flashSaleViewModelService;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._storeService = storeService;
            this._permissionService = permissionService;
            this._workContext = workContext;
            
        }
        #endregion

        #region Methods

        #region list, create, update, delete

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [HttpPost]
        public IActionResult List(DataSourceRequest command)
        {
            var flashSales = _flashSaleService.GetAllFlashSale("", command.Page - 1, command.PageSize, true);
            var gridModel = new DataSourceResult
            {
                Data = flashSales.Select(x =>
                {
                    var m = x.ToModel();
                    return m;
                }),
                Total = flashSales.TotalCount
            };

            return Json(gridModel);
        }

        public IActionResult Create()
        {
            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
            var model = new FlashSaleModel();
            //default values
            model.IsEnabled = true;

            return View(model);
            
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Create(FlashSaleModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var flashSale = model.ToEntity();
                flashSale.CreatedOnUtc = DateTime.UtcNow;
                flashSale.UpdateDateOnUtc = DateTime.UtcNow;

                _flashSaleService.InsertFlashSale(flashSale);

                SuccessNotification(_localizationService.GetResource("Admin.Promotions.FlashSales.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = flashSale.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);

            return View(model);
        }

        public IActionResult Edit(string id)
        {
            var flashSale = _flashSaleService.GetFlashSaleById(id);
            if (flashSale == null)
               return RedirectToAction("List");

            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);
            var model = flashSale.ToModel();

            return View(model);
        }


        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Edit(FlashSaleModel model, bool continueEditing)
        {
            var flashSale = _flashSaleService.GetFlashSaleById(model.Id);
            if (flashSale == null)
               
            return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                flashSale = model.ToEntity(flashSale);

                flashSale.UpdateDateOnUtc = DateTime.UtcNow;

                _flashSaleService.UpdateFlashSale(flashSale);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.flashSale.Updated"));

                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = flashSale.Id });
                }
                return RedirectToAction("List");
            }

            ViewBag.AllLanguages = _languageService.GetAllLanguages(true);

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var flashSale = _flashSaleService.GetFlashSaleById(id);
            if (flashSale == null)
                
                return RedirectToAction("List");
            if (ModelState.IsValid)
            {
                _flashSaleService.DeleteFlashSale(flashSale);

                SuccessNotification(_localizationService.GetResource("Admin.ContentManagement.FlashSales.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);

            return RedirectToAction("Edit", new { id = flashSale.Id });
        }

        #endregion

        #region Products Lists

        [HttpPost]
        public IActionResult ProductList(DataSourceRequest command, string flashSaleId, [FromServices] IProductService productService)
        {
            var flashSale = _flashSaleService.GetFlashSaleById(flashSaleId);
            if (flashSale == null)
                throw new Exception("No flashSale item found with the specified id");

            var products = productService.GetProductsByFlashSale(flashSale.Id, pageIndex: command.Page - 1, pageSize: command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = products.Select(x => new FlashSaleModel.SelectedToProductModel
                {
                    ProductId = x.Id,
                    ProductName = x.Name
                }),
                Total = products.TotalCount
            };

            return Json(gridModel);
        }

        public IActionResult ProductDelete(string flashSaleId, string productId, [FromServices] IProductService productService)
        {
            var flashSale = _flashSaleService.GetFlashSaleById(flashSaleId);
            if (flashSale == null)
                throw new Exception("No flashSale found with the specified id");

            var product = productService.GetProductById(productId);
            if (product == null)
                throw new Exception("No product found with the specified id");

            if (ModelState.IsValid)
            {
                _flashSaleViewModelService.DeleteProduct(flashSale, product);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }

        public IActionResult ProductAddPopup(string flashSaleId)
        {
            var model = _flashSaleViewModelService.PrepareProductToFlashSaleModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult ProductAddPopupList(DataSourceRequest command, FlashSaleModel.AddProductToFlashSaleModel model)
        {
            var products = _flashSaleViewModelService.PrepareProductModel(model, command.Page, command.PageSize);

            var gridModel = new DataSourceResult();
            gridModel.Data = products.products.ToList();
            gridModel.Total = products.totalCount;

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public IActionResult ProductAddPopup(FlashSaleModel.AddProductToFlashSaleModel model)
        {
            var flashSale = _flashSaleService.GetFlashSaleById(model.FlashSaleId);
            if (flashSale == null)
                throw new Exception("No flashSale found with the specified id");

            if (model.SelectedProductIds != null)
            {
                _flashSaleViewModelService.InsertProductToFlashSaleModel(model);
            }

            ViewBag.RefreshPage = true;
            return View(model);
        }


        #endregion

        #endregion
    }

}