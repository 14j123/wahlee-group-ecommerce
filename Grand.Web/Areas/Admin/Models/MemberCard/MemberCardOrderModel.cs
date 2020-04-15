using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Grand.Web.Areas.Admin.Models.MemberCard
{
    public partial class MemberCardOrderModel : BaseGrandEntityModel
    {

        public MemberCardOrderModel()
        {

        }

        public string NameOnCard;
        public string MaillingAddress;
        public DateTime CreateTime;
        public decimal AmountPaid;
        public string CustomerID;

        
    }

}