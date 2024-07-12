using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Warblade
{
  static class BattleSkill
  {
    public const string Guid = "8698E196-421E-48A0-80A0-8015FBF14CD9";
    const string name = "BattleSkill.Name";
    const string desc = "BattleSkill.Desc";

    public static BlueprintFeature Configure()
    {
      Main.Logger.Info($"Configuring {nameof(BattleSkill)}");

      BlueprintFeature battleSkill = FeatureConfigurator.New("BattleSkill", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIsClassFeature()
        .AddRecalculateOnStatChange(stat: Kingmaker.EntitySystem.Stats.StatType.Intelligence)
        .AddCMDBonus(descriptor: Kingmaker.Enums.ModifierDescriptor.Insight, value: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusIntelligence))
        .Configure();

      return battleSkill;
    }
  }
}