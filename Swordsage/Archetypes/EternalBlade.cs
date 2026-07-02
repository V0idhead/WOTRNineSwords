using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Swordsage.Archetypes
{
    static class EternalBlade
    {
        public const string Guid = "9DC45444-3F03-4BF3-BB73-64AC6E14DEC2";

        public static void Configure()
        {
            var timelessAwareness = FeatureConfigurator.New("TimelessAwareness", "6AFB19B3-80C8-4DB0-835F-8105916ED39B")
                .SetDisplayName("TimelessAwareness.Name")
                .SetDescription("TimelessAwareness.Desc")
                .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.LoseDexterityToAC)
                .Configure();

            var timelessReflexes = FeatureConfigurator.New("TimelessReflexes", "2239CCB0-9084-4C60-8D6A-46D4C2BC51F2")
                .SetDisplayName("TimelessReflexes.Name")
                .SetDescription("TimelessReflexes.Desc")
                .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.Inherent, stat: Kingmaker.EntitySystem.Stats.StatType.SaveReflex, value: 2)
                .Configure();

            var timelessReaction1 = FeatureConfigurator.New("TimelessReaction1", "6F1F6C7B-6690-4130-9472-36DE0E4687CF")
                .SetDisplayName("TimelessReaction1.Name")
                .SetDescription("TimelessReaction1.Desc")
                .AddAttackOfOpportunityDamageBonus(false, ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, true))
                .Configure();

            var timelessBody1 = FeatureConfigurator.New("TimelessBody1", "BB5402ED-AABC-4231-9499-39969982DF31")
                .SetDisplayName("TimelessBody1.Name")
                .SetDescription("TimelessBody1.Desc")
                .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.Slowed)
                .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.Paralyzed)
                .Configure();

            var timelessReaction2 = FeatureConfigurator.New("TimelessReaction2", "3C5933C0-8B49-4FF9-8AAD-45C742D81009")
                .SetDisplayName("TimelessReaction2.Name")
                .SetDescription("TimelessReaction2.Desc")
                .AddAttackOfOpportunityAttackBonus(ContextValues.Constant(4), Kingmaker.Enums.ModifierDescriptor.Inherent)
                .Configure();

            var timelessBody2 = FeatureConfigurator.New("TimelessBody2", "5ECC068F-41CB-4DDE-BD0B-072B59D8BBC6")
                .SetDisplayName("TimelessBody2.Name")
                .SetDescription("TimelessBody2.Desc")
                .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.Paralyzed)
                .Configure();

            ArchetypeConfigurator.New("EternalBlade", Guid, SwordsageC.Guid)
                .SetLocalizedName("EternalBlade.Name")
                .SetLocalizedDescription("EternalBlade.Desc")
                .AddToAddFeatures(2, timelessAwareness)
                .AddToAddFeatures(5, timelessReflexes)
                .AddToAddFeatures(8, timelessReaction1)
                .AddToAddFeatures(11, timelessBody1)
                .AddToAddFeatures(13, timelessReaction2)
                .AddToAddFeatures(16, FeatureRefs.DeflectArrows.Reference.Get())
                .AddToAddFeatures(18, timelessBody2)
                .Configure();
        }
    }
}