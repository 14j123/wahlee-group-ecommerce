using Grand.Core.Domain.LoyaltyPoint;
using System;
using System.Collections.Generic;

namespace Grand.Services.LoyaltyPoint
{
    public partial interface IPointWalletSummaryService
    {
        List<PointWalletSummary> GETAllPointWalletSummary();
        void AddSummary(PointWalletSummary pointWallet);
        //List<PointWallet> GETAllPoint();
        //void AddPoint(PointWallet pointWallet);
        //List<PointWallet> GETAllPointWallet();
        //PointWallet GETPointWalletInfo(string id);
        //void UpdatePointWalletInfo(PointWallet pointWallet);
    }

}
