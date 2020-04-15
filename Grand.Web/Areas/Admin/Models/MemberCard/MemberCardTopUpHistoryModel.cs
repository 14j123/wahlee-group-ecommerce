using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grand.Web.Areas.Admin.Models.MemberCard
{
    public partial class MemberCardTopUpHistoryModel : BaseGrandEntityModel
    {

        public MemberCardTopUpHistoryModel()
        {

        }

        public string MemberCardId { get; set; }
        public decimal TopUpAmount { get; set; }  
        public DateTime TopUpDate { get; set; }
        
    }
}
