using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.IronHeart;
using VoidHeadWOTRNineSwords.StoneDragon;

namespace VoidHeadWOTRNineSwords.Warblade.Archetypes
{
    static class DeepstoneSentinelManeuverSelection
    {
        public const string Guid = "{C6CC8DED-7609-427F-AC01-05689FD6DE6C}";

        public static BlueprintFeatureSelection Configure()
        {
            BlueprintFeatureSelection deepstoneSentinelManeuverSelection = FeatureSelectionConfigurator.New("DeepstoneSentinelManeuverSelection", Guid)
              .SetDisplayName("DeepstoneSentinelManeuverSelection.Name")
              .SetDescription("DeepstoneSentinelManeuverSelection.Desc")
              .SetIsClassFeature()
              .SetMode(SelectionMode.OnlyNew)
              .AddFacts([ManeuverResources.ManeuverResourceFactGuid])
              .SetAllFeatures(
                StoneBones.Guid,
                ChargingMinotaur.Guid,
                SteelyStrike.Guid,
                DisarmingStrike.Guid,
                MountainHammer.Guid,
                StoneVise.Guid,
                ExorcismOfSteel.Guid,
                BonesplittingStrike.Guid,
                MithralTornado.Guid,
                OverwhelmingMountainStrike.Guid,
                DazingStrike.Guid,
                ElderMountainHammer.Guid,
                CrushingVise.Guid,
                IronBones.Guid,
                IrresistibleMountainStrike.Guid,
                AncientMountainHammer.Guid,
                ColossusStrike.Guid,
                FinishingMove.Guid,
                ScythingBlade.Guid,
                AdamantineBones.Guid,
                SteelWind.Guid,
                AdamantineHurricane.Guid,
                EarthstrikeQuake.Guid,
                BoomerangThrow.Guid,
                ViciousThrow.Guid,
                LightningThrow.Guid,
                MountainTombstoneStrike.Guid,
                StrikeOfPerfectClarity.Guid,
                WallOfBlades.Guid,
                LightningRecovery.Guid,
                IronHeartFocus.Guid,
                ManticoreParry.Guid
              ).Configure();

            return deepstoneSentinelManeuverSelection;
        }
    }
}