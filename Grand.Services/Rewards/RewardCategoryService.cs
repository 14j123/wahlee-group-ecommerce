using Grand.Core.Data;
using Grand.Core.Domain.Rewards;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Services.Rewards
{
    public partial class RewardCategoryService : IRewardCategoryService
    {
        #region Fields
        private readonly IRepository<RewardCategory> _RewardCategoryRepository;
        #endregion

        #region Ctor
        public RewardCategoryService(

        IRepository<RewardCategory> RewardCategoryRepository

        )
        {
            this._RewardCategoryRepository = RewardCategoryRepository;
        }
        #endregion

        #region Method

        #region Add Reward Gift
        public virtual void AddRewardCategory (RewardCategory reward)
        {
            _RewardCategoryRepository.Insert(reward);
        }
        #endregion

        #region Get All Reward Gift
        public virtual List<RewardCategory> GETAllRewardCategory()
        {
            return _RewardCategoryRepository.Table.ToList();
        }

        #endregion

        #region Update Gift
        public virtual void UpdateRewardCategory(RewardCategory gift)
        {
            _RewardCategoryRepository.Update(gift);
        }
        #endregion

        #region Get Lucky Draw Gift
        public virtual RewardCategory GETRewardCategoryInfo(string id)
        {
            var query = from c in _RewardCategoryRepository.Table
                        where c.Id == id
                        select c;
            return query.ToList().FirstOrDefault();
        }
        #endregion

        #region Delete Lucky Draw Gift
        //public virtual void DeleteGift(LuckyDrawGiftManage gift)
        //{
        //    _LuckyDrawGiftManageRepository.Update(gift);
        //}
        //#endregion
        #endregion
        #endregion
    }
}
