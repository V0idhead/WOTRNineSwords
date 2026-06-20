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
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.ShadowHand;

namespace VoidHeadWOTRNineSwords.Swordsage.Archetypes
{
    static class PiercingIcicle
    {
        public const string Guid = "CC3627BE-B344-4749-BF1F-03CE80A36C52";
        const string name = "PiercingIcicle.Name";
        const string desc = "PiercingIcicle.Desc";
        //const string icon = Helpers.IconPrefix + "piercingicicle.png";
        static Sprite icon = AbilityRefs.Flare.Reference.Get().Icon;

        public static BlueprintFeature Configure()
        {
            Main.Logger.Info($"Configuring {nameof(PiercingIcicle)}");

            var ability = AbilityConfigurator.New("PiercingIcicleAbility", "3FF6534B-89BE-4FCA-9720-3EB149E41263")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional)
              .SetCanTargetEnemies()
              .SetCanTargetFriends(false)
              .SetCanTargetSelf(false)
              .SetRange(AbilityRange.Close)
              .SetUseCurrentWeaponAsReasonItem()
              .SetActionType(UnitCommand.CommandType.Standard)
              .SetShouldTurnToTarget()
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityDeliverProjectile(needAttackRoll: true, weapon: ItemWeaponRefs.RayItem.Reference.Guid, type: Kingmaker.UnitLogic.Abilities.Components.AbilityProjectileType.Simple, lineWidth: new Kingmaker.Utility.Feet(5), projectiles: new() { ProjectileRefs.RayOfFrost00.ToString() })
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                    .DealDamage(
                        DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Cold),
                        ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(2))
                    ).
                    DealDamage(
                        DamageTypes.Physical(form: Kingmaker.Enums.Damage.PhysicalDamageForm.Piercing),
                        ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(2))
                    ).
                    SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 13 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, ShadowPresence.ShadowHandFocusFactGuid),
                        onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(BuffRefs.Bleed2d6Buff.Reference.Get(), ContextDuration.Fixed(2)))
                    )
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            return FeatureConfigurator.New("PiercingIcicle", Guid, AllManeuversAndStances.featureGroup)
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