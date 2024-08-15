using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.StoneDragon;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/strike-perfect-clarity--3659/
  static class StrikeOfPerfectClarity
  {
    public const string Guid = "6AB6337F-0D78-4861-8B3D-8BCE0CABC063";
    const string name = "StrikeOfPerfectClarity.Name";
    const string desc = "StrikeOfPerfectClarity.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(StrikeOfPerfectClarity)}");

      Sprite icon = AbilityRefs.CrystalMind.Reference.Get().Icon;

      var ability = AbilityConfigurator.New("StrikeOfPerfectClarityAbility", "64CE6D26-E404-4F5E-A0EA-4D59741E9089")
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
          ActionsBuilder.New().Add<ContextMeleeAttackRolledBonusDamage>(marb => { marb.ExtraDamage = new Kingmaker.RuleSystem.DiceFormula(100, Kingmaker.RuleSystem.DiceType.One); })
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var maneuver = FeatureConfigurator.New("StrikeOfPerfectClarity", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl9Guid)
        .AddPrerequisiteFeaturesFromList(amount: 4, features: AllManeuversAndStances.IronHeartGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}