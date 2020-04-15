using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.LoyaltyAdmin
{
    [BsonIgnoreExtraElements]
    public class LuckyDrawGiftManage : BaseEntity
    {

        public LuckyDrawGiftManage()
        {

        }
        public string Gift_Name { get; set; }
        public string Gift_Description { get; set; }
        public int Quantity { get; set; }
        public int Redempted { get; set; }
        public int Remaining { get; set; }
        
        public bool Activate { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Delete { get; set; }       
        public string VoucherCategory { get; set; }
        public int Available_Quantity { get; set; }

    }


}