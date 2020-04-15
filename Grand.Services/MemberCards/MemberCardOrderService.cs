using Grand.Core.Data;
using Grand.Core.Domain.LoyaltyPoint;
using Grand.Core.Domain.MemberCard;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Services.MemberCards
{
    public partial class MemberCardOrderService : IMemberCardOrderService
    {
        #region Fields
        private readonly IRepository<MemberCardOrder> _memberCardOrderRepository;
        #endregion

        #region Ctor
        public MemberCardOrderService(

        IRepository<MemberCardOrder> memberCardOrderRepository

        )
        {
            this._memberCardOrderRepository = memberCardOrderRepository;
        }
        #endregion

        #region Method

        public virtual List<MemberCardOrder> GetAllMemberCardOrder()
        {
            var query = from c in _memberCardOrderRepository.Table

                        select c;

            return query.ToList();
        }

        #endregion


    }
}
