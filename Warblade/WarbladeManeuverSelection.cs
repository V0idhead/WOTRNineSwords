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
  static class WarbladeManeuverSelection
  {
    public const string Guid = "4D8E01B8-3EB1-46C1-8223-A9AE9322746E";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static BlueprintFeatureSelection Configure()
    {
      BlueprintFeatureSelection warbladeManeuverSelection = FeatureSelectionConfigurator.New("WarbladeManeuverSelection", Guid)
        .SetDisplayName("WarbladeManeuverSelection.Name")
        .SetDescription("WarbladeManeuverSelection.Desc")
        .SetIsClassFeature()
        .SetMode(SelectionMode.OnlyNew)
        .AddFacts([WarbladeC.ManeuverResourceFactGuid])
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
          WallOfBlades.Guid
        ).Configure();

      return warbladeManeuverSelection;
    }
  }
}