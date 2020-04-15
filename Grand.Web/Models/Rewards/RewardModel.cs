using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Media;
using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Grand.Web.Models.Rewards
{
    public partial class RewardModel : BaseGrandEntityModel
    {
        public RewardModel()
        {
            
            this.selectedList = new List<SelectListItem>();
            this.selectedList2 = new List<string>();
            this.list2 = new List<SelectListItem>();
        }

        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Reward_Category_Name")]
        [UIHint("MultiSelect")]
        public List<string> selectedList2 { get; set; }
        public List<SelectListItem> list2 { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Reward_Picture1")]
        public Picture Reward_Picture1 { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Reward_Picture2")]
        public RewardPictureModel Reward_Picture2 { get; set; }
        //public string RP_URL { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Category")]
        public string Category { get; set; }
        public List<SelectListItem> selectedList { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Reward_Name")]
        public string Reward_Name { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Reward_Description")]
        public string Reward_Description { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Quantity")]
        public int Quantity { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Redempted")]
        public int Redempted { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Activate")]
        public bool Activate { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.CreateTime")]
        public DateTime CreateTime { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Delete")]
        public bool Delete { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Redempt_StartDate")]
        public DateTime Redempt_StartDate { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Redempt_EndDate")]
        public DateTime Redempt_EndDate { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Point")]
        public string Point { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Point")]
        public string Highlights { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Point")]
        public string Term { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Point")]
        public string Contact { get; set; }
        [GrandResourceDisplayName("Admin.Reward.Reward.Fields.Point")]
        public string About { get; set; }

    }

    public partial class RewardPictureModel : BaseGrandEntityModel
    {
        [GrandResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.ProductId")]
        public string ProductId { get; set; }

        [UIHint("Picture")]
        [GrandResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.Picture")]
        public string PictureId { get; set; }

        [GrandResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.Picture")]
        public string PictureUrl { get; set; }

        //[GrandResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.DisplayOrder")]
        //public int DisplayOrder { get; set; }

        [GrandResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.OverrideAltAttribute")]

        public string OverrideAltAttribute { get; set; }

        [GrandResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.OverrideTitleAttribute")]

        public string OverrideTitleAttribute { get; set; }
        [GrandResourceDisplayName("Admin.Catalog.Products.Pictures.Fields.Url")]
        public string Url { get; set; }
    }
}