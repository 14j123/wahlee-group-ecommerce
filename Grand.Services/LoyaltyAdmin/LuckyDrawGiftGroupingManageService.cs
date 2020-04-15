using Grand.Core.Data;
using Grand.Core.Domain.LoyaltyAdmin;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Services.LoyaltyAdmin
{
    public partial class LuckyDrawGiftGroupingManageService : ILuckyDrawGiftGroupingManageService
    { 
        #region Fields
    private readonly IRepository<LuckyDrawGiftGroupingManage> _LuckyDrawGiftGroupingManageRepository;
        #endregion

        #region Ctor
        public LuckyDrawGiftGroupingManageService(

        IRepository<LuckyDrawGiftGroupingManage> LuckyDrawGiftGroupingManageRepository

        )
        {

            this._LuckyDrawGiftGroupingManageRepository = LuckyDrawGiftGroupingManageRepository;

        }
        #endregion

        #region Method
        public virtual void GroupingGiftProduct(LuckyDrawGiftGroupingManage group)
        {
            _LuckyDrawGiftGroupingManageRepository.Insert(group);
        }
        public virtual void UpdateTokenInfo(LuckyDrawGiftGroupingManage group)
        {
            //update
            _LuckyDrawGiftGroupingManageRepository.Update(group);

        }
        public virtual void DeleteGiftGrouping(LuckyDrawGiftGroupingManage group)
        {
            //update
            _LuckyDrawGiftGroupingManageRepository.Update(group);

        }
        #region Search Grouping
        public virtual LuckyDrawGiftGroupingManage GETGroupInfo(string group_id)
        {
            var query = from c in _LuckyDrawGiftGroupingManageRepository.Table
                        where c.Id == group_id
                        select c;
            return query.ToList().FirstOrDefault();
        }
        public virtual LuckyDrawGiftGroupingManage ValidGroupInfo(DateTime? dateTime,bool Activate)
        {
            var query = from c in _LuckyDrawGiftGroupingManageRepository.Table
                        where dateTime >= c.Start_Time && dateTime < c.End_Time && c.Activate == Activate
                        select c;
            return query.ToList().FirstOrDefault();
        }
        public virtual LuckyDrawGiftGroupingManage GETAllGroupInfo(DateTime? dateTime)
        {
            var query = from c in _LuckyDrawGiftGroupingManageRepository.Table
                            //dateToCheck >= startDate && dateToCheck < endDate;
                        where dateTime >= c.Start_Time && dateTime < c.End_Time
                        select c;
            return query.ToList().FirstOrDefault();
        }
        public virtual List<LuckyDrawGiftGroupingManage> GETAllGroupInfocn()
        {
            var query = from c in _LuckyDrawGiftGroupingManageRepository.Table
                        where c.Delete == false
                        select c;
            return query.ToList();
        }
        public virtual LuckyDrawGiftGroupingManage EditGroupInfo(string id)
        {
            var query = from c in _LuckyDrawGiftGroupingManageRepository.Table
                        where c.Id == id
                        select c;
            return query.ToList().FirstOrDefault();
        }
        public virtual void UpdateGroupInfo(LuckyDrawGiftGroupingManage id)
        {
            _LuckyDrawGiftGroupingManageRepository.Update(id);

        }
        public virtual LuckyDrawGiftGroupingManage LuckyDrawGiftGroupingResourceModel(string id, int pageIndex, int pageSize)
        {
            var query = from c in _LuckyDrawGiftGroupingManageRepository.Table
                        where c.Id == id && c.Delete == false
                        select c;
            return query.FirstOrDefault();
        }

        #endregion
        #endregion

    }
}
