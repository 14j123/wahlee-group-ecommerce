using Grand.Core.Domain.LoyaltyPoint;
using System;
using System.Collections.Generic;

namespace Grand.Services.LoyaltyPoint
{
    public partial interface IPointWalletService
    {
        List<PointWallet> GETAllPoint();
        void AddPoint(PointWallet pointWallet);
        List<PointWallet> GETAllPointWallet();
        PointWallet GETPointWalletInfo(string id);
        void UpdatePointWalletInfo(PointWallet pointWallet);
        List<PointWallet> GETAllPointbyCustomerID(string Id);
        PointWallet GETAllPoint(string ID);
        void UpdatePoint(PointWallet pointWallet);
        PointWallet GetPointByOrderID(string Id);
    }

}
