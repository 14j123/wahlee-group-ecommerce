using Grand.Core.Domain.LoyaltyPoint;
using System;
using System.Collections.Generic;

namespace Grand.Services.LoyaltyPoint
{
    public partial interface IPointSettingService
    {
        //PointSetting GETSettingTimes(string rewardname);
        List<PointSetting> GETSettingTimes();
        void CreateRule(PointSetting PS);
        void UpdateRule(PointSetting PS);
        PointSetting GETSettingByID(string id);
        List<PointSetting> GetAllValidRule();
    }

}
