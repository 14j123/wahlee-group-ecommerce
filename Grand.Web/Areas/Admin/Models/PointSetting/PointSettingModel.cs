using Grand.Framework.Mvc.ModelBinding;
using Grand.Framework.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Grand.Web.Areas.Admin.Models.PointSetting
{
    public partial class PointSettingModel : BaseGrandEntityModel
    {
        public PointSettingModel()
        {

        }
        [GrandResourceDisplayName("Admin.PointSetting.PointSetting.Fields.RuleName")]
        public string RuleName { get; set; }
        [GrandResourceDisplayName("Admin.PointSetting.PointSetting.Fields.RuleDescription")]
        public string RuleDescription { get; set; }
        [GrandResourceDisplayName("Admin.PointSetting.PointSetting.Fields.Times")]
        public double Times { get; set; }
        [GrandResourceDisplayName("Admin.PointSetting.PointSetting.Fields.Activate")]
        public bool Activate { get; set; }
        [GrandResourceDisplayName("Admin.PointSetting.PointSetting.Fields.Type")]
        public string Type { get; set; }
        [GrandResourceDisplayName("Admin.PointSetting.PointSetting.Fields.StartDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDate { get; set; }
        [GrandResourceDisplayName("Admin.PointSetting.PointSetting.Fields.EndDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDate { get; set; }
    }
}
