using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
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

namespace VoidHeadWOTRNineSwords.ShadowHand
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/bloodletting-strike--3684/
    static class BloodlettingStrike
    {
        public const string Guid = "6908EC74-3FDC-44BA-819B-A28CA09215DB";
        const string name = "BloodlettingStrike.Name";
        const string desc = "BloodlettingStrike.Desc";
        //const string icon = Helpers.IconPrefix + "bonesplittingstrike.png";
        static UnityEngine.Sprite icon = AbilityRefs.CausticEruption.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(BloodlettingStrike)}");

            var ability = AbilityConfigurator.New("BloodlettingStrikeAbility", "6A4FC1FB-4613-44A2-822F-B16DCC71A65C")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
              .SetCanTargetEnemies()
              .SetCanTargetFriends(false)
              .SetCanTargetSelf(false)
              .SetRange(AbilityRange.Weapon)
              .SetActionType(UnitCommand.CommandType.Standard)
              .SetShouldTurnToTarget()
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityEffectRunAction
              (
                ActionsBuilder.New()
                .SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 15 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, ShadowPresence.ShadowHandFocusFactGuid),
                    onResult: ActionsBuilder.New().ConditionalSaved(
                        failed: ActionsBuilder.New().Add<MeleeAttackWithStatDamage>(mawsd => { mawsd.statType = Kingmaker.EntitySystem.Stats.StatType.Constitution; mawsd.damageAmount = new Kingmaker.RuleSystem.DiceFormula(4, Kingmaker.RuleSystem.DiceType.One); }),
                        succeed: ActionsBuilder.New().Add<MeleeAttackWithStatDamage>(mawsd => { mawsd.statType = Kingmaker.EntitySystem.Stats.StatType.Constitution; mawsd.damageAmount = new Kingmaker.RuleSystem.DiceFormula(2, Kingmaker.RuleSystem.DiceType.One); })
                    )
                )
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var maneuver = FeatureConfigurator.New("BloodlettingStrike", Guid)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.ShadowHandProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.ShadowHandGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}