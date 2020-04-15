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

namespace Grand.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.LoyaltyReward)]
    public partial class PointWalletSummaryController : BaseAdminController
    {
        #region Fields

        private readonly IPointWalletSummaryService _pointWalletSummaryService;

        #endregion

        #region Constructors

        public PointWalletSummaryController(
            IPointWalletSummaryService pointWalletSummaryService
        )
        {
            this._pointWalletSummaryService = pointWalletSummaryService;
        }

        #endregion

        #region Point Wallet Summary

        #region Point Wallet Summary List
        public IActionResult List() => View();

        [HttpPost]
        public IActionResult List(DataSourceRequest command)
        {
            var GM = _pointWalletSummaryService.GETAllPointWalletSummary();

            var gridModel = new DataSourceResult
            {
                Data = GM.ToList(),
                Total = GM.Count
            };
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(gridModel);
        }
        #endregion

        //#region Point Wallet Edit
        //public IActionResult Edit(string id)
        //{
        //    var pointWallet = _pointWalletSummaryService.GETPointWalletInfo(id);
        //    if (pointWallet == null)
        //        //No customer role found with the specified id
        //        return RedirectToAction("List");

        //    var model = pointWallet.ToModel();
        //    //PrepareGiftModel(model, customerRole);
        //    return View(model);
        //}

        //[HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        //public IActionResult Edit(PointWalletModel model, bool continueEditing)
        //{
        //    var pointWallet = _pointWalletSummaryService.GETPointWalletInfo(model.Id);
        //    if (pointWallet == null)
        //        //No customer role found with the specified id
        //        return RedirectToAction("List");

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            if (pointWallet.LoyaltyPointUsed < model.LoyaltyPointEarn)
        //            {
        //                pointWallet.LoyaltyPointEarn = model.LoyaltyPointEarn;
        //                _pointWalletSummaryService.UpdatePointWalletInfo(pointWallet);
        //            }
        //            //SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Updated")); //pop out info
        //            return continueEditing ? RedirectToAction("Edit", new { id = pointWallet.Id }) : RedirectToAction("List");
        //        }

        //        //If we got this far, something failed, redisplay form
        //        return View(model);
        //    }
        //    catch (Exception exc)
        //    {
        //        ErrorNotification(exc);
        //        return RedirectToAction("Edit", new { id = pointWallet.Id });
        //    }
        //}
        //#endregion

        #endregion

    }
}
