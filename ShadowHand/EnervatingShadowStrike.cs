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
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.ShadowHand
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/enervating-shadow-strike--3691/
    static class EnervatingShadowStrike
    {
        public const string Guid = "0666000C-1285-4FB5-AFA6-17C8482B0726";
        const string name = "EnervatingShadowStrike.Name";
        const string desc = "EnervatingShadowStrike.Desc";
        //const string icon = Helpers.IconPrefix + "burningblade.png";
        static UnityEngine.Sprite icon = AbilityRefs.CausticEruption.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(EnervatingShadowStrike)}");

            var buff = BuffConfigurator.New("EnervatingShadowStrikeBuff", "CA61F2C3-678B-4F36-BF6F-4138CB0E4454")
                .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
                .AddTemporaryHitPointsRandom(descriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable, dice: new Kingmaker.RuleSystem.DiceFormula(5, Kingmaker.RuleSystem.DiceType.D4))
                .Configure();

            var ability = AbilityConfigurator.New("EnervatingShadowStrikeAbility", "1F8201B3-5940-49A0-AA9D-6B4081F2E31B")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
              .SetHasFastAnimation()
              .SetCanTargetEnemies()
              .SetCanTargetFriends(false)
              .SetCanTargetSelf(false)
              .AddAbilityTargetHasFact(new() { FeatureRefs.ConstructType.ToString(), FeatureRefs.UndeadType.ToString() }, inverted: true)
              .SetRange(AbilityRange.Weapon)
              .SetUseCurrentWeaponAsReasonItem()
              .SetActionType(UnitCommand.CommandType.Standard)
              .SetShouldTurnToTarget()
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New().
                    Add<MeleeAttackExtended>(mae =>
                    mae.OnHit = ActionsBuilder.New().
                        SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 18 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, ShadowPresence.ShadowHandFocusFactGuid),
                            onResult: ActionsBuilder.New().ConditionalSaved(failed:
                                ActionsBuilder.New().DealDamageTemporaryNegativeLevels(ContextDuration.Fixed(24, DurationRate.Hours), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D4, new ContextValue { Value = 1}))
                                .ApplyBuff(buff, ContextDuration.Fixed(1, DurationRate.Hours), toCaster: true)
                            )
                        ).Build()
                    )
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("EnervatingShadowStrike", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee | FeatureTag.Ranged)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.ShadowHandProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl8Guid)
        .AddPrerequisiteFeaturesFromList(amount: 3, features: AllManeuversAndStances.ShadowHandGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}