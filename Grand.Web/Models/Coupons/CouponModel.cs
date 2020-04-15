using Grand.Core.Domain.CouponsModule;
using Grand.Framework.Mvc.Models;
using Grand.Web.Models.Catalog;
using Grand.Web.Models.Media;
using System;
using System.Collections.Generic;

namespace Grand.Web.Models.Coupons 
{
    public partial class CouponModel : BaseGrandEntityModel, ICloneable
    {

     
        public string Name { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public decimal CouponAmount { get; set; }

        public decimal CouponPercentage { get; set; }


        public object Clone()
        {
            //we use a shallow copy (deep clone is not required here)
            return this.MemberwiseClone();
        }
    }

}