using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.DesertWind;
using VoidHeadWOTRNineSwords.DiamondMind;
using VoidHeadWOTRNineSwords.RivenHourglass;
using VoidHeadWOTRNineSwords.ShadowHand;
using VoidHeadWOTRNineSwords.StoneDragon;
using VoidHeadWOTRNineSwords.TigerClaw;

namespace VoidHeadWOTRNineSwords.Swordsage.Archetypes
{
    static class EternalBladeStanceSelection
    {
        public const string Guid = "3FD7D138-02F8-4C07-82A8-63F975B0DD45";

        public static BlueprintFeatureSelection Configure()
        {
            BlueprintFeatureSelection stanceSelection = FeatureSelectionConfigurator.New("EternalBladeStanceSelection", Guid)
              .SetDisplayName("EternalBladeStanceSelection.Name")
              .SetDescription("EternalBladeStanceSelection.Desc")
              .SetIsClassFeature()
              .SetMode(SelectionMode.OnlyNew)
              .SetAllFeatures(
                HearingTheAir.Guid,
                SandsOfTime.Guid,
                HourglassStance.Guid,
                SandBearersSwiftness.Guid,
                DiamondShimmerStance.Guid,
                DiamondLockStance.Guid,
                ElementalShell.Guid
              ).Configure();

            return stanceSelection;
        }
    }
}