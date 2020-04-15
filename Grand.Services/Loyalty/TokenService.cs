using Grand.Core.Data;
using Grand.Core.Domain.Loyalty;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq;

namespace Grand.Services.Loyalty
{
    public partial class TokenService : ITokenService
    {
        #region Fields
        private readonly IRepository<Token> _TokenRepository;
        #endregion

        #region Ctor
        public TokenService(

        IRepository<Token> TokenRepository

        )
        {
            this._TokenRepository = TokenRepository;
        }
        #endregion

        #region Method
        #region Token Create
        public virtual void CreateToken(Token token)
        {
            _TokenRepository.Insert(token);
        }
        #endregion

        #region Check Token
        public virtual Token GETTokenByCustomerIDwithExpires(string CustomerID, bool Expires)
        {
            var query = from c in _TokenRepository.Table
                        where c.Customer_ID == CustomerID && c.Expire == Expires
                        select c;

            return query.ToList().FirstOrDefault();
        }
        #endregion

        public virtual void UpdateTokenInfo(Token token)
        {
            //update
            _TokenRepository.Update(token);

        }
        #endregion

    }
}
