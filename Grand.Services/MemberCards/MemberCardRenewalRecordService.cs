using Grand.Core.Data;
using Grand.Core.Domain.LoyaltyPoint;
using Grand.Core.Domain.MemberCard;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Services.MemberCards
{
    public partial class MemberCardRenewalRecordService : IMemberCardRenewalRecordService
    {
        #region Fields
        private readonly IRepository<MemberCardRenewalRecord> _memberCardRenewalRecordRepository;
        #endregion

        #region Ctor
        public MemberCardRenewalRecordService(

        IRepository<MemberCardRenewalRecord> memberCardRenewalRecordRepository

        )
        {
            this._memberCardRenewalRecordRepository = memberCardRenewalRecordRepository;
        }
        #endregion

        #region Method

        public virtual List<MemberCardRenewalRecord> GetAllMemberCardRenewalRecord()
        {
            var query = from c in _memberCardRenewalRecordRepository.Table

                        select c;

            return query.ToList();
        }


        #endregion


    }
}
