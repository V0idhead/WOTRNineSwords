using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;

namespace VoidHeadWOTRNineSwords.Swordsage
{
  static class SwordsageDamageBonus
  {
    public const string Guid = "3A810BB4-C974-4ACB-B9E7-0FDFF3EAE628";
    const string name = "SwordsageDamageBonus.Name";
    const string desc = "SwordsageDamageBonus.Desc";

    public static BlueprintFeature Configure()
    {
      Main.Logger.Info($"Configuring {nameof(SwordsageDamageBonus)}");

      BlueprintFeature swordsageDamageBonus = FeatureConfigurator.New("SwordsageDamageBonus", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Wisdom))
        .AddContextStatBonus(StatType.AdditionalDamage, ContextValues.Rank(), ModifierDescriptor.UntypedStackable)
        .AddRecalculateOnStatChange(stat: StatType.Wisdom)
        .Configure();

      return swordsageDamageBonus;
    }
  }
}