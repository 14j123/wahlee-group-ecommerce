namespace Grand.Services.CouponsModule
{
    /// <summary>
    /// Represents a coupon requirement rule
    /// </summary>
    public partial interface ICouponRequirementRule
    {
        /// <summary>
        /// Check coupon requirement
        /// </summary>
        /// <param name="request">Object that contains all information required to check the requirement (Current customer, coupon, etc)</param>
        /// <returns>Result</returns>
        CouponRequirementValidationResult CheckRequirement(CouponRequirementValidationRequest request);

        /// <summary>
        /// Get URL for rule configuration
        /// </summary>
        /// <param name="couponId">Coupon identifier</param>
        /// <param name="couponRequirementId">Coupon requirement identifier (if editing)</param>
        /// <returns>URL</returns>
        string GetConfigurationUrl(string couponId, string couponRequirementId);

        /// <summary>
        /// Gets a system name of type
        /// </summary>
        /// <returns>SystemName</returns>
        string SystemName { get; }

        /// <summary>
        /// Gets a friendly name of type
        /// </summary>
        /// <returns>FriendlyName</returns>
        string FriendlyName { get; }
    }
}
