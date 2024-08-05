using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.IronHeart;
using VoidHeadWOTRNineSwords.Warblade;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Settings;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.TigerClaw
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/flesh-ripper--3736/
  static class FleshRipper
  {
    public const string Guid = "CD68DB5B-23A3-40AF-B63C-1088424CFF65";
    const string name = "FleshRipper.Name";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.InflictLightWounds.Reference.Get().Icon;

      Main.Logger.Info($"Configuring {nameof(FleshRipper)}");

      var buff = BuffConfigurator.New("FleshRipperBuffSaveFailed", "F1CE2884-33AE-4558-83AD-B12F5B2827F5")
        .SetDisplayName(name)
        .SetDescription("FleshRipper.Desc")
        .SetIcon(icon)
        .AddAttackBonus(-4)
        .AddACBonusAgainstAttacks(armorClassBonus: -4)
        .Configure();

      var triggerBuff = BuffConfigurator.New("FleshRipperTriggerBuff", "3F78B37E-C8E4-4793-A4A8-87EA64943CBB")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddInitiatorAttackRollTrigger(onlyHit: true, criticalHit: true,
          action: ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 13 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength),
            onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1, DurationRate.Minutes)))))
        .AddInitiatorAttackRollTrigger(onlyHit: true, criticalHit: true,
          action: ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 13 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength),
            onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1, DurationRate.Rounds)))))
        .Configure();

      var ability = AbilityConfigurator.New("FleshRipperAbility", "40970405-6389-476A-888B-E620F2184943")
        .SetDisplayName(name)
        .SetDescription("FleshRipper.Desc")
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
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
          actions: ActionsBuilder.New().ApplyBuff(triggerBuff, ContextDuration.Fixed(1), toCaster: true).MeleeAttack()
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("FleshRipper", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription("FleshRipper.Desc")
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl3Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.TigerClawGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}