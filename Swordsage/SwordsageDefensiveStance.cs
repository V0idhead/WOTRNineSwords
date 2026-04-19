using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;

namespace VoidHeadWOTRNineSwords.Swordsage
{
    static class SwordsageDefensiveStance
    {
        public const string Guid = "5C7D1006-6F19-41C5-B5E8-9709B1877F52";
        const string name = "SwordsageDefensiveStance.Name";
        const string desc = "SwordsageDefensiveStance.Desc";

        public static BlueprintFeature Configure()
        {
            Main.Logger.Info($"Configuring {nameof(SwordsageDefensiveStance)}");

            BlueprintFeature swordsageDefensiveStance = FeatureConfigurator.New("SwordsageDefensiveStance", Guid)
              .SetDisplayName(name)
              .SetDescription(desc)
              .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Wisdom))
              .AddContextStatBonus(StatType.SaveFortitude, ContextValues.Rank(), ModifierDescriptor.UntypedStackable)
              .AddContextStatBonus(StatType.SaveReflex, ContextValues.Rank(), ModifierDescriptor.UntypedStackable)
              .AddContextStatBonus(StatType.SaveWill, ContextValues.Rank(), ModifierDescriptor.UntypedStackable)
              .AddRecalculateOnStatChange(stat: StatType.Wisdom)
              .Configure();

            return swordsageDefensiveStance;
        }
    }
}