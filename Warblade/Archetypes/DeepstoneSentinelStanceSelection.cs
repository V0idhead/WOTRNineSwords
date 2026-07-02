using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.DiamondMind;
using VoidHeadWOTRNineSwords.IronHeart;
using VoidHeadWOTRNineSwords.StoneDragon;
using VoidHeadWOTRNineSwords.TigerClaw;
using VoidHeadWOTRNineSwords.WhiteRaven;

namespace VoidHeadWOTRNineSwords.Warblade.Archetypes
{
    static class DeepstoneSentinelStanceSelection
    {
        public const string Guid = "44CE2846-D82C-45B3-81E5-5168103733EA";

        public static BlueprintFeatureSelection Configure()
        {
            BlueprintFeatureSelection warbladeManeuverSelection = FeatureSelectionConfigurator.New("DeepstoneSentinelStanceSelection", Guid)
              .SetDisplayName("DeepstoneSentinelStanceSelection.Name")
              .SetDescription("DeepstoneSentinelStanceSelection.Desc")
              .SetIsClassFeature()
              .SetMode(SelectionMode.OnlyNew)
              .SetAllFeatures(
                PunishingStance.Guid,
                AbsoluteSteel.Guid,
                DancingBladeForm.Guid,
                GiantsStance.Guid,
                StrengthOfStone.Guid,
                SupremeBladeParry.Guid,
                StoneshellStance.Guid
              ).Configure();

            return warbladeManeuverSelection;
        }
    }
}