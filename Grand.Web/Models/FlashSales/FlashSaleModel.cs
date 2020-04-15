using Grand.Core.Domain.Catalog;
using Grand.Framework.Mvc.Models;
using Grand.Web.Models.Catalog;
using Grand.Web.Models.Media;
using System;
using System.Collections.Generic;

namespace Grand.Web.Models.FlashSales
{
    public partial class FlashSaleModel : BaseGrandEntityModel, ICloneable
    {

        public FlashSaleModel()
        {
            FlashSaleProducts = new List<ProductOverviewModel>();
        }

        public string Name { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public IList<ProductOverviewModel> FlashSaleProducts { get; set; }

        public object Clone()
        {
            //we use a shallow copy (deep clone is not required here)
            return this.MemberwiseClone();
        }
    }

}