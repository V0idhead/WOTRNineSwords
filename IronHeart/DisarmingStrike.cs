using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/disarming-strike--3645/
  public class DisarmingStrike
  {
    public const string Guid = "9EF4F66E-DA00-4ED7-A6FF-EBA2B34D0E5B";
    const string name = "DisarmingStrike.Name";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.BlessingOfCourageAndLife.Reference.Get().Icon;

      log.Info($"Configuring {nameof(DisarmingStrike)}");

      var triggerBuff = BuffConfigurator.New("DisarmingStrikeTriggerBuff", "CC2107FB-4BCE-45E7-B607-719D7EF1D149")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddInitiatorAttackRollTrigger(
          onlyHit: true,
          action: ActionsBuilder.New().CombatManeuver(type: Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm, onSuccess: ActionsBuilder.New()))
        .Configure();

      var ability = AbilityConfigurator.New("DisarmingStrikeAbility", "2E21730D-F59D-47CC-869A-12E0829EA809")
        .SetDisplayName(name)
        .SetDescription("DisarmingStrike.Desc")
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
        .SetCanTargetEnemies()
        .SetCanTargetFriends(false)
        .SetCanTargetSelf(false)
        .SetRange(AbilityRange.Touch)
        .SetActionType(UnitCommand.CommandType.Standard)
        .SetShouldTurnToTarget()
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction
      (
          ActionsBuilder.New().Add<MeleeAttackExtended>(attack => { attack.OnHit = ActionsBuilder.New().CombatManeuver(ActionsBuilder.New(), Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm, newStat: Kingmaker.EntitySystem.Stats.StatType.Strength).Build(); })
        )
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("DisarmingStrike", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription("DisarmingStrike.Desc")
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl2Guid)
#endif
        .Configure();
    }
  }
}