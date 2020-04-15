using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using System;
using System.Collections.Generic;

namespace Grand.Web.Areas.Admin.Models.PointWalletSummary
{
    public partial class PointWalletSummaryModel : BaseGrandEntityModel
    {
        [GrandResourceDisplayName("Admin.PointWalletSummary.PointWalletSummary.Fields.LoyaltyPointUsed")]
        public int LoyaltyPointUsed { get; set; }
        [GrandResourceDisplayName("Admin.PointWalletSummary.PointWalletSummary.Fields.Activate")]
        public bool Activate { get; set; }
        [GrandResourceDisplayName("Admin.PointWalletSummary.PointWalletSummary.Fields.Point_Transaction_ID")]
        public string Point_Transaction_ID { get; set; }
        [GrandResourceDisplayName("Admin.PointWalletSummary.PointWalletSummary.Fields.CreateTime")]
        public DateTime CreateTime { get; set; }
        [GrandResourceDisplayName("Admin.PointWalletSummary.PointWalletSummary.Fields.Reward_ID")]
        public List<String> Reward_ID { get; set; }
        
    }
}