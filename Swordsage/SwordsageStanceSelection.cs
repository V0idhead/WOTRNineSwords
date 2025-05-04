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
          PunishingStance.Guid,
          BolsteringVoice.Guid,
          HuntersSense.Guid,
          AbsoluteSteel.Guid,
          DancingBladeForm.Guid,
          GiantsStance.Guid,
          HearingTheAir.Guid,
          StrengthOfStone.Guid,
          SupremeBladeParry.Guid,
          SwarmTactics.Guid
        ).Configure();

      return swordsageManeuverSelection;
    }
  }
}