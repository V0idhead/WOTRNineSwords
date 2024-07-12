﻿using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.StoneDragon
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/overwhelming-mountain-strike--3724/
  static class OverwhelmingMountainStrike
  {
    public const string Guid = "28A33ED1-D3D3-4953-8C0E-258C73F867B2";
    const string name = "OverwhelmingMountainStrike.Name";
    const string desc = "OverwhelmingMountainStrike.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.TarPool.Reference.Get().Icon;

      Main.Logger.Info($"Configuring {nameof(OverwhelmingMountainStrike)}");

      var buff = BuffConfigurator.New("OverwhelmingMountainStrikeBuff", "5CDC8654-F15E-40A9-8794-95D166CFA559")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddDamageBonusConditional(bonus: new ContextValue { Value = 8 }, descriptor: ModifierDescriptor.UntypedStackable) //TODO: damage bonus should be 2d6
        .AddInitiatorAttackRollTrigger(onlyHit: true,
          action: ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 14, Property = Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength, ValueType = ContextValueType.CasterProperty },
            onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(BuffRefs.Staggered.Reference.Get(), ContextDuration.Fixed(1))
            )
          )
        )
        .Configure();

      var ability = AbilityConfigurator.New("OverwhelmingMountainStrikeAbility", "97851D37-F1AD-4826-A342-035C8A19B8E3")
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
          actions: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true).MeleeAttack()
         )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("OverwhelmingMountainStrike", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl4Guid)
#endif
        .Configure();
    }
  }
}