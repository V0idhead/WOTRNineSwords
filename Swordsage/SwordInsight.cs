using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace VoidHeadWOTRNineSwords.Swordsage
{
    static class SwordInsight
    {
        public const string Guid = "6C91345C-7986-4E46-8B5E-3581C59FBB62";
        const string name = "SwordInsight.Name";
        const string desc = "SwordInsight.Desc";

        public static BlueprintFeature Configure()
        {
            Main.Logger.Info($"Configuring {nameof(SwordInsight)}");

            var ACBonusBuff = BuffConfigurator.New("SwordInsightBuff", "D8DE7C09-4CF8-4843-8B9D-9D4AD8C42929")
              .SetDisplayName(name)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddContextStatBonus(Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus, ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom))
              .AddRecalculateOnStatChange(stat: Kingmaker.EntitySystem.Stats.StatType.Wisdom)
              .Configure();

            BlueprintFeature swordsageAattackBonus = FeatureConfigurator.New("SwordInsight", Guid)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIsClassFeature()
              .AddRecalculateOnStatChange(stat: Kingmaker.EntitySystem.Stats.StatType.Wisdom)
              .AddBuffOnLightOrNoArmor(ACBonusBuff)
              .Configure();

            return swordsageAattackBonus;
        }
    }
}