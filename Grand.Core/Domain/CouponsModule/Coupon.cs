using System;
using System.Collections.Generic;

namespace Grand.Core.Domain.CouponsModule 
{
    /// <summary>
    /// Represents a coupon
    /// </summary>
    public partial class Coupon : BaseEntity
    {
        private ICollection<CouponRequirement> _couponRequirements;

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the coupon type identifier
        /// </summary>
        public int CouponTypeId { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to use percentage
        /// </summary>
        public bool UsePercentage { get; set; }

        /// <summary>
        /// Gets or sets the coupon percentage
        /// </summary>
        public decimal CouponPercentage { get; set; }

        /// <summary>
        /// Gets or sets the coupon amount
        /// </summary>
        public decimal CouponAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to calculate by plugin
        /// </summary>
        public bool CalculateByPlugin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to calculate by plugin
        /// </summary>
        public string CouponPluginName { get; set; }

        /// <summary>
        /// Gets or sets the maximum coupon amount (used with "UsePercentage")
        /// </summary>
        public decimal? MaximumCouponAmount { get; set; }

        /// <summary>
        /// Gets or sets the coupon start date and time
        /// </summary>
        public DateTime? StartDateUtc { get; set; }

        /// <summary>
        /// Gets or sets the coupon end date and time
        /// </summary>
        public DateTime? EndDateUtc { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether coupon requires coupon code
        /// </summary>
        public bool RequiresCouponCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether coupon code can be reused
        /// </summary>
        public bool Reused { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether coupon can be used simultaneously with other coupons (with the same coupon type)
        /// </summary>
        public bool IsCumulative { get; set; }

        /// <summary>
        /// Gets or sets the coupon limitation identifier
        /// </summary>
        public int CouponLimitationId { get; set; }

        /// <summary>
        /// Gets or sets the coupon limitation times (used when Limitation is set to "N Times Only" or "N Times Per Customer")
        /// </summary>
        public int LimitationTimes { get; set; }

        /// <summary>
        /// Gets or sets the maximum product quantity which could be couponed
        /// Used with "Assigned to products" or "Assigned to categories" type
        /// </summary>
        public int? MaximumCouponedQuantity { get; set; }

        /// <summary>
        /// Gets or sets the coupon type Purpose identifier
        /// </summary>
        public int CouponTypePurpose { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether coupon is enabled
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets the coupon type
        /// </summary>
        public CouponType CouponType
        {
            get
            {
                return (CouponType)this.CouponTypeId;
            }
            set
            {
                this.CouponTypeId = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the coupon limitation
        /// </summary>
        public CouponLimitationType CouponLimitation
        {
            get
            {
                return (CouponLimitationType)this.CouponLimitationId;
            }
            set
            {
                this.CouponLimitationId = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the coupon requirement
        /// </summary>
        public virtual ICollection<CouponRequirement> CouponRequirements
        {
            get { return _couponRequirements ?? (_couponRequirements = new List<CouponRequirement>()); }
            protected set { _couponRequirements = value; }
        }

    }
}
