using Grand.Core.Domain.MemberCard;
using System.Collections.Generic;

namespace Grand.Services.MemberCards
{
    public partial interface IMemberCardOrderService
    {

        List<MemberCardOrder> GetAllMemberCardOrder();
    }

}
