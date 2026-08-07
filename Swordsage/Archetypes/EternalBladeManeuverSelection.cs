using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.DiamondMind;
using VoidHeadWOTRNineSwords.RivenHourglass;

namespace VoidHeadWOTRNineSwords.Swordsage.Archetypes
{
    static class EternalBladeManeuverSelection
    {
        public const string Guid = "7A07686C-734F-4A55-BA9A-B16E908BF781";

        public static BlueprintFeatureSelection Configure()
        {
            BlueprintFeatureSelection maneuverSelection = FeatureSelectionConfigurator.New("EternalBladeManeuverSelection", Guid)
              .SetDisplayName("EternalBladeManeuverSelection.Name")
              .SetDescription("EternalBladeManeuverSelection.Desc")
              .SetIsClassFeature()
              .SetMode(SelectionMode.OnlyNew)
              .AddFacts([ManeuverResources.ManeuverResourceFactGuid])
              .SetAllFeatures(
                  SapphireNightmareBlade.Guid,
                  EmeraldRazor.Guid,
                  BoundingAssault.Guid,
                  MindStrike.Guid,
                  RubyNightmareBlade.Guid,
                  DisruptingBlow.Guid,
                  AvalancheOfBlades.Guid,
                  DiamondNightmareBlade.Guid,
                  TimeStandsStill.Guid,
                  RapidCounter.Guid,
                  DiamondDefense.Guid,
                  MinuteHand.Guid,
                  TiringTouch.Guid,
                  StrikeTheHourglass.Guid,
                  ChronalAgression.Guid,
                  TemporalBurn.Guid,
                  UnhinderedStep.Guid,
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
                  BuzzingStrike.Guid,
                  PainEcho.Guid,
                  DiamondFocus.Guid
              ).Configure();

            return maneuverSelection;
        }
    }
}