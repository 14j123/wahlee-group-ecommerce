using Grand.Core.Data;
using Grand.Core.Domain.LoyaltyPoint;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Services.LoyaltyPoint
{
    public partial class PointSettingService : IPointSettingService
    {
        #region Fields
        private readonly IRepository<PointSetting> _pointSettingRepository;
        #endregion

        #region Ctor
        public PointSettingService(

        IRepository<PointSetting> pointSettingRepository

        )
        {
            this._pointSettingRepository = pointSettingRepository;
        }
        #endregion

        #region Method

        #region Get All Setting
        public virtual List<PointSetting> GETSettingTimes()
        {
            var query = from c in _pointSettingRepository.Table
                        
                        select c;

            return query.ToList();
        }
        #endregion

        #region Create Rule
        public virtual void CreateRule(PointSetting PS)
        {
            _pointSettingRepository.Insert(PS);
        }
        #endregion

        #region Update Rule
        public virtual void UpdateRule(PointSetting PS)
        {
            _pointSettingRepository.Update(PS);
        }
        #endregion

        #region Update Rule
        public virtual PointSetting GETSettingByID(string id)
        {
            var query = from c in _pointSettingRepository.Table
                        where c.Id == id    
                        select c;

            return query.ToList().FirstOrDefault();
        }
        #endregion
        #region Get All Valid Rule
        public virtual List<PointSetting> GetAllValidRule()
        {
            var query = from c in _pointSettingRepository.Table
                        where c.Activate == true
                        select c;

            return query.ToList();
        }
        #endregion
        #endregion

    }
}
