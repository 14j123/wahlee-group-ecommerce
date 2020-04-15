using Grand.Core.Domain.Loyalty;

namespace Grand.Services.Loyalty
{
    public partial interface IConsecutiveLoginLogService
    {

        void InsertCheckIn(ConsecutiveLoginLog consecutiveLoginLog);
        ConsecutiveLoginLog GETConsecutiveLoginInfoByCustomerIDwithExpires(string CustomerID,bool Expires);
        void UpdateConsecutiveLoginInfo(ConsecutiveLoginLog ConsecutiveLoginLog);

    }
    //new add

}
