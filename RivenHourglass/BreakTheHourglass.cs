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

namespace VoidHeadWOTRNineSwords.RivenHourglass
{
    static class BreakTheHourglass
    {
        public const string Guid = "713A88EF-1629-485A-9474-87FAD85BAE42";
        const string name = "BreakTheHourglass.Name";
        const string desc = "BreakTheHourglass.Desc";
        //const string icon = Helpers.IconPrefix + "breakthehourglass.png";
        static UnityEngine.Sprite icon = AbilityRefs.FlareBurst.Reference.Get().Icon;
        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(BreakTheHourglass)}");

            var ability = AbilityConfigurator.New("BreakTheHourglassAbility", "85B8C649-BD1B-4E47-8ECE-1DEE5145F628")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional)
              .SetCanTargetPoint()
              .SetCanTargetEnemies()
              .SetEffectOnEnemy(AbilityEffectOnUnit.Harmful)
              .SetRange(AbilityRange.Projectile)
              .SetUseCurrentWeaponAsReasonItem()
              .SetActionType(UnitCommand.CommandType.Standard)
              .SetShouldTurnToTarget()
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityDeliverProjectile(type: Kingmaker.UnitLogic.Abilities.Components.AbilityProjectileType.Cone, length: new Kingmaker.Utility.Feet(30), lineWidth: new Kingmaker.Utility.Feet(5), projectiles: new() { ProjectileRefs.EnchantmentCone30Feet00.ToString() })
              .AddAbilityEffectRunAction
              (
                  ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Reflex, customDC: ContextValues.Constant(19), conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, EternalMoment.RivenHourglassFocusFactGuid),
                  onResult: ActionsBuilder.New().ConditionalSaved(
                      failed: ActionsBuilder.New().Kill(),
                      succeed: ActionsBuilder.New().ApplyBuff(BuffRefs.ExcaustedBuff.Reference.Get(), ContextDuration.FixedDice(Kingmaker.RuleSystem.DiceType.D4, 2)).ApplyBuff(BuffRefs.SlowBuff.Reference.Get(), ContextDuration.FixedDice(Kingmaker.RuleSystem.DiceType.D4, 2))
                  ))
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("BreakTheHourglass", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.RivenHourglassProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl9Guid)
        .AddPrerequisiteFeaturesFromList(amount: 4, features: AllManeuversAndStances.RivenHourglassGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}