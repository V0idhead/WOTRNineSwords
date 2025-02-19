﻿using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.DiamondMind
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/disrupting-blow--3624/
  static class DisruptingBlow
  {
    public const string Guid = "D2BC0B6D-23B7-4D5A-A494-CADC42744408";
    const string name = "DisruptingBlow.Name";
    const string desc = "DisruptingBlow.Desc";
    const string icon = Helpers.IconPrefix + "disruptingblow.png";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(DisruptingBlow)}");

      var ability = AbilityConfigurator.New("DisruptingBlowAbility", "92EA1684-0100-408D-9DF7-7DDA01B6AEC0")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
        .SetHasFastAnimation()
        .SetCanTargetEnemies()
        .SetCanTargetFriends(false)
        .SetCanTargetSelf(false)
        .SetRange(AbilityRange.Weapon)
        .SetUseCurrentWeaponAsReasonItem()
        .SetActionType(UnitCommand.CommandType.Standard)
        .SetShouldTurnToTarget()
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(
          actions: ActionsBuilder.New().Add<MeleeAttackExtended>(mae =>
            mae.OnHit = ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Will, customDC: new ContextValue { Value = 15 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength, UnnervingCalm.DiamondFocusFactGuid),
              onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(BuffRefs.Stunned.Reference.Get(), ContextDuration.Fixed(1)))).AddAll(UnnervingCalm.GetEffectAction()).Build()
         ))
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("DisruptingBlow", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))

#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.DiamondMindGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}