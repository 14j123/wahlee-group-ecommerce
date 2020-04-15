using Grand.Core.Domain.Rewards;
using System.Collections.Generic;

namespace Grand.Services.Rewards
{
    public partial interface IRewardIDService
    {
        void AddRewardGiftID(RewardID gift);
        List<RewardID> GETAllRedeemptedRewardGiftID();
        List<RewardID> GETAllRewardGiftIDwithCustomerID(string Customer_ID);
        List<RewardID> GETAllRedeemedRewardGiftID(string Customer_ID);
        List<RewardID> GETAllExpiredRewardGiftID(string Customer_ID);
        RewardID GETRewardGiftID(string Id);
        RewardID GETRewardGiftIDbyRewardMainID(string Reward_Id);
        void UpdateRewardGift(RewardID gift);
        List<RewardID> GETAllCanRewardGiftID(string Customer_ID);

    }
}
