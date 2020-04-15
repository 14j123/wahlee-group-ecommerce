using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.LoyaltyPoint
{
    [BsonIgnoreExtraElements]
    public class PointWalletSummary : BaseEntity
    {

        public PointWalletSummary()
        {
            this.Point_Wallet_ID = new List<String>();
        }

        
        public int LoyaltyPointUsed { get; set; }
        public bool Activate { get; set; }
        public List<String> Point_Wallet_ID { get; set; }
        public DateTime CreateTime { get; set; }
        public string Reward_ID { get; set; }
        public string Reward_Name { get; set; }


    }
    
}