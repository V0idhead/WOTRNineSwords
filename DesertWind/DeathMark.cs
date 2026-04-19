using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.DesertWind
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/death-mark--3570/
    static class DeathMark
    {
        public const string Guid = "E1C97259-A7D0-4E10-8809-03AAE28DCF80";
        private const string subAbilityGuid = "7D694B9E-6F0A-41CD-B6E4-0DAAF6122961";
        const string name = "DeathMark.Name";
        const string desc = "DeathMark.Desc";
        //const string icon = Helpers.IconPrefix + "burningblade.png";
        static UnityEngine.Sprite icon = AbilityRefs.CausticEruption.Reference.Get().Icon;
        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(DeathMark)}");

            var subAbility = AbilityConfigurator.New("DeathMarkEffect", subAbilityGuid)
              .SetDisplayName(name)
              .SetCanTargetSelf()
              .SetCanTargetEnemies()
              .SetEffectOnAlly(AbilityEffectOnUnit.Harmful)
              .SetEffectOnEnemy(AbilityEffectOnUnit.Harmful)
              .SetRange(AbilityRange.Close)
              .SetActionType(UnitCommand.CommandType.Free)
              .AddAbilityTargetsAround(radius: new Kingmaker.Utility.Feet(20))
              .AddAbilitySpawnFx(AbilitySpawnFxAnchor.Caster, prefabLink: AbilityRefs.FlareBurst.Reference.Get().GetComponent<AbilitySpawnFx>().PrefabLink, time: AbilitySpawnFxTime.OnApplyEffect)
              .AddAbilityEffectRunAction(ActionsBuilder.New().
                  SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Reflex, customDC: new ContextValue { Value = 13 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, DesertWindDodge.DesertWindFocusFactGuid),
                  onResult: ActionsBuilder.New()
                  .ConditionalSaved
                  (
                      failed: ActionsBuilder.New().DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Fire), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, 6)),
                      succeed: ActionsBuilder.New().DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Fire), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, 3))
                  )
              ))
              .Configure();

            var ability = AbilityConfigurator.New("DeathMarkAbility", "3148D96F-CE18-48AE-962F-2423BF901C94")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
              .SetCanTargetEnemies()
              .SetCanTargetFriends(false)
              .SetCanTargetSelf(false)
              .SetRange(AbilityRange.Weapon)
              .SetUseCurrentWeaponAsReasonItem()
              .SetActionType(UnitCommand.CommandType.Standard)
              .SetShouldTurnToTarget()
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New().
                    Add<MeleeAttackExtended>(mae => mae.OnHit = ActionsBuilder.New()
                        .CastSpell(subAbilityGuid, markAsChild: true, spendAction: false, logIfCanNotTarget: true)
                        .Build()
                    )
               )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("DeathMark", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl3Guid)
#endif
              .Configure();
        }
    }
}