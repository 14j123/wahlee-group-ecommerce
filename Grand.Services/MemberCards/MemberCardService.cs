using Grand.Core.Data;
using Grand.Core.Domain.LoyaltyPoint;
using Grand.Core.Domain.MemberCard;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Services.MemberCards
{
    public partial class MemberCardService : IMemberCardService
    {
        #region Fields
        private readonly IRepository<MemberCard> _memberCardRepository;
        #endregion

        #region Ctor
        public MemberCardService(

        IRepository<MemberCard> MemberCardRepository

        )
        {
            this._memberCardRepository = MemberCardRepository;
        }
        #endregion

        #region Method

        public virtual List<MemberCard> GetAllMemberCard()
        {
            var query = from c in _memberCardRepository.Table

                        select c;

            return query.ToList();
        }

        #endregion


    }
}
