using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System.Collections.Generic;
using VoidHeadWOTRNineSwords.DesertWind;
using VoidHeadWOTRNineSwords.DiamondMind;
using VoidHeadWOTRNineSwords.IronHeart;
using VoidHeadWOTRNineSwords.RivenHourglass;
using VoidHeadWOTRNineSwords.ShadowHand;
using VoidHeadWOTRNineSwords.StoneDragon;
using VoidHeadWOTRNineSwords.TigerClaw;
using VoidHeadWOTRNineSwords.WhiteRaven;

namespace VoidHeadWOTRNineSwords
{
    internal static class AllManeuversAndStances
    {
        public static readonly FeatureGroup featureGroup = (Kingmaker.Blueprints.Classes.FeatureGroup)100; //None == Background :(
        public static readonly IEnumerable<Blueprint<BlueprintFeatureReference>> DiamondMindGuids = [SapphireNightmareBlade.Guid, EmeraldRazor.Guid, BoundingAssault.Guid, MindStrike.Guid, RubyNightmareBlade.Guid, DisruptingBlow.Guid, AvalancheOfBlades.Guid, HearingTheAir.Guid, DiamondNightmareBlade.Guid, TimeStandsStill.Guid, RapidCounter.Guid, DiamondDefense.Guid];
        public static readonly IEnumerable<Blueprint<BlueprintFeatureReference>> DesertWindGuids = [BlisteringFlourish.Guid, BurningBlade.Guid, FlamesBlessing.Guid, FireRiposte.Guid, WindStride.Guid, FlashingSun.Guid, HatchlingsFlame.Guid, DeathMark.Guid, FanTheFlames.Guid, /*ZephyrDance.Guid,*/ SearingBlade.Guid, SearingCharge.Guid, DragonsFlame.Guid, LeapingFlame.Guid, LingeringInferno.Guid, InfernoBlade.Guid, WyrmsFlame.Guid, InfernoBlast.Guid, EyeOfTheStorm.Guid];
        public static readonly IEnumerable<Blueprint<BlueprintFeatureReference>> IronHeartGuids = [SteelyStrike.Guid, DisarmingStrike.Guid, ExorcismOfSteel.Guid, PunishingStance.Guid, AbsoluteSteel.Guid, MithralTornado.Guid, DazingStrike.Guid, FinishingMove.Guid, DancingBladeForm.Guid, ScythingBlade.Guid, SteelWind.Guid, AdamantineHurricane.Guid, BoomerangThrow.Guid, ViciousThrow.Guid, LightningThrow.Guid, SupremeBladeParry.Guid, StrikeOfPerfectClarity.Guid, WallOfBlades.Guid, LightningRecovery.Guid, IronHeartFocus.Guid, ManticoreParry.Guid];
        public static readonly IEnumerable<Blueprint<BlueprintFeatureReference>> ShadowHandGuids = [ChildOfShadow.Guid, ClingingShadowStrike.Guid, ShadowBladeTechnique.Guid, CloakOfDeception.Guid, DrainVitality.Guid, ShadowJaunt.Guid, ShadowGarotte.Guid, StrengthDrainingStrike.Guid, ObscuringShadowVeil.Guid, BloodlettingStrike.Guid, ShadowStride.Guid, ShadowBlink.Guid, EnervatingShadowStrike.Guid, CreepingIceStrike.Guid, DeepShadowAura.Guid, LocalEclipse.Guid, ThickShadows.Guid, Shadowsight.Guid];
        public static readonly IEnumerable<Blueprint<BlueprintFeatureReference>> StoneDragonGuids = [StoneBones.Guid, ChargingMinotaur.Guid, MountainHammer.Guid, StoneVise.Guid, BonesplittingStrike.Guid, OverwhelmingMountainStrike.Guid, ElderMountainHammer.Guid, CrushingVise.Guid, IronBones.Guid, IrresistibleMountainStrike.Guid, AncientMountainHammer.Guid, ColossusStrike.Guid, GiantsStance.Guid, AdamantineBones.Guid, EarthstrikeQuake.Guid, StrengthOfStone.Guid, MountainTombstoneStrike.Guid];
        public static readonly IEnumerable<Blueprint<BlueprintFeatureReference>> TigerClawGuids = [HuntersSense.Guid, ClawAtTheMoon.Guid, RabidWolfStrike.Guid, FleshRipper.Guid, PouncingCharge.Guid, RabidBearStrike.Guid, HamstringAttack.Guid, FeralDeathBlow.Guid, TigerSnap.Guid, TigerRake.Guid, TigerMaul.Guid];
        public static readonly IEnumerable<Blueprint<BlueprintFeatureReference>> WhiteRavenGuids = [LeadingTheAttack.Guid, BattleLeadersCharge.Guid, LionsRoar.Guid, WhiteRavenStrike.Guid, WarLeadersCharge.Guid, BolsteringVoice.Guid, SwarmTactics.Guid, WhiteRavenHammer.Guid, WhiteRavenCall.Guid];
        public static readonly IEnumerable<Blueprint<BlueprintFeatureReference>> RivenHourglassGuids = [MinuteHand.Guid, SandsOfTime.Guid, TiringTouch.Guid, StrikeTheHourglass.Guid, ChronalAgression.Guid, TemporalBurn.Guid, UnhinderedStep.Guid, HourglassStance.Guid, TemporalFury.Guid, ChronalDraw.Guid, TipTheHourglass.Guid, HourHand.Guid, SandsOfTimeTornado.Guid, SandBearersSwiftness.Guid, ShatterTheHourglass.Guid, TemporalWave.Guid, SandsOfTimeHurricane.Guid, WrathOfTime.Guid, BreakTheHourglass.Guid];

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
            //Lvl8
            DiamondNightmareBlade.Configure();
            //Lvl9
            TimeStandsStill.Configure();
            //--Stances--
            HearingTheAir.Configure();
            //--Counters--
            //Lvl5
            RapidCounter.Configure();
            //--Other--
            DiamondDefense.Configure();
            #endregion
            #region Desert Wind
            //--Maneuvers--
            //Lvl1
            BlisteringFlourish.Configure();
            BurningBlade.Configure();
            WindStride.Configure();
            //Lvl2
            FireRiposte.Configure();
            FlashingSun.Configure();
            HatchlingsFlame.Configure();
            //Lvl3
            DeathMark.Configure();
            FanTheFlames.Configure();
            //Lvl4
            SearingBlade.Configure();
            SearingCharge.Configure();
            //Lvl5
            DragonsFlame.Configure();
            LingeringInferno.Configure();
            //Lvl7
            InfernoBlade.Configure();
            //Lvl8
            WyrmsFlame.Configure();
            //Lvl9
            InfernoBlast.Configure();
            //--Stances--
            //Lvl1
            FlamesBlessing.Configure();
            //Lvl5
            EyeOfTheStorm.Configure();
            //--Counters--
            //Lvl3
            //ZephyrDance.Configure();
            //Lvl5
            LeapingFlame.Configure();
            #endregion
            #region Iron Heart
            //Maneuvers
            //Lvl1
            SteelyStrike.Configure();
            SteelWind.Configure();
            BoomerangThrow.Configure();
            //Lvl2
            DisarmingStrike.Configure();
            //Lvl3
            ExorcismOfSteel.Configure();
            ViciousThrow.Configure();
            //Lvl4
            MithralTornado.Configure();
            //Lvl5
            DazingStrike.Configure();
            //Lvl6
            //IronHeartEndurance.Configure();
            //Lvl7
            FinishingMove.Configure();
            ScythingBlade.Configure();
            //Lvl8
            AdamantineHurricane.Configure();
            LightningThrow.Configure();
            //Lvl9
            StrikeOfPerfectClarity.Configure();
            //Stances
            //Lvl1
            PunishingStance.Configure();
            //Lvl3
            AbsoluteSteel.Configure();
            //Lvl5
            DancingBladeForm.Configure();
            //Lvl8
            SupremeBladeParry.Configure();
            //Counters
            //Lvl2
            WallOfBlades.Configure();
            //Lvl4
            LightningRecovery.Configure();
            //Lvl5
            IronHeartFocus.Configure();
            //Lvl6
            ManticoreParry.Configure();
            #endregion
            #region Riven Hourglass
            //--Maneuvers--
            //Lvl1
            MinuteHand.Configure();
            TiringTouch.Configure();
            StrikeTheHourglass.Configure();
            //Lvl2
            ChronalAgression.Configure();
            TemporalBurn.Configure();
            UnhinderedStep.Configure();
            //Lvl3
            TemporalFury.Configure();
            //Lvl4
            ChronalDraw.Configure();
            TipTheHourglass.Configure();
            //Lvl5
            HourHand.Configure();
            SandsOfTimeTornado.Configure();
            //Lvl6
            ShatterTheHourglass.Configure();
            TemporalWave.Configure();
            //Lvl7
            SandsOfTimeHurricane.Configure();
            //Lvl8
            WrathOfTime.Configure();
            //Lvl9
            BreakTheHourglass.Configure();
            //--Stances--
            //Lvl1
            SandsOfTime.Configure();
            //Lvl3
            HourglassStance.Configure();
            //Lvl6
            SandBearersSwiftness.Configure();
            #endregion
            #region Shadow Hand
            //--Maneuvers--
            //Lvl1
            ClingingShadowStrike.Configure();
            ShadowBladeTechnique.Configure();
            //Lvl2
            CloakOfDeception.Configure();
            DrainVitality.Configure();
            ShadowJaunt.Configure();
            //Lvl3
            ShadowGarotte.Configure();
            StrengthDrainingStrike.Configure();
            //Lvl4
            ObscuringShadowVeil.Configure();
            //Lvl5
            BloodlettingStrike.Configure();
            ShadowStride.Configure();
            //Lvl7
            ShadowBlink.Configure();
            //Lvl8
            EnervatingShadowStrike.Configure();
            //Lvl9
            CreepingIceStrike.Configure();
            //--Stances--
            //Lvl1
            ChildOfShadow.Configure();
            //Lvl2
            Shadowsight.Configure();
            //Lvl3
            ThickShadows.Configure();
            //Lvl5
            DeepShadowAura.Configure();
            //Lvl6
            LocalEclipse.Configure();
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
            EarthstrikeQuake.Configure();
            //Lvl9
            MountainTombstoneStrike.Configure();
            //--Stances--
            GiantsStance.Configure();
            //Lvl8
            StrengthOfStone.Configure();
            #endregion
            #region Tiger Claw
            //--Maneuvers--
            //Lvl2
            ClawAtTheMoon.Configure();
            RabidWolfStrike.Configure();
            //Lvl3
            FleshRipper.Configure();
            TigerSnap.Configure();
            //Lvl5
            //DancingMongoose.Configure();
            PouncingCharge.Configure();
            TigerRake.Configure();
            //Lvl6
            RabidBearStrike.Configure();
            //Lvl7
            HamstringAttack.Configure();
            TigerMaul.Configure();
            //Lvl9
            FeralDeathBlow.Configure();
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
            //Lvl8
            WhiteRavenHammer.Configure();
            //--Stances--
            //Lvl1
            BolsteringVoice.Configure();
            //Lvl8
            SwarmTactics.Configure();
            //Lvl9
            WhiteRavenCall.Configure();
            #endregion
        }
    }
}