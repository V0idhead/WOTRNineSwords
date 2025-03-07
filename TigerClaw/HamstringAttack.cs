﻿using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.TigerClaw
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/hamstring-attack--3739/
  static class HamstringAttack
  {
    public const string Guid = "BA431FF7-AF61-4D21-A40E-6D54C2BDD4BE";
    const string name = "HamstringAttack.Name";
    const string desc = "HamstringAttack.Desc";
    const string icon = Helpers.IconPrefix + "hamstringattack.png";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(HamstringAttack)}");

      /*var targetBuff = BuffConfigurator.New("HamstringAttackTargetBuff", "84E070B9-00B1-4504-AA1B-C660356054AA")
        .SetDisplayName(name)
        .SetDescription("HamstringAttack.TargetBuff")
        .SetIcon(icon)
        .AddContextCalculateSharedValue(value: new ContextDiceValue { DiceCountValue = 1, DiceType = DiceType.D8 })
        .AddContextStatBonus(Kingmaker.EntitySystem.Stats.StatType.Dexterity, new ContextValue { ValueShared = Kingmaker.UnitLogic.Abilities.AbilitySharedValue.StatBonus})
        .AddBuffMovementSpeed(value: -10)
        .Configure();

      var targetSavedBuff = BuffConfigurator.New("HamstringAttackTargetSavedBuff", "84E070B9-00B1-4504-AA1B-C660356054AB")
        .SetDisplayName(name)
        .SetDescription("HamstringAttack.TargetBuff")
        .SetIcon(icon)
        .AddContextCalculateSharedValue(value: new ContextDiceValue { DiceCountValue = 1, DiceType = DiceType.D4 })
        .AddContextStatBonus(Kingmaker.EntitySystem.Stats.StatType.Dexterity, new ContextValue { ValueShared = Kingmaker.UnitLogic.Abilities.AbilitySharedValue.StatBonus })
        .AddBuffMovementSpeed(value: -5)
        .Configure();

      var buff = BuffConfigurator.New("HamstringAttackBuff", "C4E6CDB0-6EF5-4EBB-860B-D01529114186")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddInitiatorAttackRollTrigger(onlyHit: true,
          action: ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 17 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength),
            onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(targetBuff, ContextDuration.Fixed(1)))
          )
        )
        .Configure();*/

      var moveDebuffSaved = BuffConfigurator.New("HamstringMoveDebuffSaved", "84E070B9-00B1-4504-AA1B-C660356054AA")
        .SetDisplayName(name)
        .SetDescription("HamstringAttack.TargetBuff")
        .SetIcon(icon)
        .AddBuffMovementSpeed(value: -5)
        .Configure();

      var moveDebuff = BuffConfigurator.New("HamstringMoveDebuff", "84E070B9-00B1-4504-AA1B-C660356054AB")
        .SetDisplayName(name)
        .SetDescription("HamstringAttack.TargetBuff")
        .SetIcon(icon)
        .AddBuffMovementSpeed(value: -10)
        .Configure();

      var ability = AbilityConfigurator.New("HamstringAttackAbility", "B9C00737-A3D7-472B-AA4D-4805CF9139B9")
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
          //actions: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true).MeleeAttack()
          actions: ActionsBuilder.New().AddAll(TigerBlooded.GetEffectAction()).SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 17 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength, TigerBlooded.TigerClawFocusFactGuid),
            onResult: ActionsBuilder.New().ConditionalSaved(
              succeed: ActionsBuilder.New().Add<MeleeAttackWithStatDamage>(mawsd => { mawsd.statType = Kingmaker.EntitySystem.Stats.StatType.Dexterity; mawsd.damageAmount = new DiceFormula(1, DiceType.D4); }).ApplyBuff(moveDebuffSaved, ContextDuration.Fixed(1, DurationRate.Minutes)),
              failed: ActionsBuilder.New().Add<MeleeAttackWithStatDamage>(mawsd => { mawsd.statType = Kingmaker.EntitySystem.Stats.StatType.Dexterity; mawsd.damageAmount = new DiceFormula(1, DiceType.D8); }).ApplyBuff(moveDebuff, ContextDuration.Fixed(1, DurationRate.Minutes))
            )
          )
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("HamstringAttack", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl7Guid)
        .AddPrerequisiteFeaturesFromList(amount: 3, features: AllManeuversAndStances.TigerClawGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}