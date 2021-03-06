﻿namespace Grand.Services.CouponsModule
{
    /// <summary>
    /// Represents a result of coupon validation
    /// </summary>
    public partial class CouponValidationResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether coupon is valid
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets an error that a customer should see when enterting a coupon code (in case if "IsValid" is set to "false")
        /// </summary>
        public string UserError { get; set; }

        /// <summary>
        /// Gets or sets a coupon code value
        /// </summary>
        public string CouponCode { get; set; }
    }
}
