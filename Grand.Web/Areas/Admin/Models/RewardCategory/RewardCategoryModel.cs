using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grand.Web.Areas.Admin.Models.RewardCategory
{
    public partial class RewardCategoryModel : BaseGrandEntityModel
    {
        public RewardCategoryModel()
        {

        }
        [GrandResourceDisplayName("Admin.RewardCategory.RewardCategory.Fields.Reward_Category_Name")]
        public string Reward_Category_Name { get; set; }
        [GrandResourceDisplayName("Admin.RewardCategory.RewardCategory.Fields.Reward_Description")]
        public string Reward_Description { get; set; }
        [GrandResourceDisplayName("Admin.RewardCategory.RewardCategory.Fields.Reward_Item")]
        public string Reward_Item { get; set; }
        [GrandResourceDisplayName("Admin.RewardCategory.RewardCategory.Fields.CreateTime")]
        public DateTime CreateTime { get; set; }
        [GrandResourceDisplayName("Admin.RewardCategory.RewardCategory.Fields.Delete")]
        public bool Delete { get; set; }
        
    }
}