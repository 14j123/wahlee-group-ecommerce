using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using System;
using System.Collections.Generic;

namespace Grand.Web.Areas.Admin.Models.LuckyDrawGiftID
{
    public partial class LuckyDrawGiftIDModel : BaseGrandEntityModel
    {
        
        [GrandResourceDisplayName("Admin.LuckyDrawGiftID.LuckyDrawGiftID.Fields.Gift_Name")]
        public string Gift_Name { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGiftID.LuckyDrawGiftID.Fields.Gift_Description")]
        public string Gift_Description { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGiftID.LuckyDrawGiftID.Fields.VoucherCategory")]
        public string VoucherCategory { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGiftID.LuckyDrawGiftID.Fields.RedemptTime")]
        public DateTime RedemptTime { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGiftID.LuckyDrawGiftID.Fields.Customer_ID")]
        public string Customer_ID { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGiftID.LuckyDrawGiftID.Fields.Gift_Type_ID")]
        public string Gift_Type_ID { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGiftID.LuckyDrawGiftID.Fields.CreateTime")]
        public DateTime CreateTime { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGiftID.LuckyDrawGiftID.Fields.Delete")]
        public bool Delete { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGiftID.LuckyDrawGiftID.Fields.VoucherNumber")]
        public string VoucherNumber { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGiftID.LuckyDrawGiftID.Fields.Group_ID")]
        public string Group_ID { get; set; }
    }
}