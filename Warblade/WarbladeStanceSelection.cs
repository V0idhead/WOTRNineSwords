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

namespace VoidHeadWOTRNineSwords.Warblade
{
  static class WarbladeStanceSelection
  {
    public const string Guid = "D4E1FCD7-E813-4099-846F-06692E23D475";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static BlueprintFeatureSelection Configure()
    {
      BlueprintFeatureSelection warbladeManeuverSelection = FeatureSelectionConfigurator.New("WarbladeStanceSelection", Guid)
        .SetDisplayName("WarbladeStanceSelection.Name")
        .SetDescription("WarbladeStanceSelection.Desc")
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

      return warbladeManeuverSelection;
    }
  }
}