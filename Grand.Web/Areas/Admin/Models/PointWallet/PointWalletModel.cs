using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Grand.Web.Areas.Admin.Models.PointWallet 
{
    public partial class PointWalletModel : BaseGrandEntityModel
    {
        public PointWalletModel()
        {
            this.selectedList = new List<SelectListItem>();
            
        }

        [GrandResourceDisplayName("Admin.PointWallet.PointWallet.Fields.LoyaltyPointEarn")]
        public int LoyaltyPointEarn { get; set; }
        [GrandResourceDisplayName("Admin.PointWallet.PointWallet.Fields.LoyaltyPointUsed")]
        public int LoyaltyPointUsed { get; set; }
        [GrandResourceDisplayName("Admin.PointWallet.PointWallet.Fields.Activate")]
        public bool Activate { get; set; }
        [GrandResourceDisplayName("Admin.PointWallet.PointWallet.Fields.Order_ID")]
        public string Order_ID { get; set; }
        [GrandResourceDisplayName("Admin.PointWallet.PointWallet.Fields.Description")]
        public string Description { get; set; }
        [GrandResourceDisplayName("Admin.PointWallet.PointWallet.Fields.CreateTime")]
        public DateTime CreateTime { get; set; }
        [GrandResourceDisplayName("Admin.PointWallet.PointWallet.Fields.ExpiredTime")]
        public DateTime? ExpiredTime { get; set; }
        [GrandResourceDisplayName("Admin.PointWallet.PointWallet.Fields.ReasonOfAdjustment")]
        public string Reason { get; set; }
        [GrandResourceDisplayName("Admin.PointWallet.PointWallet.Fields.Customer_Name")]
        public string Customer_Name { get; set; }

        [GrandResourceDisplayName("Admin.PointWallet.PointWallet.Fields.Customer_Email")]
        public string Customer_Email { get; set; }
        public List<SelectListItem> selectedList { get; set; }

        [GrandResourceDisplayName("Admin.PointWallet.PointWallet.Fields.Order_Status")]
        public string Order_Status { get; set; }

    }
}