using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.StoneDragon;

namespace VoidHeadWOTRNineSwords.ShadowHand
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/drain-vitality--3690/
  static class DrainVitality
  {
    public const string Guid = "F5FFA51C-82C4-4DF4-B541-686E497007EE";
    const string name = "DrainVitality.Name";
    const string desc = "DrainVitality.Desc";
    const string icon = Helpers.IconPrefix + "drainvitality.png";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(BonesplittingStrike)}");

      var ability = AbilityConfigurator.New("DrainVitalityAbility", "{50419275-9413-4D53-9222-5D80DC98EEB2}")
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
          ActionsBuilder.New().Add<MeleeAttackExtended>(mae => {
              mae.OnHit =
            ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: ContextValues.Constant(12), conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, ShadowPresence.ShadowHandFocusFactGuid),
              onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().DealDamageToAbility(Kingmaker.EntitySystem.Stats.StatType.Constitution, ContextDice.Value(DiceType.One, 2), disableSneakDamage: true).DealDamageToAbility(Kingmaker.EntitySystem.Stats.StatType.Strength, ContextDice.Value(DiceType.One, 2), disableSneakDamage: true))).Build();
            })
        )
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var maneuver = FeatureConfigurator.New("DrainVitalityFeat", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.ShadowHandProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl2Guid)
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.ShadowHandGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}