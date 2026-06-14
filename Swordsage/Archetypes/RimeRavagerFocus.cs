using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Swordsage.Archetypes
{
    static class RimeRavagerFocus
    {
        public const string RimeRavagerFocusGuid = "33178F5F-DC3C-4942-AEB0-E2B19AE9AEE6";
        public const string RimeRavagerFocusFactGuid = "F1CAE8D5-22FB-4EE9-8E3C-E2382227E8C7";

        public static void Configure()
        {
            var shadowHandFocusFact = UnitFactConfigurator.New("RimeRavagerFocusFact", RimeRavagerFocusFactGuid)
              .Configure();
            FeatureConfigurator.New("RimeRavagerFocus", RimeRavagerFocusGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
              .SetDisplayName("RimeRavagerFocus.Name")
              .SetDescription("RimeRavagerFocus.Desc")
              .SetIcon(FeatureRefs.SpellFocusNecromancy.Reference.Get().Icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { shadowHandFocusFact })
              .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.ShadowHandGuids.ToList())
              .Configure();
        }
    }
}