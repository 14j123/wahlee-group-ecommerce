using Grand.Framework.Kendoui;
using Grand.Framework.Mvc.Filters;
using Grand.Framework.Security.Authorization;
using Grand.Services.Localization;
using Grand.Services.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Grand.Core.Domain.LoyaltyAdmin;
using Grand.Services.LoyaltyAdmin;
using Grand.Web.Areas.Admin.Models.LuckyDrawGift;
using Grand.Web.Areas.Admin.Extensions;
using Grand.Web.Areas.Admin.Models.PointSetting;
using Grand.Core.Domain.LoyaltyPoint;
using Grand.Services.LoyaltyPoint;

namespace Grand.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.LoyaltyReward)]
    public partial class PointSettingController : BaseAdminController
    {
        #region Fields

        private readonly IPointSettingService _pointSettingService;
        private readonly ILocalizationService _localizationService;
      
        #endregion

        #region Constructors

        public PointSettingController(

            IPointSettingService pointSettingService,
            ILocalizationService localizationService
        )
        {
            this._pointSettingService = pointSettingService;
            this._localizationService = localizationService;
           
        }

        #endregion

        #region Method

        #region LoyaltyPoint List
        public IActionResult List() => View();

        [HttpPost]
        public IActionResult List(DataSourceRequest command)
        {
            var GM = _pointSettingService.GETSettingTimes();

            var gridModel = new DataSourceResult
            {
                Data = GM.ToList(),
                Total = GM.Count
            };
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(gridModel);
        }
        #endregion

        #region Point Rule Create
        public IActionResult Create()
        {
            var model = new PointSettingModel();
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Create(PointSettingModel model, bool continueEditing)
        {

            if (model.RuleName == null)
            {
                ModelState.AddModelError("", "CreateFail, blank Rule Name Found");
            }

            if (ModelState.IsValid)
            {
                PointSetting PS = new PointSetting();
                PS.RuleName = model.RuleName;
                PS.RuleDescription = model.RuleDescription;
                PS.Times = model.Times;
                PS.Activate = model.Activate;
                PS.Type = "Special";
                _pointSettingService.CreateRule(PS);
                SuccessNotification(_localizationService.GetResource("Admin.PointSetting.PointSetting.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form

            return View(model);
        }
        #endregion

        #region Point Rule Edit
        public IActionResult Edit(string id)
        {
            var pointWallet = _pointSettingService.GETSettingByID(id);
            if (pointWallet == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            var model = pointWallet.ToModel();
            //PrepareGiftModel(model, customerRole);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Edit(PointSettingModel model, bool continueEditing)
        {
            var PS = _pointSettingService.GETSettingByID(model.Id);
            if (PS == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {

                    PS.Times = model.Times;
                    PS.StartDate = model.StartDate;
                    PS.EndDate = model.EndDate;
                    PS.Activate = model.Activate;
                    _pointSettingService.UpdateRule(PS);
                    //SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Updated")); //pop out info
                    return continueEditing ? RedirectToAction("Edit", new { id = PS.Id }) : RedirectToAction("List");
                }

                //If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = PS.Id });
            }
        }
        #endregion
        #endregion
    }
}
