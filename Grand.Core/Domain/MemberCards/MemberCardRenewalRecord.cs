using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.MemberCard
{
    [BsonIgnoreExtraElements]
    public class MemberCardRenewalRecord : BaseEntity
    {

        public MemberCardRenewalRecord()
        {

        }

        public string MemberCardId { get; set; }
        public decimal AmountPaid { get; set; }  
        public DateTime RenewalTime { get; set; }
        public DateTime NextExpiredDate { get; set; }

    }
}
