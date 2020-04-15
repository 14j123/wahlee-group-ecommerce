using Grand.Core.Domain.Catalog;
using Grand.Core.Domain.Customers;
using Grand.Core.Domain.CouponsModule;
using Grand.Core.Plugins;

namespace Grand.Services.CouponsModule
{
    public partial interface ICouponAmountProvider : IPlugin
    {
        decimal CouponAmount(Coupon coupon, Customer customer, Product product, decimal amount);
    }
}
