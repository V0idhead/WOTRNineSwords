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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.TigerClaw
{
    static class TigerMaul
    {
        public const string Guid = "6B5DF913-29D9-49D6-A486-86FBC1001475";
        const string name = "TigerMaul.Name";
        const string desc = "TigerMaul.Desc";
        //const string icon = Helpers.IconPrefix + "tigermaul.png";
        static UnityEngine.Sprite icon = AbilityRefs.CausticEruption.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(TigerMaul)}");

            var ability = AbilityConfigurator.New(name, "6EAFED35-90A4-4833-9CC5-5ED322F23624")
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
                    maa.AttackCount = 4;
                    maa.HitEffects = new List<Kingmaker.ElementsSystem.ActionList>
                    {
                        Constants.Empty.Actions, //Effect for HitCount >= 1
                        ActionsBuilder.New().ApplyBuff(BuffRefs.Bleed2d6Buff.Reference.Get(), ContextDuration.Fixed(3)).Build(),
                        ActionsBuilder.New().DealDamageToAbility(Kingmaker.EntitySystem.Stats.StatType.Strength, ContextDice.Value(DiceType.D6, ContextValues.Constant(1))).Build(),
                        ActionsBuilder.New().DealDamage(DamageTypes.Direct(),ContextDice.Value(DiceType.D12, ContextValues.Constant(4))).Build()
                    };
                })
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("TigerMaul", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.TigerClawProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl7Guid)
              .AddPrerequisiteFeaturesFromList(amount: 3, features: AllManeuversAndStances.TigerClawGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}