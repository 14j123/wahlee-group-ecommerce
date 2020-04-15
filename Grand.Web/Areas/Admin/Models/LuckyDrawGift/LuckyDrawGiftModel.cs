using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using Grand.Web.Areas.Admin.Models.LuckyDrawGiftID;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Grand.Web.Areas.Admin.Models.LuckyDrawGift
{
    public partial class LuckyDrawGiftModel : BaseGrandEntityModel
    {

        public LuckyDrawGiftModel()
        {
            //this.VoucherID = new List<LuckyDrawGiftIDModel>();
            this.Vouhcer = new List<string>();
            this.selectedList = new List<SelectListItem>();
        }

        [GrandResourceDisplayName("Admin.LuckyDrawGift.LuckyDrawGift.Fields.Gift_Name")]
        public string Gift_Name { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGift.LuckyDrawGift.Fields.Gift_Description")]
        public string Gift_Description { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGift.LuckyDrawGift.Fields.Quantity")]
        public int Quantity { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGift.LuckyDrawGift.Fields.Redempt")]
        public int Redempt { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGift.LuckyDrawGift.Fields.Available_Quantity")]
        public int Available_Quantity { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGift.LuckyDrawGift.Fields.Activate")]
        public bool Activate { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGift.LuckyDrawGift.Fields.CreateTime")]
        public DateTime CreateTime { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGift.LuckyDrawGift.Fields.VoucherList")]
        public List<string> Vouhcer { get; set; }

        [GrandResourceDisplayName("Admin.LuckyDrawGiftID.LuckyDrawGiftID.Fields.VoucherID")]
        public LuckyDrawGiftIDModel VoucherID { get; set; }

        [GrandResourceDisplayName("Admin.LuckyDrawGiftID.LuckyDrawGiftID.Fields.VoucherCategory")]
        public string VoucherCategory { get; set; }

        [GrandResourceDisplayName("Admin.LuckyDrawGiftID.LuckyDrawGiftID.Fields.Category")]
        public string Category { get; set; }
        public IList<SelectListItem> selectedList { get; set; }
        

    }
}