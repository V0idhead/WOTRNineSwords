using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;

namespace VoidHeadWOTRNineSwords.Common
{
  static class BonusWeaponFocus
  {
    public const string Guid = "2CCAAB8E-0F37-45AB-A6C3-35233A39D220";
    const string name = "BonusWeaponFocus.Name";
    const string desc = "BonusWeaponFocus.Desc";

    public static BlueprintFeatureSelection Configure()
    {
      Main.Logger.Info($"Configuring {nameof(BonusWeaponFocus)}");

      BlueprintFeatureSelection bonusWeaponFocus = FeatureSelectionConfigurator.New("BonusWeaponFocus", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIsClassFeature()
        .SetIgnorePrerequisites() //granted at level 1, but Swordsage has 0 BAB at level 1
        .SetAllFeatures(ParametrizedFeatureRefs.WeaponFocus.Reference.Guid)
        .Configure();

      return bonusWeaponFocus;
    }
  }
}