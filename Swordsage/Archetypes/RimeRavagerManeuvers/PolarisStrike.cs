using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
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

namespace VoidHeadWOTRNineSwords.Swordsage.Archetypes.RimeRavagerManeuvers
{
    static class PolarisStrike
    {
        public const string Guid = "0196F824-1D2A-49DA-999D-686DD1C05AC4";
        const string name = "PolarisStrike.Name";
        const string desc = "PolarisStrike.Desc";
        //const string icon = Helpers.IconPrefix + "polarisstrike.png";
        static Sprite icon = AbilityRefs.Flare.Reference.Get().Icon;

        public static BlueprintFeature Configure()
        {
            Main.Logger.Info($"Configuring {nameof(PolarisStrike)}");

            var targetBuff = BuffConfigurator.New("PolarisStrikeTargetBuff", "76EAE337-1BB4-4C9C-AD9E-FDC039D67AB1")
              .SetDisplayName(name)
              .SetDescription("PolarisStrike.TargetBuff")
              .SetIcon(icon)
              .AddAttackBonus(-2)
              .Configure();

            var ability = AbilityConfigurator.New("PolarisStrikeAbility", "BFB2ADC2-85EA-4F0E-AFF7-D35F2029E2BB")
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
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New().
                    Add<MeleeAttackExtended>(mae => mae.OnHit = ActionsBuilder.New().
                        DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Cold), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(1))).
                        SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Will, customDC: new ContextValue { Value = 12 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom),
                            onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(targetBuff, ContextDuration.Fixed(1))
                            )
                        ).Build()
                    )
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            return FeatureConfigurator.New("PolarisStrike", Guid, AllManeuversAndStances.featureGroup)
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