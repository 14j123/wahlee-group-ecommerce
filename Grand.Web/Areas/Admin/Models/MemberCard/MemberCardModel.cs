using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grand.Web.Areas.Admin.Models.MemberCard
{
    public partial class MemberCardModel : BaseGrandEntityModel
    {

        public MemberCardModel()
        {

        }

        [GrandResourceDisplayName("Admin.MemberCard.MemberCard.Fields.MemberCardId")]
        public string MemberCardId { get; set; }
        [GrandResourceDisplayName("Admin.MemberCard.MemberCard.Fields.ExpiredDate")]
        public DateTime? ExpiredDate { get; set; }
        [GrandResourceDisplayName("Admin.MemberCard.MemberCard.Fields.BalanceOnCard")]
        public decimal BalanceOnCard { get; set; }
        [GrandResourceDisplayName("Admin.MemberCard.MemberCard.Fields.CustomerFullName")]
        public string CustomerFullName { get; set; }
        [GrandResourceDisplayName("Admin.MemberCard.MemberCard.Fields.CustomerEmail")]
        public string CustomerEmail { get; set; }
        [GrandResourceDisplayName("Admin.MemberCard.MemberCard.Fields.CustomerID")]
        public string CustomerID { get; set; }
    }
}
