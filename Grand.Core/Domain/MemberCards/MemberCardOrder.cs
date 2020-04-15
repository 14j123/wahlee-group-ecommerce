using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.MemberCard
{
    [BsonIgnoreExtraElements]
    public class MemberCardOrder : BaseEntity
    {

        public MemberCardOrder()
        {

        }

        
        public string NameOnCard;
        public string MaillingAddress;
        public DateTime CreateTime;
        public decimal AmountPaid;

        
    }

}