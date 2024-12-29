using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.StoneDragon
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/mountain-tombstone-strike--3723/
  static class MountainTombstoneStrike
  {
    public const string Guid = "D4483160-C20B-4739-A8F6-21D955B80D6C";
    const string name = "MountainTombstoneStrike.Name";
    const string desc = "MountainTombstoneStrike.Desc";
    const string icon = Helpers.IconPrefix + "mountaintombstonestrike.png";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(MountainTombstoneStrike)}");

      var ability = AbilityConfigurator.New("MountainTombstoneStrikeAbility", "27C2A08E-9DF6-4004-9F9A-243DE069FE74")
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
          ActionsBuilder.New().Add<MeleeAttackWithStatDamage>(mawsd => { mawsd.statType = Kingmaker.EntitySystem.Stats.StatType.Constitution; mawsd.damageAmount = new Kingmaker.RuleSystem.DiceFormula(2, Kingmaker.RuleSystem.DiceType.D6); })
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var maneuver = FeatureConfigurator.New("MountainTombstoneStrike", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl9Guid)
#endif
        .Configure();
    }
  }
}