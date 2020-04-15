namespace Grand.Core.Domain.CouponsModule 
{
    public class CouponTicket : BaseEntity
    {
        /// <summary>
        /// Gets or sets the coupon code
        /// </summary>
        public string CouponCode { get; set; }

        /// <summary>
        /// Gets or sets the coupon id
        /// </summary>
        public string CouponId { get; set; }

        /// <summary>
        /// Gets or sets coupon used
        /// </summary>
        public bool Used { get; set; }

        /// <summary>
        /// How many times was used
        /// </summary>
        public int Qty { get; set; }
    }
}
