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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.IronHeart;

namespace VoidHeadWOTRNineSwords.RivenHourglass
{
    //https://www.d20pfsrd.com/ALTERNATIVE-RULE-SYSTEMS/3RD-PARTY-RULES-SYSTEMS/PATH-OF-WAR/DISCIPLINES-AND-MANEUVERS/RIVEN-HOURGLASS-MANEUVERS/#TOC-Sands-of-Time-Tornado
    static class SandsOfTimeTornado
    {
        public const string Guid = "9DBCE1C6-DD5B-4A8A-9A5F-0AEFA92B6F11";
        const string name = "SandsOfTimeTornado.Name";
        const string desc = "SandsOfTimeTornado.Desc";
        //const string icon = Helpers.IconPrefix + "sandsoftimetornado.png";
        static UnityEngine.Sprite icon = AbilityRefs.FlareBurst.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(SandsOfTimeTornado)}");

            var ability = AbilityConfigurator.New("SandsOfTimeTornadoAbility", "5872C680-FB10-41CD-9505-1EDDD1D1828C")
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
              .AddAbilityEffectRunAction(ActionsBuilder.New().Add<MeleeAttackExtended>(bd =>
              {
                  bd.OnHit = ActionsBuilder.New()
                  .DealDamage(DamageTypes.Direct(), ContextDice.Value(DiceType.One, ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, toCaster: true), ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, toCaster: true)))
                  .SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 15 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, MarchOfTime.RivenHourglassFocusFactGuid),
                    onResult: ActionsBuilder.New()
                      .ConditionalSaved
                      (
                          failed: ActionsBuilder.New().ApplyBuff(BuffRefs.Sickened.Reference.Get(), ContextDuration.FixedDice(DiceType.D4))
                      )
                    ).Build();
              }))
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("SandsOfTimeTornado", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.RivenHourglassProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
              .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.RivenHourglassGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}