using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.WhiteRaven
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/white-raven-hammer--3771/
  static class WhiteRavenHammer
  {
    public const string Guid = "022457B2-27A3-4F8E-BF99-356017858E7B";
    const string name = "WhiteRavenHammer.Name";
    const string desc = "WhiteRavenHammer.Desc";
    const string icon = Helpers.IconPrefix + "whiteravenhammer.png";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(WhiteRavenHammer)}");

      var ability = AbilityConfigurator.New(name, "E0CC2722-F732-49AF-8E3B-AF6801301E2E")
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
          ActionsBuilder.New().Add<ContextMeleeAttackRolledBonusDamage>(marb => { marb.ExtraDamage = new Kingmaker.RuleSystem.DiceFormula(6, Kingmaker.RuleSystem.DiceType.D6); marb.OnHit = ActionsBuilder.New().ApplyBuff(BuffRefs.Stunned.Reference.Guid, ContextDuration.Fixed(1)).AddAll(WhiteRavenDefense.GetEffectAction()).Build(); })
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("WhiteRavenHammer", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl8Guid)
        .AddPrerequisiteFeaturesFromList(amount: 3, features: AllManeuversAndStances.WhiteRavenGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}