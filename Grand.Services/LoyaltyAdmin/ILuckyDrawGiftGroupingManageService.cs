using Grand.Core.Domain.LoyaltyAdmin;
using System;
using System.Collections.Generic;

namespace Grand.Services.LoyaltyAdmin
{
    public partial interface ILuckyDrawGiftGroupingManageService
    {
        void GroupingGiftProduct(LuckyDrawGiftGroupingManage group);
        void DeleteGiftGrouping(LuckyDrawGiftGroupingManage group);
        LuckyDrawGiftGroupingManage GETGroupInfo(string group_id);
        LuckyDrawGiftGroupingManage GETAllGroupInfo(DateTime? dateTime);
        LuckyDrawGiftGroupingManage ValidGroupInfo(DateTime? dateTime, bool Activate);
        List<LuckyDrawGiftGroupingManage> GETAllGroupInfocn();
        LuckyDrawGiftGroupingManage EditGroupInfo(string id);
        void UpdateGroupInfo(LuckyDrawGiftGroupingManage id);
        LuckyDrawGiftGroupingManage LuckyDrawGiftGroupingResourceModel(string id, int pageIndex, int pageSize);
    }
    //new add

}
