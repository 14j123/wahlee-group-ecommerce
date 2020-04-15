using Grand.Core.Data;
using Grand.Core.Domain.LoyaltyPoint;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Services.LoyaltyPoint
{
    public partial class PointWalletSummaryService : IPointWalletSummaryService
    {
        #region Fields
        private readonly IRepository<PointWalletSummary> _PointWalletSummaryRepository;
        #endregion

        #region Ctor
        public PointWalletSummaryService(

        IRepository<PointWalletSummary> PointWalletSummaryRepository

        )
        {
            this._PointWalletSummaryRepository = PointWalletSummaryRepository;
        }
        #endregion

        #region Method

        #region Get All Point
        public virtual List<PointWalletSummary> GETAllPointWalletSummary()
        {
            return _PointWalletSummaryRepository.Table.ToList();
        }
        #endregion

        public virtual void AddSummary(PointWalletSummary pointWallet)
        {
            _PointWalletSummaryRepository.Insert(pointWallet);
        }
        //#region Add point
        //public virtual void AddPoint(PointWallet pointWallet)
        //{
        //    _PointWalletRepository.Insert(pointWallet);
        //}
        //public virtual List<PointWallet> GETAllPointWallet()
        //{
        //    return _PointWalletRepository.Table.ToList();
        //}


        //#endregion
        //#region Edit point
        //public virtual PointWallet GETPointWalletInfo(string id)
        //{
        //    var query = from c in _PointWalletRepository.Table
        //                where c.Id == id
        //                select c;
        //    return query.ToList().FirstOrDefault();
        //}
        //public virtual void UpdatePointWalletInfo(PointWallet pointWallet)
        //{
        //    _PointWalletRepository.Update(pointWallet);
        //}

        #endregion


    }
}
