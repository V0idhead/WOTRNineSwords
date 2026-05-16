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
    //https://www.d20pfsrd.com/ALTERNATIVE-RULE-SYSTEMS/3RD-PARTY-RULES-SYSTEMS/PATH-OF-WAR/DISCIPLINES-AND-MANEUVERS/RIVEN-HOURGLASS-MANEUVERS/#TOC-Temporal-Fury
    static class TemporalFury
    {
        public const string Guid = "E6CF7313-CE65-4151-81AB-A43839741DF9";
        const string name = "TemporalFury.Name";
        const string desc = "TemporalFury.Desc";
        //const string icon = Helpers.IconPrefix + "temporalfury.png";
        static UnityEngine.Sprite icon = AbilityRefs.FlareBurst.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(TemporalFury)}");

            var ability = AbilityConfigurator.New("TemporalFuryAbility", "CCB313D6-B3C6-42FC-A6E7-37756899F309")
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
                    bd.ExtraDamage = new DiceFormula(2, DiceType.D6);
                    bd.OnHit = ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 13 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, EternalMoment.RivenHourglassFocusFactGuid),
                      onResult: ActionsBuilder.New()
                        .ConditionalSaved
                        (
                            failed: ActionsBuilder.New().ApplyBuff(BuffRefs.SlowBuff.Reference.Get(), ContextDuration.Fixed(2))
                        )
                      )
                      .AddAll(EternalMoment.GetEffectAction())
                      .Build();
                })
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("TemporalFury", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.RivenHourglassProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl3Guid)
              .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.RivenHourglassGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}