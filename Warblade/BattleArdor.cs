using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Warblade
{
  static class BattleArdor
  {
    public const string Guid = "C80589A0-688C-4830-B8DB-05A39845B356";
    const string name = "BattleArdor.Name";
    const string desc = "BattleArdor.Desc";

    public static BlueprintFeature Configure()
    {
      Main.Logger.Info($"Configuring {nameof(BattleArdor)}");

      BlueprintFeature battleArdor = FeatureConfigurator.New("BattleArdor", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIsClassFeature()
        .AddRecalculateOnStatChange(stat: Kingmaker.EntitySystem.Stats.StatType.Intelligence)
        .AddCriticalConfirmationBonus(onlyPositiveValue: true, type: Kingmaker.Enums.WeaponRangeType.Melee, value: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusIntelligence))
        .Configure();

      return battleArdor;
    }
  }
}