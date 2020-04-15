using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.Rewards

{
    [BsonIgnoreExtraElements]
    public class RewardCategory : BaseEntity
    {

        public RewardCategory()
        {

        }
        public string Reward_Category_Name { get; set; }
        public string Reward_Description { get; set; }
        public string Reward_Item { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Delete { get; set; }

    }
}