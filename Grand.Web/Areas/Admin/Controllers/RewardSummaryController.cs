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
using Grand.Services.Rewards;
using Grand.Web.Areas.Admin.Models.Reward;
using Grand.Core.Domain.Rewards;
using Grand.Web.Areas.Admin.Models.RewardSummary;

namespace Grand.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.LoyaltyReward)]
    public partial class RewardSummaryController : BaseAdminController
    {
        #region Fields

        private readonly IRewardIDService _RewardIDService;
        private readonly IRewardService _RewardService;
        private readonly ILocalizationService _localizationService;
      
        #endregion

        #region Constructors

        public RewardSummaryController(

            IRewardIDService RewardIDService,
            IRewardService RewardService,
            ILocalizationService localizationService
        )
        {
            this._RewardIDService = RewardIDService;
            this._RewardService = RewardService;
            this._localizationService = localizationService;
           
        }

        #endregion

        #region Reward Gift ID

        
        #region Reward Gift List
        public IActionResult List() => View();

        [HttpPost]
        public IActionResult List(DataSourceRequest command)
        {
            var Reward = _RewardIDService.GETAllRedeemptedRewardGiftID();

            var gridModel = new DataSourceResult
            {
                Data = Reward.ToList(),
                Total = Reward.Count
            };
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(gridModel);
        }
        #endregion

        #region Reward Gift Edit
        public IActionResult Edit(string id)
        {
            var RGift = _RewardService.GETGiftInfo(id);
            if (RGift == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            var model = RGift.ToModel();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Edit(LuckyDrawGiftManage model, bool continueEditing)
        {
            var UpdateGift = _RewardService.GETGiftInfo(model.Id);

            if (UpdateGift == null)
                //No customer role found with the specified id
                return RedirectToAction("List");
            var cal = UpdateGift.Quantity - UpdateGift.AvailableQuantity;
            if (model.Quantity < cal)
            {
                return RedirectToAction("Edit");
            }
            try
            {
                if (ModelState.IsValid)
                {

                    UpdateGift.Activate = model.Delete;

                    _RewardService.UpdateGift(UpdateGift);
                    SuccessNotification(_localizationService.GetResource("Admin.Reward.Reward.Updated")); //pop out info
                    return continueEditing ? RedirectToAction("Edit", new { id = UpdateGift.Id }) : RedirectToAction("List");
                }

                //If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = UpdateGift.Id });
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
