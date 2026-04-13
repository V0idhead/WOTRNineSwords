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
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.DesertWind
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/wyrms-flame--3591/
    static class WyrmsFlame
    {
        public const string Guid = "E6D93DBD-13B3-4399-86F5-F5A3E3AB8AB7";
        const string name = "WyrmsFlame.Name";
        const string desc = "WyrmsFlame.Desc";
        //const string icon = Helpers.IconPrefix + "burningblade.png";
        static UnityEngine.Sprite icon = AbilityRefs.FlareBurst.Reference.Get().Icon;
        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(WyrmsFlame)}");

            var ability = AbilityConfigurator.New("WyrmsFlameAbility", "364FCAD5-474A-47D2-A11A-C81174BE078E")
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
                      failed: ActionsBuilder.New().DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Fire), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(10))),
                      succeed: ActionsBuilder.New().DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Fire), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(5)))))
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("WyrmsFlame", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl8Guid)
        .AddPrerequisiteFeaturesFromList(amount: 3, features: AllManeuversAndStances.DesertWindGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}