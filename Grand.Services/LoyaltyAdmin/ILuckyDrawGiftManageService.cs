using Grand.Core.Domain.LoyaltyAdmin;
using System.Collections.Generic;

namespace Grand.Services.LoyaltyAdmin
{
    public partial interface ILuckyDrawGiftManageService
    {
        void AddLuckyDrawGift(LuckyDrawGiftManage gift);
        List<LuckyDrawGiftManage> GETAllGiftInfo();
        void UpdateGift(LuckyDrawGiftManage gift);
        LuckyDrawGiftManage GETGiftInfo(string id);
        void DeleteGift(LuckyDrawGiftManage gift);
        List<LuckyDrawGiftManage> GetAllAvailablewithActive();
        LuckyDrawGiftManage GETGiftInfoById(string Id);
        List<LuckyDrawGiftManage> GETAllActiveGiftInfo();
    }
}
