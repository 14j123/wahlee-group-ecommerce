using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.Rewards

{
    [BsonIgnoreExtraElements]
    public class RewardID : BaseEntity
    {

        public RewardID()
        {

        }
        public string Reward_Name { get; set; }
        public string Reward_Description { get; set; }
        public DateTime RedemptTime { get; set; }
        public DateTime RewardRedemptTime { get; set; }
        public string Customer_ID { get; set; }
        public string Reward_ID { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Delete { get; set; }
        public string Category { get; set; }
        public int Point { get; set; }
        public DateTime PurchaseStartTime { get; set; }
        public DateTime PurchaseEndTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpiredTime { get; set; }
        public string Url { get; set; }
        public string Highlights { get; set; }
        public string Term { get; set; }
        public string Contact { get; set; }
        public string About { get; set; }

    }
}