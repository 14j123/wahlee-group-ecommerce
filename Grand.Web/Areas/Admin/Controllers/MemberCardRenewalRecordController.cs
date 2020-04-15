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
    public partial class MemberCardRenewalRecordController : BaseAdminController
    {
        #region Fields

        private readonly IMemberCardRenewalRecordService _memberCardRenewalRecordService;

        #endregion

        #region Constructors

        public MemberCardRenewalRecordController(
            IMemberCardRenewalRecordService memberCardRenewalRecordService

        )
        {
            this._memberCardRenewalRecordService = memberCardRenewalRecordService;


        }

        #endregion

        #region Method

        #region List Member Card Renewal Record
        public IActionResult List() => View();

        [HttpPost]
        public IActionResult List(DataSourceRequest command)
        {
            var MCRR = _memberCardRenewalRecordService.GetAllMemberCardRenewalRecord();

            var gridModel = new DataSourceResult
            {
                Data = MCRR.ToList(),
                Total = MCRR.Count
            };
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(gridModel);
        }
        #endregion
        #endregion

    }
}
