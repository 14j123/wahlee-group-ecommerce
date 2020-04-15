using Grand.Core;
using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Customers;
using Grand.Core.Domain.FlashSales;
using System.Collections.Generic;

namespace Grand.Services.FlashSales 
{

    public partial interface IFlashSaleService
    {
        void DeleteFlashSale(FlashSale flashSale);

        void InsertFlashSale(FlashSale flashSale);

        void UpdateFlashSale(FlashSale flashSale);

        FlashSale GetFlashSaleById(string flashSaleId);
        
        IPagedList<FlashSale> GetAllFlashSale(string storeId = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        IPagedList<FlashSale> GetAllCurrentFlashSale(string storeId = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

    }
}
