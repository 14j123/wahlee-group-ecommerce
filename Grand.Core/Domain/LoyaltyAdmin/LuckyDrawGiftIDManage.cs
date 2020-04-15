using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.LoyaltyAdmin
{
    [BsonIgnoreExtraElements]
    public class LuckyDrawGiftIDManage : BaseEntity
    {

        public LuckyDrawGiftIDManage()
        {
            
        }
        public string Gift_Name { get; set; }
        public string Gift_Description { get; set; }
        public string VoucherCategory { get; set; }
        public DateTime RedemptTime { get; set; }
        public string Customer_ID { get; set; }
        public string Gift_Type_ID { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Delete { get; set; }
        public string VoucherNumber { get; set; }
        public string Group_ID { get; set; }

    }
}