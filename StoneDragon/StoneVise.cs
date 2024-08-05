using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
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

namespace VoidHeadWOTRNineSwords.StoneDragon
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/stone-vise--3728/
  static class StoneVise
  {
    public const string Guid = "480A4CEF-54A6-4822-B499-1164C3F8C6CF";
    const string name = "StoneVise.Name";
    const string desc = "StoneVise.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.TarPool.Reference.Get().Icon;

      Main.Logger.Info($"Configuring {nameof(StoneVise)}");

      var targetBuff = BuffConfigurator.New("StoneViseTargetBuff", "0A551C24-33DE-4DCA-B3E3-F2C4DB8979D7")
        .SetDisplayName(name)
        .SetDescription("StoneVise.TargetBuff")
        .SetIcon(icon)
        .AddBuffMovementSpeed(value: -200)
        .Configure();

      var buff = BuffConfigurator.New("StoneViseBuff", "3AE23D95-EF16-4912-903F-AD5652BADE91")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddInitiatorAttackRollTrigger(onlyHit: true,
          action: ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 12 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength),
            onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(targetBuff, ContextDuration.Fixed(1))
            )
          )
        )
        .Configure();

      var ability = AbilityConfigurator.New("StoneViseAbility", "8F93242D-2A72-4DA3-9908-51F3FCC83DB0")
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
          actions: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true).Add<ContextMeleeAttackRolledBonusDamage>(bd => bd.ExtraDamage = new DiceFormula(1, DiceType.D6))
         )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("StoneVise", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl2Guid)
#endif

        .Configure();
    }
  }
}