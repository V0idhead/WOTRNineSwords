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
    static class MarchOfTime //Riven Hourglass focus
    {
        public const string MarchOfTimeGuid = "64A0B773-F621-4655-A360-4EE727FC2EC5";
        public const string RivenHourglassFocusFactGuid = "AEE54D5F-EBCB-4C0F-AD5C-085887DA0FA1";

        public static void Configure() //TODO: effect
        {
            var rivenHourglassFocusFact = UnitFactConfigurator.New("RivenHourglassFocusFact", RivenHourglassFocusFactGuid)
              .Configure();
            FeatureConfigurator.New("MarchOfTime", MarchOfTimeGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
              .SetDisplayName("MarchOfTime.Name")
              .SetDescription("MarchOfTime.Desc") //TODO: write
              .SetIcon(FeatureRefs.SpellFocusNecromancy.Reference.Get().Icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { rivenHourglassFocusFact })
              .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.RivenHourglassGuids.ToList())
              .Configure();
        }
    }
}