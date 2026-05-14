using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using VoidHeadWOTRNineSwords.DesertWind;
using VoidHeadWOTRNineSwords.DiamondMind;
using VoidHeadWOTRNineSwords.RivenHourglass;
using VoidHeadWOTRNineSwords.ShadowHand;
using VoidHeadWOTRNineSwords.StoneDragon;
using VoidHeadWOTRNineSwords.TigerClaw;

namespace VoidHeadWOTRNineSwords.Swordsage
{
  static class SwordsageStanceSelection
  {
    public const string Guid = "28A95960-AF4C-488F-9735-86FF592F07BF";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static BlueprintFeatureSelection Configure()
    {
      BlueprintFeatureSelection swordsageManeuverSelection = FeatureSelectionConfigurator.New("SwordsageStanceSelection", Guid)
        .SetDisplayName("SwordsageStanceSelection.Name")
        .SetDescription("SwordsageStanceSelection.Desc")
        .SetIsClassFeature()
        .SetMode(SelectionMode.OnlyNew)
        .SetAllFeatures(
          HuntersSense.Guid,
          GiantsStance.Guid,
          HearingTheAir.Guid,
          StrengthOfStone.Guid,
          ChildOfShadow.Guid,
          FlamesBlessing.Guid,
          DeepShadowAura.Guid,
          SandsOfTime.Guid,
          EyeOfTheStorm.Guid,
          LocalEclipse.Guid,
          ThickShadows.Guid,
          Shadowsight.Guid
        ).Configure();

      return swordsageManeuverSelection;
    }
  }
}