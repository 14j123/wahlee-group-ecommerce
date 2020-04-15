using Grand.Core.Domain.LoyaltyPoint;
using Grand.Core.Domain.MemberCard;
using System;
using System.Collections.Generic;

namespace Grand.Services.MemberCards
{
    public partial interface IMemberCardRenewalRecordService
    {
        List<MemberCardRenewalRecord> GetAllMemberCardRenewalRecord();

    }

}
