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
using Grand.Web.Areas.Admin.Models.LuckyDrawGiftID;
using Grand.Framework.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Grand.Services.Orders;

namespace Grand.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.LoyaltyReward)]
    public partial class LuckyDrawGiftController : BaseAdminController
    {
        #region Fields

        private readonly ILuckyDrawGiftIDManageService _LuckyDrawGiftIDManageService;
        private readonly ILuckyDrawGiftManageService _luckyDrawGiftManageService;
        private readonly ILocalizationService _localizationService;
        private readonly IGiftCardService _giftCardService;

        #endregion

        #region Constructors

        public LuckyDrawGiftController(

            ILuckyDrawGiftIDManageService LuckyDrawGiftIDManageService,
            ILuckyDrawGiftManageService luckyDrawGiftManageService,
            ILocalizationService localizationService,
            IGiftCardService giftCardService
        )
        {
            this._LuckyDrawGiftIDManageService = LuckyDrawGiftIDManageService;
            this._luckyDrawGiftManageService = luckyDrawGiftManageService;
            this._localizationService = localizationService;
            this._giftCardService = giftCardService;

        }
        
        #endregion

        #region Gift 
        
        #region Gift Create
        public IActionResult Create()
        {
            var model = new LuckyDrawGiftModel();
            model.selectedList.Add(new SelectListItem
            {
                Text = "Auto Generate Voucher",
                Value = "1",
                Selected = true
            });
            model.selectedList.Add(new SelectListItem
            {
                Text = "Customize Voucher",
                Value = "2",
                
            });
            model.selectedList.Add(new SelectListItem
            {
                Text = "Purchased Voucher",
                Value = "3"
            });
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Create(LuckyDrawGiftModel model, bool continueEditing)
        {
        
             if (model.Gift_Name == null)
            {           
                ModelState.AddModelError("", "CreateFail, blank Group Name Found");
            }

            if (ModelState.IsValid)
            {
                LuckyDrawGiftManage GM = new LuckyDrawGiftManage();
                GM.Gift_Name = model.Gift_Name;
                GM.Gift_Description = model.Gift_Description;
                
                GM.Activate = model.Activate;
                GM.CreateTime = DateTime.UtcNow;
                
                GM.VoucherCategory = model.VoucherCategory;
                if (GM.VoucherCategory == "1" || GM.VoucherCategory == "3")
                {
                    GM.Available_Quantity = model.Quantity;
                    GM.Quantity = model.Quantity;
                    GM.Remaining = model.Quantity;
                    GM.Delete = false;
                    for (int i = 1; i <= GM.Quantity; i++)
                    {
                        LuckyDrawGiftIDManage addgiftID = new LuckyDrawGiftIDManage();
                        addgiftID.Gift_Name = model.Gift_Name;
                        addgiftID.Gift_Description = model.Gift_Description;
                        addgiftID.VoucherCategory = model.VoucherCategory;
                        addgiftID.Gift_Type_ID = GM.Id;
                        addgiftID.VoucherNumber = _giftCardService.GenerateGiftCardCode();
     
                        addgiftID.CreateTime = DateTime.UtcNow;
                        _LuckyDrawGiftIDManageService.AddLuckyDrawGiftID(addgiftID);
                    }
                }

                GM.Redempted = 0;

                _luckyDrawGiftManageService.AddLuckyDrawGift(GM);
                SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
                
            }

            //If we got this far, something failed, redisplay form

            return View(model);
        }

        #endregion
        public IActionResult GenerateCouponCode()
        {
            return Json(new { CouponCode = _giftCardService.GenerateGiftCardCode() });
        }
        #region Gift List
        public IActionResult List() => View();

        [HttpPost]
        public IActionResult List(DataSourceRequest command)
        {
            var GM = _luckyDrawGiftManageService.GETAllGiftInfo();

            var gridModel = new DataSourceResult
            {
                Data = GM.ToList(),
                Total = GM.Count
            };
            //_LuckyDrawGiftGroupingManageService.GroupingGiftProduct(GM);
            return Json(gridModel);
        }
        #endregion

        #region Gift Edit
        public IActionResult Edit(string id)
        {
            var luckyDrawGift = _luckyDrawGiftManageService.GETGiftInfo(id);
            if (luckyDrawGift == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            var model = luckyDrawGift.ToModel();

            model.selectedList.Add(new SelectListItem
            {
                Text = "Auto Generate Voucher",
                Value = "1"
            });
            model.selectedList.Add(new SelectListItem
            {
                Text = "Customize Voucher",
                Value = "2"
            });
            model.selectedList.Add(new SelectListItem
            {
                Text = "Purchased Voucher",
                Value = "3"
            });

            return View(model);
        }



        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Edit(LuckyDrawGiftManage model, bool continueEditing)
        {
            var UpdateGift = _luckyDrawGiftManageService.GETGiftInfo(model.Id);
            if (UpdateGift == null)
                //No customer role found with the specified id
                return RedirectToAction("List");
            if (UpdateGift.Redempted > model.Quantity)
            {
                return RedirectToAction("Edit");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    UpdateGift.Gift_Name = model.Gift_Name;
                    UpdateGift.Gift_Description = model.Gift_Description;
                    
                    UpdateGift.Quantity = model.Quantity;
                    UpdateGift.Activate = model.Activate;

                    _luckyDrawGiftManageService.UpdateGift(UpdateGift);
                    SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Updated")); //pop out info
                    return continueEditing ? RedirectToAction("Edit", new { id = UpdateGift.Id }) : RedirectToAction("List");
                }

                //If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = UpdateGift.Id });
            }
        }
        #endregion

        #region Gift Delete
        [HttpPost]
        public IActionResult Delete(string id)
        {

            var Gift = _luckyDrawGiftManageService.GETGiftInfo(id);
            if (Gift == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    Gift.Delete = true;
                    _luckyDrawGiftManageService.DeleteGift(Gift);
                    
                    for (int i = 1; i <= Gift.Quantity; i++)
                    {
                        var addgiftID = _LuckyDrawGiftIDManageService.GETGiftInfo(id);
                        addgiftID.Delete = true;
                        _LuckyDrawGiftIDManageService.DeleteGiftID(addgiftID);
                        
                        
                    }
                    


                    SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Deleted"));
                    return RedirectToAction("List");
                }
                ErrorNotification(ModelState);
                return RedirectToAction("List", new { id = Gift.Id });
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = Gift.Id });
            }


        }
        #endregion

        #region Resource
        [HttpPost]
        public IActionResult Resources(DataSourceRequest command, LuckyDrawGiftModel IDs)
        {
            
            var resources = _LuckyDrawGiftIDManageService.PrepareLuckyDrawGiftIDResourceModel( IDs.Id , command.Page, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = resources.ToList(),
                Total = resources.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public IActionResult ResourceUpdate(LuckyDrawGiftIDModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }
            LuckyDrawGiftIDManage LDGID = _LuckyDrawGiftIDManageService.GETGiftInfo(model.Id);
            LDGID.VoucherNumber = model.VoucherNumber;
            _LuckyDrawGiftIDManageService.UpdateVoucherID(LDGID);

            return new NullJsonResult();
        }

        [HttpPost]
        public IActionResult ResourceAdd(LuckyDrawGiftModel IDs,LuckyDrawGiftIDModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });
            }
            LuckyDrawGiftIDManage LDGID = new LuckyDrawGiftIDManage();
            LDGID.Gift_Type_ID = model.Gift_Type_ID;
            LDGID.VoucherNumber = model.VoucherNumber;
            LDGID.Gift_Name = model.Gift_Name;
            LDGID.VoucherCategory = model.VoucherCategory;
            LDGID.Gift_Description = model.Gift_Description;
            LDGID.CreateTime = DateTime.Now;
            LuckyDrawGiftManage LDG = _luckyDrawGiftManageService.GETGiftInfo(model.Gift_Type_ID);
            LDG.Available_Quantity += 1;
            LDG.Quantity += 1;
            LDG.Remaining += 1;
            _luckyDrawGiftManageService.UpdateGift(LDG);
            _LuckyDrawGiftIDManageService.InsertVoucherID(LDGID);
            //if (result.error)
            //{
            //    return ErrorForKendoGridJson(result.message);
            //}
            return new NullJsonResult();
        }

        [HttpPost]
        public IActionResult ResourceDelete(LuckyDrawGiftIDModel model)
        {
            LuckyDrawGiftIDManage LDGID = _LuckyDrawGiftIDManageService.GETGiftInfo(model.Id);
            _LuckyDrawGiftIDManageService.DeleteGiftID(LDGID);
            string test = model.Gift_Type_ID.ToString();
            LuckyDrawGiftManage LDG = _luckyDrawGiftManageService.GETGiftInfo(test);
            LDG.Quantity -= 1;
            LDG.Remaining -= 1;
            LDG.Available_Quantity -= 1;
            _luckyDrawGiftManageService.UpdateGift(LDG);
            return new NullJsonResult();
        }
        #endregion
        #endregion


    }
}
