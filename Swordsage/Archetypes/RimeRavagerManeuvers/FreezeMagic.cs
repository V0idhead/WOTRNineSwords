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
    static class FreezeMagic
    {
        public const string Guid = "D02AA359-8F95-4297-94AF-EE5EB32FADE2";
        const string name = "FreezeMagic.Name";
        const string desc = "FreezeMagic.Desc";
        //const string icon = Helpers.IconPrefix + "freezemagic.png";
        static Sprite icon = AbilityRefs.Flare.Reference.Get().Icon;

        public static BlueprintFeature Configure()
        {
            Main.Logger.Info($"Configuring {nameof(FreezeMagic)}");

            var ability = AbilityConfigurator.New("FreezeMagicAbility", "99070E08-09FC-4C6E-81D9-F7A98DB70A3F")
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
                        DispelMagic(Kingmaker.UnitLogic.Mechanics.Actions.ContextActionDispelMagic.BuffType.FromSpells, Kingmaker.RuleSystem.Rules.RuleDispelMagic.CheckType.None, ContextValues.Constant(9), countToRemove: ContextValues.Constant(1),
                            onSuccess: ActionsBuilder.New().DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Cold), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D10, ContextValues.Constant(2)))
                        ).
                        Build()
                    )
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            return FeatureConfigurator.New("FreezeMagic", Guid, AllManeuversAndStances.featureGroup)
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