using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.ActivatableAbilities;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Counters;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.DiamondMind
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/rapid-counter--3635/
  static class RapidCounter
  {
    public const string Guid = "6349EBB5-1501-40A6-A030-A400EF363820";
    public const string ActiveBuffGuid = "9937751A-C8F2-4BCF-A0EE-28BC9EE103C8";
    const string FactGuid = "3377B36C-AE53-4C06-B2F9-3454847C86C4";
    public static BlueprintUnitFact Fact { get; private set; }
    public static BlueprintActivatableAbility Activatable { get; private set; }
    const string name = "RapidCounter.Name";
    const string desc = "RapidCounter.Desc";
    const string icon = Helpers.IconPrefix + "rapidcounter.png";

    public static void Configure()
    {
      Main.Log($"Configuring {nameof(RapidCounter)}");

      Fact = UnitFactConfigurator.New("RapidCounterActiveFact", FactGuid)
        .Configure();

      var activeBuff = BuffConfigurator.New("RapidCounterActiveBuff", ActiveBuffGuid)
        .AddFacts(new() { Fact })
        .SetDisplayName(name)
        .SetDescription("RapidCounterBuff.Desc")
        .SetIcon(icon)
        .Configure();

      var toggleBuff = BuffConfigurator.New("RapidCounterOn", "BAD869DC-44CF-4EF3-8AB2-0464BF4554B8")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddComponent<RapidCounterCounter>()
        .Configure();

      Activatable = ActivatableAbilityConfigurator.New("RapidCounterActivatable", "7F04B61F-6ECE-48E0-8D8D-1F094DBE29C1")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetActivationType(AbilityActivationType.Immediately)
        .SetDeactivateIfOwnerDisabled()
        .SetDeactivateIfOwnerUnconscious()
        .SetDoNotTurnOffOnRest()
        .SetBuff(toggleBuff)
        .Configure();

      var feat = FeatureConfigurator.New("RapidCounterFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { Activatable })
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
#endif
        .Configure(true);
    }
  }
}