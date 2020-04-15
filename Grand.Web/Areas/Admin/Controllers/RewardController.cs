using Grand.Core;
using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.LoyaltyAdmin;
using Grand.Core.Domain.Media;
using Grand.Core.Domain.Rewards;
using Grand.Framework.Kendoui;
using Grand.Framework.Mvc;
using Grand.Framework.Mvc.Filters;
using Grand.Framework.Security.Authorization;
using Grand.Services.Catalog;
using Grand.Services.Localization;
using Grand.Services.Media;
using Grand.Services.Rewards;
using Grand.Services.Security;
using Grand.Web.Areas.Admin.Extensions;
using Grand.Web.Areas.Admin.Models.Reward;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace Grand.Web.Areas.Admin.Controllers
{
    [PermissionAuthorize(PermissionSystemName.LoyaltyReward)]
    public partial class RewardController : BaseAdminController
    {
        #region Fields
        private readonly IRewardCategoryService _RewardCategoryService;
        private readonly IRewardIDService _RewardIDService;
        private readonly IRewardService _RewardService;
        private readonly ILocalizationService _localizationService;
        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;
        private readonly IPictureService _pictureService;
        private readonly MediaSettings _mediaSettings;
        #endregion

        #region Constructors

        public RewardController(
            IRewardCategoryService RewardCategoryService,
            IRewardIDService RewardIDService,
            IRewardService RewardService,
            ILocalizationService localizationService,
            IProductService productService,
            IWorkContext workContext,
            IPictureService pictureService,
            MediaSettings mediaSettings
        )
        {
            this._RewardCategoryService = RewardCategoryService;
            this._RewardIDService = RewardIDService;
            this._RewardService = RewardService;
            this._localizationService = localizationService;
            this._productService = productService;
            this._workContext = workContext;
            this._pictureService = pictureService;
            this._mediaSettings = mediaSettings;
        }

        #endregion

        #region Reward Gift

        #region Reward Gift Create
        public IActionResult Create()
        {
            var model = new RewardModel();
            var category = _RewardCategoryService.GETAllRewardCategory();
            foreach( var s in category) {
                model.selectedList.Add(new SelectListItem
                {
                    Text = s.Reward_Category_Name,
                    Value = s.Reward_Category_Name,
                });
            }
            //model.list2.Add(new SelectListItem
            //{
            //    Text = "1",
            //    Value = "1"
            //});
            //model.list2.Add(new SelectListItem
            //{
            //    Text = "2",
            //    Value = "2",
            //    Selected = true
            //});
            //model.list2.Add(new SelectListItem
            //{
            //    Text = "3",
            //    Value = "3"
            //});
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Create(RewardModel model, bool continueEditing)
        {

            if (model.Reward_Name == null)
            {
                ModelState.AddModelError("", "CreateFail, blank Reward Name Found");
            }

            if (ModelState.IsValid)
            {
                Reward R = new Reward();
                R.Reward_Name = model.Reward_Name;
                R.Reward_Description = model.Reward_Description;
                R.Point = model.Point;
                R.Activate = model.Activate;
                R.CreateTime = DateTime.UtcNow;
                R.Quantity = model.Quantity;
                R.Category = model.Category;
                R.StartTime = model.StartTime;
                R.ExpiredTime = model.ExpiredTime;
                R.PurchaseStartTime = model.PurchaseStartTime;
                R.PurchaseEndTime = model.PurchaseEndTime;
                R.Term = model.Term;
                R.Highlights = model.Highlights;
                R.Contact = model.Contact;
                R.About = model.About;
                
                //R.Reward_Picture2 = model.Reward_Picture2;
                //foreach(var s in model.selectedList)
                //    R.Category = s.Value.ToString();
                R.AvailableQuantity = model.Quantity;
                R.Delete = false;
                var picture = _pictureService.GetPictureById(model.Reward_Picture.PictureId);
                var url = _pictureService.GetPictureUrl(model.Reward_Picture.PictureId);

                if (picture != null)
                {
                    R.Reward_Picture = new RewardPicture()
                    {
                        PictureId = model.Reward_Picture.PictureId,
                        ProductId = R.Id,
                        Url = url
                    };
                }
                _RewardService.AddRewardGift(R);

               
                for (int i = 1; i <= R.Quantity; i++)
                {
                    RewardID RID = new RewardID();
                    RID.Reward_Name = model.Reward_Name;
                    RID.Reward_Description = model.Reward_Description;
                    RID.Point = model.Point;
                    RID.Category = model.Category;
                    RID.Delete = false;
                    RID.CreateTime = DateTime.UtcNow;
                    RID.StartTime = model.StartTime;
                    RID.ExpiredTime = model.ExpiredTime;
                    RID.PurchaseStartTime = model.PurchaseStartTime;
                    RID.PurchaseEndTime = model.PurchaseEndTime;
                    
                    RID.Url = url;
                    RID.Term = model.Term;
                    RID.Highlights = model.Highlights;
                    RID.Contact = model.Contact;
                    RID.About = model.About;
                    RID.RewardRedemptTime = default(DateTime);
                    RID.RedemptTime = default(DateTime);
                    RID.Reward_ID = R.Id;
                    _RewardIDService.AddRewardGiftID(RID);
                }

                SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = model.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form

            return View(model);
        }

        #endregion

        #region Reward Gift List
        public IActionResult List() => View();

        [HttpPost]
        public IActionResult List(DataSourceRequest command)
        {
            var Reward = _RewardService.GETAllRewardGiftInfo();

            var gridModel = new DataSourceResult
            {
                Data = Reward.Select(x =>
                {
                    var model = x.ToModel();
                    model.Reward_Picture = new RewardPictureModel();
                    model.Reward_Picture.Url = "http://wahlee.m360.com.my/content/images/thumbs/default-image_75.png";
                    if (x.Reward_Picture != null)
                        if(x.Reward_Picture.Url != null)
                            model.Reward_Picture.Url = x.Reward_Picture.Url;
                    return model;
                }),
                Total = Reward.Count
            };
            
            return Json(gridModel);
        }
        #endregion

        #region Reward Gift Edit
        public IActionResult Edit(string id)
        {
            var RGift = _RewardService.GETGiftInfo(id);
            if (RGift == null)
                //No customer role found with the specified id
                return RedirectToAction("List");


            var model = RGift.ToModel();
            var category = _RewardCategoryService.GETAllRewardCategory();
            foreach (var s in category)
            {
                model.selectedList.Add(new SelectListItem
                {
                    Text = s.Reward_Category_Name,
                    Value = s.Reward_Category_Name,
                });
            }

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public IActionResult Edit(RewardModel model, bool continueEditing)
        {
            var UpdateGift = _RewardService.GETGiftInfo(model.Id);
            if (UpdateGift == null)
                //No customer role found with the specified id
                return RedirectToAction("List");
            var cal = UpdateGift.Quantity - UpdateGift.AvailableQuantity;
            if (model.Quantity < cal)
            {
                return RedirectToAction("Edit");
            }
            try
            {
                if (ModelState.IsValid)
                {

                    UpdateGift.Reward_Name = model.Reward_Name;
                    UpdateGift.Reward_Description = model.Reward_Description;
                    UpdateGift.Quantity = model.Quantity;
                    UpdateGift.Point = model.Point;
                    UpdateGift.PurchaseStartTime = model.PurchaseStartTime;
                    UpdateGift.PurchaseEndTime = model.PurchaseEndTime;
                    UpdateGift.Category = model.Category;
                    UpdateGift.StartTime = model.StartTime;
                    UpdateGift.ExpiredTime = model.ExpiredTime;
                    UpdateGift.Activate = model.Activate;
                    
                   
                    //Update Picture
                    var picture = _pictureService.GetPictureById(model.Reward_Picture.PictureId);
                    var url = _pictureService.GetPictureUrl(model.Reward_Picture.PictureId);
                    
                    if (picture != null)
                    {
                        UpdateGift.Reward_Picture = new RewardPicture()
                        {
                            PictureId = model.Reward_Picture.PictureId,
                            ProductId = UpdateGift.Id,
                            Url = url
                        };
                    }
                    if (picture == null)
                    {
                        UpdateGift.Reward_Picture = new RewardPicture()
                        {

                        };
                    }


                    _RewardService.UpdateGift(UpdateGift);
                    SuccessNotification(_localizationService.GetResource("Admin.Reward.Reward.Updated")); //pop out info
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

        #region Reward Gift Delete
        [HttpPost]
        public IActionResult Delete(string id)
        {

            var Reward = _RewardService.GETGiftInfo(id);
            if (Reward == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    Reward.Delete = true;
                    _RewardService.UpdateGift(Reward);

                    for (int i = 1; i <= Reward.Quantity; i++)
                    {
                        var RewardID = _RewardIDService.GETRewardGiftIDbyRewardMainID(id);
                        RewardID.Delete = true;
                        _RewardIDService.UpdateRewardGift(RewardID);
                    }

                    SuccessNotification(_localizationService.GetResource("Admin.Customers.CustomerTags.Deleted"));
                    return RedirectToAction("List");
                }
                ErrorNotification(ModelState);
                return RedirectToAction("List", new { id = Reward.Id });
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = Reward.Id });
            }


        }
        #endregion

        #region Reward Gift pictures
        public IActionResult ProductPictureAdd(string pictureId, string productId)
        {
            if (String.IsNullOrEmpty(pictureId))
                throw new ArgumentException();

            var picture = _pictureService.GetPictureById(pictureId);

            var reward = new Reward();
            if (picture != null)
            {


            }

            _RewardService.AddRewardGift(reward);

            return Json(new { Result = true });
        }

        [HttpPost]
        public IActionResult ProductPictureUpdate(ProductPicture model)
        {
            //_productViewModelService.UpdateProductPicture(model);
            return new NullJsonResult();
        }

        [HttpPost]
        public IActionResult ProductPictureDelete(ProductPicture model)
        {
            //_productViewModelService.DeleteProductPicture(model);
            return new NullJsonResult();
        }
        #endregion

        #endregion

    }
}
