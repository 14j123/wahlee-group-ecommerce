using Grand.Core.Data;
using Grand.Core.Domain.LoyaltyAdmin;
using Grand.Core.Domain.Rewards;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Services.Rewards
{
    public partial class RewardIDService : IRewardIDService
    {
        #region Fields
       
        private readonly IRepository<RewardID> _RewardIDRepository;
        #endregion

        #region Ctor
        public RewardIDService(

        IRepository<RewardID> RewardIDRepository

        )
        {
            this._RewardIDRepository = RewardIDRepository;
        }
        #endregion

        //#region Method
        //#region Add Lucky Draw Gift ID
        public virtual void AddRewardGiftID(RewardID gift)
        {
            _RewardIDRepository.Insert(gift);
        }
        public virtual List<RewardID> GETAllRedeemptedRewardGiftID()
        {
            var query = from c in _RewardIDRepository.Table
                        where c.Delete == false && c.RedemptTime != default(DateTime)
                        select c;

            return query.ToList();

        }
        public virtual List<RewardID> GETAllRewardGiftIDwithCustomerID(string Customer_ID)
        {
            var query = from c in _RewardIDRepository.Table
                        where c.Customer_ID == Customer_ID && c.Delete == false
                        select c;

            return query.ToList();

        }
        public virtual List<RewardID> GETAllCanRewardGiftID(string Customer_ID)
        {
            var query = from c in _RewardIDRepository.Table
                        where c.Customer_ID == Customer_ID && c.ExpiredTime > DateTime.Now && c.RewardRedemptTime == default(DateTime) 
                        select c;

            return query.ToList();

        }
        public virtual List<RewardID> GETAllExpiredRewardGiftID(string Customer_ID)
        {
            var query = from c in _RewardIDRepository.Table
                        where c.Customer_ID == Customer_ID && c.ExpiredTime < DateTime.Now && c.RewardRedemptTime == default(DateTime)
                        select c;

            return query.ToList();

        }
        public virtual List<RewardID> GETAllRedeemedRewardGiftID(string Customer_ID)
        {
            var query = from c in _RewardIDRepository.Table
                        where c.Customer_ID == Customer_ID && c.RewardRedemptTime != default(DateTime)
                        select c;

            return query.ToList();

        }
        

        public virtual RewardID GETRewardGiftID(string Id)
        {
            var query = from c in _RewardIDRepository.Table
                        where c.Id == Id
                        select c;

            return query.FirstOrDefault();

        }

        public virtual RewardID GETRewardGiftIDbyRewardMainID(string Reward_Id)
        {
            var query = from c in _RewardIDRepository.Table
                        where c.Reward_ID == Reward_Id && c.Customer_ID == null && c.Delete == false
                        select c;

            return query.FirstOrDefault();

        }

        public virtual void UpdateRewardGift(RewardID gift)
        {
            _RewardIDRepository.Update(gift);
        }

    }
}
