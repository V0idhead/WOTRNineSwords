using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.ElementsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Feats
{
  static class UnnervingCalm //Diamond Mind Focus
  {
    public const string UnnervingCalmGuid = "6E37F470-72F6-4A52-8278-2553C17B7DBE";
    public const string DiamondFocusFactGuid = "38DF9C4F-F35A-4554-BC99-FA92C1105ADC";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring UnnervingCalm");

      var diamondMindFocusFact = UnitFactConfigurator.New("DiamondMindFocusFact", DiamondFocusFactGuid)
        .Configure();
      var UnnervingCalm = FeatureConfigurator.New("UnnervingCalm", UnnervingCalmGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
        .SetDisplayName("UnnervingCalm.Name")
        .SetDescription("UnnervingCalm.Desc")
        .SetIcon(FeatureRefs.SpellFocusDivination.Reference.Get().Icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { diamondMindFocusFact })
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.DiamondMindGuids.ToList())
        .Configure();
    }

    public static ActionList GetEffectAction()
    {
      return ActionsBuilder.New().OnContextCaster(ActionsBuilder.New().Conditional(ConditionsBuilder.New().CasterHasFact(DiamondFocusFactGuid), ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid, value: 1))).Build();
    }
  }
}