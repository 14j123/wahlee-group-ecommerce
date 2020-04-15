namespace Grand.Core.Domain.CouponsModule 
{
    /// <summary>
    /// Represents a coupon limitation type
    /// </summary>
    public enum CouponLimitationType 
    {
        /// <summary>
        /// None
        /// </summary>
        Unlimited = 0,
        /// <summary>
        /// N Times Only
        /// </summary>
        NTimesOnly = 15,
        /// <summary>
        /// N Times Per Customer
        /// </summary>
        NTimesPerCustomer = 25,
    }
}
