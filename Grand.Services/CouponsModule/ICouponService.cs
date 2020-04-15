using Grand.Core;
using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Customers;
using Grand.Core.Domain.CouponsModule;
using System.Collections.Generic;

namespace Grand.Services.CouponsModule
{
    /// <summary>
    /// Coupon service interface
    /// </summary>
    public partial interface ICouponService
    {
        /// <summary>
        /// Delete coupon
        /// </summary>
        /// <param name="coupon">Coupon</param>
        void DeleteCoupon(Coupon coupon);

        /// <summary>
        /// Gets a coupon
        /// </summary>
        /// <param name="couponId">Coupon identifier</param>
        /// <returns>Coupon</returns>
        Coupon GetCouponById(string couponId);

        /// <summary>
        /// Gets all coupons
        /// </summary>
        /// <param name="couponType">Coupon type; null to load all coupon</param>
        /// <param name="couponCode">Coupon code to find (exact match)</param>
        /// <param name="couponName">Coupon name</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Coupons</returns>
        IList<Coupon> GetAllCoupons(CouponType? couponType,
            string couponCode = "", string couponName = "", bool showHidden = false);
         

        /// <summary>
        /// Inserts a coupon
        /// </summary>
        /// <param name="coupon">Coupon</param>
        void InsertCoupon(Coupon coupon);

        /// <summary>
        /// Updates the coupon
        /// </summary>
        /// <param name="coupon">Coupon</param>
        void UpdateCoupon(Coupon coupon);

        /// <summary>
        /// Delete coupon requirement
        /// </summary>
        /// <param name="couponRequirement">Coupon requirement</param>
        void DeleteCouponRequirement(CouponRequirement couponRequirement);

        /// <summary>
        /// Load coupon plugin by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Coupon plugin</returns>
        ICoupon LoadCouponPluginBySystemName(string systemName);

        /// <summary>
        /// Load all coupon plugins
        /// </summary>
        /// <returns>Coupon requirement rules</returns>
        IList<ICoupon> LoadAllCouponPlugins();


        /// <summary>
        /// Get coupon by coupon code
        /// </summary>
        /// <param name="couponCode">CouponCode</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Coupon</returns>
        Coupon GetCouponByCouponCode(string couponCode, bool showHidden = false);

        /// <summary>
        /// Exist coupon code in coupon
        /// </summary>
        /// <param name="couponCode"></param>
        /// <param name="couponId"></param>
        /// <returns></returns>
        bool ExistsCodeInCoupon(string couponCode, string couponId, bool? used);

        /// <summary>
        /// Validate coupon
        /// </summary>
        /// <param name="coupon">Coupon</param>
        /// <param name="customer">Customer</param>
        /// <returns>Coupon validation result</returns>
        CouponValidationResult ValidateCoupon(Coupon coupon, Customer customer);

        /// <summary>
        /// Validate coupon
        /// </summary>
        /// <param name="coupon">Coupon</param>
        /// <param name="customer">Customer</param>
        /// <param name="couponCodeToValidate">Coupon code to validate</param>
        /// <returns>Coupon validation result</returns>
        CouponValidationResult ValidateCoupon(Coupon coupon, Customer customer, string couponCodeToValidate);

        /// <summary>
        /// Validate coupon
        /// </summary>
        /// <param name="coupon">Coupon</param>
        /// <param name="customer">Customer</param>
        /// <param name="couponCodesToValidate">Coupon codes to validate</param>
        /// <returns>Coupon validation result</returns>
        CouponValidationResult ValidateCoupon(Coupon coupon, Customer customer, string[] couponCodesToValidate);


        /// <summary>
        /// Gets a coupon usage history record
        /// </summary>
        /// <param name="couponUsageHistoryId">Coupon usage history record identifier</param>
        /// <returns>Coupon usage history</returns>
        CouponUsageHistory GetCouponUsageHistoryById(string couponUsageHistoryId);

