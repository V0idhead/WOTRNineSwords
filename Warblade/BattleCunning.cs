using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Mechanics.Properties;

namespace VoidHeadWOTRNineSwords.Warblade
{
  internal class BattleCunning
  {
    public const string Guid = "F8FC61BE-91B3-4101-ADB6-DCDA0C921711";
    const string name = "BattleCunning.Name";
    const string desc = "BattleCunning.Desc";

    public static BlueprintFeature Configure()
    {
      Main.Logger.Info($"Configuring {nameof(BattleCunning)}");

      BlueprintFeature battleCunning = FeatureConfigurator.New("BattleCunning", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIsClassFeature()
        .AddRecalculateOnStatChange(stat: Kingmaker.EntitySystem.Stats.StatType.Intelligence)
        .AddDamageBonusConditional(ContextValues.Property(UnitProperty.StatBonusIntelligence, toCaster: true), false, ConditionsBuilder.New().UseOr().IsFlanked().IsFlatFooted(), ModifierDescriptor.Insight, true)
        .Configure();

      return battleCunning;
    }
  }
}