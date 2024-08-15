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
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.WhiteRaven
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/war-leaders-charge--3769/
  static class WarLeadersCharge
  {
    public const string Guid = "1BE7B81B-1586-4791-89F8-A073C039100D";
    const string name = "WarLeadersCharge.Name";
    const string desc = "WarLeadersCharge.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.ArmyChargeAbility.Reference.Get().Icon;

      Main.Logger.Info($"Configuring {nameof(WarLeadersCharge)}");

      var damageBuff = BuffConfigurator.New("WarLeadersChargeBuff", "4379D511-9550-466D-B26F-523D201322F8")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddDamageBonusConditional(new ContextValue { Value = 35 }, descriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable)
        //.AddACBonusAgainstAttackOfOpportunity(new ContextValue { Value = 50 }) //not quite the same as not provoking attacks of opportunity since the enemies attack of opportunity will be wasted, but good enough
        .AddMechanicsFeature(Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature.MechanicsFeatureType.DisengageWithoutAttackOfOpportunity)
        .Configure();

      var ability = AbilityConfigurator.New("WarLeadersChargeAbility", "37E1FB25-9C12-42EE-9AA8-CFCC9B92CF1F")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
        .SetCanTargetEnemies()
        .SetCanTargetFriends(false)
        .SetCanTargetSelf(false)
        .SetRange(AbilityRange.DoubleMove)
        .SetActionType(UnitCommand.CommandType.Standard)
        .SetShouldTurnToTarget()
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(
            actions: ActionsBuilder.New().ApplyBuff(damageBuff, ContextDuration.Fixed(1), toCaster: true).CastSpell(AbilityRefs.ChargeAbility.ToString())
         )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("WarLeadersCharge", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl6Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.WhiteRavenGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}