using Grand.Core.Domain.Localization;
using Grand.Core.Domain.Security;
using Grand.Core.Domain.Stores;
using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.FlashSales
{
    public partial class FlashSale : BaseEntity, IStoreMappingSupported, ILocalizedEntity, IAclSupported
    {

        public FlashSale()
        {
            Stores = new List<string>();
            Locales = new List<LocalizedProperty>();
            CustomerRoles = new List<string>();
        }

        public string Name { get; set; }
    
        public DateTime? StartDateUtc { get; set; }
      
        public DateTime? EndDateUtc { get; set; }
    
        public bool IsEnabled { get; set; }
       
        public DateTime? CreatedOnUtc { get; set; }

        public DateTime? UpdateDateOnUtc { get; set; }

        //Gets or sets a value indicating whether the entity is subject to ACL
        public bool SubjectToAcl { get ; set; }
        public IList<string> CustomerRoles { get ; set; }

        //Gets or Sets A Value indicating whether the entity is limited/restricted to certain stores
        public bool LimitedToStores { get; set; }
        public IList<string> Stores { get; set ; }

        //Gets or Sets the Collection of locales
        public IList<LocalizedProperty> Locales { get; set; }
    }
}
