using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Grand.Core.Domain.Loyalty;
using Grand.Core.Domain.LoyaltyAdmin;
using Grand.Services.Loyalty;
using Grand.Services.LoyaltyAdmin;
using System.Text;
using MongoDB.Bson;

namespace Grand.Web.Controllers
{
    public class ConsecutiveLoginLogController : BasePublicController
    {
        #region Fields
        
        private readonly IConsecutiveLoginLogService _ConsecutiveLoginLogService;
        private readonly ITokenService _TokenService;
        private readonly ILuckyDrawGiftManageService _LuckyDrawGiftManageService;
        private readonly ILuckyDrawGiftIDManageService _LuckyDrawGiftIDManageService;
        private readonly ILuckyDrawGiftGroupingManageService _LuckyDrawGiftGroupingManageService;

        #endregion

        #region Constructors
        public ConsecutiveLoginLogController(

                IConsecutiveLoginLogService ConsecutiveLoginLogService,
                ITokenService TokenService,
                ILuckyDrawGiftManageService LuckyDrawGiftManageService,
                ILuckyDrawGiftIDManageService LuckyDrawGiftIDManageService,
                ILuckyDrawGiftGroupingManageService LuckyDrawGiftGroupingManageService

            )
        {
            this._ConsecutiveLoginLogService = ConsecutiveLoginLogService;
            this._TokenService = TokenService;
            this._LuckyDrawGiftManageService = LuckyDrawGiftManageService;
            this._LuckyDrawGiftIDManageService = LuckyDrawGiftIDManageService;
            this._LuckyDrawGiftGroupingManageService = LuckyDrawGiftGroupingManageService;
        }

        #endregion

        #region Method

        #region Admin
        
        public ActionResult AdminAddLuckyDrawGift(string Gift_Name, string Gift_Description, int Quantity, string VoucherCategory)
        {
            LuckyDrawGiftManage addgift = new LuckyDrawGiftManage();
             
            addgift.Gift_Name = Gift_Name;
            addgift.Gift_Description = Gift_Description;
            addgift.Quantity = Quantity;
            
            addgift.CreateTime = DateTime.UtcNow;
            _LuckyDrawGiftManageService.AddLuckyDrawGift(addgift);
            
            for(int i = 1; i <= Quantity; i++)
            {
                LuckyDrawGiftIDManage addgiftID = new LuckyDrawGiftIDManage();
                addgiftID.Gift_Name = Gift_Name;
                addgiftID.Gift_Description = Gift_Description;
                addgiftID.VoucherCategory = VoucherCategory;
                addgiftID.Gift_Type_ID = addgift.Id;
                addgiftID.CreateTime = DateTime.UtcNow;
                _LuckyDrawGiftIDManageService.AddLuckyDrawGiftID(addgiftID);
            }

            return Json(new { status_code = 200 });
        }
        public ActionResult GroupingLuckyDrawGift(string Group_Name, DateTime Start_Time, DateTime End_Time, bool Activate, List<String> Gift_Type_IDs )
        {
            if (Activate == true)
            {
                if (Start_Time != null)
                {
                    LuckyDrawGiftGroupingManage GMS = _LuckyDrawGiftGroupingManageService.ValidGroupInfo(Start_Time, Activate);
                    if (GMS != null)
                    {
                        return Json(new { statuscode = "Conflict Start Time" });
                    }
                }
                if (End_Time != null)
                {
                    LuckyDrawGiftGroupingManage GME = _LuckyDrawGiftGroupingManageService.ValidGroupInfo(End_Time, Activate);
                    if (GME != null)
                    {
                        return Json(new { statuscode = "Conflict End Time" });
                    }
                }
            }
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            
            LuckyDrawGiftGroupingManage GM = new LuckyDrawGiftGroupingManage();
            GM.Group_Name = Group_Name;
            GM.Start_Time = Start_Time;
            GM.End_Time = End_Time;
            GM.Activate = Activate;
           
            _LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(new { statuscode=200 });
        }
        public ActionResult GroupingDateStartTimeValid(DateTime Start_Time,bool Activate, DateTime End_Time)
        {
            if (Activate == true)
            {
                if (Start_Time != null)
                {
                    LuckyDrawGiftGroupingManage GM = _LuckyDrawGiftGroupingManageService.ValidGroupInfo(Start_Time, Activate);
                    if (GM != null)
                    {

                    }
                }
                if (End_Time != null)
                {
                    LuckyDrawGiftGroupingManage GM = _LuckyDrawGiftGroupingManageService.ValidGroupInfo(End_Time, Activate);
                    if (GM != null)
                    {

                    }
                }
            }
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(new { statuscode = 200 });
        }
        public ActionResult GetAllGroupingInfo()
        {
            List<LuckyDrawGiftGroupingManage> GM = _LuckyDrawGiftGroupingManageService.GETAllGroupInfocn();

            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(new {GM });
        }
        public ActionResult ViewLuckyDrawGiftGroup()
        {
            //LuckyDrawGiftGroupingManage GM = _LuckyDrawGiftGroupingManageService.GETAllGroupInfo();
            //GM.Group_Name = Group_Name;
            //GM.Start_Time = Start_Time;
            //GM.End_Time = End_Time;
            //GM.Activate = Activate;
            //GM.Gift_Type_IDs = Gift_Type_IDs;
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(new { statuscode = 200 });
        }
        public ActionResult EditLuckyDrawGiftGroup(string Group_ID, string Group_Name, DateTime Start_Time, DateTime End_Time, bool Activate, List<String> Gift_Type_IDs)
        {
            LuckyDrawGiftGroupingManage GM = _LuckyDrawGiftGroupingManageService.GETGroupInfo(Group_ID);
            GM.Group_Name = Group_Name;
            GM.Start_Time = Start_Time;
            GM.End_Time = End_Time;
            GM.Activate = Activate;
           
            _LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(new { statuscode = 200 });
        }
        #endregion

