using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts.Restrictions;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Swordsage
{
  static class SwordsageACBonus
  {
    public const string Guid = "F70393B2-2D20-445A-A168-013128A89C76";
    const string name = "SwordsageACBonus.Name";
    const string desc = "SwordsageACBonus.Desc";

    public static BlueprintFeature Configure()
    {
      Main.Logger.Info($"Configuring {nameof(SwordsageACBonus)}");

      var ACBonusBuff = BuffConfigurator.New("SwordsageACBonusBuff", "AECDE85F-4CC6-467C-9792-D0EE9AC277FC")
        .SetDisplayName(name)
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddContextStatBonus(Kingmaker.EntitySystem.Stats.StatType.AC, ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom))
        .AddRecalculateOnStatChange(stat: Kingmaker.EntitySystem.Stats.StatType.Wisdom)
        .Configure();

      BlueprintFeature swordsageACBonus = FeatureConfigurator.New("SwordsageACBonus", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIsClassFeature()
        .AddRecalculateOnStatChange(stat: Kingmaker.EntitySystem.Stats.StatType.Wisdom)
        .AddBuffOnLightOrNoArmor(ACBonusBuff)
        .Configure();

      return swordsageACBonus;
    }
  }
}