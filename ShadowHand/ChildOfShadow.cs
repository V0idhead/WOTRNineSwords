using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.ActivatableAbilities;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.ShadowHand
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/child-shadow--3685/
  static class ChildOfShadow
  {
    public const string Guid = "7205D65D-469E-460D-932C-C368CF58AB53";
    const string name = "ChildOfShadow.Name";
    const string desc = "ChildOfShadow.Desc";
    const string buffDesc = "ChildOfShadowBuff.Desc";
    const string icon = Helpers.IconPrefix + "childofshadow.png";

    public static void Configure()
    {
      var buff = BuffConfigurator.New("ChildOfShadowBuff", "4DE24D97-321E-49F8-B621-458C2201A13F")
        .SetDisplayName(name)
        .SetDescription(buffDesc)
        .SetIcon(icon)
        .AddConcealment(concealment: Kingmaker.Enums.Concealment.Partial, descriptor: Kingmaker.Enums.ConcealmentDescriptor.Displacement)
        .AddStatBonusIfHasFactFixed(new BlueprintCore.Blueprints.Components.Replacements.AddStatBonusIfHasFactFixed(Kingmaker.EntitySystem.Stats.StatType.AC, ContextValues.Constant(2), new System.Collections.Generic.List<BlueprintCore.Utils.Blueprint<Kingmaker.Blueprints.BlueprintUnitFactReference>> { ShadowPresence.ShadowHandFocusFactGuid }, descriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable))
        .Configure();

      var triggerBuff = BuffConfigurator.New("ChildOfShadowTriggerBuff", "DA0BD533-B972-492A-9AD6-F06420C144DB")
        .SetDisplayName("ChildOfShadowTriggerBuff")
        //.SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddMovementDistanceTrigger(distanceInFeet: 10,
          action: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true))
        .Configure();

      var activatable = ActivatableAbilityConfigurator.New("ChildOfShadowActivatable", "82A2A2E8-446A-4BF5-BB05-A5D825DC9053")
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