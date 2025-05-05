using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
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

      BlueprintFeature swordsageACBonus = FeatureConfigurator.New("SwordsageACBonus", Guid)
        .CopyFrom(FeatureRefs.ShifterACBonusUnlock.Reference.Get(), typeof(MonkNoArmorFeatureUnlock), typeof(HasArmorFeatureUnlock))
        .SetDisplayName(name)
        .SetDescription(desc)
        .EditComponents<HasArmorFeatureUnlock>(
          c => {
            c.m_ArmorProficiencyGroupEntries = Kingmaker.Blueprints.Items.Armors.ArmorProficiencyGroupFlag.Light;
            c.FilterByArmorProficiencyGroup = true;
            c.FilterByBlueprintArmorTypes = false;
            c.m_DisableWhenHasShield = true;
          }, c => true
        )
        .Configure();

      return swordsageACBonus;
    }
  }
}