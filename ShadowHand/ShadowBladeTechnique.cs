using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.DiamondMind;

namespace VoidHeadWOTRNineSwords.ShadowHand
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/shadow-blade-technique--3698/
  static class ShadowBladeTechnique
  {
    public const string Guid = "91EECC6B-84A8-4624-8683-865F13BDB259";
    const string name = "ShadowBladeTechnique.Name";
    const string desc = "ShadowBladeTechnique.Desc";
    static UnityEngine.Sprite icon = AbilityRefs.CausticEruption.Reference.Get().Icon;

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(SapphireNightmareBlade)}");

      var ability = AbilityConfigurator.New("ShadowBladeTechnique", "B4897F74-E0A6-4AED-BE0E-3C96A6931041")
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
          ActionsBuilder.New().Add<ShadowBladeTechniqueAction>()
        )
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var feat = FeatureConfigurator.New("ShadowBladeTechniqueFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DiamondMindProficencyGuid, hideInUI: true)
#endif
        .Configure();
    }
  }
}