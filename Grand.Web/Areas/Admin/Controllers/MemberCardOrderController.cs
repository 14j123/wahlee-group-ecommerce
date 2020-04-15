using Grand.Framework.Kendoui;
using Grand.Framework.Mvc.Filters;
using Grand.Framework.Security.Authorization;
using Grand.Services.Localization;
using Grand.Services.Security;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Grand.Services.MemberCards;

namespace Grand.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.MemberCard)]
    public partial class MemberCardOrderController : BaseAdminController
    {
        #region Fields

        private readonly IMemberCardOrderService _memberCardOrderService;
        private readonly ILocalizationService _localizationService;

        #endregion

        #region Constructors

        public MemberCardOrderController(
            
            IMemberCardOrderService memberCardOrderService,
            ILocalizationService localizationService

        )
        {

            this._memberCardOrderService = memberCardOrderService;
            this._localizationService = localizationService;

        }

        #endregion

        #region Method

        #region List Member Card
        public IActionResult List() => View();

        [HttpPost]
        public IActionResult List(DataSourceRequest command)
        {
            var MCO = _memberCardOrderService.GetAllMemberCardOrder();

            var gridModel = new DataSourceResult
            {
                Data = MCO.ToList(),
                Total = MCO.Count
            };
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(gridModel);
        }
        #endregion

        #endregion

    }
}
