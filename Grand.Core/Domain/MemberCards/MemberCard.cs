using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Grand.Core.Domain.MemberCard
{
    [BsonIgnoreExtraElements]
    public class MemberCard : BaseEntity
    {

        public MemberCard()
        {
        }


        public string MemberCardId { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public decimal BalanceOnCard { get; set; }

    }
}
