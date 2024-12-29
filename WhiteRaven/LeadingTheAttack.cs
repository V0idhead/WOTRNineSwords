using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.WhiteRaven
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/leading-attack--3760/
  static class LeadingTheAttack
  {
    public const string Guid = "0A97D978-182E-47BB-8919-00BA43A783D0";
    const string name = "LeadingTheAttack.Name";
    const string desc = "LeadingTheAttack.Desc";
    const string icon = Helpers.IconPrefix + "leadingtheattack.png";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(LeadingTheAttack)}");

      var leadingTheAttackBuff = BuffConfigurator.New("LeadingTheAttackBuff", "6629AAE6-77F8-420F-91D2-D776EDA8BD91")
        .SetDisplayName(name)
        .SetDescription("LeadingTheAttack.Desc")
        .SetIcon(icon)
        .AddACBonusAgainstAttacks(armorClassBonus: -4)
        .Configure();

      var triggerBuff = BuffConfigurator.New("LeadingTheAttackTriggerBuff", "A5537A1C-F46A-4B04-8135-B3703EF65B74")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddInitiatorAttackRollTrigger(
          onlyHit: true,
          action: ActionsBuilder.New().ApplyBuff(leadingTheAttackBuff, ContextDuration.Fixed(1, DurationRate.Rounds)))
        .Configure();

      var ability = AbilityConfigurator.New("LeadingTheAttackAbility", "561A0E99-F2B2-4D2C-AD9B-3E2657A20059")
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
          ActionsBuilder.New().ApplyBuff(triggerBuff, ContextDuration.Fixed(1), toCaster: true).Add<MeleeAttackExtended>(mae => mae.OnHit = WhiteRavenDefense.GetEffectAction())
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var maneuver = FeatureConfigurator.New("LeadingTheAttack", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
        .Configure();
    }
  }
}