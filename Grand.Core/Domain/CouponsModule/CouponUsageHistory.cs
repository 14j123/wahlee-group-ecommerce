using System;

namespace Grand.Core.Domain.CouponsModule
{
    /// <summary>
    /// Represents a coupon usage history entry
    /// </summary>
    public partial class CouponUsageHistory : BaseEntity
    {
        /// <summary>
        /// Gets or sets the coupon identifier
        /// </summary>
        public string CouponId { get; set; }

        /// <summary>
        /// Gets or sets the coupon code
        /// </summary>
        public string CouponCode { get; set; }

        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the canceled identifier
        /// </summary>
        public bool Canceled { get; set; }

    }
}
