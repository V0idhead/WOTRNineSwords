using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.ShadowHand
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/child-shadow--3685/
  static class ChildOfShadow
  {
    public const string Guid = "7205D65D-469E-460D-932C-C368CF58AB53";
    const string name = "ChildOfShadow.Name";
    const string desc = "ChildOfShadow.Desc";
    const string icon = Helpers.IconPrefix + "childofshadow.png";

    public static void Configure()
    {
      var triggerBuff = BuffConfigurator.New("ChildOfShadowTriggerBuff", "DA0BD533-B972-492A-9AD6-F06420C144DB")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddMovementDistanceTrigger(distanceInFeet: 10,
          action: ActionsBuilder.New().ApplyBuff(BuffRefs.BlurBuff.Reference.Get(), ContextDuration.Fixed(1), toCaster: true))
        .Configure();

      var activatable = ActivatableAbilityConfigurator.New("ChildOfShadowActivatable", "82A2A2E8-446A-4BF5-BB05-A5D825DC9053")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetActivationType(AbilityActivationType.Immediately)
        .SetBuff(triggerBuff)
        .SetDeactivateIfOwnerDisabled()
        .SetDeactivateIfOwnerUnconscious()
        .SetDoNotTurnOffOnRest()
        .SetGroup(ActivatableAbilityGroup.CombatStyle)
        .SetWeightInGroup(1)
        .SetOnlyInCombat()
        .Configure();

      var feat = FeatureConfigurator.New("ChildOfShadowFeat", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetRanks(1)
        .AddFacts(new() { activatable })
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.ShadowHandProficencyGuid, hideInUI: true)
#endif
        .Configure(true);
    }
  }
}