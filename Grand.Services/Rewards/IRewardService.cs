using Grand.Core.Domain.Rewards;
using System.Collections.Generic;

namespace Grand.Services.Rewards
{
    public partial interface IRewardService
    {
        List<Reward> GETAllRewardGiftInfo();
        void AddRewardGift(Reward reward);
        Reward GETGiftInfo(string id);
        void UpdateGift(Reward gift);
        void InsertRewardPicture(Reward product, string pictureId, int displayOrder, string overrideAltAttribute, string overrideTitleAttribute);
        List<Reward> GETAllRewardGiftInfoNoExp();
        List<Reward> GETAllRewardGiftInfoExp();
        List<Reward> GETAllValidRewardGiftInfo();

    }
}
