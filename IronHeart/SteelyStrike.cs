using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.StoneDragon;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/steely-strike--3658/
  static class SteelyStrike
  {
    public const string Guid = "818A1166-1940-402C-91ED-1C079B84DA06";
    const string name = "SteelyStrike.Name";
    const string desc = "SteelyStrike.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.MagicWeapon.Reference.Get().Icon;

      Main.Logger.Info($"Configuring {nameof(SteelyStrike)}");

      var targetBuff = BuffConfigurator.New("SteelyStrikeTargetBuff", "0A4BA9E4-5B12-45D2-BD9E-9A2C50050C5E")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .Configure();

      var buff = BuffConfigurator.New("SteelyStrikeBuff", "55FA2020-27F1-4462-BEA9-A03A927BFB47")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddACBonusAgainstAttacks(armorClassBonus: -4)
        .AddACBonusAgainstBuffOwner(bonus: 4, checkedBuff: targetBuff)
        .Configure();

      var attackBuff = BuffConfigurator.New("SteelyStrikeAttackBuff", "7B174377-B9CF-457C-8418-A472F1BACA71")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        //.AddAttackBonusAgainstTarget(value: new ContextValue { Value=4})
        .AddAttackBonus(4)
        .Configure();

      var ability = AbilityConfigurator.New("SteelyStrikeAbility", "480F8ACA-C49F-4855-A5ED-CDDB4B4B1DBD")
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
          actions: ActionsBuilder.New().ApplyBuff(attackBuff, ContextDuration.Fixed(1), toCaster: true).MeleeAttack().ApplyBuff(targetBuff, ContextDuration.Fixed(1, DurationRate.Rounds)).ApplyBuff(buff, ContextDuration.Fixed(1, DurationRate.Rounds), toCaster: true)
         )
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("SteelyStrike", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
        .Configure();
    }
  }
}