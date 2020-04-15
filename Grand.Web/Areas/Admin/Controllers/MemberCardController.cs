using Grand.Framework.Kendoui;
using Grand.Framework.Security.Authorization;
using Grand.Services.Localization;
using Grand.Services.Security;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Grand.Services.LoyaltyAdmin;
using Grand.Services.MemberCards;
using Grand.Web.Areas.Admin.Models.MemberCard;
using System.Collections.Generic;
using Grand.Core.Domain.MemberCard;
using Grand.Services.Customers;
using Grand.Core.Domain.Customers;

namespace Grand.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.MemberCard)]
    public partial class MemberCardController : BaseAdminController
    {
        #region Fields

        private readonly IMemberCardService _memberCardService;
        private readonly ICustomerService _customerService;
        private readonly ILocalizationService _localizationService;
        

        #endregion

        #region Constructors

        public MemberCardController(

            IMemberCardService memberCardService,
            ILocalizationService localizationService,
            ICustomerService customerService
        )
        {
            this._memberCardService = memberCardService;
            this._customerService = customerService;
            this._localizationService = localizationService;

        }

        #endregion

        #region Method

        #region List Member Card
        public IActionResult List() => View();

        [HttpPost]
        public IActionResult List(DataSourceRequest command)
        {
            List<MemberCard> MC  = _memberCardService.GetAllMemberCard();
            List<MemberCardModel> LMCM = new List<MemberCardModel>();
            foreach (var r in MC)
            {
                Customer c = _customerService.GetCustomerIDByMemberCardId(r.MemberCardId);
                
                LMCM.Add(new MemberCardModel
                {
                    MemberCardId = r.MemberCardId,

                    ExpiredDate = r.ExpiredDate,
                    BalanceOnCard = r.BalanceOnCard,

                    CustomerFullName = c.GetFullName(),
                    CustomerEmail = c.Email,
                    CustomerID = c.Id
                }
                );

          
            }

            var gridModel = new DataSourceResult
            {
                Data = LMCM.ToList(),
                Total = LMCM.Count
            };
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(gridModel);
        }
        #endregion

        #endregion

    }
}
