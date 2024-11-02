using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;

namespace VoidHeadWOTRNineSwords.Feats
{
  static class MythicManeuverFocus
  {
    public static string Guid = "3E0AFDF3-378B-4546-85BE-2A56D3298C54";

    public static void Configure()
    {
      UnityEngine.Sprite icon = FeatureRefs.SpellFocusAbjuration.Reference.Get().Icon;

      var fact = UnitFactConfigurator.New("MythicManeuverFocusFact", Guid)
        .Configure();

      FeatureConfigurator.New("MythicManeuverFocus", "956CAF33-3E95-40BA-B78D-23B644FDA903", Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
        .SetDisplayName("MythicManeuverFocus.Name")
        .SetDescription("MythicManeuverFocus.Desc")
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddPrerequisiteFeature(ManeuverFocus.FeatGuid)
        .AddFacts(new() { fact })
        .Configure();
    }
  }
}