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
  static class DesertWindDodge //Desert Wind Focus
  {
    //TODO: add actual effects
    public const string DesertWindDodgeGuid = "C0E4F1A2-8D7B-4F5A-9D3C-6E5B2F1A0E7F";
    public const string DesertWindFocusFactGuid = "272EF7E8-54E7-4681-BD33-64E7A7ECC242";

    public static void Configure()
    {
      var desertWindFocusFact = UnitFactConfigurator.New("DesertWindFocusFact", DesertWindFocusFactGuid)
        .Configure();
      FeatureConfigurator.New("DesertWindDodge", DesertWindDodgeGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
        .SetDisplayName("DesertWindDodge.Name") //TODO: write
        .SetDescription("DesertWindDodge.Desc") //TODO: write
        .SetIcon(FeatureRefs.SpellFocusAbjuration.Reference.Get().Icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { desertWindFocusFact })
        
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.DesertWindGuids.ToList())
        .Configure();
    }
  }
}