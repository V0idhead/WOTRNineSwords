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
    //https://www.d20pfsrd.com/ALTERNATIVE-RULE-SYSTEMS/3RD-PARTY-RULES-SYSTEMS/PATH-OF-WAR/DISCIPLINES-AND-MANEUVERS/RIVEN-HOURGLASS-MANEUVERS/#TOC-Wrath-of-Time
    static class WrathOfTime
    {
        public const string Guid = "F1226772-75D3-4EE4-B3FC-2B5939C6EF4C";
        const string name = "WrathOfTime.Name";
        const string desc = "WrathOfTime.Desc";
        //const string icon = Helpers.IconPrefix + "wrathoftime.png";
        static UnityEngine.Sprite icon = AbilityRefs.FlareBurst.Reference.Get().Icon;
        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(WrathOfTime)}");

            var ability = AbilityConfigurator.New("WrathOfTimeAbility", "BCF02774-2B19-4ABE-B5DC-C80EBAD9D4AA")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional)
              .SetCanTargetPoint()
              .SetCanTargetEnemies()
              .SetEffectOnEnemy(AbilityEffectOnUnit.Harmful)
              .SetCanTargetFriends()
              .SetEffectOnAlly(AbilityEffectOnUnit.Harmful)
              .SetCanTargetSelf()
              .SetRange(AbilityRange.Projectile)
              .SetUseCurrentWeaponAsReasonItem()
              .SetActionType(UnitCommand.CommandType.Standard)
              .SetShouldTurnToTarget()
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityDeliverProjectile(type: Kingmaker.UnitLogic.Abilities.Components.AbilityProjectileType.Cone, length: new Kingmaker.Utility.Feet(120), lineWidth: new Kingmaker.Utility.Feet(5), projectiles: new() { ProjectileRefs.NecromancyCone50Feet00.ToString() })
              .AddAbilityEffectRunAction
              (
                  ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Reflex, customDC: ContextValues.Constant(18), conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, EternalMoment.RivenHourglassFocusFactGuid),
                  onResult: ActionsBuilder.New().ConditionalSaved(
                      failed: ActionsBuilder.New().DealDamage(DamageTypes.Direct(), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(12))),
                      succeed: ActionsBuilder.New().DealDamage(DamageTypes.Direct(), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(6)))))
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("WrathOfTime", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.RivenHourglassProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl8Guid)
        .AddPrerequisiteFeaturesFromList(amount: 3, features: AllManeuversAndStances.RivenHourglassGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}