using Grand.Framework.Components;
using Grand.Services.Rewards;
using Grand.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Grand.Web.Components
{
    public class RewardViewComponent : BaseViewComponent
    {
        #region Fields
        private readonly IRewardService _rewardService;
        #endregion

        #region Constructors

        public RewardViewComponent(
            IRewardService rewardService)
        {
            this._rewardService = rewardService;
        }

        #endregion

        #region Invoker

        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View();
        }

        #endregion
    }
}
