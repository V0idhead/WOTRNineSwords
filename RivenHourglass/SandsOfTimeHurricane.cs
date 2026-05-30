using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
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
    //https://www.d20pfsrd.com/ALTERNATIVE-RULE-SYSTEMS/3RD-PARTY-RULES-SYSTEMS/PATH-OF-WAR/DISCIPLINES-AND-MANEUVERS/RIVEN-HOURGLASS-MANEUVERS/#TOC-Sands-of-Time-Hurricane
    static class SandsOfTimeHurricane
    {
        public const string Guid = "FC361935-9BA3-488F-9BB2-9D7367D87172";
        const string name = "SandsOfTimeHurricane.Name";
        const string desc = "SandsOfTimeHurricane.Desc";
        const string icon = Helpers.IconPrefix + "sandsoftimehurricane.png";

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(SandsOfTimeHurricane)}");

            var buff = BuffConfigurator.New("SandsOfTimeHurricaneBuff", "762ED39C-46D8-4C21-8E5D-28AD6C5422BD")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddAttackBonus(4)
              .Configure();

            var ability = AbilityConfigurator.New("SandsOfTimeHurricaneAbility", "B576F057-2570-487C-874B-B624808998DC")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
              .SetCanTargetEnemies()
              .SetCanTargetFriends(false)
              .SetCanTargetSelf(true)
              .SetRange(AbilityRange.Personal)
              .SetActionType(UnitCommand.CommandType.Standard)
              .SetShouldTurnToTarget()
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityTargetsAround(radius: new Kingmaker.Utility.Feet(5))
              .AddAbilityEffectRunAction(ActionsBuilder.New()
                  .ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true)
                  .Add<MeleeAttackExtended>(bd =>
                  {
                      bd.OnHit = ActionsBuilder.New()
                      .SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 17 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, EternalMoment.RivenHourglassFocusFactGuid),
                        onResult: ActionsBuilder.New()
                          .ConditionalSaved
                          (
                              failed: ActionsBuilder.New().ApplyBuff(BuffRefs.Exhausted.Reference.Get(), ContextDuration.FixedDice(DiceType.D6, 2)),
                              succeed: ActionsBuilder.New().ApplyBuff(BuffRefs.Exhausted.Reference.Get(), ContextDuration.Fixed(1))
                          )
                        )
                        .AddAll(EternalMoment.GetEffectAction())
                        .Build();
                  })
                  .Add<MeleeAttackExtended>(bd =>
                  {
                      bd.OnHit = ActionsBuilder.New()
                      .SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 17 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, EternalMoment.RivenHourglassFocusFactGuid),
                        onResult: ActionsBuilder.New()
                          .ConditionalSaved
                          (
                              failed: ActionsBuilder.New().ApplyBuff(BuffRefs.SlowBuff.Reference.Get(), ContextDuration.FixedDice(DiceType.D4)),
                              succeed: ActionsBuilder.New().ApplyBuff(BuffRefs.SlowBuff.Reference.Get(), ContextDuration.Fixed(1))
                          )
                        )
                        .AddAll(EternalMoment.GetEffectAction())
                        .Build();
                  })
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("SandsOfTimeHurricane", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.RivenHourglassProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl7Guid)
              .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.RivenHourglassGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}