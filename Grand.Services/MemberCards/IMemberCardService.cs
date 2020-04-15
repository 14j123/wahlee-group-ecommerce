using Grand.Core.Domain.MemberCard;
using System.Collections.Generic;

namespace Grand.Services.MemberCards
{
    public partial interface IMemberCardService
    {

        List<MemberCard> GetAllMemberCard();

    }

}
