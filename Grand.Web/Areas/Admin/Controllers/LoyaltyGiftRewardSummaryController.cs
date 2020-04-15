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

namespace Grand.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.LoyaltyReward)]
    public partial class LoyaltyGiftRewardSummaryController : BaseAdminController
    {
        #region Fields

        private readonly ILuckyDrawGiftIDManageService _LuckyDrawGiftIDManageService;
        private readonly ILocalizationService _localizationService;
      
        #endregion

        #region Constructors

        public LoyaltyGiftRewardSummaryController(

            ILuckyDrawGiftIDManageService LuckyDrawGiftIDManageService,
            ILocalizationService localizationService
        )
        {
            this._LuckyDrawGiftIDManageService = LuckyDrawGiftIDManageService;
            this._localizationService = localizationService;
           
        }
        
        #endregion

        #region Gift Report
        
        #region Gift List
        public IActionResult List() => View();

        [HttpPost]
        public IActionResult List(DataSourceRequest command)
        {
            var GM = _LuckyDrawGiftIDManageService.GetAllWinnerWinnerChickenDinner();

            var gridModel = new DataSourceResult
            {
                Data = GM.ToList(),
                Total = GM.Count
            };
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(gridModel);
        }
        #endregion
        
        #endregion

    }
}
