using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.Loyalty
{
    [BsonIgnoreExtraElements]
    public class Token : BaseEntity
    {

        public Token()
        {
            
        }
        public string Customer_ID { get; set; }
        public string Token_ID { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime RedemptTime { get; set; }
        public bool Expire { get; set; }
        public string Gift_ID { get; set; }

    }



}