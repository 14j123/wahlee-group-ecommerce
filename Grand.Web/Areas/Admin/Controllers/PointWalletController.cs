using Grand.Framework.Kendoui;
using Grand.Framework.Mvc.Filters;
using Grand.Framework.Security.Authorization;
using Grand.Services.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Grand.Web.Areas.Admin.Extensions;
using Grand.Services.LoyaltyPoint;
using Grand.Web.Areas.Admin.Models.PointWallet;
using Grand.Services.Customers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Grand.Core.Domain.LoyaltyPoint;
using Grand.Core.Domain.Customers;
using Grand.Services.Localization;

namespace Grand.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.LoyaltyReward)]
    public partial class PointWalletController : BaseAdminController
    {
        #region Fields

        private readonly IPointWalletService _pointWalletService;
        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Constructors

        public PointWalletController(
            IPointWalletService pointWalletService,
            ICustomerService customerService,
            ILocalizationService localizationService
        )
        {
            
            this._pointWalletService = pointWalletService;
            this._customerService = customerService;
            this._localizationService = localizationService;

        }
        
        #endregion

        #region Method
                
        #region Point Wallet List
        public IActionResult List() => View();

        [HttpPost]
        public IActionResult List(DataSourceRequest command)
        {
            var GM = _pointWalletService.GETAllPointWallet();

            var gridModel = new DataSourceResult
            {
                Data = GM.ToList(),
                Total = GM.Count
            };
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(gridModel);
        }
        #endregion
        
        #region Point Wallet Edit
        public IActionResult Edit(string id)
        {
            var pointWallet = _pointWalletService.GETPointWalletInfo(id);
            if (pointWallet == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            var model = pointWallet.ToModel();
            //PrepareGiftModel(model, customerRole);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Edit(PointWalletModel model, bool continueEditing)
        {
            var pointWallet = _pointWalletService.GETPointWalletInfo(model.Id);
            if (pointWallet == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    if (pointWallet.LoyaltyPointUsed < model.LoyaltyPointEarn)
                    {
                        pointWallet.LoyaltyPointEarn = model.LoyaltyPointEarn;
                        _pointWalletService.UpdatePointWalletInfo(pointWallet);
                        return continueEditing ? RedirectToAction("Edit", new { id = pointWallet.Id }) : RedirectToAction("List");
                    }
                    //SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Updated")); //pop out info
                    return continueEditing ? RedirectToAction("Edit", new { id = pointWallet.Id }) : RedirectToAction("List");
                }

                //If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = pointWallet.Id });
            }
        }
        #endregion
        
        #region Point Wallet RewardPointAdjustment
        
        public IActionResult RewardPointAdjustment()
        {
            var model = new PointWalletModel();
            var Customer_Email = _customerService.GetAllValidCustomerEmail();
            foreach (var s in Customer_Email)
            {
                model.selectedList.Add(new SelectListItem
                {
                    Text = s.Email,
                    Value = s.Id,
                });
            }
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult RewardPointAdjustment(PointWalletModel model, bool continueEditing)
        {
            if (model.Customer_Email == null)
            {
                ModelState.AddModelError("", "CreateFail, blank Customer Email Found");
            }
            if (model.ExpiredTime == null)
            {
                ModelState.AddModelError("", "CreateFail, blank Expired Time Found");
            }
            if (model.Reason == null)
            {
                ModelState.AddModelError("", "CreateFail, blank Adjustment Reason Found");
            }



            if (ModelState.IsValid)
            {
                PointWallet PW = new PointWallet();
                PW = model.ToEntity();
                PW.CreateTime = DateTime.UtcNow;
                Customer customer = _customerService.GetValidCustomerById(PW.Customer_Email);
                PW.Customer_Full_Name = customer.GetFullName();
                PW.Customer_Email = customer.Email;
                PW.Description = "Adjustment";
                _pointWalletService.AddPoint(PW);
                SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = PW.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            var Customer_Email = _customerService.GetAllValidCustomerEmail();
            foreach (var s in Customer_Email)
            {
                model.selectedList.Add(new SelectListItem
                {
                    Text = s.Email,
                    Value = s.Id,
                });
            }
            return View(model);
        }


        #endregion
        #endregion
    }
}
