using Grand.Core.Data;
using Grand.Core.Domain.Loyalty;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;

namespace Grand.Services.Loyalty
{
    public partial class ConsecutiveLoginLogService : IConsecutiveLoginLogService
    {
        #region Fields
        private readonly IRepository<ConsecutiveLoginLog> _ConsecutiveLoginLogRepository;
        #endregion

        #region Ctor
        public ConsecutiveLoginLogService(

        IRepository<ConsecutiveLoginLog> ConsecutiveLoginLogRepository

        )
        {

            this._ConsecutiveLoginLogRepository = ConsecutiveLoginLogRepository;

        }
        #endregion

        #region Method
        #region Check In
        public virtual void InsertCheckIn(ConsecutiveLoginLog consecutiveLoginLog)
        {
            _ConsecutiveLoginLogRepository.Insert(consecutiveLoginLog);
        }
        #endregion

        public virtual ConsecutiveLoginLog GETConsecutiveLoginInfoByCustomerIDwithExpires(string CustomerID,bool Expires)
        {
            var query = from c in _ConsecutiveLoginLogRepository.Table
                        where c.Customer_ID == CustomerID && c.Expires == Expires
                        select c;

            return query.ToList().FirstOrDefault();
        }

        public virtual void UpdateConsecutiveLoginInfo(ConsecutiveLoginLog ConsecutiveLoginLog)
        {
            //update
            _ConsecutiveLoginLogRepository.Update(ConsecutiveLoginLog);
            
        }
        #endregion

    }
}
