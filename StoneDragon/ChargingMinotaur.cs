using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
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
using Kingmaker.UnitLogic.Buffs;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.BasicEx;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics.Properties;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.StoneDragon
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/charging-minotaur--3712/
  static class ChargingMinotaur
  {
    public const string Guid = "9B3039DA-6500-43B5-B15D-ACFAB6DE8246";
    const string name = "ChargingMinotaur.Name";
    const string desc = "ChargingMinotaur.Desc";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.ArmyChargeAbility.Reference.Get().Icon;

      log.Info($"Configuring {nameof(ChargingMinotaur)}");

      var buff = BuffConfigurator.New("ChargingMinotaurBuff", "B2AE1B4A-712A-4B25-AFF7-265E39A31E73")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddACBonusAgainstAttackOfOpportunity(new ContextValue { Value = 50 }) //not quite the same as not provoking attacks of opportunity since the enemies attack of opportunity will be wasted, but good enough
        .AddInitiatorAttackRollTrigger(onlyHit: true,
          action: ActionsBuilder.New().CombatManeuver(type: Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush,
              onSuccess: ActionsBuilder.New().DealDamage(new DamageTypeDescription { Physical = new DamageTypeDescription.PhysicalData { Form = PhysicalDamageForm.Bludgeoning } }, new ContextDiceValue { DiceType = Kingmaker.RuleSystem.DiceType.D6, DiceCountValue = 2, BonusValue = new ContextValue { Property = UnitProperty.StatBonusStrength } })
          )
        )
        .AddInitiatorAttackRollTrigger(action: ActionsBuilder.New().RemoveSelf())
        .Configure();

      /* kinda works, but is klunky in game
      var ability = AbilityConfigurator.New("ChargingMinotaurAbility", "A1529257-E0AF-4D6E-B4A3-5816D7A0E18C")
        .SetDisplayName(name)
        .SetDescription("ChargingMinotaur.Desc")
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
        .SetCanTargetEnemies()
        .SetCanTargetFriends(false)
        .SetCanTargetSelf(false)
        .SetRange(AbilityRange.DoubleMove)
        .SetActionType(UnitCommand.CommandType.Standard)
        .SetShouldTurnToTarget()
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(
            actions: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true).CastSpell(AbilityRefs.ChargeAbility.Reference.Guid).CombatManeuver(type: Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush,
              onSuccess: ActionsBuilder.New().DealDamage(new DamageTypeDescription { Physical = new DamageTypeDescription.PhysicalData { Form=PhysicalDamageForm.Bludgeoning } }, new ContextDiceValue { DiceType = Kingmaker.RuleSystem.DiceType.D6, DiceCountValue = 2, BonusValue = new ContextValue { Property = UnitProperty.StatBonusStrength } })
            )
         )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();*/

      /* does not work
      var ability = AbilityConfigurator.New("ChargingMinotaurAbility", "A1529257-E0AF-4D6E-B4A3-5816D7A0E18C")
        .CopyFrom(AbilityRefs.ChargeAbility)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(
            actions: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true).CombatManeuver(type: Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush,
              onSuccess: ActionsBuilder.New().DealDamage(new DamageTypeDescription { Physical = new DamageTypeDescription.PhysicalData { Form = PhysicalDamageForm.Bludgeoning } }, new ContextDiceValue { DiceType = Kingmaker.RuleSystem.DiceType.D6, DiceCountValue = 2, BonusValue = new ContextValue { Property = UnitProperty.StatBonusStrength } })
            )
         )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();*/

      /*var spell = FeatureConfigurator.New("ChargingMinotaur", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription("ChargingMinotaur.Desc")
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
        .Configure();*/

      var chargeBuff = BuffConfigurator.New("ChargingMinotaurChargeBuff", "8BF0ABBB-6187-4898-A335-586DB998C47D")
        .SetDisplayName(name)
        .SetDescription(desc)
        .AddBuffExtraEffects(BuffRefs.ChargeBuff.Reference.Guid, extraEffectBuff: buff)
        .Configure();

      var ability = AbilityConfigurator.New("ChargingMinotaurAbility", "A1529257-E0AF-4D6E-B4A3-5816D7A0E18C")
        .SetDisplayName(name)
        .SetDescription("ChargingMinotaur.Desc")
        .SetIcon(icon)
        .SetCanTargetEnemies(false)
        .SetCanTargetFriends(false)
        .SetCanTargetSelf()
        .SetRange(AbilityRange.Personal)
        .SetActionType(UnitCommand.CommandType.Swift)
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(chargeBuff, ContextDuration.Fixed(1), toCaster: true))
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("ChargingMinotaur", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription("ChargingMinotaur.Desc")
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
        .Configure();
    }
  }
}