using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.RivenHourglass
{
    //https://www.d20pfsrd.com/ALTERNATIVE-RULE-SYSTEMS/3RD-PARTY-RULES-SYSTEMS/PATH-OF-WAR/DISCIPLINES-AND-MANEUVERS/RIVEN-HOURGLASS-MANEUVERS/#TOC-Tip-the-Hourglass
    static class TipTheHourglass
    {
        public const string Guid = "F7D60F4A-0FC8-4A3D-82CC-A911828635F8";
        const string name = "TipTheHourglass.Name";
        const string desc = "TipTheHourglass.Desc";
        const string icon = Helpers.IconPrefix + "tipthehourglass.png";

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(TipTheHourglass)}");

            var ability = AbilityConfigurator.New("TipTheHourglassAbility", "BA372BD8-634C-47B0-8112-012A6018A639")
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
                ActionsBuilder.New()
                .Add<ContextMeleeAttackRolledBonusDamage>(bd =>
                {
                    bd.ExtraDamage = new DiceFormula(4, DiceType.D6);
                    bd.OnHit = ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 14 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, EternalMoment.RivenHourglassFocusFactGuid),
                      onResult: ActionsBuilder.New()
                        .ConditionalSaved
                        (
                            failed: ActionsBuilder.New().ApplyBuff(BuffRefs.SlowBuff.Reference.Get(), ContextDuration.Variable(ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, toCaster: true)))
                        )
                      )
                      .AddAll(EternalMoment.GetEffectAction())
                      .Build();
                })
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("TipTheHourglass", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.RivenHourglassProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl4Guid)
              .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.RivenHourglassGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}