using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Feats
{
  static class ShadowPresence //Shadow Hand Focus
  {
    public const string ShadowPresenceGuid = "C0E4F1A2-8D7B-4F5A-9D3C-6E5B2F1A0E7F";
    public const string ShadowHandFocusFactGuid = "44B11E8F-31EE-45DF-A559-27CF59EEA9D7";

    public static void Configure()
    {
      var shadowHandFocusFact = UnitFactConfigurator.New("ShadowHandFocusFact", ShadowHandFocusFactGuid)
        .Configure();
      FeatureConfigurator.New("ShadowPresence", ShadowPresenceGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
        .SetDisplayName("ShadowPresence.Name")
        .SetDescription("ShadowPresence.Desc")
        .SetIcon(FeatureRefs.SpellFocusAbjuration.Reference.Get().Icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { shadowHandFocusFact })
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.ShadowHandGuids.ToList())
        .Configure();
    }
  }
}