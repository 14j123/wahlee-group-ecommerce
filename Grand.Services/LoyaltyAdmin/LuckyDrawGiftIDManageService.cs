using Grand.Core.Data;
using Grand.Core.Domain.LoyaltyAdmin;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Services.LoyaltyAdmin
{
    public partial class LuckyDrawGiftIDManageService : ILuckyDrawGiftIDManageService
    {
        #region Fields
        private readonly IRepository<LuckyDrawGiftIDManage> _LuckyDrawGiftIDManageRepository;
        #endregion

        #region Ctor
        public LuckyDrawGiftIDManageService(

        IRepository<LuckyDrawGiftIDManage> LuckyDrawGiftIDManageRepository

        )
        {
            this._LuckyDrawGiftIDManageRepository = LuckyDrawGiftIDManageRepository;
        }
        #endregion

        #region Method
        #region Add Lucky Draw Gift ID
        public virtual void AddLuckyDrawGiftID(LuckyDrawGiftIDManage gift)
        {
            _LuckyDrawGiftIDManageRepository.Insert(gift);
        }

        #region Check Token
        public virtual LuckyDrawGiftIDManage GETavailableGiftID(string CustomerID, bool Expires)
        {
            var query = from c in _LuckyDrawGiftIDManageRepository.Table
                        select c;

            return query.ToList().FirstOrDefault();
        }
        #endregion
        public virtual LuckyDrawGiftIDManage GETGiftInfo(string id)
        {
            var query = from c in _LuckyDrawGiftIDManageRepository.Table
                        where c.Id == id && c.Delete == false
                        select c;
            return query.ToList().FirstOrDefault();
        }
        public virtual List<LuckyDrawGiftIDManage> LuckyDrawGiftIDResourceModel(string id, int pageIndex, int pageSize)
        {
            var query = from c in _LuckyDrawGiftIDManageRepository.Table
                        where c.Group_ID == id && c.Delete == false 
                        select c;
            return query.ToList();
        }
        public virtual LuckyDrawGiftIDManage GETGiftInfowithGiftTypeID(string id)
        {
            var query = from c in _LuckyDrawGiftIDManageRepository.Table
                        where c.Gift_Type_ID == id && c.Delete == false && c.Group_ID == null
                        select c;
            return query.ToList().FirstOrDefault();
        }
        public virtual LuckyDrawGiftIDManage GETGiftInfowithGroupID(string id)
        {
            var query = from c in _LuckyDrawGiftIDManageRepository.Table
                        where c.Group_ID == id && c.Delete == false 
                        select c;
            return query.ToList().FirstOrDefault();
        }
        public virtual LuckyDrawGiftIDManage GETGiftInfowithGiftTypeIDANDGroupID(string id,string groupid )
        {
            var query = from c in _LuckyDrawGiftIDManageRepository.Table
                        where c.Group_ID == groupid && c.Delete == false && c.Gift_Type_ID == id 
                        select c;
            return query.ToList().FirstOrDefault();
        }
        public virtual void DeleteGiftID(LuckyDrawGiftIDManage Gift)
        {
            _LuckyDrawGiftIDManageRepository.Delete(Gift);
        }
        public virtual void AddLuckyDrawGiftWinner(LuckyDrawGiftIDManage Gift)
        {
            //update
            _LuckyDrawGiftIDManageRepository.Update(Gift);

        }
        #endregion

        #region 
        public virtual List<LuckyDrawGiftIDManage> GetAllWinnerWinnerChickenDinner()
        {
            var query = from c in _LuckyDrawGiftIDManageRepository.Table
                        where c.Customer_ID != null
                        select c;
            return query.ToList();
        }
        #endregion

        public virtual List<LuckyDrawGiftIDManage> PrepareLuckyDrawGiftIDResourceModel( string id, int pageIndex, int pageSize)
        {
            var query = from c in _LuckyDrawGiftIDManageRepository.Table
                        where c.Gift_Type_ID == id && c.Delete == false && c.VoucherCategory == "2"
                        select c;
            return query.ToList();   
        }

        public virtual void InsertVoucherID(LuckyDrawGiftIDManage ID)
        {
            _LuckyDrawGiftIDManageRepository.Insert(ID);
        }
        public virtual void UpdateVoucherID(LuckyDrawGiftIDManage ID)
        {
            _LuckyDrawGiftIDManageRepository.Update(ID);
        }

        #region Admin get Manual fill in Voucher quantity
        public virtual List<LuckyDrawGiftIDManage> ManualVoucherCount(string id)
        {
            var query = from c in _LuckyDrawGiftIDManageRepository.Table
                        where c.Gift_Type_ID == id && c.VoucherCategory == "2"
                        select c;
            return query.ToList();
        }
        #endregion
        #endregion


    }
}
