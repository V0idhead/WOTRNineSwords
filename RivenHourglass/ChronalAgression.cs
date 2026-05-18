using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.DesertWind;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.RivenHourglass
{
    //https://www.d20pfsrd.com/ALTERNATIVE-RULE-SYSTEMS/3RD-PARTY-RULES-SYSTEMS/PATH-OF-WAR/DISCIPLINES-AND-MANEUVERS/RIVEN-HOURGLASS-MANEUVERS/#TOC-Chronal-Aggression
    static class ChronalAgression
    {
        public const string Guid = "0A4965C3-FFF7-46BC-996E-5AC6C1CA152B";
        const string name = "ChronalAgression.Name";
        const string desc = "ChronalAgression.Desc";
        //const string icon = Helpers.IconPrefix + "chronalagression.png";
        static UnityEngine.Sprite icon = AbilityRefs.CausticEruption.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(ChronalAgression)}");

            var ability = AbilityConfigurator.New("ChronalAgressionAbility", "B7A8A9E1-E8AF-4F37-9094-BB882B32ECD6")
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
              .AddAbilityDeliverProjectile(needAttackRoll: true, weapon: ItemWeaponRefs.RayItem.Reference.Guid, type: Kingmaker.UnitLogic.Abilities.Components.AbilityProjectileType.Simple, lineWidth: new Kingmaker.Utility.Feet(5), projectiles: new() { ProjectileRefs.Scorching_Ray00.ToString() })
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                    .DealDamage(
                        DamageTypes.Direct(),
                        ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(2)))
                    .SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: ContextValues.Constant(12), conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, EternalMoment.RivenHourglassFocusFactGuid),
                        onResult: ActionsBuilder.New().ConditionalSaved(
                            failed: ActionsBuilder.New().ApplyBuff(BuffRefs.Sickened.Reference.Get(), ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.Minutes)),
                            succeed: ActionsBuilder.New().ApplyBuff(BuffRefs.Sickened.Reference.Get(), ContextDuration.Fixed(1))
                        ))
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("ChronalAgression", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee | FeatureTag.Ranged)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.RivenHourglassProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl2Guid)
#endif
              .Configure();
        }
    }
}