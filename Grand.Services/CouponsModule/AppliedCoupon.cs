namespace Grand.Services.CouponsModule
{
    /// <summary>
    /// Applied coupon
    /// </summary>
    public class AppliedCoupon
    {
        /// <summary>
        /// Gets or sets the coupon Id
        /// </summary>
        public string CouponId { get; set; }

        /// <summary>
        /// Gets or sets the coupon 
        /// </summary>
        public string CouponCode { get; set; }


        /// <summary>
        /// Gets or sets is coupon is cumulative
        /// </summary>
        public bool IsCumulative { get; set; }
    }
}
