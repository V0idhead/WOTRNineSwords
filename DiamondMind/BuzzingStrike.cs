using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.DiamondMind
{
    static class BuzzingStrike
    {
        public const string Guid = "BA1CB9D6-9AD1-4147-9F6B-816A414808BF";
        const string name = "BuzzingStrike.Name";
        const string desc = "BuzzingStrike.Desc";
        //const string icon = Helpers.IconPrefix + "buzzingstrike.png";
        static Sprite icon = AbilityRefs.Flare.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(BuzzingStrike)}");

            var buff = BuffConfigurator.New("BuzzingStrikeBuff", "91AACC5D-E113-4173-883A-BE602A80A33D")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.Harmful)
              .SetIcon(icon)
              .AddArcaneSpellFailureIncrease(10, universal: true)
              .Configure();

            var ability = AbilityConfigurator.New("BuzzingStrikeAbility", "062FC25B-6742-45DD-999B-E31D55F932B4")
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
                ActionsBuilder.New().Add<MeleeAttackExtended>(mae =>
                    mae.OnHit = ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Will, customDC: new ContextValue { Value = 11 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength, UnnervingCalm.DiamondFocusFactGuid),
                        onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(6)))).AddAll(UnnervingCalm.GetEffectAction()).Build()
                )
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var maneuver = FeatureConfigurator.New("BuzzingStrike", Guid)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.DiamondMindProficencyGuid, hideInUI: true)
#endif
              .Configure();
        }
    }
}