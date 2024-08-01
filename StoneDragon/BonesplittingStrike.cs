using BlueprintCore.Actions.Builder;
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
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Warblade;
using VoidHeadWOTRNineSwords.WhiteRaven;

namespace VoidHeadWOTRNineSwords.StoneDragon
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/bonesplitting-strike--3709/
  static class BonesplittingStrike
  {
    public const string Guid = "07F4E91F-9953-4A47-982B-850A1373C477";
    const string name = "BonesplittingStrike.Name";
    const string desc = "BonesplittingStrike.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(BonesplittingStrike)}");

      Sprite icon = AbilityRefs.Boneshaker.Reference.Get().Icon;

      var buff = BuffConfigurator.New("BonesplittingStrikeBuff", "EDCAC8F2-4331-4F98-A7C4-00B07ED3E67D")
        .SetDisplayName(name)
        .SetDescription("BonesplittingStrike.Desc")
        .SetIcon(icon)
        .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.StatDamage, stat: Kingmaker.EntitySystem.Stats.StatType.Constitution, value: -2)
        .Configure();

      var ability = AbilityConfigurator.New("BonesplittingStrikeAbility", "A0CBA7B9-3DEA-45E1-A91F-EF37E5D8891B")
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
          ActionsBuilder.New().Add<MeleeAttackExtended>(attack => { attack.OnHit = ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(4, DurationRate.Hours)).Build(); })
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var maneuver = FeatureConfigurator.New("BonesplittingStrike", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl4Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.StoneDragonGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}