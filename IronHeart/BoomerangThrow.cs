using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.DiamondMind;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //Throw your weapon at target enemy in close range. You deal normal weapon damage.
  static class BoomerangThrow
  {
    public const string Guid = "272FD08F-88C1-46B7-AC3A-F0DCB8142B5E";
    const string name = "BoomerangThrow.Name";
    const string desc = "BoomerangThrow.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(BoomerangThrow)}");

      UnityEngine.Sprite icon = AbilityRefs.SilverDarts.Reference.Get().Icon;

      var ability = AbilityConfigurator.New("BoomerangThrowAbility", "178A4F0A-5C10-4A06-97C9-C605F87720B8")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
        .SetCanTargetEnemies()
        .SetCanTargetFriends(false)
        .SetCanTargetSelf(false)
        .SetRange(AbilityRange.Close)
        .SetActionType(UnitCommand.CommandType.Standard)
        .SetShouldTurnToTarget()
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(ActionsBuilder.New().MeleeAttack()) //switch to how "Hand of the Apprentice" (Universalist Wizard Lvl0) works?
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("BoomerangThrow", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
        .Configure();
    }
  }
}