using Grand.Core.Domain.Loyalty;

namespace Grand.Services.Loyalty
{
    public partial interface ITokenService
    {

        //void InsertCheckIn(ITokenService tokenService);
        //ConsecutiveLoginLog GETConsecutiveLoginInfoByCustomerIDwithExpires(string CustomerID,bool Expires);
        //void UpdateConsecutiveLoginInfo(ConsecutiveLoginLog ConsecutiveLoginLog);
        void CreateToken(Token token);
        Token GETTokenByCustomerIDwithExpires(string CustomerID, bool Expires);
        void UpdateTokenInfo(Token token);
    }
    //new add

}
