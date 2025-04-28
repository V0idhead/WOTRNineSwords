using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Facts;

namespace VoidHeadWOTRNineSwords.Common
{
  static class Common
  {
    public const string ParryActiveBuffGuid = "D2AEC456-221A-45EE-B9D4-4EFFFBEB028A";
    public const string ParryActiveFactGuid = "88B6A14A-A5CC-4B0A-8FD3-992CFC787262";
    public static BlueprintUnitFact ParryActiveFact { get; private set; }

    public static void Configure()
    {
      ParryActiveFact = UnitFactConfigurator.New("ParryActiveFact", ParryActiveFactGuid).Configure();

      UnityEngine.Sprite icon = FeatureRefs.DuelistParryFeature.Reference.Get().Icon;
      BuffConfigurator.New("ParryActiveBuff", ParryActiveBuffGuid)
        .AddFacts(new() { ParryActiveFact })
        .SetDisplayName("ParryActive.Name")
        .SetDescription("ParryActive.Desc")
        .SetIcon(icon)
        .Configure();
    }
  }
}