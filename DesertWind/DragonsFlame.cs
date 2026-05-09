using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.DesertWind
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/dragons-flame--3573/
    static class DragonsFlame
    {
        public const string Guid = "91CF2611-F740-42D7-81A1-D864D95E3DC7";
        const string name = "DragonsFlame.Name";
        const string desc = "DragonsFlame.Desc";
        const string icon = Helpers.IconPrefix + "dragonsflame.png";
        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(DragonsFlame)}");

            var ability = AbilityConfigurator.New("DragonsFlameAbility", "BA583E1F-8508-428F-B6F5-BC872ED63BB1")
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
              .AddAbilityDeliverProjectile(type: Kingmaker.UnitLogic.Abilities.Components.AbilityProjectileType.Cone, length: new Kingmaker.Utility.Feet(30), lineWidth: new Kingmaker.Utility.Feet(5), projectiles: new() { ProjectileRefs.FireCone30Feet00.ToString() })
              .AddAbilityEffectRunAction
              (
                  ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Reflex, customDC: ContextValues.Constant(12), conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength, UnnervingCalm.DiamondFocusFactGuid),
                  onResult: ActionsBuilder.New().ConditionalSaved(
                      failed: ActionsBuilder.New().DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Fire), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(6))),
                      succeed: ActionsBuilder.New().DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Fire), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(3)))))
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("DragonsFlame", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
#endif
              .Configure();
        }
    }
}