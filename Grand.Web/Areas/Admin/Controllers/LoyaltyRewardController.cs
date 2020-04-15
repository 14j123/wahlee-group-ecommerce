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
using Grand.Web.Areas.Admin.Extensions;
using Grand.Web.Areas.Admin.Models.LuckyDrawGift;
using Grand.Web.Areas.Admin.Models.LuckyDrawGiftGroupingManage;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Grand.Web.Areas.Admin.Models.LuckyDrawGiftID;
using Grand.Framework.Mvc;
using Grand.Services.Catalog;
using Grand.Core.Domain.Catalog;

namespace Grand.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.LoyaltyReward)]
    public partial class LoyaltyRewardController : BaseAdminController
    {
        #region Fields

  
        private readonly ILuckyDrawGiftGroupingManageService _luckyDrawGiftGroupingManageService;
        private readonly ILuckyDrawGiftManageService _luckyDrawGiftManageService;
        private readonly ILuckyDrawGiftIDManageService _luckyDrawGiftIDManageService;
        private readonly ILocalizationService _localizationService;
        private readonly IProductService _productService;
        #endregion

        #region Constructors

        public LoyaltyRewardController(
    
           ILuckyDrawGiftGroupingManageService luckyDrawGiftGroupingManageService,
           ILuckyDrawGiftManageService luckyDrawGiftManageService,
           ILuckyDrawGiftIDManageService luckyDrawGiftIDManageService,
           ILocalizationService localizationService,
           IProductService productService
        )
        {

            this._luckyDrawGiftGroupingManageService = luckyDrawGiftGroupingManageService;
            this._luckyDrawGiftManageService = luckyDrawGiftManageService;
            this._luckyDrawGiftIDManageService = luckyDrawGiftIDManageService;
            this._localizationService = localizationService;
            this._productService = productService;
        }


        #endregion

        #region Gift Grouping
        
        #region Gift Grouping Create
        public IActionResult Create()
        {
            var model = new LuckyDrawGiftGroupingManageModel();
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Create(LuckyDrawGiftGroupingManageModel model, bool continueEditing)
        {
        
             if (model.Group_Name == null)
            {           
                ModelState.AddModelError("", "CreateFail, blank Group Name Found");
            }

            if (model.Start_Time == model.End_Time)
            {
                ModelState.AddModelError("", "CreateFail, blank Group Name Found 11111");
            }

            if (model.Start_Time > model.End_Time)
            {
                ModelState.AddModelError("", "CreateFail, blank Group Name Found 1111111111111");
            }

            if (ModelState.IsValid)
            {
                LuckyDrawGiftGroupingManage GM = new LuckyDrawGiftGroupingManage();
                GM.Group_Name = model.Group_Name;
                GM.Start_Time = model.Start_Time;
                GM.End_Time = model.End_Time;
                GM.Activate = model.Activate;
               
                GM.Delete = false;
                if (model.Activate == true)
                {
                    if (model.Start_Time != null)
                    {
                        LuckyDrawGiftGroupingManage GMS = _luckyDrawGiftGroupingManageService.ValidGroupInfo(model.Start_Time, model.Activate);
                        if (GMS != null)
                        {
                            return continueEditing ? RedirectToAction("Create") : RedirectToAction("Create", new { id = model.Id });
                        }
                    }
                    if (model.End_Time != null)
                    {
                        LuckyDrawGiftGroupingManage GME = _luckyDrawGiftGroupingManageService.ValidGroupInfo(model.End_Time, model.Activate);
                        if (GME != null)
                        {
                            return continueEditing ? RedirectToAction("Create") : RedirectToAction("Create", new { id = model.Id });
                        }
                    }
                }
                _luckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
                SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = GM.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form

            return View(model);
        }

        #endregion
        
        #region Gift Grouping List
        public IActionResult List() => View();

        [HttpPost]
        public IActionResult List(DataSourceRequest command)
        {
            var GM = _luckyDrawGiftGroupingManageService.GETAllGroupInfocn();

            var gridModel = new DataSourceResult
            {
                Data = GM.ToList(),
                Total = GM.Count
            };
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(gridModel);
        }
        #endregion
              
        #region Gift Grouping Edit
        public IActionResult Edit(string id)
        {
            var GroupInfo = _luckyDrawGiftGroupingManageService.GETGroupInfo(id);
            if (GroupInfo == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            var model = GroupInfo.ToModel();
            var category = _luckyDrawGiftManageService.GETAllActiveGiftInfo();
            
            foreach (var s in category)
            {
                LuckyDrawGiftIDManage ID = _luckyDrawGiftIDManageService.GETGiftInfowithGiftTypeIDANDGroupID(s.Id, id);
                if(ID == null) {
                    var AvailableID = _luckyDrawGiftManageService.GETGiftInfoById(s.Id);
                    if (AvailableID.Available_Quantity > 0 ) {
                        
                        model.selectedList.Add(new SelectListItem
                        {
                            Text = s.Gift_Name,
                            Value = s.Id,
                        });
                        if (model.Quantity == 0)
                        {
                            
                            model.Quantity = AvailableID.Available_Quantity;
                        }
                    }
                }
            }

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Edit(LuckyDrawGiftGroupingManageModel model, bool continueEditing)
        {
            var customertag = _luckyDrawGiftGroupingManageService.GETGroupInfo(model.Id);
            if (customertag == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    customertag.Activate = model.Activate;
                    _luckyDrawGiftGroupingManageService.UpdateGroupInfo(customertag);
                    SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Updated")); //pop out info
                    return continueEditing ? RedirectToAction("Edit", new { id = customertag.Id }) : RedirectToAction("List");
                }

                //If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = customertag.Id });
            }
        }
        #endregion

        #region Gift Grouping Delete
        [HttpPost]
        public IActionResult Delete(string id)
        {

            var GiftGrouping = _luckyDrawGiftGroupingManageService.GETGroupInfo(id);
            if (GiftGrouping == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    GiftGrouping.Delete = true;
                    _luckyDrawGiftGroupingManageService.DeleteGiftGrouping(GiftGrouping);
                    SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Deleted"));
                    return RedirectToAction("List");
                }
                ErrorNotification(ModelState);
                return RedirectToAction("Edit", new { id = GiftGrouping.Id });
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = GiftGrouping.Id });
            }


        }
        #endregion

        #region Active Gift List


        [HttpPost]
        public IActionResult ListActiveGift(DataSourceRequest command)
        {
            var GM = _luckyDrawGiftManageService.GetAllAvailablewithActive();

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
        [HttpPost]
        public ActionResult GetQuantity(string GiftID)
        {
            LuckyDrawGiftManage LDGM = _luckyDrawGiftManageService.GETGiftInfoById(GiftID);

            return Json(new { LDGM.Available_Quantity });

          
        }
        [HttpPost]
        public IActionResult AddGift(DataSourceRequest command,string GiftID, int Quantity, string accountId)
        {

            Product pro = _productService.GetLDGiftInfo(GiftID);
            //Coupon haven't done 

            LuckyDrawGiftManage LDGM = _luckyDrawGiftManageService.GETGiftInfo(GiftID);
            
            if (LDGM.Remaining >= Quantity) {
                LDGM.Available_Quantity = LDGM.Available_Quantity - Quantity;
                _luckyDrawGiftManageService.UpdateGift(LDGM);
                _luckyDrawGiftManageService.UpdateGift(LDGM);
                for (int i = 0; i < Quantity; i++)
                {
                    
                    LuckyDrawGiftIDManage LDGMID = _luckyDrawGiftIDManageService.GETGiftInfowithGiftTypeID(GiftID);
                    LDGMID.Group_ID = accountId;
                    _luckyDrawGiftIDManageService.UpdateVoucherID(LDGMID);
                }
                LuckyDrawGiftGroupingManage LDGGM = _luckyDrawGiftGroupingManageService.GETGroupInfo(accountId);
                LDGGM.Gift.Add(new Gifts{
                    Gift_Name = GiftID,
                    Quantity = Quantity
                });
                _luckyDrawGiftGroupingManageService.UpdateGroupInfo(LDGGM);
                //
                var GroupInfo = _luckyDrawGiftGroupingManageService.GETGroupInfo(accountId);
                if (GroupInfo == null)
                    //No customer role found with the specified id
                    return RedirectToAction("List");

                var model = GroupInfo.ToModel();
                var category = _luckyDrawGiftManageService.GETAllActiveGiftInfo();

                foreach (var s in category)
                {
                    LuckyDrawGiftIDManage ID = _luckyDrawGiftIDManageService.GETGiftInfowithGiftTypeIDANDGroupID(s.Id, accountId);
                    if (ID == null)
                    {
                        var AvailableID = _luckyDrawGiftManageService.GETGiftInfoById(s.Id);
                        if (AvailableID.Available_Quantity > 0)
                        {

                            model.selectedList.Add(new SelectListItem
                            {
                                Text = s.Gift_Name,
                                Value = s.Id,
                            });
                            if (model.Quantity == 0)
                            {

                                model.Quantity = AvailableID.Available_Quantity;
                            }
                        }
                    }
                }
                return Json(new { model.selectedList });
            }
            return Json(new { statusCode = 200 });
        }

        //[HttpPost]
        //public ActionResult SifuCommentAdd(string CustId, string Comment, int Rating, string accountId)
        //{

        //    var CustName = _accService.GetAccountById(CustId);

        //    var newComment = new SifuComment
        //    {
        //        Id = accountId,
        //        custId = CustId,
        //        comment = Comment,
        //        rating = Rating,
        //        custName = CustName.name,
        //        commentDate = DateTime.UtcNow
        //    };
        //    _customerService.InsertSifuComment(newComment, accountId);
        //    _customerService.UpdateRating(accountId);

        //    return Json(new { Result = true });
        //}

        #region Resource
        [HttpPost]
        public IActionResult Resources(DataSourceRequest command, LuckyDrawGiftModel IDs)
        {
            
            LuckyDrawGiftGroupingManage subresources = _luckyDrawGiftGroupingManageService.LuckyDrawGiftGroupingResourceModel(IDs.Id, command.Page, command.PageSize);
            var resources = subresources.Gift.ToList();
            var model = new List<LuckyDrawGiftGroupingManageModel>();
            foreach(var a in resources)
            {
                model.Add(new LuckyDrawGiftGroupingManageModel
                {
                    Gift_Name = a.Gift_Name,
                    Quantity = a.Quantity
                });
            }
            
            var gridModel = new DataSourceResult
            {
                Data = model.ToList(),
                Total = model.Count
            };

            return Json(gridModel);
        }

        //[HttpPost]
        //public IActionResult ResourceUpdate(LuckyDrawGiftIDModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
        //    }
        //    LuckyDrawGiftIDManage LDGID = _LuckyDrawGiftIDManageService.GETGiftInfo(model.Id);
        //    LDGID.VoucherNumber = model.VoucherNumber;
        //    _LuckyDrawGiftIDManageService.UpdateVoucherID(LDGID);

        //    return new NullJsonResult();
        //}

        //[HttpPost]
        //public IActionResult ResourceAdd(LuckyDrawGiftModel IDs, LuckyDrawGiftIDModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
        //    }
        //    LuckyDrawGiftIDManage LDGID = new LuckyDrawGiftIDManage();
        //    LDGID.Gift_Type_ID = model.Gift_Type_ID;
        //    LDGID.VoucherNumber = model.VoucherNumber;
        //    LDGID.Gift_Name = model.Gift_Name;
        //    LDGID.VoucherCategory = model.VoucherCategory;
        //    LDGID.Gift_Description = model.Gift_Description;
        //    LDGID.CreateTime = DateTime.Now;
        //    LuckyDrawGiftManage LDG = _luckyDrawGiftManageService.GETGiftInfo(model.Gift_Type_ID);

        //    LDG.Quantity += 1;
        //    LDG.Remaining += 1;
        //    _luckyDrawGiftManageService.UpdateGift(LDG);
        //    _LuckyDrawGiftIDManageService.InsertVoucherID(LDGID);
        //    //if (result.error)
        //    //{
        //    //    return ErrorForKendoGridJson(result.message);
        //    //}
        //    return new NullJsonResult();
        //}

        [HttpPost]
        public IActionResult ResourceDelete(LuckyDrawGiftGroupingManageModel model)
        {
            
            //LuckyDrawGiftIDManage LDGID = _LuckyDrawGiftIDManageService.GETGiftInfo(model.Id);
            //_LuckyDrawGiftIDManageService.DeleteGiftID(LDGID);
            //string test = model.Gift_Type_ID.ToString();
            //LuckyDrawGiftManage LDG = _luckyDrawGiftManageService.GETGiftInfo(test);
            //LDG.Quantity -= 1;
            //LDG.Remaining -= 1;
            //_luckyDrawGiftManageService.UpdateGift(LDG);
            return new NullJsonResult();
        }
        #endregion

    }
}
