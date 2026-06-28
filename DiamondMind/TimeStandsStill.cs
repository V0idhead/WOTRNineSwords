using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.DiamondMind
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/time-stands-still--3640/
  static class TimeStandsStill
  {
    public const string Guid = "9B04BE05-15E0-4CFC-A584-17C48F6D24AF";
    const string name = "TimeStandsStill.Name";
    const string desc = "TimeStandsStill.Desc";
    const string icon = Helpers.IconPrefix + "timestandsstill.png";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static void Configure()
    {
      log.Info($"Configuring {nameof(TimeStandsStill)}");

      const string buffGuid = "7324DBE1-BDDA-4CA3-A63B-897E374F3BB8";
      var buff = BuffConfigurator.New("TimeStandsStillUnnervingCalmTrigger", buffGuid)
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddInitiatorAttackRollTrigger(ActionsBuilder.New().AddAll(UnnervingCalm.GetEffectAction()).RemoveBuff(buffGuid), onlyHit: true, onOwner: true)
        .Configure();

      var ability = AbilityConfigurator.New("TimeStandsStillAbility", "7BD1E120-4082-423F-A4D5-5A6BC0E699AA")
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
          actions: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true).MeleeAttack(fullAttack: true).MeleeAttack(fullAttack: true).RemoveBuff(buff, onlyFromCaster: true, toCaster: true)
        )
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("TimeStandsStill", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription("TimeStandsStill.Desc")
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DiamondMindProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl9Guid)
        .AddPrerequisiteFeaturesFromList(amount: 4, features: AllManeuversAndStances.DiamondMindGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}