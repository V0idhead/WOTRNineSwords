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
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.WhiteRaven
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/battle-leaders-charge--3754/
  static class BattleLeadersCharge
  {
    public const string Guid = "FE5069CE-1F4D-4DFB-9058-2B7A5A310A84";
    const string name = "BattleLeadersCharge.Name";
    const string desc = "BattleLeadersCharge.Desc";
    const string icon = Helpers.IconPrefix + "battleleaderscharge.png";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(BattleLeadersCharge)}");

      var buff = BuffConfigurator.New("BattleLeadersBuff", "FB066522-0C78-407B-84CB-25F84C633E03")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddDamageBonusConditional(new ContextValue { Value = 10 }, descriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable)
        .AddMechanicsFeature(Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature.MechanicsFeatureType.DisengageWithoutAttackOfOpportunity)
        .Configure();

      var chargeBuff = BuffConfigurator.New("BattleLeadersChargeBuff", "EB47386F-F01C-4375-9440-29C8B712E7D3")
        .SetDisplayName(name)
        .SetDescription(desc)
        .AddBuffExtraEffects(BuffRefs.ChargeBuff.Reference.Guid, extraEffectBuff: buff)
        .Configure();

      var ability = AbilityConfigurator.New("BattleLeadersChargeAbility", "E1B12C94-2094-4666-9534-759957AC2B0B")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetCanTargetEnemies(false)
        .SetCanTargetFriends(false)
        .SetCanTargetSelf()
        .SetRange(AbilityRange.Personal)
        .SetActionType(UnitCommand.CommandType.Free)
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(chargeBuff, ContextDuration.Fixed(1), toCaster: true))
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("BattleLeadersCharge", Guid, AllManeuversAndStances.featureGroup)
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