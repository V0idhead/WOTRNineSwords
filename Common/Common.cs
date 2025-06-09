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

    public const string CounterAttackBuffGuid = "5041877F-00AC-4701-9C5C-60B19CB12C07";
    public const string CounterAttackFactGuid = "{205E6C1D-5A40-40FF-817D-E5FC75B184EB}";
    public static BlueprintUnitFact CounterAttackActiveFact { get; private set; }

    public static void Configure()
    {
      ParryActiveFact = UnitFactConfigurator.New("ParryActiveFact", ParryActiveFactGuid).Configure();

      UnityEngine.Sprite parryIcon = FeatureRefs.DuelistParryFeature.Reference.Get().Icon;
      BuffConfigurator.New("ParryActiveBuff", ParryActiveBuffGuid)
        .AddFacts(new() { ParryActiveFact })
        .SetDisplayName("ParryActive.Name")
        .SetDescription("ParryActive.Desc")
        .SetIcon(parryIcon)
        .Configure();

      CounterAttackActiveFact = UnitFactConfigurator.New("CounterAttackFact", CounterAttackFactGuid).Configure();

      UnityEngine.Sprite counterAttackIcon = FeatureRefs.DuelistRiposte.Reference.Get().Icon;
      BuffConfigurator.New("CounterAttackBuff", CounterAttackBuffGuid)
        .AddFacts(new() { CounterAttackActiveFact })
        .SetDisplayName("CounterAttack.Name")
        .SetDescription("CounterAttack.Desc")
        .SetIcon(counterAttackIcon)
        .Configure();
    }
  }
}