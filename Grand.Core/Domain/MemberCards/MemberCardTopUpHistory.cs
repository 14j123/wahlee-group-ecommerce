using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.MemberCard
{
    [BsonIgnoreExtraElements]
    public class MemberCardTopUpHistory : BaseEntity
    {

        public MemberCardTopUpHistory()
        {

        }
        
        public string MemberCardId { get; set; }
        public decimal TopUpAmount { get; set; }  
        public DateTime TopUpDate { get; set; }

    }
}
