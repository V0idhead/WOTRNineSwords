using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.DesertWind
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/lingering-inferno--3584/
    static class LingeringInferno
    {
        public const string Guid = "CCB10F6A-6E02-42CD-BFEF-F27EAB5AB6B9";
        const string name = "LingeringInferno.Name";
        const string desc = "LingeringInferno.Desc";
        const string icon = Helpers.IconPrefix + "lingerininferno.png";
        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(LingeringInferno)}");

            var damageOverTime = BuffConfigurator.New("LingeringInfernoDeBuff", "7D912B6A-A407-4FFC-8C01-E696D86886F4")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.Harmful)
              .SetIcon(icon)
              .AddDamageOverTime(new Kingmaker.RuleSystem.DiceFormula(2, Kingmaker.RuleSystem.DiceType.D6), Kingmaker.Enums.Damage.DamageEnergyType.Fire, false)
              .Configure();

            var ability = AbilityConfigurator.New("LingeringInfernoAbility", "EB21A2AA-9411-48C5-BBCF-CAB54530516C")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
              .SetCanTargetEnemies()
              .SetEffectOnEnemy(AbilityEffectOnUnit.Harmful)
              .SetRange(AbilityRange.Weapon)
              .SetUseCurrentWeaponAsReasonItem()
              .SetActionType(UnitCommand.CommandType.Standard)
              .SetShouldTurnToTarget()
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityEffectRunAction
              (
                  ActionsBuilder.New().
                    Add<MeleeAttackExtended>(mae => mae.OnHit = ActionsBuilder.New()
                      .DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Fire), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(2)))
                      .ApplyBuff(damageOverTime, ContextDuration.Fixed(3)).Build()
                  )
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("LingeringInferno", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.DesertWindGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}