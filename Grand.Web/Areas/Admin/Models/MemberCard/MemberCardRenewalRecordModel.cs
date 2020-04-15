using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grand.Web.Areas.Admin.Models.MemberCard
{
    public partial class MemberCardRenewalRecordModel : BaseGrandEntityModel
    {

        public MemberCardRenewalRecordModel()
        {

        }
        public string MemberCardId { get; set; }
        public decimal AmountPaid { get; set; }  
        public DateTime RenewalTime { get; set; }
        public DateTime NextExpiredDate { get; set; }
        public string CustomerID { get; set; }
        

    }
}
