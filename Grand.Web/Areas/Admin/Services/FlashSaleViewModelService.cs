using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Customers;
using Grand.Core.Domain.FlashSales;
using Grand.Framework.Extensions;
using Grand.Services.Catalog;
using Grand.Services.Customers;
using Grand.Services.Helpers;
using Grand.Services.FlashSales;
using Grand.Services.Localization;
using Grand.Services.Logging;
using Grand.Services.Messages;
using Grand.Services.Stores;
using Grand.Services.Vendors;
using Grand.Web.Areas.Admin.Extensions;
using Grand.Web.Areas.Admin.Interfaces;
using Grand.Web.Areas.Admin.Models.Catalog;
using Grand.Web.Areas.Admin.Models.Customers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using static Grand.Core.Domain.Customers.CustomerAction;
using Grand.Web.Areas.Admin.Models.FlashSales;

namespace Grand.Web.Areas.Admin.Services
{
    public partial class FlashSaleViewModelService : IFlashSaleViewModelService
    {
        private readonly ICustomerService _customerService;
        private readonly ICustomerTagService _customerTagService;
        private readonly IProductService _productService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IStoreService _storeService;
        private readonly IVendorService _vendorService;
        private readonly ICustomerActionService _customerActionService;
        private readonly IBannerService _bannerService;
        private readonly IInteractiveFormService _interactiveFormService;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IFlashSaleService _flashSaleService;


        public FlashSaleViewModelService(ICustomerService customerService,
            ICustomerTagService customerTagService,
            IProductService productService,
            ILocalizationService localizationService,
            ICustomerActivityService customerActivityService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IStoreService storeService,
            IVendorService vendorService,
            ICustomerActionService customerActionService,
            IProductAttributeService productAttributeService,
            IBannerService bannerService,
            IInteractiveFormService interactiveFormService,
            IMessageTemplateService messageTemplateService,
            IDateTimeHelper dateTimeHelper,
            IFlashSaleService flashSaleService)
        {
            _customerService = customerService;
            _customerTagService = customerTagService;
            _productService = productService;
            _localizationService = localizationService;
            _customerActivityService = customerActivityService;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
            _storeService = storeService;
            _vendorService = vendorService;
            _customerActionService = customerActionService;
            _bannerService = bannerService;
            _interactiveFormService = interactiveFormService;
            _messageTemplateService = messageTemplateService;
            _dateTimeHelper = dateTimeHelper;
            _flashSaleService = flashSaleService;
        }

        public virtual void DeleteProduct(FlashSale flashSale, Product product)
        {
            //remove flashSale
            if (product.AppliedFlashSales.Count(d => d == flashSale.Id) > 0)
            {
                product.AppliedFlashSales.Remove(flashSale.Id);
                _productService.DeleteFlashSaleProduct(flashSale.Id, product.Id);
            }
        }

        public virtual FlashSaleModel.AddProductToFlashSaleModel PrepareProductToFlashSaleModel()
        {
            var model = new FlashSaleModel.AddProductToFlashSaleModel();
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

        public virtual (IList<ProductModel> products, int totalCount) PrepareProductModel(FlashSaleModel.AddProductToFlashSaleModel model, int pageIndex, int pageSize)
        {
            var products = _productService.PrepareProductList(model.SearchCategoryId, model.SearchManufacturerId, model.SearchStoreId, model.SearchVendorId, model.SearchProductTypeId, model.SearchProductName, pageIndex, pageSize);
            return (products.Select(x => x.ToModel()).ToList(), products.TotalCount);
        }

        public virtual void InsertProductToFlashSaleModel(FlashSaleModel.AddProductToFlashSaleModel model)
        {
            foreach (string id in model.SelectedProductIds)
            {
                var product = _productService.GetProductById(id);
                if (product != null)
                {
                    if (product.AppliedFlashSales.Count(d => d == model.FlashSaleId) == 0)
                    {
                        product.AppliedFlashSales.Add(model.FlashSaleId);
                        _productService.InsertFlashSaleProduct(model.FlashSaleId, product.Id);
                    }
                }
            }
        }

    }
}
