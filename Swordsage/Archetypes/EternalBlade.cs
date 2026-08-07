using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Warblade.Archetypes;

namespace VoidHeadWOTRNineSwords.Swordsage.Archetypes
{
    static class EternalBlade
    {
        public const string Guid = "9DC45444-3F03-4BF3-BB73-64AC6E14DEC2";

        public static void Configure()
        {
            var maneuverSelector = EternalBladeManeuverSelection.Configure();
            var stanceSelector = EternalBladeStanceSelection.Configure();

            var timelessReflexes = FeatureConfigurator.New("TimelessReflexes", "2239CCB0-9084-4C60-8D6A-46D4C2BC51F2")
                .SetDisplayName("TimelessReflexes.Name")
                .SetDescription("TimelessReflexes.Desc")
                .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.UntypedStackable, stat: Kingmaker.EntitySystem.Stats.StatType.SaveReflex, value: 2)
                .Configure();

            var timelessReaction = FeatureConfigurator.New("TimelessReaction", "6F1F6C7B-6690-4130-9472-36DE0E4687CF")
                .SetDisplayName("TimelessReaction.Name")
                .SetDescription("TimelessReaction.Desc")
                .AddAttackOfOpportunityAttackBonus(ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, true), Kingmaker.Enums.ModifierDescriptor.Inherent)
                .AddAttackOfOpportunityDamageBonus(false, ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, true))
                .Configure();

            var timelessBody = FeatureConfigurator.New("TimelessBody", "BB5402ED-AABC-4231-9499-39969982DF31")
                .SetDisplayName("TimelessBody.Name")
                .SetDescription("TimelessBody.Desc")
                .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.Slowed)
                .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.Paralyzed)
                .Configure();

            var timelessAwareness = FeatureConfigurator.New("TimelessAwareness", "3C5933C0-8B49-4FF9-8AAD-45C742D81009")
                .SetDisplayName("TimelessAwareness.Name")
                .SetDescription("TimelessAwareness.Desc")
                .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.UntypedStackable, stat: Kingmaker.EntitySystem.Stats.StatType.AttackOfOpportunityCount, value: 2)
                .Configure();

            var islandInTime = FeatureConfigurator.New("IslandInTime", "5ECC068F-41CB-4DDE-BD0B-072B59D8BBC6")
                .SetDisplayName("IslandInTime.Name")
                .SetDescription("IslandInTime.Desc")
                .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.Paralyzed) //find something cooler, move paralyzed to timelessBody
                .Configure();

            ArchetypeConfigurator.New("EternalBlade", Guid, SwordsageC.Guid)
                .SetLocalizedName("EternalBlade.Name")
                .SetLocalizedDescription("EternalBlade.Desc")
                .AddToRemoveFeatures(1, SwordsageManeuverSelection.Guid, SwordsageManeuverSelection.Guid, SwordsageManeuverSelection.Guid, SwordsageManeuverSelection.Guid, SwordsageManeuverSelection.Guid, SwordsageManeuverSelection.Guid, SwordsageStanceSelection.Guid)
                .AddToRemoveFeatures(2, SwordsageManeuverSelection.Guid, SwordsageStanceSelection.Guid)
                .AddToRemoveFeatures(3, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(4, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(5, SwordsageManeuverSelection.Guid, SwordsageStanceSelection.Guid)
                .AddToRemoveFeatures(6, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(7, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(8, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(9, SwordsageManeuverSelection.Guid, SwordsageStanceSelection.Guid)
                .AddToRemoveFeatures(10, SwordsageManeuverSelection.Guid, SwordsageStanceSelection.Guid)
                .AddToRemoveFeatures(11, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(12, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(13, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(14, SwordsageManeuverSelection.Guid, SwordsageStanceSelection.Guid)
                .AddToRemoveFeatures(15, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(16, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(17, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(18, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(19, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(20, SwordsageManeuverSelection.Guid, SwordsageStanceSelection.Guid)
                .AddToAddFeatures(1, maneuverSelector, maneuverSelector, maneuverSelector, maneuverSelector, maneuverSelector, maneuverSelector, stanceSelector)
                .AddToAddFeatures(2, FeatureRefs.UncannyDodge.Reference.Get(), maneuverSelector, stanceSelector)
                .AddToAddFeatures(3, maneuverSelector)
                .AddToAddFeatures(4, maneuverSelector)
                .AddToAddFeatures(5, timelessReflexes, maneuverSelector, stanceSelector)
                .AddToAddFeatures(6, maneuverSelector)
                .AddToAddFeatures(7, maneuverSelector)
                .AddToAddFeatures(8, timelessReaction, maneuverSelector)
                .AddToAddFeatures(9, maneuverSelector, stanceSelector)
                .AddToAddFeatures(10, maneuverSelector, stanceSelector)
                .AddToAddFeatures(11, timelessBody, maneuverSelector)
                .AddToAddFeatures(12, maneuverSelector)
                .AddToAddFeatures(13, timelessAwareness, maneuverSelector)
                .AddToAddFeatures(14, maneuverSelector, stanceSelector)
                .AddToAddFeatures(15, maneuverSelector)
                .AddToAddFeatures(16, FeatureRefs.DeflectArrows.Reference.Get(), maneuverSelector)
                .AddToAddFeatures(17, maneuverSelector)
                .AddToAddFeatures(18, islandInTime, maneuverSelector)
                .AddToAddFeatures(19, maneuverSelector)
                .AddToAddFeatures(20, maneuverSelector, stanceSelector)
                .Configure();
        }
    }
}