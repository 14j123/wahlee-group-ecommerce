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
using Grand.Services.MemberCards;

namespace Grand.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.MemberCard)]
    public partial class MemberCardTopUpHistoryController : BaseAdminController
    {
        #region Fields

        private readonly IMemberCardTopUpHistoryService _memberCardTopUpHistoryService; 

        #endregion

        #region Constructors

        public MemberCardTopUpHistoryController(
            IMemberCardTopUpHistoryService memberCardTopUpHistoryService

        )
        {

            this._memberCardTopUpHistoryService = memberCardTopUpHistoryService;

        }

        #endregion
        #region Method

        #region List Member Card Top Up History
        public IActionResult List() => View();

        [HttpPost]
        public IActionResult List(DataSourceRequest command)
        {
            var MCTUH = _memberCardTopUpHistoryService.GetAllMemberCardTopUpHistory();

            var gridModel = new DataSourceResult
            {
                Data = MCTUH.ToList(),
                Total = MCTUH.Count
            };
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(gridModel);
        }
        #endregion
        #endregion

    }
}
