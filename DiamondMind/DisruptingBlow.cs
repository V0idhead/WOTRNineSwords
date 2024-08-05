using BlueprintCore.Actions.Builder;
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
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.IronHeart;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.DiamondMind
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/disrupting-blow--3624/
  static class DisruptingBlow
  {
    public const string Guid = "D2BC0B6D-23B7-4D5A-A494-CADC42744408";
    const string name = "DisruptingBlow.Name";
    const string desc = "DisruptingBlow.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.StunningFistAbility.Reference.Get().Icon;

      Main.Logger.Info($"Configuring {nameof(DisruptingBlow)}");

      var buff = BuffConfigurator.New("DisruptingBlowBuff", "C54A69F2-B6EA-4899-BEBF-38C13FAF8AEB")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddInitiatorAttackRollTrigger(onlyHit: true,
          action: ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Will, customDC: new ContextValue { Value = 15}, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength),
            onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(BuffRefs.Stunned.Reference.Get(), ContextDuration.Fixed(1))
            )
          )
        )
        .Configure();

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
          actions: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true).MeleeAttack()
         )
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