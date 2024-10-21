using Abp.Application.Features;
using Abp.Localization;
using Abp.Runtime.Validation;
using Abp.UI.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanHangBeautify.Features
{
    public class AppFeatureProvider : FeatureProvider
    {
        public override void SetFeatures(IFeatureDefinitionContext context)
        {
            context.Create(
                AppFeatureConst.MaxUserCount,
                defaultValue: "3", //0 = unlimited
                displayName: L("MaximumUserCount"),
                description: L("MaximumUserCount_Description"),
                inputType: new SingleLineStringInputType(new NumericValueValidator(0, int.MaxValue))
            );
            context.Create(
                AppFeatureConst.MaxBranchCount,
                defaultValue: "1", //0 = unlimited
                displayName: L("MaximumBranchCount"),
                description: L("MaximumBranchCount_Description"),
                inputType: new SingleLineStringInputType(new NumericValueValidator(0, int.MaxValue))
            );
            context.Create(
                AppFeatureConst.GoiDichVu,
                defaultValue: "false",
                displayName: L(AppFeatureConst.GoiDichVu),
                inputType: new CheckboxInputType()
            ); 
            context.Create(
                AppFeatureConst.TheGiaTri,
                defaultValue: "false", //0 = unlimited
                displayName: L(AppFeatureConst.TheGiaTri),
                inputType: new CheckboxInputType()
            );
            //context.Create(
            //    "App.ConnectZalo",
            //    defaultValue: "false", //0 = unlimited
            //    displayName: L("Kết nối với Zalo OA"),
            //    description: L("Kết nối với Zalo OA"),
            //    inputType: new CheckboxInputType()
            //);
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, SPAConsts.LocalizationSourceName);
        }
    }
}