        #region User
        #region Login Reward Module
        public ActionResult Showcheckin(string Customer_ID)
        {
            bool Expires = false;
            ConsecutiveLoginLog consecutive = _ConsecutiveLoginLogService.GETConsecutiveLoginInfoByCustomerIDwithExpires(Customer_ID, Expires);
            if (consecutive != null)
            {
                TimeSpan interval = DateTime.Today - consecutive.LastCheckIn;
                int i = interval.Days;
                if (i == 1)
                {
                    TimeSpan interval1 = consecutive.LastCheckIn - consecutive.CreateTime;
                    int j = interval1.Days;
                    if (j == 1)
                    {
                        return Json(new { consecutive.CheckIn_Info.Count, status_code = "Will allow customer to check in" });
                    }
                    if(j >= 2)
                    {
                        return Json(new { consecutive.CheckIn_Info.Count, status_code = "done check in" });
                    }
                }
                if (i >= 2)
                {
                    consecutive.Expires = true;
                    _ConsecutiveLoginLogService.UpdateConsecutiveLoginInfo(consecutive);
                    return Json(new { status_code = "No any data" });
                }
                return Json(new { status_code = 70 });
            }
          
            return Json(new { status_code = "No any data" });
        }
        public ActionResult Checkin(string Customer_ID)
        {

            bool Expires = false;
            ConsecutiveLoginLog consecutive = _ConsecutiveLoginLogService.GETConsecutiveLoginInfoByCustomerIDwithExpires(Customer_ID, Expires);
            if (consecutive == null)
            {
                ConsecutiveLoginLog log = new ConsecutiveLoginLog();
                log.Customer_ID = Customer_ID;
                log.CheckIn_Info.Add(new Check_In() { Day = "Day 1", CheckIn_Time = DateTime.Today });
                log.LastCheckIn = DateTime.Today;
                log.CreateTime = DateTime.Today;
                log.Expires = false;
               // consecutive.LastCheckInView = DateTime.UtcNow;
                _ConsecutiveLoginLogService.InsertCheckIn(log);
                return Json(new { status_code = 200 });
            }
            switch (consecutive.CheckIn_Info.Count) {
                case 1:
                    consecutive.CheckIn_Info.Add(new Check_In() { Day = "Day 2", CheckIn_Time = DateTime.Today });
                    consecutive.LastCheckIn = DateTime.Today;
                    
                    _ConsecutiveLoginLogService.UpdateConsecutiveLoginInfo(consecutive);
                    return Json(new { status_code = 2 });
                    
                case 2:
                    consecutive.CheckIn_Info.Add(new Check_In() { Day = "Day 3", CheckIn_Time = DateTime.Today });
                    consecutive.LastCheckIn = DateTime.Today;
                    
                    _ConsecutiveLoginLogService.UpdateConsecutiveLoginInfo(consecutive);
                    return Json(new { status_code = 3 });
                case 3:
                    consecutive.CheckIn_Info.Add(new Check_In() { Day = "Day 4", CheckIn_Time = DateTime.Today });
                    consecutive.LastCheckIn = DateTime.Today;
                   
                    _ConsecutiveLoginLogService.UpdateConsecutiveLoginInfo(consecutive);
                    return Json(new { status_code = 4 });
                case 4:
                    consecutive.CheckIn_Info.Add(new Check_In() { Day = "Day 5", CheckIn_Time = DateTime.Today });
                    consecutive.LastCheckIn = DateTime.Today;
                    
                    _ConsecutiveLoginLogService.UpdateConsecutiveLoginInfo(consecutive);
                    return Json(new { status_code = 5 });
                case 5:
                    consecutive.CheckIn_Info.Add(new Check_In() { Day = "Day 6", CheckIn_Time = DateTime.Today });
                    consecutive.LastCheckIn = DateTime.Today;
                    
                    _ConsecutiveLoginLogService.UpdateConsecutiveLoginInfo(consecutive);
                    return Json(new { status_code = 6 });
                case 6:
                    consecutive.CheckIn_Info.Add(new Check_In() { Day = "Day 7", CheckIn_Time = DateTime.Today });
                    consecutive.LastCheckIn = DateTime.Today;
                    
                    consecutive.Expires = true;
                    _ConsecutiveLoginLogService.UpdateConsecutiveLoginInfo(consecutive);

                    #region Create Token
                    //Random random = new Random();
                    Token tokenvalid = _TokenService.GETTokenByCustomerIDwithExpires(Customer_ID, false);
                    if (tokenvalid != null)
                    {
                        tokenvalid.Expire = true;
                        _TokenService.UpdateTokenInfo(tokenvalid);

                    }
                    Token token = new Token();
                    token.Customer_ID = Customer_ID;
                    token.CreateTime = DateTime.Today;
                    token.Expire = false;
                    //string token = ObjectId.GenerateNewId().ToString();                                                                                                                                                       
                    //string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                    //StringBuilder tokenID = new StringBuilder(30);

                    //for (int i = 0; i < 30; i++)
                    //{
                    //    tokenID.Append(characters[random.Next(characters.Length)]);
                    //}
                    //token.Token_ID = "sdsa";

                    _TokenService.CreateToken(token);
                    return Json(new { status_code = 7 });
                    #endregion
            }


            return Json(new { status_code = 201 });
        }
        #endregion
        #region Lucky draw Entry
        #region Valid customer had access to luckydraw
        public ActionResult ShowLuckyDrawGroupItem()
        {
            _LuckyDrawGiftGroupingManageService.GETAllGroupInfo(DateTime.Today);
            return Json(new { status_code = 7 });
        }
        public ActionResult LuckyDrawValid(string Customer_ID)
        {
            Token tokenvalid = _TokenService.GETTokenByCustomerIDwithExpires(Customer_ID, false);
            if (tokenvalid != null)
            {
                //LuckyDrawGiftIDManage
                tokenvalid.Expire = true;
                tokenvalid.Customer_ID = Customer_ID;
                tokenvalid.RedemptTime = DateTime.UtcNow;
                _TokenService.UpdateTokenInfo(tokenvalid);

            }
            return Json(new { status_code = 7 });
        }
        #endregion
        #region LuckyDraw Perform
        public ActionResult LuckyDraw(string Customer_ID)
        {
            Token tokenvalid = _TokenService.GETTokenByCustomerIDwithExpires(Customer_ID, false);
            if (tokenvalid != null)
            {
                
                tokenvalid.Gift_ID = "asd";
                tokenvalid.Expire = true;
                tokenvalid.RedemptTime = DateTime.UtcNow;
                _TokenService.UpdateTokenInfo(tokenvalid);
            }
            return Json(new { status_code = 7 });
        }
        #endregion
        #endregion
        #endregion

        #endregion
    }
}
