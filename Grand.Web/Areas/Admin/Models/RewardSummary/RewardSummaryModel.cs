using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using System;
using System.Collections.Generic;

namespace Grand.Web.Areas.Admin.Models.RewardSummary
{
    public partial class RewardSummaryModel : BaseGrandEntityModel
    {

        public RewardSummaryModel()
        {

        }

        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.Reward_Name")]
        public string Reward_Name { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.Reward_Description")]
        public string Reward_Description { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.RedemptTime")]
        public DateTime RedemptTime { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.Customer_ID")]
        public string Customer_ID { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.Reward_ID")]
        public string Reward_ID { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.CreateTime")]
        public DateTime CreateTime { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.Delete")]
        public bool Delete { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.Category")]
        public string Category { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.Point")]
        public string Point { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.PurchaseStartTime")]
        public string PurchaseStartTime { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.PurchaseEndTime")]
        public string PurchaseEndTime { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.StartTime")]
        public string StartTime { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.ExpiredTime")]
        public string ExpiredTime { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.Highlights")]
        public string Highlights { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.Term")]
        public string Term { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.Contact")]
        public string Contact { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.About")]
        public string About { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.Date")]
        public string Date { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.Url")]
        public string Url { get; set; }
        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.CurrentTime")]
        public DateTime CurrentTime { get; set; }

        [GrandResourceDisplayName("Admin.RewardID.RewardID.Fields.RewardRedemptTime")]
        public DateTime RewardRedemptTime { get; set; }
    }
}