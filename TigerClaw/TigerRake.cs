using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using System.Collections.Generic;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.TigerClaw
{
    static class TigerRake
    {
        public const string Guid = "41A39C53-92C7-4D9E-85D7-E6E246A34BE0";
        const string name = "TigerRake.Name";
        const string desc = "TigerRake.Desc";
        const string icon = Helpers.IconPrefix + "tigerrake.png";

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(TigerRake)}");

            var ability = AbilityConfigurator.New(name, "325B75E3-0656-4A96-9929-7F9A8B5AECEC")
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
                .AddAll(TigerBlooded.GetEffectAction())
                .Add<MultiAttackAction>(maa =>
                {
                    maa.AttackCount = 3;
                    maa.HitEffects = new List<Kingmaker.ElementsSystem.ActionList>
                    {
                        Constants.Empty.Actions, //Effect for HitCount >= 1
                        ActionsBuilder.New().ApplyBuff(BuffRefs.Bleed2d6Buff.Reference.Get(), ContextDuration.Fixed(3)).Build(),
                        ActionsBuilder.New().DealDamageToAbility(Kingmaker.EntitySystem.Stats.StatType.Strength, ContextDice.Value(DiceType.D6, ContextValues.Constant(1))).Build()
                    };
                })
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("TigerRake", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.TigerClawProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
              .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.TigerClawGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}