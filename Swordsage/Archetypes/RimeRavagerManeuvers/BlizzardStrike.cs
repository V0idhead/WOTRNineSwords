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
    static class BlizzardStrike
    {
        public const string Guid = "ACC1D5ED-57D9-4D1E-A6BC-E35876289C45";
        const string name = "BlizzardStrike.Name";
        const string desc = "BlizzardStrike.Desc";
        //const string icon = Helpers.IconPrefix + "blizzardstrike.png";
        static Sprite icon = AbilityRefs.Flare.Reference.Get().Icon;

        public static BlueprintFeature Configure()
        {
            Main.Logger.Info($"Configuring {nameof(BlizzardStrike)}");

            var targetBuff = BuffConfigurator.New("BlizzardStrikeTargetBuff", "551D8E3C-68CB-420D-B68F-F357D49D27D2")
              .SetDisplayName(name)
              .SetDescription("BlizzardStrike.TargetBuff")
              .SetIcon(icon)
              .AddBuffStatusCondition(Kingmaker.UnitLogic.UnitCondition.Blindness)
              .AddSpellFailureChance(25)
              .Configure();

            var ability = AbilityConfigurator.New("BlizzardStrikeAbility", "4881C0AF-A442-4F1E-B015-166886EBC9A9")
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
                        DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Cold), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D8, ContextValues.Constant(8))).
                        SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 19 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom),
                            onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(targetBuff, ContextDuration.Fixed(10))
                            )
                        ).Build()
                    )
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            return FeatureConfigurator.New("BlizzardStrike", Guid, AllManeuversAndStances.featureGroup)
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