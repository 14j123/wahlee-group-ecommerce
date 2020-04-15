using Grand.Core.Plugins;
using System.Collections.Generic;

namespace Grand.Services.CouponsModule
{
    /// <summary>
    /// Represents a coupon requirement rule
    /// </summary>
    public partial interface ICoupon : IPlugin
    {
        IList<ICouponRequirementRule> GetRequirementRules();
    }
}
