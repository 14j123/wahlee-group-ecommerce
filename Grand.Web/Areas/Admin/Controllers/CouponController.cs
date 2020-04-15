using Grand.Core;
using Grand.Framework.Controllers;
using Grand.Framework.Kendoui;
using Grand.Framework.Mvc;
using Grand.Framework.Mvc.Filters;
using Grand.Framework.Security.Authorization;
using Grand.Services.Catalog;
using Grand.Services.CouponsModule;
using Grand.Services.Localization;
using Grand.Services.Security;
using Grand.Services.Stores;
using Grand.Services.Vendors;
using Grand.Web.Areas.Admin.Extensions;
using Grand.Web.Areas.Admin.Models.CouponsModule;
using Grand.Web.Areas.Admin.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;


namespace Grand.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.Coupons)]
    public partial class CouponController : BaseAdminController
    {
        #region Fields
        private readonly ICouponViewModelService _couponViewModelService;
        private readonly ICouponService _couponService;
        private readonly ILocalizationService _localizationService;
        #endregion

        #region Constructors

        public CouponController(
            ICouponViewModelService couponViewModelService,
            ICouponService couponService, 
            ILocalizationService localizationService)
        {
            this._couponViewModelService = couponViewModelService;
            this._couponService = couponService;
            this._localizationService = localizationService;
        }

        #endregion

        #region Coupons

        //list
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List()
        {
            var model = _couponViewModelService.PrepareCouponListModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult List(CouponListModel model, DataSourceRequest command)
        {
            var coupons = _couponViewModelService.PrepareCouponModel(model, command.Page, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = coupons.couponModel.ToList(),
                Total = coupons.totalCount
            };
            return Json(gridModel);
        }
        
        //create
        public IActionResult Create()
        {
            var model = new CouponModel();
            _couponViewModelService.PrepareCouponModel(model, null);
            //default values
            model.LimitationTimes = 1;
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Create(CouponModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var coupon = _couponViewModelService.InsertCouponModel(model);
                SuccessNotification(_localizationService.GetResource("Admin.Promotions.Coupons.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = coupon.Id }) : RedirectToAction("List");
            }
            //If we got this far, something failed, redisplay form
            _couponViewModelService.PrepareCouponModel(model, null);
            return View(model);
        }

        //edit
        public IActionResult Edit(string id)
        {
            var coupon = _couponService.GetCouponById(id);
            if (coupon == null)
                //No coupon found with the specified id
                return RedirectToAction("List");

            var model = coupon.ToModel();
            _couponViewModelService.PrepareCouponModel(model, coupon);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Edit(CouponModel model, bool continueEditing)
        {
            var coupon = _couponService.GetCouponById(model.Id);
            if (coupon == null)
                //No coupon found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                coupon = _couponViewModelService.UpdateCouponModel(coupon, model);
                SuccessNotification(_localizationService.GetResource("Admin.Promotions.Coupons.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabIndex();

                    return RedirectToAction("Edit",  new {id = coupon.Id});
                }
                return RedirectToAction("List");
            }
            //If we got this far, something failed, redisplay form
            _couponViewModelService.PrepareCouponModel(model, coupon);
            return View(model);
        }

        //delete
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var coupon = _couponService.GetCouponById(id);
            if (coupon == null)
                //No coupon found with the specified id
                return RedirectToAction("List");

            var usagehistory = _couponService.GetAllCouponUsageHistory(coupon.Id);
            if (usagehistory.Count > 0)
            {
                ErrorNotification(_localizationService.GetResource("Admin.Promotions.Coupons.Deleted.UsageHistory"));
                return RedirectToAction("Edit", new { id = coupon.Id });
            }
            if (ModelState.IsValid)
            {
                _couponViewModelService.DeleteCoupon(coupon);
                SuccessNotification(_localizationService.GetResource("Admin.Promotions.Coupons.Deleted"));
                return RedirectToAction("List");
            }
            ErrorNotification(ModelState);
            return RedirectToAction("Edit", new { id = coupon.Id });
        }

        #endregion

        #region Coupon Ticket codes
        [HttpPost]
        public IActionResult CouponCodeList(DataSourceRequest command, string couponId)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            var couponcodes = _couponService.GetAllCouponCodesByCouponId(coupon.Id, pageIndex: command.Page - 1, pageSize: command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = couponcodes.Select(x => new 
                {
                    Id = x.Id,
                    CouponCode = x.CouponCode,
                    Used = x.Used
                }),
                Total = couponcodes.TotalCount
            };
            return Json(gridModel);
        }

        public IActionResult CouponCodeDelete(string couponId, string Id)
        {
            var discount = _couponService.GetCouponById(couponId);
            if (discount == null)
                throw new Exception("No discount found with the specified id");

            var coupon = _couponService.GetCouponCodeById(Id);
            if (coupon == null)
                throw new Exception("No coupon code found with the specified id");
            if (ModelState.IsValid)
            {
                if (!coupon.Used)
                    _couponService.DeleteCouponTicket(coupon);
                else
                    return new JsonResult(new DataSourceResult() { Errors = "You can't delete coupon code, it was used" });

                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);

        }
        public IActionResult CouponCodeInsert(string couponId, string couponCode)
        {
            if(string.IsNullOrEmpty(couponCode))
                throw new Exception("Coupon code can't be empty");

            var discount = _couponService.GetCouponById(couponId);
            if (discount == null)
                throw new Exception("No discount found with the specified id");

            couponCode = couponCode.ToUpper();

            if (_couponService.GetCouponByCouponCode(couponCode)!=null)
                return new JsonResult(new DataSourceResult() { Errors = "Coupon code exists" });
            if (ModelState.IsValid)
            {
                _couponViewModelService.InsertCouponCode(couponId, couponCode);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }
        #endregion

        #region Coupon requirements

        [AcceptVerbs("GET")]
        public IActionResult GetCouponRequirementConfigurationUrl(string systemName, string couponId, string couponRequirementId)
        {
            if (String.IsNullOrEmpty(systemName))
                throw new ArgumentNullException("systemName");

            var couponPlugin = _couponService.LoadCouponPluginBySystemName(systemName);

            if (couponPlugin == null)
                throw new ArgumentException("Coupon requirement rule could not be loaded");

            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new ArgumentException("Coupon could not be loaded");

            var singleRequirement = couponPlugin.GetRequirementRules().Single(x => x.SystemName == systemName);
            string url = _couponViewModelService.GetRequirementUrlInternal(singleRequirement, coupon, couponRequirementId);
            return Json(new { url = url });
        }

        public IActionResult GetCouponRequirementMetaInfo(string couponRequirementId, string couponId)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new ArgumentException("Discount could not be loaded");

            var couponRequirement = coupon.CouponRequirements.FirstOrDefault(dr => dr.Id == couponRequirementId);
            if (couponRequirement == null)
                throw new ArgumentException("Coupon requirement could not be loaded");

            var couponPlugin = _couponService.LoadCouponPluginBySystemName(couponRequirement.CouponRequirementRuleSystemName);
            if (couponPlugin == null)
                throw new ArgumentException("Coupon requirement rule could not be loaded");

            var couponRequirementRule = couponPlugin.GetRequirementRules().First(x => x.SystemName == couponRequirement.CouponRequirementRuleSystemName);
            string url = _couponViewModelService.GetRequirementUrlInternal(couponRequirementRule, coupon, couponRequirementId);
            string ruleName = couponRequirementRule.FriendlyName;

            return Json(new { url = url, ruleName = ruleName });
        }

        [HttpPost]
        public IActionResult DeleteCouponRequirement(string couponRequirementId, string couponId)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new ArgumentException("Coupon could not be loaded");

            var couponRequirement = coupon.CouponRequirements.FirstOrDefault(dr => dr.Id == couponRequirementId);
            if (couponRequirement == null)
                throw new ArgumentException("Coupon requirement could not be loaded");

            if (ModelState.IsValid)
            {
                _couponViewModelService.DeleteCouponRequirement(couponRequirement, coupon);
                return Json(new { Result = true });
            }
            return ErrorForKendoGridJson(ModelState);
        }

        #endregion

        #region Applied to products

        [HttpPost]
        public IActionResult ProductList(DataSourceRequest command, string couponId, [FromServices] IProductService productService)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            var products = productService.GetProductsByCoupon(coupon.Id, pageIndex: command.Page - 1, pageSize: command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = products.Select(x => new CouponModel.AppliedToProductModel
                {
                    ProductId = x.Id,
                    ProductName = x.Name
                }),
                Total = products.TotalCount
            };

            return Json(gridModel);
        }

        public IActionResult ProductDelete(string couponId, string productId, [FromServices] IProductService productService)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            var product = productService.GetProductById(productId);
            if (product == null)
                throw new Exception("No product found with the specified id");

            if (ModelState.IsValid)
            {
                _couponViewModelService.DeleteProduct(coupon, product);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }

        public IActionResult ProductAddPopup(string couponId)
        {
            var model = _couponViewModelService.PrepareProductToCouponModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult ProductAddPopupList(DataSourceRequest command, CouponModel.AddProductToCouponModel model)
        {
            var products = _couponViewModelService.PrepareProductModel(model, command.Page, command.PageSize);

            var gridModel = new DataSourceResult();
            gridModel.Data = products.products.ToList();
            gridModel.Total = products.totalCount;

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public IActionResult ProductAddPopup(CouponModel.AddProductToCouponModel model)
        {
            var coupon = _couponService.GetCouponById(model.CouponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            if (model.SelectedProductIds != null)
            {
                _couponViewModelService.InsertProductToCouponModel(model);
            }

            ViewBag.RefreshPage = true;
            return View(model);
        }

        #endregion

        #region Applied to categories

        [HttpPost]
        public IActionResult CategoryList(DataSourceRequest command, string couponId, [FromServices] ICategoryService categoryService)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            var categories = categoryService.GetAllCategoriesByCoupon(coupon.Id);
            var gridModel = new DataSourceResult
            {
                Data = categories.Select(x => new CouponModel.AppliedToCategoryModel
                {
                    CategoryId = x.Id,
                    CategoryName = x.GetFormattedBreadCrumb(categoryService)
                }),
                Total = categories.Count
            };

            return Json(gridModel);
        }

        public IActionResult CategoryDelete(string couponId, string categoryId, [FromServices] ICategoryService categoryService)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            var category = categoryService.GetCategoryById(categoryId);
            if (category == null)
                throw new Exception("No category found with the specified id");

            if (ModelState.IsValid)
            {
                _couponViewModelService.DeleteCategory(coupon, category);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }

        public IActionResult CategoryAddPopup(string couponId)
        {
            var model = new CouponModel.AddCategoryToCouponModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult CategoryAddPopupList(DataSourceRequest command, CouponModel.AddCategoryToCouponModel model, [FromServices] ICategoryService categoryService)
        {
            var categories = categoryService.GetAllCategories(model.SearchCategoryName,
                pageIndex: command.Page - 1, pageSize: command.PageSize, showHidden: true);
            var gridModel = new DataSourceResult
            {
                Data = categories.Select(x =>
                {
                    var categoryModel = x.ToModel();
                    categoryModel.Breadcrumb = x.GetFormattedBreadCrumb(categoryService);
                    return categoryModel;
                }),
                Total = categories.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public IActionResult CategoryAddPopup(CouponModel.AddCategoryToCouponModel model)
        {
            var coupon = _couponService.GetCouponById(model.CouponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            if (model.SelectedCategoryIds != null)
            {
                _couponViewModelService.InsertCategoryToCouponModel(model);
            }
            ViewBag.RefreshPage = true;
            return View(model);
        }

        #endregion

        #region Applied to manufacturers

        [HttpPost]
        public IActionResult ManufacturerList(DataSourceRequest command, string couponId, [FromServices] IManufacturerService manufacturerService)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            var manufacturers = manufacturerService.GetAllManufacturersByCoupon(coupon.Id);
            var gridModel = new DataSourceResult
            {
                Data = manufacturers.Select(x => new CouponModel.AppliedToManufacturerModel
                {
                    ManufacturerId = x.Id,
                    ManufacturerName = x.Name
                }),
                Total = manufacturers.Count
            };

            return Json(gridModel);
        }

        public IActionResult ManufacturerDelete(string couponId, string manufacturerId, [FromServices] IManufacturerService manufacturerService)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            var manufacturer = manufacturerService.GetManufacturerById(manufacturerId);
            if (manufacturer == null)
                throw new Exception("No manufacturer found with the specified id");

            if (ModelState.IsValid)
            {
                _couponViewModelService.DeleteManufacturer(coupon, manufacturer);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }

        public IActionResult ManufacturerAddPopup(string couponId)
        {
            var model = new CouponModel.AddManufacturerToCouponModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult ManufacturerAddPopupList(DataSourceRequest command, CouponModel.AddManufacturerToCouponModel model, [FromServices] IManufacturerService manufacturerService)
        {
            var manufacturers = manufacturerService.GetAllManufacturers(model.SearchManufacturerName,"",
                command.Page - 1, command.PageSize, true);
            var gridModel = new DataSourceResult
            {
                Data = manufacturers.Select(x => x.ToModel()),
                Total = manufacturers.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public IActionResult ManufacturerAddPopup(CouponModel.AddManufacturerToCouponModel model)
        {
            var coupon = _couponService.GetCouponById(model.CouponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            if (model.SelectedManufacturerIds != null)
            {
                _couponViewModelService.InsertManufacturerToCouponModel(model);
            }
            ViewBag.RefreshPage = true;
            return View(model);
        }

        #endregion

        #region Applied to vendors

        [HttpPost]
        public IActionResult VendorList(DataSourceRequest command, string couponId, [FromServices] IVendorService vendorService)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            var vendors = vendorService.GetAllVendorsByCoupon(coupon.Id);
            var gridModel = new DataSourceResult
            {
                Data = vendors.Select(x => new CouponModel.AppliedToVendorModel
                {
                    VendorId = x.Id,
                    VendorName = x.Name
                }),
                Total = vendors.Count
            };

            return Json(gridModel);
        }

        public IActionResult VendorDelete(string couponId, string vendorId, [FromServices] IVendorService vendorService)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            var vendor = vendorService.GetVendorById(vendorId);
            if (vendor == null)
                throw new Exception("No vendor found with the specified id");
            if (ModelState.IsValid)
            {
                _couponViewModelService.DeleteVendor(coupon, vendor);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }

        public IActionResult VendorAddPopup(string couponId)
        {
            var model = new CouponModel.AddVendorToCouponModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult VendorAddPopupList(DataSourceRequest command, CouponModel.AddVendorToCouponModel model, [FromServices] IVendorService vendorService)
        {
            var vendors = vendorService.GetAllVendors(model.SearchVendorName, command.Page - 1, command.PageSize, true);

            //search for emails
            if (!(string.IsNullOrEmpty(model.SearchVendorEmail)))
            {
                var tempVendors = vendors.Where(x => x.Email.ToLowerInvariant().Contains(model.SearchVendorEmail.Trim()));
                vendors = new PagedList<Core.Domain.Vendors.Vendor>(tempVendors, command.Page - 1, command.PageSize);
            }

            var gridModel = new DataSourceResult
            {
                Data = vendors.Select(x => x.ToModel()),
                Total = vendors.TotalCount
            };

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public IActionResult VendorAddPopup(CouponModel.AddVendorToCouponModel model)
        {
            var coupon = _couponService.GetCouponById(model.CouponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            if (model.SelectedVendorIds != null)
            {
                _couponViewModelService.InsertVendorToCouponModel(model);
            }
            ViewBag.RefreshPage = true;
            return View(model);
        }

        #endregion

        #region Applied to store
        [HttpPost]
        public IActionResult StoreList(DataSourceRequest command, string couponId, [FromServices] IStoreService storeService)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            var stores = storeService.GetAllStoresByCoupon(coupon.Id);
            var gridModel = new DataSourceResult
            {
                Data = stores.Select(x => new CouponModel.AppliedToStoreModel
                {
                    StoreId = x.Id,
                    StoreName = x.Name
                }),
                Total = stores.Count
            };

            return Json(gridModel);
        }

        public IActionResult StoreDelete(string couponId, string storeId, [FromServices] IStoreService storeService)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");
            var store = storeService.GetStoreById(storeId);
            if (store == null)
                throw new Exception("No store found with the specified id");

            if (ModelState.IsValid)
            {
                _couponViewModelService.DeleteStore(coupon, store);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }

        public IActionResult StoreAddPopup(string couponId)
        {
            var model = new CouponModel.AddStoreToCouponModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult StoreAddPopupList(DataSourceRequest command, CouponModel.AddStoreToCouponModel model, [FromServices] IStoreService storeService)
        {
            var stores = storeService.GetAllStores();
            var gridModel = new DataSourceResult
            {
                Data = stores.Select(x => x.ToModel()),
                Total = stores.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public IActionResult StoreAddPopup(CouponModel.AddStoreToCouponModel model)
        {
            var coupon = _couponService.GetCouponById(model.CouponId);
            if (coupon == null)
                throw new Exception("No coupon found with the specified id");

            if (model.SelectedStoreIds != null)
            {
                _couponViewModelService.InsertStoreToCouponModel(model);
            }
            ViewBag.RefreshPage = true;
            return View(model);
        }

        #endregion

        #region Coupon usage history

        [HttpPost]
        public IActionResult UsageHistoryList(string couponId, DataSourceRequest command)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new ArgumentException("No coupon found with the specified id");

            var duh = _couponViewModelService.PrepareCouponUsageHistoryModel(coupon, command.Page, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = duh.usageHistoryModels.ToList(),
                Total = duh.totalCount
            };
            return Json(gridModel);
        }

        [HttpPost]
        public IActionResult UsageHistoryDelete(string couponId, string id)
        {
            var coupon = _couponService.GetCouponById(couponId);
            if (coupon == null)
                throw new ArgumentException("No coupon found with the specified id");
            
            var duh = _couponService.GetCouponUsageHistoryById(id);
            if (duh != null)
            {
                if (ModelState.IsValid)
                {
                    _couponService.DeleteCouponUsageHistory(duh);
                }
                else
                    return ErrorForKendoGridJson(ModelState);
            }
            return new NullJsonResult();
        }

        #endregion
    }
}
