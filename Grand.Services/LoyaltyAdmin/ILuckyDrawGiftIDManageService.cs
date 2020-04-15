using Grand.Core.Domain.LoyaltyAdmin;
using System.Collections.Generic;

namespace Grand.Services.LoyaltyAdmin
{
    public partial interface ILuckyDrawGiftIDManageService
    {
        void AddLuckyDrawGiftID(LuckyDrawGiftIDManage gift);
        void DeleteGiftID(LuckyDrawGiftIDManage Gift);
        LuckyDrawGiftIDManage GETGiftInfo(string id);
        List<LuckyDrawGiftIDManage> GetAllWinnerWinnerChickenDinner();
        List<LuckyDrawGiftIDManage> PrepareLuckyDrawGiftIDResourceModel(string id, int pageIndex, int pageSize);
        List<LuckyDrawGiftIDManage> LuckyDrawGiftIDResourceModel(string id, int pageIndex, int pageSize);
        void InsertVoucherID(LuckyDrawGiftIDManage ID);
        void UpdateVoucherID(LuckyDrawGiftIDManage ID);
        List<LuckyDrawGiftIDManage> ManualVoucherCount(string id);
        LuckyDrawGiftIDManage GETGiftInfowithGiftTypeID(string id);
        LuckyDrawGiftIDManage GETGiftInfowithGroupID(string id);
        LuckyDrawGiftIDManage GETGiftInfowithGiftTypeIDANDGroupID(string id, string groupid);
        
            //List<LuckyDrawGiftManage> GETAllGiftInfo();
            //void InsertCheckIn(ITokenService tokenService);
            //ConsecutiveLoginLog GETConsecutiveLoginInfoByCustomerIDwithExpires(string CustomerID,bool Expires);
            //void UpdateConsecutiveLoginInfo(ConsecutiveLoginLog ConsecutiveLoginLog);
            //void CreateToken(Token token);
            //Token GETTokenByCustomerIDwithExpires(string CustomerID, bool Expires);
            //void UpdateTokenInfo(Token token);
        }
}
