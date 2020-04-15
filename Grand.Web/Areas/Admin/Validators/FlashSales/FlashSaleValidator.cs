using FluentValidation;
using Grand.Framework.Validators;
using Grand.Services.Localization;

using Grand.Web.Areas.Admin.Models.FlashSales;

namespace Grand.Web.Areas.Admin.Validators.FlashSales 
{
    public class FlashSaleValidator : BaseGrandValidator<FlashSaleModel>
    {
        public FlashSaleValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Promotions.FlashSales.Fields.Name.Required"));
            RuleFor(x => x.StartDateUtc).NotEmpty().WithMessage(localizationService.GetResource("Admin.Promotions.FlashSales.Fields.StartTime.Required"));
            RuleFor(x => x.EndDateUtc).NotEmpty().WithMessage(localizationService.GetResource("Admin.Promotions.FlashSales.Fields.EndTime.Required"));
        }
    }
}