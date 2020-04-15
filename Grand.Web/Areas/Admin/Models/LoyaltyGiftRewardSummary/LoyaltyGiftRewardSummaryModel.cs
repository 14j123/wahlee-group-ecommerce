using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using System;
using System.Collections.Generic;

namespace Grand.Web.Areas.Admin.Models.LoyaltyGiftRewardSummary
{
    public partial class LoyaltyGiftRewardSummaryModel : BaseGrandEntityModel
    {

        [GrandResourceDisplayName("Admin.LoyaltyGiftRewardSummary.LoyaltyGiftRewardSummary.Fields.Gift_Name")]
        public string Gift_Name { get; set; }
        [GrandResourceDisplayName("Admin.LoyaltyGiftRewardSummary.LoyaltyGiftRewardSummary.Fields.Gift_Description")]
        public string Gift_Description { get; set; }
        [GrandResourceDisplayName("Admin.LoyaltyGiftRewardSummary.LoyaltyGiftRewardSummary.Fields.Priority")]
        public int Priority { get; set; }
        [GrandResourceDisplayName("Admin.LoyaltyGiftRewardSummary.LoyaltyGiftRewardSummary.Fields.RedemptTime")]
        public DateTime RedemptTime { get; set; }
        [GrandResourceDisplayName("Admin.LoyaltyGiftRewardSummary.LoyaltyGiftRewardSummary.Fields.Customer_ID")]
        public string Customer_ID { get; set; }
        [GrandResourceDisplayName("Admin.LoyaltyGiftRewardSummary.LoyaltyGiftRewardSummary.Fields.Gift_Type_ID")]
        public string Gift_Type_ID { get; set; }
        [GrandResourceDisplayName("Admin.LoyaltyGiftRewardSummary.LoyaltyGiftRewardSummary.Fields.CreateTime")]
        public DateTime CreateTime { get; set; }
        
    }
}