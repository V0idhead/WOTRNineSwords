using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Warblade
{
  static class WeaponAptitude
  {
    public const string Guid = "3D41EF64-35FB-47DE-B70B-77E21A3E3695";
    const string name = "WeaponAptitude.Name";
    const string desc = "WeaponAptitude.Desc";

    public static BlueprintFeature Configure()
    {
      Main.Logger.Info($"Configuring {nameof(WeaponAptitude)}");

      BlueprintFeature weaponAptitude = FeatureConfigurator.New("WeaponAptitude", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIsClassFeature()
        .AddClassLevelsForPrerequisites(WarbladeC.Guid, CharacterClassRefs.FighterClass.Reference.Guid, FeatureSelectionRefs.BasicFeatSelection.Reference.Guid, summand: -2)
        .Configure();

      return weaponAptitude;
    }
  }
}