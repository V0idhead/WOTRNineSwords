using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Warblade
{
  static class BattleMastery
  {
    public const string Guid = "FF9B556F-4734-4D9B-B8D1-262213340957";
    const string name = "BattleMastery.Name";
    const string desc = "BattleMastery.Desc";

    public static BlueprintFeature Configure()
    {
      Main.Logger.Info($"Configuring {nameof(BattleMastery)}");

      BlueprintFeature battleMastery = FeatureConfigurator.New("BattleMastery", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIsClassFeature()
        .AddRecalculateOnStatChange(stat: Kingmaker.EntitySystem.Stats.StatType.Intelligence)
        .AddAttackOfOpportunityAttackBonus(ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusIntelligence), Kingmaker.Enums.ModifierDescriptor.Insight)
        .AddAttackOfOpportunityDamageBonus(weaponType: Kingmaker.Enums.WeaponRangeType.Melee, checkWeaponRangeType: true, damageBonus: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusIntelligence))
        .Configure();

      return battleMastery;
    }
  }
}