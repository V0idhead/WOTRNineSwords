using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;

namespace VoidHeadWOTRNineSwords.Swordsage
{
  //see Duelist:ImprovedReaction
  static class QuickToAct
  {
    public const string Guid = "3E8B5F20-9CCE-4112-9FBC-35426FC8077B";
    const string name = "QuickToAct.Name";
    const string desc = "QuickToAct.Desc";

    public static BlueprintFeature Configure()
    {
      Main.Logger.Info($"Configuring {nameof(QuickToAct)}");

      BlueprintFeature quickToAct = FeatureConfigurator.New("QuickToAct", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIsClassFeature()
        .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.UntypedStackable, stat: Kingmaker.EntitySystem.Stats.StatType.Initiative, value: 1)
        .SetRanks(5)
        .Configure();

      return quickToAct;
    }
  }
}