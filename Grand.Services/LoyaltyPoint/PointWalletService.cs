using Grand.Core.Data;
using Grand.Core.Domain.LoyaltyPoint;

using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Services.LoyaltyPoint
{
    public partial class PointWalletService : IPointWalletService
    {
        #region Fields
        private readonly IRepository<PointWallet> _PointWalletRepository;
        #endregion

        #region Ctor
        public PointWalletService(

        IRepository<PointWallet> PointWalletRepository

        )
        {
            this._PointWalletRepository = PointWalletRepository;
        }
        #endregion

        #region Method

        #region Get All Point
        public virtual List<PointWallet> GETAllPoint()
        {
            var query = from c in _PointWalletRepository.Table
                        
                        select c;

            return query.ToList();
        }
        #endregion

        #region Get Point
        public virtual PointWallet GETAllPoint(string ID)
        {
            var query = from c in _PointWalletRepository.Table
                        where c.Customer_ID == ID && c.Activate == true && c.ExpiredTime > DateTime.Now 
                        orderby c.CreateTime
                        select c;

            return query.FirstOrDefault();
        }
        #endregion

        #region Get All Point By CustomerID
        public virtual List<PointWallet> GETAllPointbyCustomerID(string Id)
        {
            var query = from c in _PointWalletRepository.Table
                        where c.Customer_ID == Id && c.ExpiredTime > DateTime.Now
                        select c;

            return query.ToList();
        }
        #endregion

        #region Add point
        public virtual void AddPoint(PointWallet pointWallet)
        {
            _PointWalletRepository.Insert(pointWallet);
        }


        public virtual void UpdatePoint(PointWallet pointWallet)
        {
            _PointWalletRepository.Update(pointWallet);
        }
        public virtual List<PointWallet> GETAllPointWallet()
        {
            return _PointWalletRepository.Table.ToList();
        }


        #endregion

        #region Edit point
        public virtual PointWallet GETPointWalletInfo(string id)
        {
            var query = from c in _PointWalletRepository.Table
                        where c.Id == id
                        select c;
            return query.ToList().FirstOrDefault();
        }
        public virtual void UpdatePointWalletInfo(PointWallet pointWallet)
        {
            _PointWalletRepository.Update(pointWallet);
        }

        #endregion

        #region Get Point by Order ID 
        public virtual PointWallet GetPointByOrderID(string Id)
        {
            var query = from c in _PointWalletRepository.Table
                        where c.Order_ID == Id
                        select c;

            return query.ToList().FirstOrDefault();
        }
        #endregion

        #endregion
    }
}
