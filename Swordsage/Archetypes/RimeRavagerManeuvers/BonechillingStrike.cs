using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
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
    static class BonechillingStrike
    {
        public const string Guid = "1A6A71A4-C710-4783-A102-E3376A783D2F";
        const string name = "BonechillingStrike.Name";
        const string desc = "BonechillingStrike.Desc";
        //const string icon = Helpers.IconPrefix + "bonechillingstrike.png";
        static Sprite icon = AbilityRefs.Flare.Reference.Get().Icon;

        public static BlueprintFeature Configure()
        {
            Main.Logger.Info($"Configuring {nameof(BonechillingStrike)}");

            var ability = AbilityConfigurator.New("BonechillingStrikeAbility", "CE24CDC0-C6EC-40CF-8C45-A2A4663313D0")
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
                        DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Cold), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D12, ContextValues.Constant(8))).
                        SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Will, customDC: new ContextValue { Value = 20 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom),
                            onResult: ActionsBuilder.New().ConditionalSaved(
                                failed: ActionsBuilder.New().ApplyBuff(BuffRefs.Paralyzed.Reference.Get(), ContextDuration.Variable(ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, true))),
                                succeed: ActionsBuilder.New().ApplyBuff(BuffRefs.Paralyzed.Reference.Get(), ContextDuration.Fixed(1))
                            )
                        ).Build()
                    )
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            return FeatureConfigurator.New("BonechillingStrike", Guid, AllManeuversAndStances.featureGroup)
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