using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.ActivatableAbilities;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.TigerClaw
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/hunters-sense--3740/
  static class HuntersSense
  {
    public const string Guid = "89E9000F-0175-4BBA-82E2-42C0A1BDEA07";
    const string name = "HuntersSense.Name";
    const string desc = "HuntersSense.Desc";
    const string icon = Helpers.IconPrefix + "hunterssense.png";

    public static void Configure()
    {
      var buff = BuffConfigurator.New("HuntersSenseBuff", "6E232DBE-C965-422A-A0D7-E632EDA4915E")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddBlindsense(new Kingmaker.Utility.Feet(30))
        .Configure();

      var activatable = ActivatableAbilityConfigurator.New("HuntersSenseActivatable", "E06BA33F-E394-4B98-A568-C372E959D5A4")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetActivationType(AbilityActivationType.Immediately)
        .SetBuff(buff)
        .SetDeactivateIfOwnerDisabled()
        .SetDeactivateIfOwnerUnconscious()
        .SetDoNotTurnOffOnRest()
        .SetGroup(ActivatableAbilityGroup.CombatStyle)
        .SetWeightInGroup(1)
        .Configure();

      var feat = FeatureConfigurator.New("HuntersSenseFeat", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { activatable })
        .Configure(true);
    }
  }
}