namespace Grand.Core.Domain.CouponsModule
{
    /// <summary>
    /// Represents a coupon requirement
    /// </summary>
    public partial class CouponRequirement : SubBaseEntity
    {
        /// <summary>
        /// Gets or sets the coupon identifier
        /// </summary>
        public string CouponId { get; set; }

        /// <summary>
        /// Gets or sets the coupon requirement rule system name
        /// </summary>
        public string CouponRequirementRuleSystemName { get; set; }
        
    }
}
