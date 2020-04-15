using Microsoft.AspNetCore.Mvc;
using System;
using Grand.Services.LoyaltyPoint;
using Grand.Core.Domain.LoyaltyPoint;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ZXing.QrCode;
using System.IO;
using ZXing;

namespace Grand.Web.Controllers
{
    public class LoyaltyPointModuleController : BasePublicController
    {
        #region Fields

        private readonly IPointWalletService _PointWalletService;


        #endregion

        #region Constructors
        public LoyaltyPointModuleController(

            IPointWalletService PointWalletService

            )
        {
            this._PointWalletService = PointWalletService;

        }

        #endregion

        #region Method
        public ActionResult LoyaltyPointCountRule(string Order_ID, string CustomerID,int amount)
        {
            
            PointWallet PW = new PointWallet();
            PW.Customer_ID = CustomerID;
            PW.Order_ID = Order_ID;
            PW.CreateTime = DateTime.UtcNow;
            PW.ExpiredTime = PW.CreateTime.AddYears(1);
            PW.Activate = true;
            PW.LoyaltyPointUsed = 0;


            DateTime CustomerBirtday =  new DateTime(1996, 4, 10);
            int dy = DateTime.Now.Day;
            int mn = DateTime.Now.Month;
            int Birthdaydy = CustomerBirtday.Day;
            int Birthdaymn = CustomerBirtday.Month;
            int Memberdaydy = CustomerBirtday.Day;
            int Memberdaymn = CustomerBirtday.Month;
            
            //Birthday 
            if(dy == Birthdaydy && mn == Birthdaymn)
            {
                PW.LoyaltyPointEarn = amount * 2;
                PW.Description = "Birthday";
                _PointWalletService.AddPoint(PW);
                return Json(new { statuscode = 200 });
                

            }
            else if(dy == Memberdaydy && mn == Memberdaymn)
            {
                PW.LoyaltyPointEarn = amount*2;
                PW.Description = "Member day";
                _PointWalletService.AddPoint(PW);
                return Json(new { statuscode = 200 });
            }
            else
            {
                PW.LoyaltyPointEarn = amount;
                PW.Description = "Basic";
                _PointWalletService.AddPoint(PW);
                return Json(new { statuscode = 200 });
            }
        }
        public ActionResult QrCode()
        {
           

            return Json(new{ statuscode = "404 no found" });
        }

        #endregion
    }
}