using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.LoyaltyPoint
{
    [BsonIgnoreExtraElements]
    public class PointWallet : BaseEntity
    {

        public PointWallet()
        {

        }

        
        public int LoyaltyPointEarn { get; set; }
        public int LoyaltyPointUsed { get; set; }
        public bool Activate { get; set; }
        public string Order_ID { get; set; }
        public int Order_Number { get; set; }
        public string Order_Status { get; set; }
        public string Customer_ID { get; set; }
        public string Customer_Email { get; set; }
        public string Customer_Full_Name { get; set; }
        public string PointRule_ID { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ExpiredTime { get; set; }
        public string Reward_Point_Adjustment_Reason { get; set; }
        
    }

}