        /// <summary>
        /// Gets all coupon usage history records
        /// </summary>
        /// <param name="couponId">Coupon identifier; null to load all records</param>
        /// <param name="customerId">Customer identifier; null to load all records</param>
        /// <param name="orderId">Order identifier; null to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Coupon usage history records</returns>
        IPagedList<CouponUsageHistory> GetAllCouponUsageHistory(string couponId = "",
            string customerId = "", string orderId = "", bool? canceled = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Insert coupon usage history record
        /// </summary>
        /// <param name="couponUsageHistory">Coupon usage history record</param>
        void InsertCouponUsageHistory(CouponUsageHistory couponUsageHistory);

        /// <summary>
        /// Update coupon usage history record
        /// </summary>
        /// <param name="couponUsageHistory">Coupon usage history record</param>
        void UpdateCouponUsageHistory(CouponUsageHistory couponUsageHistory);

        /// <summary>
        /// Delete coupon usage history record
        /// </summary>
        /// <param name="couponUsageHistory">Coupon usage history record</param>
        void DeleteCouponUsageHistory(CouponUsageHistory couponUsageHistory);

        /// <summary>
        /// Get all coupon codes for coupon
        /// </summary>
        /// <param name="couponId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<CouponTicket> GetAllCouponCodesByCouponId(string couponId, int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// Get coupon code by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CouponTicket GetCouponCodeById(string id);

        /// <summary>
        /// Get coupon code by coupon code
        /// </summary>
        /// <param name="couponCode"></param>
        /// <returns></returns>
        CouponTicket GetCouponCodeByCode(string couponCode);

        /// <summary>
        /// Delete coupon code
        /// </summary>
        /// <param name="coupon"></param>
        void DeleteCouponTicket(CouponTicket coupon);

        /// <summary>
        /// Update coupon code - set as used or not
        /// </summary>
        /// <param name="couponCode"></param>
        /// <param name="couponId"></param>
        /// <param name="used"></param>
        void CouponTicketSetAsUsed(string couponCode, bool used);

        /// <summary>
        /// Cancel coupon if order was canceled or deleted
        /// </summary>
        /// <param name="orderId"></param>
        void CancelCoupon(string orderId);

        /// <summary>
        /// Insert coupon code
        /// </summary>
        /// <param name="coupon"></param>
        void InsertCouponTicket(CouponTicket coupon);

        /// <summary>
        /// Get coupon amount from plugin
        /// </summary>
        /// <param name="coupon"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        decimal GetCouponAmount(Coupon coupon, Customer customer, Product product, decimal amount);

        /// <summary>
        /// Get preferred coupon
        /// </summary>
        /// <param name="coupons"></param>
        /// <param name="amount"></param>
        /// <param name="customer"></param>
        /// <param name="product"></param>
        /// <param name="couponAmount"></param>
        /// <returns></returns>
        List<AppliedCoupon> GetPreferredCoupon(IList<AppliedCoupon> Coupons,
            Customer customer, Product product, decimal amount, out decimal couponAmount);

        /// <summary>
        /// Get preferred coupon
        /// </summary>
        /// <param name="coupons"></param>
        /// <param name="amount"></param>
        /// <param name="customer"></param>
        /// <param name="couponAmount"></param>
        /// <returns></returns>
        List<AppliedCoupon> GetPreferredCoupon(IList<AppliedCoupon> Coupons,
            Customer customer, decimal amount, out decimal couponAmount);

        /// <summary>
        /// GetCouponAmountProvider
        /// </summary>
        /// <param name="coupon"></param>
        /// <param name="customer"></param>
        /// <param name="product"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        decimal GetCouponAmountProvider(Coupon coupon, Customer customer, Product product, decimal amount);

        /// <summary>
        /// Load coupon amount provider by systemName
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        ICouponAmountProvider LoadCouponAmountProviderBySystemName(string systemName);

        /// <summary>
        /// Load coupon amount providers
        /// </summary>
        /// <returns></returns>
        IList<ICouponAmountProvider> LoadCouponAmountProviders();
    }
}
