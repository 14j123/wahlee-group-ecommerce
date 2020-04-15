using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.LoyaltyPoint
{
    [BsonIgnoreExtraElements]
    public class PointSetting : BaseEntity
    {

        public PointSetting()
        {

        }
        public string RuleName;
        public string RuleDescription;
        public double Times;
        public bool Activate;
        public string Type;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }

}