using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.DesertWind
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/hatchlings-flame--3579/
    static class HatchlingsFlame
    {
        public const string Guid = "007BE8BA-0F3F-466B-B842-8DDD00EFDACC";
        const string name = "HatchlingsFlame.Name";
        const string desc = "HatchlingsFlame.Desc";
        const string icon = Helpers.IconPrefix + "hatchlingsflame.png";
        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(HatchlingsFlame)}");

            var ability = AbilityConfigurator.New("HatchlingsFlameAbility", "F6CF3A8D-7C57-4CF7-999A-1C084E8EF28F")
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
              .AddAbilityDeliverProjectile(type: Kingmaker.UnitLogic.Abilities.Components.AbilityProjectileType.Cone, length: new Kingmaker.Utility.Feet(30), lineWidth: new Kingmaker.Utility.Feet(5), projectiles: new() { ProjectileRefs.FireCone30Feet00.ToString() } )
              .AddAbilityEffectRunAction
              (
                  ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Reflex, customDC: ContextValues.Constant(12), conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, RelentlessSirocco.DesertWindFocusFactGuid),
                  onResult: ActionsBuilder.New().ConditionalSaved(
                      failed: ActionsBuilder.New().DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Fire), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(2))),
                      succeed: ActionsBuilder.New().DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Fire), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(1)))))
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("HatchlingsFlame", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl2Guid)
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.DesertWindGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}