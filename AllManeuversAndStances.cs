using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
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

namespace VoidHeadWOTRNineSwords
{
  internal static class AllManeuversAndStances
  {
    public static readonly FeatureGroup featureGroup = (Kingmaker.Blueprints.Classes.FeatureGroup)100; //None == Background :(
    public static readonly IEnumerable<Blueprint<BlueprintFeatureReference>> DiamondMindGuids = [SapphireNightmareBlade.Guid, EmeraldRazor.Guid, BoundingAssault.Guid, MindStrike.Guid, RubyNightmareBlade.Guid, DisruptingBlow.Guid, AvalancheOfBlades.Guid, HearingTheAir.Guid];
    public static readonly IEnumerable<Blueprint<BlueprintFeatureReference>> IronHeartGuids = [SteelyStrike.Guid, DisarmingStrike.Guid, ExorcismOfSteel.Guid, PunishingStance.Guid, AbsoluteSteel.Guid, MithralTornado.Guid, DazingStrike.Guid, IronHeartEndurance.Guid, FinishingMove.Guid, DancingBladeForm.Guid, ScythingBlade.Guid];
    public static readonly IEnumerable<Blueprint<BlueprintFeatureReference>> StoneDragonGuids = [StoneBones.Guid, ChargingMinotaur.Guid, MountainHammer.Guid, StoneVise.Guid, BonesplittingStrike.Guid, OverwhelmingMountainStrike.Guid, ElderMountainHammer.Guid, CrushingVise.Guid, IronBones.Guid, IrresistibleMountainStrike.Guid, AncientMountainHammer.Guid, ColossusStrike.Guid, GiantsStance.Guid, AdamantineBones.Guid];
    public static readonly IEnumerable<Blueprint<BlueprintFeatureReference>> TigerClawGuids = [HuntersSense.Guid, ClawAtTheMoon.Guid, RabidWolfStrike.Guid, FleshRipper.Guid, DancingMongoose.Guid, PouncingCharge.Guid, RabidBearStrike.Guid, HamstringAttack.Guid];
    public static readonly IEnumerable<Blueprint<BlueprintFeatureReference>> WhiteRavenGuids = [LeadingTheAttack.Guid, BattleLeadersCharge.Guid, LionsRoar.Guid, WhiteRavenStrike.Guid, WarLeadersCharge.Guid, BolsteringVoice.Guid];

    public static void Configure()
    {
      #region Diamond Mind
      //--Maneuvers--
      //Lvl1
      SapphireNightmareBlade.Configure();
      //Lvl2
      EmeraldRazor.Configure();
      //Lvl4
      BoundingAssault.Configure();
      MindStrike.Configure();
      RubyNightmareBlade.Configure();
      //Lvl5
      DisruptingBlow.Configure();
      //Lvl7
      AvalancheOfBlades.Configure();
      //--Stances--
      HearingTheAir.Configure();
      #endregion
      #region Iron Heart
      //Maneuvers
      //Lvl1
      SteelyStrike.Configure();
      //Lvl2
      DisarmingStrike.Configure();
      //Lvl3
      ExorcismOfSteel.Configure();
      //Lvl4
      MithralTornado.Configure();
      //Lvl5
      DazingStrike.Configure();
      //Lvl6
      IronHeartEndurance.Configure();
      //Lvl7
      FinishingMove.Configure();
      ScythingBlade.Configure();
      //Stances
      //Lvl1
      PunishingStance.Configure();
      //Lvl3
      AbsoluteSteel.Configure();
      //Lvl5
      DancingBladeForm.Configure();
      #endregion
      #region Stone Dragon
      //--Maneuvers--
      //Lvl1
      StoneBones.Configure();
      ChargingMinotaur.Configure();
      //Lvl2
      MountainHammer.Configure();
      StoneVise.Configure();
      //Lvl4
      BonesplittingStrike.Configure();
      OverwhelmingMountainStrike.Configure();
      //Lvl5
      ElderMountainHammer.Configure();
      //Lvl6
      CrushingVise.Configure();
      IronBones.Configure();
      IrresistibleMountainStrike.Configure();
      //Lvl7
      AncientMountainHammer.Configure();
      ColossusStrike.Configure();
      //Lvl8
      AdamantineBones.Configure();
      //--Stances--
      GiantsStance.Configure();
      #endregion
      #region Tiger Claw
      //--Maneuvers--
      //Lvl2
      ClawAtTheMoon.Configure();
      RabidWolfStrike.Configure();
      //Lvl3
      FleshRipper.Configure();
      //Lvl5
      DancingMongoose.Configure();
      PouncingCharge.Configure();
      //Lvl6
      RabidBearStrike.Configure();
      //Lvl7
      HamstringAttack.Configure();
      //--Stances
      //Lvl1
      HuntersSense.Configure();
      #endregion
      #region White Raven
      //--Maneuvers--
      //Lvl1
      LeadingTheAttack.Configure();
      //Lvl2
      BattleLeadersCharge.Configure();
      //Lvl3
      LionsRoar.Configure();
      //Lvl4
      WhiteRavenStrike.Configure();
      //Lvl6
      WarLeadersCharge.Configure();
      //--Stances--
      //Lvl1
      BolsteringVoice.Configure();
      #endregion
    }
  }
}