﻿using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.StoneDragon;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.DiamondMind
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/mind-strike--3630/
  static class MindStrike
  {
    public const string Guid = "D4E572BB-DF94-458C-9052-B6BAC5EBDB66";
    const string name = "MindStrike.Name";
    const string desc = "MindStrike.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(MindStrike)}");

      Sprite icon = AbilityRefs.PredictionOfFailure.Reference.Get().Icon;

      var buff = BuffConfigurator.New("MindStrikeBuff", "DA1676AF-9D47-4FFE-B2F2-DBDDC6F264A0")
        .SetDisplayName(name)
        .SetDescription("MindStrike.Desc")
        .SetIcon(icon)
        .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.StatDamage, stat: Kingmaker.EntitySystem.Stats.StatType.Wisdom, value: -4)
        .Configure();

      var triggerBuff = BuffConfigurator.New("MindStrikeTriggerBuff", "B5BD2306-418C-4F56-8FAB-BE766E73B85B")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddInitiatorAttackRollTrigger(
          onlyHit: true,
          action: ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Will, customDC: new ContextValue { Value = 14, m_AbilityParameter = AbilityParameterType.CasterStatBonus, Property = Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength, ValueType = ContextValueType.CasterProperty })
            .ConditionalSaved
            (
              failed: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(4, DurationRate.Minutes))
            )
        )
        .Configure();

      var ability = AbilityConfigurator.New("MindStrikeAbility", "93A7C909-E17C-45F5-988A-85C07BA003F3")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
        .SetCanTargetEnemies()
        .SetCanTargetFriends(false)
        .SetCanTargetSelf(false)
        .SetRange(AbilityRange.Weapon)
        .SetActionType(UnitCommand.CommandType.Standard)
        .SetShouldTurnToTarget()
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction
        (
          ActionsBuilder.New().ApplyBuff(triggerBuff, ContextDuration.Fixed(1)).MeleeAttack()
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var maneuver = FeatureConfigurator.New("MindStrike", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl4Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.DiamondMindGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}