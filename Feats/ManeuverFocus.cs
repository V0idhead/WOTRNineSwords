using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Feats
{
  static class ManeuverFocus
  {
    public static string Guid = "C240A96B-C9BA-48AF-9F36-224755451508";
    public static string FeatGuid = "BE231C9F-08B7-4303-832F-420A039A1EF6";

    public static void Configure()
    {
      UnityEngine.Sprite icon = FeatureRefs.SpellFocusAbjuration.Reference.Get().Icon;

      var fact = UnitFactConfigurator.New("ManeuverFocusFact", Guid)
        .Configure();

      FeatureConfigurator.New("ManeuverFocus", FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
        .SetDisplayName("ManeuverFocus.Name")
        .SetDescription("ManeuverFocus.Desc")
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 4)
        .AddFacts(new() { fact })
        .Configure();
    }
  }
}