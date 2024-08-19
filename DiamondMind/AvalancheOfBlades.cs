using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.StoneDragon;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.DiamondMind
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/avalanche-blades--3620/
  static class AvalancheOfBlades
  {
    public const string Guid = "74C4B005-F777-4D9C-8C65-E9F31201F4FD";
    const string name = "AvalancheOfBlades.Name";
    const string desc = "AvalancheOfBlades.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(AvalancheOfBlades)}");

      UnityEngine.Sprite icon = AbilityRefs.TricksterRayOfHalberds.Reference.Get().Icon;

      var buff = BuffConfigurator.New("AvalancheOfBladesBuff", "DF324072-D04C-47C5-A289-022B5B203144")
        //.SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .SetDisplayName(name)
        .SetDescription(desc)
        //.AddAttackBonus(-4)
        //.AddAttackBonusAgainstFactOwner(bonus: new ContextValue { Value = -4, ValueType = ContextValueType.Shared})
        .AddInitiatorAttackRollTrigger(
          onlyHit: true,
          action: ActionsBuilder.New().BuffActionAddStatBonus(ModifierDescriptor.UntypedStackable, Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus, new ContextValue { Value = -4 }))
        .Configure();

      var ability = AbilityConfigurator.New(name, "6F55DCF7-6E10-494B-BF5F-9A882B1D52A1")
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
        //.AddAbilityEffectRunAction(RecurseAttack(10, buff))
        .AddAbilityEffectRunAction(ActionsBuilder.New().Add<MeleeAttackAvalanche>())
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("AvalancheOfBlades", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl7Guid)
        .AddPrerequisiteFeaturesFromList(amount: 3, features: AllManeuversAndStances.DiamondMindGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }

    private static ActionList RecurseAttack(int count, BlueprintBuff buff)
    {
      if (--count >= 0)
        return ActionsBuilder.New().Add<MeleeAttackExtended>(attack => { attack.OnHit = RecurseAttack(count, buff); attack.SelectNewTarget = false; }).ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true).Build();
      else
        return ActionsBuilder.New().Build();
    }
  }
}