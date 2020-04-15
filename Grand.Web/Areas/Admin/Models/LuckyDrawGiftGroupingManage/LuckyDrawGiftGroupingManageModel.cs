using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grand.Web.Areas.Admin.Models.LuckyDrawGiftGroupingManage
{
    public partial class LuckyDrawGiftGroupingManageModel : BaseGrandEntityModel
    {
       
        public LuckyDrawGiftGroupingManageModel()
        {
            this.selectedList = new List<SelectListItem>();
        }

        [GrandResourceDisplayName("Admin.LuckyDrawGiftGroupingManage.LuckyDrawGiftGroupingManage.Fields.Group_Name")]
        public string Group_Name { get; set; }

        [GrandResourceDisplayName("Admin.LuckyDrawGiftGroupingManage.LuckyDrawGiftGroupingManage.Fields.Start_Time")]
        [UIHint("DateTimeNullable")]
        public DateTime? Start_Time { get; set; }

        [GrandResourceDisplayName("Admin.LuckyDrawGiftGroupingManage.LuckyDrawGiftGroupingManage.Fields.End_Time")]
        [UIHint("DateTimeNullable")]
        public DateTime? End_Time { get; set; }

        [GrandResourceDisplayName("Admin.LuckyDrawGiftID.LuckyDrawGiftID.Fields.Gift_Name")]
        public string Gift_Name { get; set; }
        public List<SelectListItem> selectedList { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGift.LuckyDrawGift.Fields.Available_Quantity")]
        public int Available_Quantity { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGift.LuckyDrawGift.Fields.Quantity")]
        public int Quantity { get; set; }
        [GrandResourceDisplayName("Admin.LuckyDrawGiftGroupingManage.LuckyDrawGiftGroupingManage.Fields.Activate")]
        public bool Activate { get; set; }

    }
}