using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.TigerClaw;

namespace VoidHeadWOTRNineSwords.Warblade
{
  static class BattleClarityReflex
  {
    public const string Guid = "330A9ACB-77A6-4A8B-B070-2412332A53A7";
    const string name = "BattleClarityReflex.Name";
    const string desc = "BattleClarityReflex.Desc";

    public static BlueprintFeature Configure()
    {
      Main.Logger.Info($"Configuring {nameof(BattleClarityReflex)}");

      BlueprintFeature battleClarityReflex = FeatureConfigurator.New("BattleClarity", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIsClassFeature()
        .SetReapplyOnLevelUp()
        .AddRecalculateOnStatChange(stat: Kingmaker.EntitySystem.Stats.StatType.Intelligence)
        .AddContextCalculateAbilityParamsBasedOnClass(WarbladeC.Guid, statType: Kingmaker.EntitySystem.Stats.StatType.Intelligence)
        .AddContextStatBonus(Kingmaker.EntitySystem.Stats.StatType.SaveReflex, ContextValues.Rank(Kingmaker.Enums.AbilityRankType.StatBonus), Kingmaker.Enums.ModifierDescriptor.UntypedStackable)
        .Configure();

      return battleClarityReflex;
    }
  }
}