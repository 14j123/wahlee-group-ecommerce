using Grand.Core.Domain.Customers;
using Grand.Core.Domain.FlashSales;
using Grand.Web.Areas.Admin.Models.Catalog;
using Grand.Web.Areas.Admin.Models.Customers;
using Grand.Web.Areas.Admin.Models.FlashSales;
using System.Collections.Generic;
using Grand.Core.Domain.Catalog;

namespace Grand.Web.Areas.Admin.Interfaces
{
    public interface IFlashSaleViewModelService 
    {
        
        FlashSaleModel.AddProductToFlashSaleModel PrepareProductToFlashSaleModel();

        (IList<ProductModel> products, int totalCount) PrepareProductModel(FlashSaleModel.AddProductToFlashSaleModel model, int pageIndex, int pageSize);

        void DeleteProduct(FlashSale flashSale, Product product);

        void InsertProductToFlashSaleModel(FlashSaleModel.AddProductToFlashSaleModel model);
    }
}
