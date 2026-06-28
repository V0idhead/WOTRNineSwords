using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
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
using UnityEngine;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.Swordsage.Archetypes.RimeRavagerManeuvers
{
    static class IceSpikeStrike
    {
        public const string Guid = "914C5650-7C65-4CED-8043-7EE1FB98F9FE";
        const string name = "IceSpikeStrike.Name";
        const string desc = "IceSpikeStrike.Desc";
        //const string icon = Helpers.IconPrefix + "icespikestrike.png";
        static Sprite icon = AbilityRefs.Flare.Reference.Get().Icon;

        public static BlueprintFeature Configure()
        {
            Main.Logger.Info($"Configuring {nameof(ShatterResistance)}");

            var targetBuff1 = BuffConfigurator.New("IceSpikeStrikeBuff1", "92696320-C4F6-4776-8FF2-A6CA7AB8AD05")
              .SetDisplayName(name)
              .SetDescription("ShatterResistance.TargetBuff1")
              .SetIcon(icon)
              .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.Penalty, stat: Kingmaker.EntitySystem.Stats.StatType.Dexterity, value: -4)
              .Configure();

            var targetBuff2 = BuffConfigurator.New("IceSpikeStrikeBuff2", "D178E216-5A5D-4C2D-9AE1-D1088D1871C4")
              .SetDisplayName(name)
              .SetDescription("ShatterResistance.TargetBuff2")
              .SetIcon(icon)
              .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.Penalty, stat: Kingmaker.EntitySystem.Stats.StatType.Dexterity, value: -6)
              .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.Penalty, stat: Kingmaker.EntitySystem.Stats.StatType.Strength, value: -6)
              .Configure();

            var targetBuff3 = BuffConfigurator.New("IceSpikeStrikeBuff3", "CF08FF5C-EEAF-413F-ADF7-4DECE5CEF06C")
              .SetDisplayName(name)
              .SetDescription("ShatterResistance.TargetBuff3")
              .SetIcon(icon)
              .AddBuffStatusCondition(Kingmaker.UnitLogic.UnitCondition.Paralyzed)
              .Configure();

            var ability = AbilityConfigurator.New("IceSpikeStrikeAbility", "EEC310EB-A26B-42A1-9444-DD8B539DDEA0")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
              .SetCanTargetEnemies()
              .SetCanTargetFriends(false)
              .SetCanTargetSelf(false)
              .SetRange(AbilityRange.Weapon)
              .SetUseCurrentWeaponAsReasonItem()
              .SetActionType(UnitCommand.CommandType.Standard)
              .SetShouldTurnToTarget()
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityEffectRunAction(actions: ActionsBuilder.New().Add<MeleeAttackExtended>(mae => mae.OnHit = ActionsBuilder.New().
                Conditional(ConditionsBuilder.New().HasBuff(targetBuff3),
                    ifTrue: ActionsBuilder.New().DealDamage(DamageTypes.Energy(DamageEnergyType.Cold), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D12, ContextValues.Constant(6))).IncreaseBuffDuration(ContextDuration.Fixed(2), targetBuff3).IncreaseBuffDuration(ContextDuration.Fixed(2), targetBuff2),
                    ifFalse: ActionsBuilder.New().Conditional(ConditionsBuilder.New().HasBuff(targetBuff2),
                        ifTrue: ActionsBuilder.New().DealDamage(DamageTypes.Energy(DamageEnergyType.Cold), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D8, ContextValues.Constant(6))).IncreaseBuffDuration(ContextDuration.Fixed(1), targetBuff2).SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 18 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom), onResult: ActionsBuilder.New().ConditionalSaved(ActionsBuilder.New().ApplyBuff(targetBuff3, ContextDuration.Fixed(2)))),
                        ifFalse: ActionsBuilder.New().Conditional(ConditionsBuilder.New().HasBuff(targetBuff1),
                            ifTrue: ActionsBuilder.New().DealDamage(DamageTypes.Energy(DamageEnergyType.Cold), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D4, ContextValues.Constant(6))).SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 18 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom), onResult: ActionsBuilder.New().ConditionalSaved(ActionsBuilder.New().RemoveBuff(targetBuff1).ApplyBuff(targetBuff2, ContextDuration.Fixed(2)))),
                            ifFalse: ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 18 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom), onResult: ActionsBuilder.New().ConditionalSaved(ActionsBuilder.New().ApplyBuff(targetBuff1, ContextDuration.Fixed(2))))
                        )
                    )
                ).Build()
              ))
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            return FeatureConfigurator.New("IceSpikeStrike", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee | FeatureTag.Ranged)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
              .Configure();
        }
    }
}