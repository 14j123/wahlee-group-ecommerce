using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.Loyalty
{
    [BsonIgnoreExtraElements]
    public class ConsecutiveLoginLog : BaseEntity
    {

        public ConsecutiveLoginLog()
        {
            this.CheckIn_Info = new List<Check_In>();
        }
        public string Consecutive_Login_ID { get; set; } 
        public string Customer_ID { get; set; }
        public List<Check_In> CheckIn_Info { get; set; }
        public bool Expires { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastCheckIn { get; set; }
        
    }

    public class Check_In
    {
  
        public string Day { get; set; }
        public DateTime CheckIn_Time { get; set; }

    }


}