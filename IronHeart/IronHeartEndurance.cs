using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.StoneDragon;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/iron-heart-endurance--3648/
  static class IronHeartEndurance
  {
    public const string Guid = "194D4185-D916-486F-85D3-1BAFCCD73152";
    const string name = "IronHeartEndurance.Name";
    const string desc = "IronHeartEndurance.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.CureModerateWounds.Reference.Get().Icon;

      Main.Logger.Info($"Configuring {nameof(IronHeartEndurance)}");

      var ability = AbilityConfigurator.New("IronHeartEnduranceAbility", "B3066B06-3B07-4E7B-AAA4-531C239C9F45")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.SelfTouch)
        .SetCanTargetEnemies(false)
        .SetCanTargetFriends(false)
        .SetCanTargetSelf()
        .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
        .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
        .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(
          ActionsBuilder.New()
          .HealTarget(new ContextDiceValue { BonusValue = new ContextValue { Property = UnitProperty.Level }, DiceType = Kingmaker.RuleSystem.DiceType.One, DiceCountValue = new ContextValue { Value = 1 } })
          .HealTarget(new ContextDiceValue { BonusValue = new ContextValue { Property = UnitProperty.Level }, DiceType = Kingmaker.RuleSystem.DiceType.One, DiceCountValue = new ContextValue { Value = 1 } })
        )
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var feature = FeatureConfigurator.New("IronHeartEndurance", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl6Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.IronHeartGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}