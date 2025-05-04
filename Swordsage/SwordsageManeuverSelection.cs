using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.DiamondMind;
using VoidHeadWOTRNineSwords.IronHeart;
using VoidHeadWOTRNineSwords.StoneDragon;
using VoidHeadWOTRNineSwords.TigerClaw;
using VoidHeadWOTRNineSwords.WhiteRaven;

namespace VoidHeadWOTRNineSwords.Swordsage
{
  static class SwordsageManeuverSelection
  {
    public const string Guid = "C199F292-D2CB-4993-9859-9B7EEDC07104";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static BlueprintFeatureSelection Configure()
    {
      BlueprintFeatureSelection swordsageManeuverSelection = FeatureSelectionConfigurator.New("SwordsageManeuverSelection", Guid)
        .SetDisplayName("SwordsageManeuverSelection.Name")
        .SetDescription("SwordsageManeuverSelection.Desc")
        .SetIsClassFeature()
        .SetMode(SelectionMode.OnlyNew)
        .AddFacts([ManeuverResources.ManeuverResourceFactGuid])
        .SetAllFeatures(
          StoneBones.Guid,
          ChargingMinotaur.Guid,
          SteelyStrike.Guid,
          SapphireNightmareBlade.Guid,
          LeadingTheAttack.Guid,
          BattleLeadersCharge.Guid,
          ClawAtTheMoon.Guid,
          DisarmingStrike.Guid,
          EmeraldRazor.Guid,
          MountainHammer.Guid,
          RabidWolfStrike.Guid,
          StoneVise.Guid,
          ExorcismOfSteel.Guid,
          FleshRipper.Guid,
          LionsRoar.Guid,
          BonesplittingStrike.Guid,
          BoundingAssault.Guid,
          MindStrike.Guid,
          MithralTornado.Guid,
          OverwhelmingMountainStrike.Guid,
          RubyNightmareBlade.Guid,
          WhiteRavenStrike.Guid,
          //DancingMongoose.Guid,
          DazingStrike.Guid,
          DisruptingBlow.Guid,
          ElderMountainHammer.Guid,
          PouncingCharge.Guid,
          CrushingVise.Guid,
          IronBones.Guid,
          //IronHeartEndurance.Guid,
          IrresistibleMountainStrike.Guid,
          RabidBearStrike.Guid,
          WarLeadersCharge.Guid,
          AncientMountainHammer.Guid,
          AvalancheOfBlades.Guid,
          ColossusStrike.Guid,
          FinishingMove.Guid,
          HamstringAttack.Guid,
          ScythingBlade.Guid,
          AdamantineBones.Guid,
          SteelWind.Guid,
          AdamantineHurricane.Guid,
          DiamondNightmareBlade.Guid,
          EarthstrikeQuake.Guid,
          BoomerangThrow.Guid,
          ViciousThrow.Guid,
          LightningThrow.Guid,
          WhiteRavenHammer.Guid,
          FeralDeathBlow.Guid,
          MountainTombstoneStrike.Guid,
          StrikeOfPerfectClarity.Guid,
          TimeStandsStill.Guid,
          WallOfBlades.Guid,
          LightningRecovery.Guid,
          IronHeartFocus.Guid,
          RapidCounter.Guid,
          ManticoreParry.Guid,
          DiamondDefense.Guid,
          WhiteRavenCall.Guid
        ).Configure();

      return swordsageManeuverSelection;
    }
  }
}