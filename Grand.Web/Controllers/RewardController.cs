using Grand.Core;
using Grand.Core.Domain.Blogs;
using Grand.Core.Domain.Customers;
using Grand.Framework.Controllers;
using Grand.Framework.Mvc;
using Grand.Framework.Mvc.Filters;
using Grand.Framework.Mvc.Rss;
using Grand.Framework.Security;
using Grand.Framework.Security.Captcha;
using Grand.Services.Blogs;
using Grand.Services.Localization;
using Grand.Services.Security;
using Grand.Services.Seo;
using Grand.Services.Stores;
using Grand.Web.Models.Blogs;
using Grand.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Grand.Services.Rewards;
using Grand.Framework.Kendoui;
using System.Linq;
using Grand.Web.Areas.Admin.Extensions;
using Grand.Web.Areas.Admin.Models.RewardSummary;

namespace Grand.Web.Controllers
{
    public partial class RewardController : BasePublicController
    {
        #region Fields

        private readonly IRewardService _rewardService;
        private readonly IRewardIDService _rewardIDService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Constructors

        public RewardController(
            IRewardService rewardService,
            IRewardIDService rewardIDService,
            IWorkContext workContext
            )
        {
            this._rewardService = rewardService;
            this._rewardIDService = rewardIDService;
            this._workContext = workContext;
        }

        #endregion

        #region Methods
        #region Reward Gift List
        public IActionResult RewardList() => View();

        [HttpPost]
        public IActionResult RewardList(DataSourceRequest command)
        {
            var Reward = _rewardService.GETAllValidRewardGiftInfo();

            var gridModel = new DataSourceResult
            {
                Data = Reward.Select(x =>
                {
                    var model = x.ToModel();
                    model.Date = x.ExpiredTime.Day.ToString();
                    return model;
                }),
                
                Total = Reward.Count
            };
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(Reward.ToList());
        }

        public IActionResult MyVoucherList() => View();

        [HttpPost]
        public IActionResult MyVoucherList(DataSourceRequest command)
        {

            string Customer_ID = _workContext.CurrentCustomer.Id;
            var RewardSummary = _rewardIDService.GETAllCanRewardGiftID(Customer_ID);
            var model = new List<RewardSummaryModel>();
            foreach (var a in RewardSummary)
            {
                model.Add(a.ToModel());
                
            }

            return Json(model);
        }

        public IActionResult MyVoucherListExpired() => View();

        [HttpPost]
        public IActionResult MyVoucherListExpired(DataSourceRequest command)
        {
            string Customer_ID = _workContext.CurrentCustomer.Id;
            var RewardSummary = _rewardIDService.GETAllExpiredRewardGiftID(Customer_ID);
            var model = new List<RewardSummaryModel>();
            foreach (var a in RewardSummary)
            {
                model.Add(a.ToModel());
            }

            return Json(model);
        }

        public IActionResult MyVoucherListRedeemed() => View();

        [HttpPost]
        public IActionResult MyVoucherListRedeemed(DataSourceRequest command)
        {

            string Customer_ID = _workContext.CurrentCustomer.Id;
            var RewardSummary = _rewardIDService.GETAllRedeemedRewardGiftID(Customer_ID);
            var model = new List<RewardSummaryModel>();
            foreach (var a in RewardSummary)
            {
                model.Add(a.ToModel());

            }

            return Json(model);
        }

        public IActionResult PurchaseVoucher(string Id)
        {
            var Reward = _rewardService.GETGiftInfo(Id);
            var model = Reward.ToModel();
            return View(model);
        }
        public IActionResult PurchaseVoucherConfirm(string Id)
        {
            var Reward = _rewardService.GETGiftInfo(Id);
            var model = Reward.ToModel();
            return View(model);
        }

        public IActionResult RedeemVoucher(string Id)
        {
            var Reward = _rewardIDService.GETRewardGiftID(Id);
            var model = Reward.ToModel();
            return View(model);
        }
        public IActionResult RedeemVoucherQRcode(string Id)
        {
            var Reward = _rewardIDService.GETRewardGiftID(Id);
            var model = Reward.ToModel();
            return View(model);
        }
        #endregion
        #endregion
    }
}
