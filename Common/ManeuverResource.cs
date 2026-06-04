using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Common
{
  static class ManeuverResources
  {
    public const string ManeuverResourceGuid = "06B1EFA3-CF54-47EA-9BA8-D788F239FE1C";
    public const string ManeuverResourceFactGuid = "B77E4216-1AC6-4E58-92E7-0550C8ACA4EE";
    public static BlueprintAbilityResource ManeuverResource { get; private set; }

    public const string IncreaseManeuverUsesGuid = "24E794CE-D160-4FD0-BB2A-1BBE03097288";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static void Configure()
    {
      log.Info($"{nameof(WarbladeC)} configuring resource");
      ManeuverResource = AbilityResourceConfigurator.New("WarbladeManeuvers.Resource", ManeuverResourceGuid)
      //.SetMaxAmount(new BlueprintAbilityResource.Amount { BaseValue=3, IncreasedByLevel=true, StartingLevel=0, LevelStep=4, PerStepIncrease=1 })
      //.SetMaxAmount(ResourceAmountBuilder.New(3).IncreaseByLevelStartPlusDivStep(classes: new string[] { Guid }, startingLevel: 1, levelsPerStep: 4, bonusPerStep: 1))
      //.SetUseMax()
      //.AddPlayerLeaveCombatTrigger(ActionsBuilder.New().RestoreResource(ManeuverResourceGuid, 3)) //doesn't seem to do anything, which is a shame because now I need an end combat trigger on every maneuver
      .SetMaxAmount(new BlueprintAbilityResource.Amount { BaseValue=3})
      .SetMin(0)
      .Configure();

      var manResFact = UnitFactConfigurator.New("WarbladeManeuvers.Resource.Fact", ManeuverResourceFactGuid)
        .AddAbilityResources(resource: ManeuverResource, restoreAmount: true)
        .Configure();

      FeatureConfigurator.New("IncreaseManeuverUses", IncreaseManeuverUsesGuid)
        .SetHideInUI(true)
        .AddIncreaseResourceAmount(ManeuverResource, 1)
        .SetIsClassFeature(true)
        .SetRanks(20)
        .Configure();
    }
  }
}