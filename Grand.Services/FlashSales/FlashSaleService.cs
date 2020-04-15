using Grand.Core;
using Grand.Core.Caching;
using Grand.Core.Data;
using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Customers;
using Grand.Core.Domain.FlashSales;
using Grand.Services.Common;
using Grand.Services.Customers;
using Grand.Services.Events;
using Grand.Services.Localization;
using Grand.Services.Orders;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Grand.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Grand.Services.FlashSales
{

    public partial class FlashSaleService : IFlashSaleService
    {
        #region Fields

        private readonly IRepository<FlashSale> _flashSaleRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWorkContext _workContext;
        private readonly CatalogSettings _catalogSettings;

        #endregion

        #region Ctor
        public FlashSaleService(
            IRepository<FlashSale> flashSaleRepository,
            IRepository<Product> productRepository,
            IEventPublisher eventPublisher,
            IWorkContext workContext,
            CatalogSettings catalogSettings


           )
        {
            this._flashSaleRepository = flashSaleRepository;
            this._productRepository = productRepository;
            this._eventPublisher = eventPublisher;
            this._workContext = workContext;
            this._catalogSettings = catalogSettings;

        }

        #endregion

        #region Method

        public virtual void DeleteFlashSale(FlashSale flashSale)
        {
            if (flashSale == null)
                throw new ArgumentNullException("flashSale");
            _flashSaleRepository.Delete(flashSale);

            //event notification
            _eventPublisher.EntityDeleted(flashSale);
        }

        public virtual FlashSale GetFlashSaleById(string flashSaleId)
        {
            
            return _flashSaleRepository.GetById(flashSaleId);
        }


        public virtual void InsertFlashSale(FlashSale flashSale)
        {
            if (flashSale == null)
                throw new ArgumentNullException("flashSale");
           
            _flashSaleRepository.Insert(flashSale);


            //event notification
            _eventPublisher.EntityInserted(flashSale);
        }

        public virtual void UpdateFlashSale(FlashSale flashSale)
        {
            if (flashSale == null)
                throw new ArgumentNullException("flashSale");

            _flashSaleRepository.Update(flashSale);

            //event notification
            _eventPublisher.EntityUpdated(flashSale);
        }

        public virtual IPagedList<FlashSale> GetAllFlashSale(string storeId = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _flashSaleRepository.Table;

            if (!showHidden)
            {
                var utcNow = DateTime.UtcNow;
                query = query.Where(p => p.IsEnabled);

                //query = query.Where(p => !p.StartDateUtc.HasValue || p.StartDateUtc <= utcNow);
                query = query.Where(p => !p.EndDateUtc.HasValue || p.EndDateUtc >= utcNow).OrderBy(p => p.StartDateUtc);
            }
            
  
            var flashSales = new PagedList<FlashSale>(query, pageIndex, pageSize);
            return flashSales;
        }

        public virtual IPagedList<FlashSale> GetAllCurrentFlashSale(string storeId = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var builder = Builders<FlashSale>.Filter;
            var filter = builder.Eq(x => x.IsEnabled, true);
            var nowUtc = DateTime.UtcNow;
            var query = from c in _flashSaleRepository.Table
                        where ((c.StartDateUtc == null || c.StartDateUtc <= nowUtc) && (c.EndDateUtc == null || c.EndDateUtc >= nowUtc))
                        select c;

            query = query.OrderBy(p => p.StartDateUtc);

            if (!showHidden)
            {
                query = query.Where(p => p.IsEnabled);
            }

            var flashSale = new PagedList<FlashSale>(query, pageIndex, pageSize);
            return flashSale;

        }


        #endregion
    }
}
