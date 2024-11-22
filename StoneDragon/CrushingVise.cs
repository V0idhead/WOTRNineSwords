using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
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
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.StoneDragon
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/crushing-vise--3714/
  static class CrushingVise
  {
    public const string Guid = "BDA9E99E-9662-4593-9C26-BA9E3AA06BE2";
    const string name = "CrushingVise.Name";
    const string desc = "CrushingVise.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.TarPool.Reference.Get().Icon;

      Main.Logger.Info($"Configuring {nameof(CrushingVise)}");

      var targetBuff = BuffConfigurator.New("CrushingViseTargetBuff", "EF1F58D0-18CA-4244-9BA7-12DC3939CF93")
        .SetDisplayName(name)
        .SetDescription("CrushingVise.TargetBuff")
        .SetIcon(icon)
        .AddBuffMovementSpeed(value: -1000)
        .Configure();

      var buff = BuffConfigurator.New("CrushingViseBuff", "D0584294-E854-4AFF-9C46-14D870F9F2D2")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddInitiatorAttackRollTrigger(onlyHit: true,
          action: ActionsBuilder.New().ApplyBuff(targetBuff, ContextDuration.Fixed(1))
        )
        .Configure();

      var ability = AbilityConfigurator.New("CrushingViseAbility", "EB9A758A-C037-4F68-9040-5E696B6F197B")
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
          actions: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true).Add<ContextMeleeAttackRolledBonusDamage>(bd => { bd.ExtraDamage = new DiceFormula(4, DiceType.D6); bd.OnHit = EnduranceOfStone.GetEffectAction(); })
         )
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("CrushingVise", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl6Guid)
#endif
        .Configure();
    }
  }
}