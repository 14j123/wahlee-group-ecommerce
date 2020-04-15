using Grand.Core.Domain.Customers;
using Grand.Core.Domain.Stores;

namespace Grand.Services.CouponsModule
{
    /// <summary>
    /// Represents a request of coupon requirement validation
    /// </summary>
    public partial class CouponRequirementValidationRequest
    {
        /// <summary>
        /// Gets or sets the appropriate coupon requirement ID (identifier)
        /// </summary>
        public string CouponRequirementId { get; set; }

        /// <summary>
        /// Gets or sets the coupon ID (identifier)
        /// </summary>
        public string CouponId { get; set; }

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the store
        /// </summary>
        public Store Store { get; set; }
    }
}
