using Grand.Core.Data;
using Grand.Core.Domain.LoyaltyPoint;
using Grand.Core.Domain.MemberCard;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Services.MemberCards
{
    public partial class MemberCardTopUpHistoryService : IMemberCardTopUpHistoryService
    {
        #region Fields
        private readonly IRepository<MemberCardTopUpHistory> _memberCardTopUpHistoryRepository;
        #endregion

        #region Ctor
        public MemberCardTopUpHistoryService(

        IRepository<MemberCardTopUpHistory> 
            memberCardTopUpHistoryRepository

        )
        {
            this._memberCardTopUpHistoryRepository = memberCardTopUpHistoryRepository;
        }
        #endregion

        #region Method
        public virtual List<MemberCardTopUpHistory> GetAllMemberCardTopUpHistory()
        {
            var query = from c in _memberCardTopUpHistoryRepository.Table

                        select c;

            return query.ToList();
        }


        #endregion


    }
}
