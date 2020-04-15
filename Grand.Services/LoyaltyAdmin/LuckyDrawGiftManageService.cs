using Grand.Core.Data;
using Grand.Core.Domain.LoyaltyAdmin;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Services.LoyaltyAdmin
{
    public partial class LuckyDrawGiftManageService : ILuckyDrawGiftManageService
    {
        #region Fields
        private readonly IRepository<LuckyDrawGiftManage> _LuckyDrawGiftManageRepository;
        #endregion

        #region Ctor
        public LuckyDrawGiftManageService(

        IRepository<LuckyDrawGiftManage> LuckyDrawGiftManageRepository

        )
        {
            this._LuckyDrawGiftManageRepository = LuckyDrawGiftManageRepository;
        }
        #endregion

        #region Method

        #region Add Lucky Draw Gift
        public virtual void AddLuckyDrawGift(LuckyDrawGiftManage gift)
        {
            _LuckyDrawGiftManageRepository.Insert(gift);
        }
        #endregion

        #region Get All Lucky Draw Gift
        public virtual List<LuckyDrawGiftManage> GETAllGiftInfo()
        {
            var query = from c in _LuckyDrawGiftManageRepository.Table
                        where c.Delete == false
                        select c;
            return query.ToList();
        }

        #endregion
        #region Get All Active Lucky Draw Gift
        public virtual List<LuckyDrawGiftManage> GETAllActiveGiftInfo()
        {
            var query = from c in _LuckyDrawGiftManageRepository.Table
                        where c.Delete == false && c.Activate == true
                        select c;
            return query.ToList();
        }

        #endregion
        public virtual LuckyDrawGiftManage GETGiftInfoById(string Id)
        {
            var query = from c in _LuckyDrawGiftManageRepository.Table
                        where c.Id == Id
                        select c;
            return query.ToList().FirstOrDefault();
        }
        #region Update Gift
        public virtual void UpdateGift(LuckyDrawGiftManage gift)
        {
            _LuckyDrawGiftManageRepository.Update(gift);
        }
        #endregion

        #region Get Lucky Draw Gift
        public virtual LuckyDrawGiftManage GETGiftInfo(string id)
        {
            var query = from c in _LuckyDrawGiftManageRepository.Table
                        where c.Id == id
                        select c;
            return query.ToList().FirstOrDefault(); 
        }
        #endregion
        #region 

        #endregion

        #region Delete Lucky Draw Gift
        public virtual void DeleteGift(LuckyDrawGiftManage gift)
        {
            _LuckyDrawGiftManageRepository.Update(gift);
        }
        #endregion

        public virtual List<LuckyDrawGiftManage> GetAllAvailablewithActive()
        {
            var query = from c in _LuckyDrawGiftManageRepository.Table
                        where c.Delete == false && c.Activate == true
                        select c;
            return query.ToList();
        }
        #endregion
    }
}
