using Grand.Core.Domain.Rewards;
using System.Collections.Generic;

namespace Grand.Services.Rewards
{
    public partial interface IRewardCategoryService
    {
        void AddRewardCategory(RewardCategory reward);
        List<RewardCategory> GETAllRewardCategory();
        void UpdateRewardCategory(RewardCategory gift);
        RewardCategory GETRewardCategoryInfo(string id);

    }
}
