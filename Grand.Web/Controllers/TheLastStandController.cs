using Grand.Core;
using Grand.Core.Domain.Customers;
using Grand.Core.Domain.LoyaltyPoint;
using Grand.Core.Domain.Rewards;
using Grand.Framework.Mvc.Filters;
using Grand.Services.Customers;
using Grand.Services.Localization;
using Grand.Services.LoyaltyPoint;
using Grand.Services.Rewards;
using Grand.Services.Security;
using Grand.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Grand.Web.Controllers
{
    public partial class TheLastStandController : BasePublicController
	{
		#region Fields

        private readonly ICountryViewModelService _countryViewModelService;
        private readonly IEncryptionService _encryptionService;
        private readonly ICustomerService _customerService;
        private readonly IRewardService _rewardService;
        private readonly IRewardIDService _rewardIDService;
        private readonly IWorkContext _workContext;
        private readonly IPointWalletService _pointWalletService;
        private readonly IPointWalletSummaryService _pointWalletSummaryService;

        #endregion

        #region Constructors

        public TheLastStandController(
            ICountryViewModelService countryViewModelService,
            IEncryptionService encryptionService,
            ICustomerService customerService,
            IRewardIDService rewardIDService,
            IRewardService rewardService,
            IWorkContext workContext,
            IPointWalletService pointWalletService,
            IPointWalletSummaryService pointWalletSummaryService)
		{
            this._countryViewModelService = countryViewModelService;
            this._encryptionService = encryptionService;
            this._customerService = customerService;
            this._rewardIDService = rewardIDService;
            this._rewardService = rewardService;
            this._workContext = workContext;
            this._pointWalletService = pointWalletService;
            this._pointWalletSummaryService = pointWalletSummaryService;
        }

        #endregion

        #region States / provinces

        
        public ActionResult PasswordVerify(string ID,string Password)
        {
            if(Password == null)
            {
                return Json(new { statuscode = "blank password" });
            }
            string customer_id = _workContext.CurrentCustomer.Id;
            Customer customer = _customerService.GetCustomerById(customer_id);

            
            string Pass = _encryptionService.CreatePasswordHash(Password, customer.PasswordSalt);
            if(Pass == customer.Password)
            {
                Reward rewardID = _rewardService.GETGiftInfo(ID);
                
                RewardID reward = _rewardIDService.GETRewardGiftIDbyRewardMainID(ID);
                if (reward == null)
                {
                    return Json(new { statuscode = "gift is fully redeem page" });
                }
                int point = 0;
                int pointused = 0;
          
                //Checkpoint
                var t = _pointWalletService.GETAllPointbyCustomerID(customer_id);
                foreach (var q in t)
                {

                    point = q.LoyaltyPointEarn + point;
                    pointused = q.LoyaltyPointUsed + pointused;

                }
                int pointcompare = point - pointused;
                if (pointcompare >= reward.Point) {
                    PointWalletSummary pointWallet = new PointWalletSummary();
                    pointWallet.LoyaltyPointUsed = reward.Point;
                    pointWallet.Activate = true;
                    pointWallet.CreateTime = DateTime.Now;
                    pointWallet.Reward_Name = reward.Reward_Name;
                    pointWallet.Reward_ID = reward.Id;
                    
                    pointcompare = reward.Point;
                    while (pointcompare != 0)
                    {

                        PointWallet check = _pointWalletService.GETAllPoint(customer_id);
                        if (check != null)
                        {
                            int pointcheck = check.LoyaltyPointEarn - check.LoyaltyPointUsed;
                            if (pointcheck > pointcompare)
                            {
                                check.LoyaltyPointUsed = check.LoyaltyPointUsed + pointcompare;
                                _pointWalletService.UpdatePoint(check);
                                pointcompare = 0;
                                pointWallet.Point_Wallet_ID.Add(check.Id);
                                _pointWalletSummaryService.AddSummary(pointWallet);
                            }
                            if (pointcheck <= pointcompare)
                            {
                                check.Activate = false;
                                pointcompare = pointcompare - pointcheck;
                                check.LoyaltyPointUsed = check.LoyaltyPointEarn;
                                _pointWalletService.UpdatePoint(check);
                                pointWallet.Point_Wallet_ID.Add(check.Id);

                            }
                            rewardID.AvailableQuantity -= 1;
                            reward.Customer_ID = customer_id;
                            reward.RedemptTime = DateTime.Now;
                            _rewardService.UpdateGift(rewardID);
                            _rewardIDService.UpdateRewardGift(reward);
                            return Json(new { statuscode = "successfully redeem page" });
                        }
                        return Json(new { statuscode = "no enough point" });
                    }
                }
                return Json(new { statuscode = "no enough point" });
            }
            return Json(new { statuscode = "wrong password" });
        }

        #endregion
    }
}
