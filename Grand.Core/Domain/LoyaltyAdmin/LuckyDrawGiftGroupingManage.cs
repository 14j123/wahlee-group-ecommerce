using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.LoyaltyAdmin
{
    [BsonIgnoreExtraElements]
    public class LuckyDrawGiftGroupingManage : BaseEntity
    {

        public LuckyDrawGiftGroupingManage()
        {
            this.Gift = new List<Gifts>();
        }
        public string Group_Name { get; set; }
        public DateTime? Start_Time { get; set; }
        public DateTime? End_Time { get; set; }
        public bool Activate { get; set; }
        public bool Delete { get; set; }
        public List<Gifts> Gift { get; set; }
    }
    public class Gifts
    {
        public string Gift_Name { get; set; }

        public string Gift_Type { get; set; }
        public int Quantity { get; set; }
    }



}