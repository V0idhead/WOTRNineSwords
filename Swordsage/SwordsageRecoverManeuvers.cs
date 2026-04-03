using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using VoidHeadWOTRNineSwords.Common;

namespace VoidHeadWOTRNineSwords.Swordsage
{
  static class SwordsageRecoverManeuvers
  {
    public const string Guid = "BD841AB2-1F94-423E-BC56-3E3E8B93C35C";
    const string Name = "RecoverManeuvers.Name";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(SwordsageRecoverManeuvers)}");

      UnityEngine.Sprite icon = AbilityRefs.Restoration.Reference.Get().Icon;

      var ability = AbilityConfigurator.New("SwordsageRecoverManeuversAbility", "81367449-D9AB-4256-BF53-4D326D9902DB")
        .SetDisplayName(Name)
        .SetDescription("SwordsageRecoverManeuvers.Desc")
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
        .SetCanTargetSelf()
        .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
        .SetIsFullRoundAction(true)
        .AddAbilityEffectRunAction(ActionsBuilder.New().OnContextCaster(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid, value: 40)))
        .Configure();

      var feature = FeatureConfigurator.New("SwordsageRecoverManeuvers", Guid)
        .SetDisplayName(Name)
        .SetDescription("SwordsageRecoverManeuvers.Desc")
        .AddFacts([ability])
        .Configure();
    }
  }
}