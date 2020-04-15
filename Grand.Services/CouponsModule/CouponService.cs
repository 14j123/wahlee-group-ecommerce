using Grand.Core;
using Grand.Core.Caching;
using Grand.Core.Data;
using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Customers;
using Grand.Core.Domain.CouponsModule;
using Grand.Core.Domain.Orders;
using Grand.Core.Domain.Stores;
using Grand.Core.Domain.Vendors;
using Grand.Core.Plugins;
using Grand.Services.Common;
using Grand.Services.Customers;
using Grand.Services.CouponsModule.Cache;
using Grand.Services.Events;
using Grand.Services.Localization;
using Grand.Services.Orders;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grand.Services.CouponsModule
{
    /// <summary>
    /// coupon service
    /// </summary>
    public partial class CouponService : ICouponService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : discont ID
        /// </remarks>
        private const string COUPONS_BY_ID_KEY = "Grand.coupon.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : coupon code
        /// {2} : coupon name
        /// </remarks>
        private const string COUPONS_ALL_KEY = "Grand.coupon.all-{0}-{1}-{2}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string COUPONS_PATTERN_KEY = "Grand.coupon.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string PRODUCTS_PATTERN_KEY = "Grand.product.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string MANUFACTURERS_PATTERN_KEY = "Grand.manufacturer.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CATEGORIES_PATTERN_KEY = "Grand.category.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string VENDORS_PATTERN_KEY = "Grand.vendor.";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string STORES_PATTERN_KEY = "Grand.store.";

        #endregion

        #region Fields

        private readonly IRepository<Coupon> _couponRepository;
        private readonly IRepository<CouponTicket> _couponTicketRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Manufacturer> _manufacturerRepository;
        private readonly IRepository<CouponUsageHistory> _couponUsageHistoryRepository;
        private readonly IRepository<Vendor> _vendorRepository;
        private readonly IRepository<Store> _storeRepository;
        private readonly ILocalizationService _localizationService;
        private readonly ICacheManager _cacheManager;
        private readonly IStoreContext _storeContext;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IPluginFinder _pluginFinder;
        private readonly IEventPublisher _eventPublisher;
        private readonly PerRequestCacheManager _perRequestCache;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        public CouponService(ICacheManager cacheManager,
            IRepository<Coupon> couponRepository,
            IRepository<CouponTicket> couponTicketRepository,
            IRepository<CouponUsageHistory> couponUsageHistoryRepository,
            ILocalizationService localizationService,
            IStoreContext storeContext,
            IGenericAttributeService genericAttributeService,
            IPluginFinder pluginFinder,
            IEventPublisher eventPublisher,
            IRepository<Product> productRepository,
            IRepository<Category> categoryRepository,
            IRepository<Manufacturer> manufacturerRepository,
            IRepository<Vendor> vendorRepository,
            IRepository<Store> storeRepository,
            PerRequestCacheManager perRequestCache
            )
        {
            this._cacheManager = cacheManager;
            this._couponRepository = couponRepository;
            this._couponTicketRepository = couponTicketRepository;
            this._couponUsageHistoryRepository = couponUsageHistoryRepository;
            this._localizationService = localizationService;
            this._storeContext = storeContext;
            this._genericAttributeService = genericAttributeService;
            this._pluginFinder = pluginFinder;
            this._eventPublisher = eventPublisher;
            this._productRepository = productRepository;
            this._categoryRepository = categoryRepository;
            this._manufacturerRepository = manufacturerRepository;
            this._vendorRepository = vendorRepository;
            this._storeRepository = storeRepository;
            this._perRequestCache = perRequestCache;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete coupon
        /// </summary>
        /// <param name="coupon">Coupon</param>
        public virtual void DeleteCoupon(Coupon coupon)
        {
            if (coupon == null)
                throw new ArgumentNullException("coupon");

            var usagehistory = GetAllCouponUsageHistory(coupon.Id);
            if (usagehistory.Count > 0)
                throw new ArgumentNullException("coupon has a history");

            var builder = Builders<BsonDocument>.Filter;
            if (coupon.CouponType == CouponType.AssignedToSkus)
            {
                var builderproduct = Builders<Product>.Update;
                var updatefilter = builderproduct.Pull(x => x.AppliedCoupons, coupon.Id);
                var result = _productRepository.Collection.UpdateManyAsync(new BsonDocument(), updatefilter).Result;
                _cacheManager.RemoveByPattern(PRODUCTS_PATTERN_KEY);
            }

            if (coupon.CouponType == CouponType.AssignedToCategories)
            {
                var buildercategory = Builders<Category>.Update;
                var updatefilter = buildercategory.Pull(x => x.AppliedCoupons, coupon.Id);
                var result = _categoryRepository.Collection.UpdateManyAsync(new BsonDocument(), updatefilter).Result;
                _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            }

            if (coupon.CouponType == CouponType.AssignedToManufacturers)
            {
                var buildermanufacturer = Builders<Manufacturer>.Update;
                var updatefilter = buildermanufacturer.Pull(x => x.AppliedCoupons, coupon.Id);
                var result = _manufacturerRepository.Collection.UpdateManyAsync(new BsonDocument(), updatefilter).Result;
                _cacheManager.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
            }
            if (coupon.CouponType == CouponType.AssignedToVendors)
            {
                var buildervendor = Builders<Vendor>.Update;
                var updatefilter = buildervendor.Pull(x => x.AppliedCoupons, coupon.Id);
                var result = _vendorRepository.Collection.UpdateManyAsync(new BsonDocument(), updatefilter).Result;
                _cacheManager.RemoveByPattern(VENDORS_PATTERN_KEY);
            }

            if (coupon.CouponType == CouponType.AssignedToStores)
            {
                var builderstore = Builders<Store>.Update;
                var updatefilter = builderstore.Pull(x => x.AppliedCoupons, coupon.Id);
                var result = _storeRepository.Collection.UpdateManyAsync(new BsonDocument(), updatefilter).Result;
                _cacheManager.RemoveByPattern(STORES_PATTERN_KEY);
            }


            //remove coupon codes
            var filtersCoupon = Builders<CouponTicket>.Filter;
            var filterCrp = filtersCoupon.Eq(x => x.CouponId, coupon.Id);
            _couponTicketRepository.Collection.DeleteMany(filterCrp);

            _couponRepository.Delete(coupon);

            _cacheManager.RemoveByPattern(COUPONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(coupon);
        }

        /// <summary>
        /// Gets a coupon
        /// </summary>
        /// <param name="couponId">Coupon identifier</param>
        /// <returns>Coupon</returns>
        public virtual Coupon GetCouponById(string couponId)
        {
            string key = string.Format(COUPONS_BY_ID_KEY, couponId);
            return _cacheManager.Get(key, () => _couponRepository.GetById(couponId));
        }

        /// <summary>
        /// Gets all coupons
        /// </summary>
        /// <param name="couponType">Coupon type; null to load all coupon</param>
        /// <param name="couponCode">Coupon code to find (exact match)</param>
        /// <param name="couponName">Coupon name</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Coupons</returns>
        public virtual IList<Coupon> GetAllCoupons(CouponType? couponType,
            string couponCode = "", string couponName = "", bool showHidden = false)
        {
            //we load all coupons, and filter them by passed "couponType" parameter later
            //we do it because we know that this method is invoked several times per HTTP request with distinct "couponType" parameter
            //that's why let's access the database only once
            string key = string.Format(COUPONS_ALL_KEY, showHidden, couponCode, couponName);
            var result = _cacheManager.Get(key, () =>
            {
                var query = _couponRepository.Table;
                if (!showHidden)
                {
                    //The function 'CurrentUtcDateTime' is not supported by SQL Server Compact. 
                    //That's why we pass the date value
                    var nowUtc = DateTime.UtcNow;
                    query = query.Where(d =>
                        (!d.StartDateUtc.HasValue || d.StartDateUtc <= nowUtc)
                        && (!d.EndDateUtc.HasValue || d.EndDateUtc >= nowUtc)
                        && d.IsEnabled);
                }
                if (!String.IsNullOrEmpty(couponCode))
                {
                    var _coupon = _couponTicketRepository.Table.FirstOrDefault(x => x.CouponCode == couponCode);
                    if(_coupon!=null)
                        query = query.Where(d => d.Id == _coupon.CouponId);
                }
                if (!String.IsNullOrEmpty(couponName))
                {
                    query = query.Where(d => d.Name != null && d.Name.ToLower().Contains(couponName.ToLower()));
                }
                query = query.OrderBy(d => d.Name);

                var coupons = query.ToList();
                return coupons;
            });
            //we know that this method is usually inkoved multiple times
            //that's why we filter coupons by type on the application layer
            if (couponType.HasValue)
            {
                result = result.Where(d => d.CouponType == couponType.Value).ToList();
            }
            return result;
        }

        /// <summary>
        /// Inserts a coupon
        /// </summary>
        /// <param name="coupon">Coupon</param>
        public virtual void InsertCoupon(Coupon coupon)
        {
            if (coupon == null)
                throw new ArgumentNullException("coupon");

            _couponRepository.Insert(coupon);

            _cacheManager.RemoveByPattern(COUPONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(coupon);
        }

        /// <summary>
        /// Updates the coupon
        /// </summary>
        /// <param name="coupon">Coupon</param>
        public virtual void UpdateCoupon(Coupon coupon)
        {
            if (coupon == null)
                throw new ArgumentNullException("coupon");

            foreach (var req in coupon.CouponRequirements)
            {
                req.CouponId = coupon.Id;
            }

            _couponRepository.Update(coupon);

            _cacheManager.RemoveByPattern(COUPONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(coupon);
        }

        /// <summary>
        /// Delete coupon requirement
        /// </summary>
        /// <param name="couponRequirement">Coupon requirement</param>
        public virtual void DeleteCouponRequirement(CouponRequirement couponRequirement)
        {
            if (couponRequirement == null)
                throw new ArgumentNullException("couponRequirement");

            var coupon = _couponRepository.GetById(couponRequirement.CouponId);
            if (coupon == null)
                throw new ArgumentNullException("coupon");
            var req = coupon.CouponRequirements.FirstOrDefault(x => x.Id == couponRequirement.Id);
            if (req == null)
                throw new ArgumentNullException("couponRequirement");

            coupon.CouponRequirements.Remove(req);
            UpdateCoupon(coupon);

            _cacheManager.RemoveByPattern(COUPONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(couponRequirement);
        }

        /// <summary>
        /// Load coupon by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Found coupon</returns>
        public virtual ICoupon LoadCouponPluginBySystemName(string systemName)
        {
            var couponPlugins = LoadAllCouponPlugins();
            foreach (var couponPlugin in couponPlugins)
            {
                var rules = couponPlugin.GetRequirementRules();

                if (!rules.Any(x => x.SystemName == systemName))
                    continue;
                return couponPlugin;
            }
            return null;
        }

        /// <summary>
        /// Load all coupon requirement rules
        /// </summary>
        /// <returns>Coupon requirement rules</returns>
        public virtual IList<ICoupon> LoadAllCouponPlugins()
        {
            var couponPlugins = _pluginFinder.GetPlugins<ICoupon>();
            return couponPlugins.ToList();
        }


        /// <summary>
        /// Get coupon by coupon code
        /// </summary>
        /// <param name="couponCode">Coupon code</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Coupon</returns>
        public virtual Coupon GetCouponByCouponCode(string couponCode, bool showHidden = false)
        {
            if (String.IsNullOrWhiteSpace(couponCode))
                return null;

            var builder = Builders<CouponTicket>.Filter;
            var filter = builder.Eq(x => x.CouponCode, couponCode);
            var query = _couponTicketRepository.Collection.Find(filter);

            var coupon = query.FirstOrDefault();
            if(coupon == null)
                return null;

            var coupons = GetCouponById(coupon.CouponId);
            return coupons;
        }

        /// <summary>
        /// Exist coupon code in coupon
        /// </summary>
        /// <param name="couponCode"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        public virtual bool ExistsCodeInCoupon(string couponCode, string couponId, bool? used)
        {
            if (String.IsNullOrWhiteSpace(couponCode))
                return false;

            var builder = Builders<CouponTicket>.Filter;
            var filter = builder.Eq(x => x.CouponCode, couponCode);
            filter = filter & builder.Eq(x => x.CouponId, couponId);
            if(used.HasValue)
                filter = filter & builder.Eq(x => x.Used, used.Value);
            var query = _couponTicketRepository.Collection.Find(filter);
            if (query.Any())
                return true;
            else
                return false;
        }

        /// <summary>
        /// Get all coupon codes for coupon
        /// </summary>
        /// <param name="couponId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public virtual IPagedList<CouponTicket> GetAllCouponCodesByCouponId(string couponId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _couponTicketRepository.Table;

            if (!String.IsNullOrEmpty(couponId))
                query = query.Where(duh => duh.CouponId == couponId);
            query = query.OrderByDescending(c => c.CouponCode);
            return new PagedList<CouponTicket>(query, pageIndex, pageSize);
        }


        /// <summary>
        /// Gets a coupon
        /// </summary>
        /// <param name="couponId">Coupon identifier</param>
        /// <returns>Coupon</returns>
        public virtual CouponTicket GetCouponCodeById(string id)
        {
            return _couponTicketRepository.GetById(id);
        }

        /// <summary>
        /// Get coupon code by coupon code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public CouponTicket GetCouponCodeByCode(string couponCode)
        {
            var builder = Builders<CouponTicket>.Filter;
            var filter = builder.Eq(x => x.CouponCode, couponCode);
            var query = _couponTicketRepository.Collection.Find(filter);
            return query.FirstOrDefault();
        }


        /// <summary>
        /// Delete coupon code
        /// </summary>
        /// <param name="coupon"></param>
        public virtual void DeleteCouponTicket(CouponTicket coupon)
        {
            _couponTicketRepository.Delete(coupon);
        }

        /// <summary>
        /// Insert coupon code
        /// </summary>
        /// <param name="coupon"></param>
        public virtual void InsertCouponTicket(CouponTicket coupon)
        {
            _couponTicketRepository.Insert(coupon);
        }

        /// <summary>
        /// Update coupon code - set as used or not
        /// </summary>
        /// <param name="coupon"></param>
        public virtual void CouponTicketSetAsUsed(string couponCode, bool used)
        {
            if (string.IsNullOrEmpty(couponCode))
                return;

            var coupon = GetCouponCodeByCode(couponCode);
            if (coupon != null)
            {
                if (used)
                {
                    coupon.Used = used;
                    coupon.Qty = coupon.Qty + 1;
                }
                else
                {
                    coupon.Qty = coupon.Qty - 1;
                    coupon.Used = coupon.Qty > 0 ? true : false;
                }
                _couponTicketRepository.Update(coupon);
            }
        }

        /// <summary>
        /// Cancel coupon if order was canceled or deleted
        /// </summary>
        /// <param name="orderId"></param>
        public virtual void CancelCoupon(string orderId)
        {
            var couponUsage = _couponUsageHistoryRepository.Table.Where(x => x.OrderId == orderId);
            foreach (var item in couponUsage)
            {
                CouponTicketSetAsUsed(item.CouponCode, false);
                item.Canceled = true;
                UpdateCouponUsageHistory(item);
            }
        }

        /// <summary>
        /// Validate coupon
        /// </summary>
        /// <param name="coupon">Coupon</param>
        /// <param name="customer">Customer</param>
        /// <returns>Coupon validation result</returns>
        public virtual CouponValidationResult ValidateCoupon(Coupon coupon, Customer customer)
        {
            if (coupon == null)
                throw new ArgumentNullException("coupon");

            string[] couponCodesToValidate = null;
            if (customer != null)
                couponCodesToValidate = customer.ParseAppliedCouponTicketCodes();

            return ValidateCoupon(coupon, customer, couponCodesToValidate);
        }

        /// <summary>
        /// Validate coupon
        /// </summary>
        /// <param name="coupon">Coupon</param>
        /// <param name="customer">Customer</param>
        /// <param name="couponCodeToValidate">Coupon code to validate</param>
        /// <returns>Coupon validation result</returns>
        public virtual CouponValidationResult ValidateCoupon(Coupon coupon, Customer customer, string couponCodeToValidate)
        {
            if (!String.IsNullOrEmpty(couponCodeToValidate))
            {
                return ValidateCoupon(coupon, customer, new string[] { couponCodeToValidate });
            }
            else
                return ValidateCoupon(coupon, customer, new string[0]);

        }

        /// <summary>
        /// Validate coupon
        /// </summary>
        /// <param name="coupon">Coupon</param>
        /// <param name="customer">Customer</param>
        /// <param name="couponCodesToValidate">Coupon codes to validate</param>
        /// <returns>Coupon validation result</returns>
        public virtual CouponValidationResult ValidateCoupon(Coupon coupon, Customer customer, string[] couponCodesToValidate)
        {
            if (coupon == null)
                throw new ArgumentNullException("coupon");

            if (customer == null)
                throw new ArgumentNullException("customer");

            //invalid by default

            string key = $"CouponValidationResult_{customer.Id}_{coupon.Id}_{string.Join("_", couponCodesToValidate)}";
            var validationResult = _perRequestCache.Get(key, () =>
            {
                var result = new CouponValidationResult();

                //check is enabled
                if (!coupon.IsEnabled)
                    return result;

                //check coupon code
                if (coupon.RequiresCouponCode)
                {
                    if (couponCodesToValidate == null || !couponCodesToValidate.Any())
                        return result;
                    var exists = false;
                    foreach (var item in couponCodesToValidate)
                    {
                        if (coupon.Reused)
                        {
                            if (ExistsCodeInCoupon(item, coupon.Id, null))
                            {
                                result.CouponCode = item;
                                exists = true;
                            }
                        }
                        else
                        {
                            if (ExistsCodeInCoupon(item, coupon.Id, false))
                            {
                                result.CouponCode = item;
                                exists = true;
                            }
                        }
                    }
                    if (!exists)
                        return result;
                }

                //Do not allow coupons applied to order subtotal or total when a customer has gift cards in the cart.
                //Otherwise, this customer can purchase gift cards with coupon and get more than paid ("free money").
                if (coupon.CouponType == CouponType.AssignedToOrderSubTotal ||
                    coupon.CouponType == CouponType.AssignedToOrderTotal)
                {
                    var cart = customer.ShoppingCartItems
                        .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                        .LimitPerStore(_storeContext.CurrentStore.Id)
                        .ToList();

                    var hasGiftCards = cart.Any(x => x.IsGiftCard);
                    if (hasGiftCards)
                    {
                        result.UserError = _localizationService.GetResource("ShoppingCart.Coupon.CannotBeUsedWithGiftCards");
                        return result;
                    }
                }

                //check date range
                DateTime now = DateTime.UtcNow;
                if (coupon.StartDateUtc.HasValue)
                {
                    DateTime startDate = DateTime.SpecifyKind(coupon.StartDateUtc.Value, DateTimeKind.Utc);
                    if (startDate.CompareTo(now) > 0)
                    {
                        result.UserError = _localizationService.GetResource("ShoppingCart.Coupon.NotStartedYet");
                        return result;
                    }
                }
                if (coupon.EndDateUtc.HasValue)
                {
                    DateTime endDate = DateTime.SpecifyKind(coupon.EndDateUtc.Value, DateTimeKind.Utc);
                    if (endDate.CompareTo(now) < 0)
                    {
                        result.UserError = _localizationService.GetResource("ShoppingCart.Coupon.Expired");
                        return result;
                    }
                }

                //coupon limitation
                switch (coupon.CouponLimitation)
                {
                    case CouponLimitationType.NTimesOnly:
                        {
                            var usedTimes = GetAllCouponUsageHistory(coupon.Id, null, null, false, 0, 1).TotalCount;
                            if (usedTimes >= coupon.LimitationTimes)
                                return result;
                        }
                        break;
                    case CouponLimitationType.NTimesPerCustomer:
                        {
                            if (customer.IsRegistered())
                            {
                                var usedTimes = GetAllCouponUsageHistory(coupon.Id, customer.Id, null, false, 0, 1).TotalCount;
                                if (usedTimes >= coupon.LimitationTimes)
                                {
                                    result.UserError = _localizationService.GetResource("ShoppingCart.Coupon.CannotBeUsedAnymore");
                                    return result;
                                }
                            }
                        }
                        break;
                    case CouponLimitationType.Unlimited:
                    default:
                        break;
                }

                //coupon requirements
                string keyReq = string.Format(CouponRequirementEventConsumer.COUPON_REQUIREMENT_MODEL_KEY, coupon.Id);
                var requirements = _cacheManager.Get(keyReq, () =>
                {
                    return coupon.CouponRequirements.ToList();
                });
                foreach (var req in requirements)
                {
                    //load a plugin
                    var couponRequirementPlugin = LoadCouponPluginBySystemName(req.CouponRequirementRuleSystemName);

                    if (couponRequirementPlugin == null)
                        continue;

                    if (!_pluginFinder.AuthenticateStore(couponRequirementPlugin.PluginDescriptor, _storeContext.CurrentStore.Id))
                        continue;

                    var ruleRequest = new CouponRequirementValidationRequest
                    {
                        CouponRequirementId = req.Id,
                        CouponId = req.CouponId,
                        Customer = customer,
                        Store = _storeContext.CurrentStore
                    };

                    var singleRequirementRule = couponRequirementPlugin.GetRequirementRules().Single(x => x.SystemName == req.CouponRequirementRuleSystemName);
                    var ruleResult = singleRequirementRule.CheckRequirement(ruleRequest);
                    if (!ruleResult.IsValid)
                    {
                        result.UserError = ruleResult.UserError;
                        return result;
                    }
                }

                result.IsValid = true;
                return result;
            });
            
            return validationResult;
        }
        /// <summary>
        /// Gets a coupon usage history record
        /// </summary>
        /// <param name="couponUsageHistoryId">Coupon usage history record identifier</param>
        /// <returns>Coupon usage history</returns>
        public virtual CouponUsageHistory GetCouponUsageHistoryById(string couponUsageHistoryId)
        {
            return _couponUsageHistoryRepository.GetById(couponUsageHistoryId);
        }

        /// <summary>
        /// Gets all coupon usage history records
        /// </summary>
        /// <param name="couponId">Coupon identifier; null to load all records</param>
        /// <param name="customerId">Customer identifier; null to load all records</param>
        /// <param name="orderId">Order identifier; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Coupon usage history records</returns>
        public virtual IPagedList<CouponUsageHistory> GetAllCouponUsageHistory(string couponId = "",
            string customerId = "", string orderId = "", bool? canceled = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _couponUsageHistoryRepository.Table;

            if (!String.IsNullOrEmpty(couponId))
                query = query.Where(duh => duh.CouponId == couponId);
            if (!String.IsNullOrEmpty(customerId))
                query = query.Where(duh => duh.CustomerId == customerId);
            if (!String.IsNullOrEmpty(orderId))
                query = query.Where(duh => duh.OrderId == orderId);
            if(canceled.HasValue)
                query = query.Where(duh => duh.Canceled == canceled.Value);
            query = query.OrderByDescending(c => c.CreatedOnUtc);
            return new PagedList<CouponUsageHistory>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Insert coupon usage history record
        /// </summary>
        /// <param name="couponUsageHistory">Coupon usage history record</param>
        public virtual void InsertCouponUsageHistory(CouponUsageHistory couponUsageHistory)
        {
            if (couponUsageHistory == null)
                throw new ArgumentNullException("couponUsageHistory");

            _couponUsageHistoryRepository.Insert(couponUsageHistory);

            //Support for couponcode
            CouponTicketSetAsUsed(couponUsageHistory.CouponCode, true);

            _cacheManager.RemoveByPattern(COUPONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(couponUsageHistory);
        }


        /// <summary>
        /// Update coupon usage history record
        /// </summary>
        /// <param name="couponUsageHistory">Coupon usage history record</param>
        public virtual void UpdateCouponUsageHistory(CouponUsageHistory couponUsageHistory)
        {
            if (couponUsageHistory == null)
                throw new ArgumentNullException("couponUsageHistory");

            _couponUsageHistoryRepository.Update(couponUsageHistory);

            _cacheManager.RemoveByPattern(COUPONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(couponUsageHistory);
        }

        /// <summary>
        /// Delete coupon usage history record
        /// </summary>
        /// <param name="couponUsageHistory">Coupon usage history record</param>
        public virtual void DeleteCouponUsageHistory(CouponUsageHistory couponUsageHistory)
        {
            if (couponUsageHistory == null)
                throw new ArgumentNullException("couponUsageHistory");

            _couponUsageHistoryRepository.Delete(couponUsageHistory);

            _cacheManager.RemoveByPattern(COUPONS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(couponUsageHistory);
        }

        /// <summary>
        /// Get coupon amount
        /// </summary>
        /// <param name="coupon"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public decimal GetCouponAmount(Coupon coupon, Customer customer, Product product, decimal amount)
        {
            if (coupon == null)
                throw new ArgumentNullException("coupon");

            //calculate coupon amount
            decimal result = decimal.Zero;
            if (!coupon.CalculateByPlugin)
            {
                if (coupon.UsePercentage)
                    result = (decimal)((((float)amount) * ((float)coupon.CouponPercentage)) / 100f);
                else
                    result = coupon.CouponAmount;
            }
            else
            {
                result = GetCouponAmountProvider(coupon, customer, product, amount);
            }

            //validate maximum coupon amount
            if (coupon.UsePercentage &&
                coupon.MaximumCouponAmount.HasValue &&
                result > coupon.MaximumCouponAmount.Value)
                result = coupon.MaximumCouponAmount.Value;

            if (result < decimal.Zero)
                result = decimal.Zero;

            return result;
        }

        /// <summary>
        /// Get preferred coupon (with maximum coupon value)
        /// </summary>
        /// <param name="coupons">A list of coupons to check</param>
        /// <param name="customer"
        /// <param name="product"></param>
        /// <param name="amount">Amount</param>
        /// <param name="couponAmount"></param>
        /// <returns>Preferred coupon</returns>
        public List<AppliedCoupon> GetPreferredCoupon(IList<AppliedCoupon> coupons,
            Customer customer, Product product,
            decimal amount, out decimal couponAmount)
        {
            if (coupons == null)
                throw new ArgumentNullException("coupons");

            var result = new List<AppliedCoupon>();
            couponAmount = decimal.Zero;
            if (!coupons.Any())
                return result;

            //first we check simple coupons
            foreach (var appliedcoupon in coupons)
            {
                var coupon = GetCouponById(appliedcoupon.CouponId);
                decimal currentCouponValue = GetCouponAmount(coupon, customer, product, amount);
                if (currentCouponValue > couponAmount)
                {
                    couponAmount = currentCouponValue;
                    result.Clear();
                    result.Add(appliedcoupon);
                }
            }
            //now let's check cumulative coupon
            //right now we calculate coupon values based on the original amount value
            //please keep it in mind if you're going to use coupons with "percentage"
            var cumulativeCoupons = coupons.Where(x => x.IsCumulative).ToList();
            if (cumulativeCoupons.Count > 1)
            {
                var cumulativeCouponAmount = cumulativeCoupons.Sum(d => GetCouponAmount(GetCouponById(d.CouponId),customer, product, amount));
                if (cumulativeCouponAmount > couponAmount)
                {
                    couponAmount = cumulativeCouponAmount;

                    result.Clear();
                    result.AddRange(cumulativeCoupons);
                }
            }

            return result;
        }
        /// <summary>
        /// Get preferred coupon (with maximum coupon value)
        /// </summary>
        /// <param name="coupons">A list of coupons to check</param>
        /// <param name="customer"
        /// <param name="amount">Amount</param>
        /// <param name="couponAmount"></param>
        /// <returns>Preferred coupon</returns>
        public List<AppliedCoupon> GetPreferredCoupon(IList<AppliedCoupon> coupons,
            Customer customer,
            decimal amount, out decimal couponAmount)
        {
            return GetPreferredCoupon(coupons, customer, null, amount, out couponAmount);
        }

        /// <summary>
        /// Get amount from coupon amount provider 
        /// </summary>
        /// <param name="coupon"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public virtual decimal GetCouponAmountProvider(Coupon coupon, Customer customer, Product product, decimal amount)
        {
            var couponAmountProvider = LoadCouponAmountProviderBySystemName(coupon.CouponPluginName);
            if (couponAmountProvider == null)
                return 0;
            return couponAmountProvider.CouponAmount(coupon, customer, product, amount);
        }


        public virtual ICouponAmountProvider LoadCouponAmountProviderBySystemName(string systemName)
        {
            var descriptor = _pluginFinder.GetPluginDescriptorBySystemName<ICouponAmountProvider>(systemName);
            if (descriptor != null)
                return descriptor.Instance<ICouponAmountProvider>();

            return null;
        }

        /// <summary>
        /// Get all coupon amount providers
        /// </summary>
        /// <returns></returns>
        public IList<ICouponAmountProvider> LoadCouponAmountProviders()
        {
            var couponAmountProviders = _pluginFinder.GetPlugins<ICouponAmountProvider>();
            return couponAmountProviders
                .OrderBy(tp => tp.PluginDescriptor)
                .ToList();
        }

        #endregion
    }
}
