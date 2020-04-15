using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Media;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;


namespace Grand.Core.Domain.Rewards
{
    [BsonIgnoreExtraElements]
    public class Reward : BaseEntity
    {

        public Reward()
        {

        }
        //public Picture Reward_Picture1 { get; set; }
        public RewardPicture Reward_Picture { get; set; }
        public string Reward_Name { get; set; }
        public string Reward_Description { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity { get; set; }
        public bool Activate { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime PurchaseStartTime { get; set; }
        public DateTime PurchaseEndTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpiredTime { get; set; }
        public bool Delete { get; set; }
        public string Category { get; set; }
        public int Point { get; set; }
        public string Highlights { get; set; }
        public string Term { get; set; }
        public string Contact { get; set; }
        public string About { get; set; }
    }
    public partial class RewardPicture : BaseEntity
    {
        public string ProductId { get; set; }
        public string PictureId { get; set; }
        public string PictureUrl { get; set; }
                
        //public int DisplayOrder { get; set; }
     
        public string OverrideAltAttribute { get; set; }
        
        public string OverrideTitleAttribute { get; set; }
        public string Url { get; set; }
    }

}