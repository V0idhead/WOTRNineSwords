using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Feats
{
    static class EternalMoment //Riven Hourglass focus
    {
        public const string EternalMomentGuid = "64A0B773-F621-4655-A360-4EE727FC2EC5";
        public const string RivenHourglassFocusFactGuid = "AEE54D5F-EBCB-4C0F-AD5C-085887DA0FA1";

        public static void Configure()
        {
            var rivenHourglassFocusFact = UnitFactConfigurator.New("RivenHourglassFocusFact", RivenHourglassFocusFactGuid)
              .Configure();
            FeatureConfigurator.New("EternalMoment", EternalMomentGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
              .SetDisplayName("EternalMoment.Name")
              .SetDescription("EternalMoment.Desc")
              .SetIcon(FeatureRefs.SpellFocusIllusion.Reference.Get().Icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { rivenHourglassFocusFact })
              .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.RivenHourglassGuids.ToList())
              .Configure();
        }

        public static ActionList GetEffectAction()
        {
            return ActionsBuilder.New().Conditional(
              ConditionsBuilder.New().CasterHasFact(RivenHourglassFocusFactGuid),
                ActionsBuilder.New().MeleeAttack()
              ).Build();
        }
    }
}