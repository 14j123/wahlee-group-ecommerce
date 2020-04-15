using Grand.Core.Caching;
using Grand.Core.Domain.CouponsModule;
using Grand.Core.Events;
using Grand.Core.Infrastructure;
using Grand.Services.Events;

namespace Grand.Services.CouponsModule.Cache
{
    /// <summary>
    /// Cache event consumer (used for caching of coupon requirements)
    /// </summary>
    public partial class CouponRequirementEventConsumer :
        //coupons
        IConsumer<EntityUpdated<Coupon>>,
        IConsumer<EntityDeleted<Coupon>>

    {
        /// <summary>
        /// Key for coupon requirement of a certain coupon
        /// </summary>
        /// <remarks>
        /// {0} : coupon id
        /// </remarks>
        public const string COUPON_REQUIREMENT_MODEL_KEY = "Grand.couponrequirements.all-{0}";
        public const string COUPON_REQUIREMENT_PATTERN_KEY = "Grand.couponrequirements";

        private readonly ICacheManager _cacheManager;

        public CouponRequirementEventConsumer()
        {
            this._cacheManager = EngineContext.Current.Resolve<ICacheManager>();
        }

        //coupons
        public void HandleEvent(EntityUpdated<Coupon> eventMessage)
        {
            _cacheManager.RemoveByPattern(COUPON_REQUIREMENT_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeleted<Coupon> eventMessage)
        {
            _cacheManager.RemoveByPattern(COUPON_REQUIREMENT_PATTERN_KEY);
        }

    }
}
