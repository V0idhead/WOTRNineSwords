using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.DesertWind;
using VoidHeadWOTRNineSwords.DiamondMind;
using VoidHeadWOTRNineSwords.RivenHourglass;
using VoidHeadWOTRNineSwords.ShadowHand;
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
          SapphireNightmareBlade.Guid,
          ClawAtTheMoon.Guid,
          EmeraldRazor.Guid,
          MountainHammer.Guid,
          RabidWolfStrike.Guid,
          StoneVise.Guid,
          FleshRipper.Guid,
          BonesplittingStrike.Guid,
          BoundingAssault.Guid,
          MindStrike.Guid,
          OverwhelmingMountainStrike.Guid,
          RubyNightmareBlade.Guid,
          WhiteRavenStrike.Guid,
          //DancingMongoose.Guid,
          DisruptingBlow.Guid,
          ElderMountainHammer.Guid,
          PouncingCharge.Guid,
          CrushingVise.Guid,
          IronBones.Guid,
          IrresistibleMountainStrike.Guid,
          RabidBearStrike.Guid,
          AncientMountainHammer.Guid,
          AvalancheOfBlades.Guid,
          ColossusStrike.Guid,
          HamstringAttack.Guid,
          AdamantineBones.Guid,
          DiamondNightmareBlade.Guid,
          EarthstrikeQuake.Guid,
          FeralDeathBlow.Guid,
          MountainTombstoneStrike.Guid,
          TimeStandsStill.Guid,
          RapidCounter.Guid,
          DiamondDefense.Guid,
          BlisteringFlourish.Guid,
          BurningBlade.Guid,
          ClingingShadowStrike.Guid,
          ShadowBladeTechnique.Guid,
          CloakOfDeception.Guid,
          DrainVitality.Guid,
          FireRiposte.Guid,
          WindStride.Guid,
          FlashingSun.Guid,
          ShadowJaunt.Guid,
          HatchlingsFlame.Guid,
          DeathMark.Guid,
          FanTheFlames.Guid,
          ShadowGarotte.Guid,
          StrengthDrainingStrike.Guid,
          //ZephyrDance.Guid,
          ObscuringShadowVeil.Guid,
          SearingBlade.Guid,
          SearingCharge.Guid,
          BloodlettingStrike.Guid,
          DragonsFlame.Guid,
          LeapingFlame.Guid,
          LingeringInferno.Guid,
          ShadowStride.Guid,
          InfernoBlade.Guid,
          ShadowBlink.Guid,
          EnervatingShadowStrike.Guid,
          WyrmsFlame.Guid,
          InfernoBlast.Guid,
          CreepingIceStrike.Guid,
          MinuteHand.Guid,
          TiringTouch.Guid,
          StrikeTheHourglass.Guid,
          ChronalAgression.Guid,
          TemporalBurn.Guid,
          UnhinderedStep.Guid,
          TigerSnap.Guid,
          TigerRake.Guid,
          TigerMaul.Guid,
          TemporalFury.Guid,
          ChronalDraw.Guid,
          TipTheHourglass.Guid,
          HourHand.Guid,
          SandsOfTimeTornado.Guid,
          ShatterTheHourglass.Guid,
          TemporalWave.Guid,
          SandsOfTimeHurricane.Guid,
          WrathOfTime.Guid,
          BreakTheHourglass.Guid,
          Shadowcloak.Guid
        ).Configure();

      return swordsageManeuverSelection;
    }
  }
}