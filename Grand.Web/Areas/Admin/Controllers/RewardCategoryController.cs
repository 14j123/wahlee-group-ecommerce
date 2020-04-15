using Grand.Framework.Kendoui;
using Grand.Framework.Mvc.Filters;
using Grand.Framework.Security.Authorization;
using Grand.Services.Localization;
using Grand.Services.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Grand.Web.Areas.Admin.Extensions;
using Grand.Services.Rewards;
using Grand.Core.Domain.Rewards;
using Grand.Web.Areas.Admin.Models.RewardCategory;

namespace Grand.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.LoyaltyReward)]
    public partial class RewardCategoryController : BaseAdminController
    {
        #region Fields
        private readonly IRewardCategoryService _RewardCategoryService;
        private readonly IRewardIDService _RewardIDService;
        private readonly IRewardService _RewardService;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Constructors

        public RewardCategoryController(
            IRewardCategoryService RewardCategoryService,
            IRewardIDService RewardIDService,
            IRewardService RewardService,
            ILocalizationService localizationService
        )
        {
            this._RewardCategoryService = RewardCategoryService;
            this._RewardIDService = RewardIDService;
            this._RewardService = RewardService;
            this._localizationService = localizationService;

        }

        #endregion

        #region Reward Category

        #region Reward Category Create
        public IActionResult Create()
        {
            var model = new RewardCategoryModel();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Create(RewardCategoryModel model, bool continueEditing)
        {

            if (model.Reward_Category_Name == null)
            {
                ModelState.AddModelError("", "CreateFail, blank Reward Name Found");
            }

            if (ModelState.IsValid)
            {
                RewardCategory R = new RewardCategory();
                R.Reward_Category_Name = model.Reward_Category_Name;
                R.Reward_Description = model.Reward_Description;
                R.CreateTime = DateTime.UtcNow;
                R.Delete = false;

                _RewardCategoryService.AddRewardCategory(R);
                SuccessNotification(_localizationService.GetResource("Admin.RewardCategory.RewardCategory.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form

            return View(model);
        }

        #endregion

        #region Reward Category List
        public IActionResult List() => View();

        [HttpPost]
        public IActionResult List(DataSourceRequest command)
        {
            var RewardCategory = _RewardCategoryService.GETAllRewardCategory();

            var gridModel = new DataSourceResult
            {
                Data = RewardCategory.ToList(),
                Total = RewardCategory.Count
            };
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(gridModel);
        }
        #endregion

        #region Reward Category Edit
        public IActionResult Edit(string id)
        {
            var RGift = _RewardCategoryService.GETRewardCategoryInfo(id);
            if (RGift == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            var model = RGift.ToModel();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Edit(RewardCategory model, bool continueEditing)
        {
            var RewardCategory = _RewardCategoryService.GETRewardCategoryInfo(model.Id);
            if (RewardCategory == null)
                //No customer role found with the specified id
                return RedirectToAction("List");
            //if (UpdateGift.Redempted > model.Quantity)
            //{
            //    return RedirectToAction("Edit");
            //}
            try
            {
                if (ModelState.IsValid)
                {

                    //RewardCategory.Delete = model.Delete;

                    _RewardCategoryService.UpdateRewardCategory(RewardCategory);
                    SuccessNotification(_localizationService.GetResource("Admin.Reward.Reward.Updated")); //pop out info
                    return continueEditing ? RedirectToAction("Edit", new { id = RewardCategory.Id }) : RedirectToAction("List");
                }

                //If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = RewardCategory.Id });
            }
        }
        #endregion

        //#region Reward Gift Delete
        //[HttpPost]
        //public IActionResult Delete(string id)
        //{

        //    var Gift = _luckyDrawGiftManageService.GETGiftInfo(id);
        //    if (Gift == null)
        //        //No customer role found with the specified id
        //        return RedirectToAction("List");

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            Gift.Delete = true;
        //            _luckyDrawGiftManageService.DeleteGift(Gift);

        //            for (int i = 1; i <= Gift.Quantity; i++)
        //            {
        //                var addgiftID = _LuckyDrawGiftIDManageService.GETGiftInfo(id);
        //                addgiftID.Delete = true;
        //                _LuckyDrawGiftIDManageService.DeleteGiftID(addgiftID);


        //            }



        //            SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Deleted"));
        //            return RedirectToAction("List");
        //        }
        //        ErrorNotification(ModelState);
        //        return RedirectToAction("List", new { id = Gift.Id });
        //    }
        //    catch (Exception exc)
        //    {
        //        ErrorNotification(exc.Message);
        //        return RedirectToAction("Edit", new { id = Gift.Id });
        //    }


        //}
        //#endregion

        #endregion

    }
}
