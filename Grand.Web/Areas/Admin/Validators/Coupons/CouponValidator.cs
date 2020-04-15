using FluentValidation;
using Grand.Framework.Validators;
using Grand.Services.Localization;
using Grand.Web.Areas.Admin.Models.CouponsModule;

namespace Grand.Web.Areas.Admin.Validators.Coupons
{
    public class CouponValidator : BaseGrandValidator<CouponModel>
    {
        public CouponValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Promotions.Discounts.Fields.Name.Required"));
            RuleFor(x => x).Must((x, context) =>
            {
                if (x.CalculateByPlugin && string.IsNullOrEmpty(x.CouponPluginName))
                {
                    return false;
                }
                return true;
            }).WithMessage(localizationService.GetResource("Admin.Promotions.Discounts.Fields.DiscountPluginName.Required"));
        }
    }
}