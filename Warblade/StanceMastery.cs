using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Warblade
{
  static class StanceMastery
  {
    public const string Guid = "54D4908F-27B2-4E0F-8E22-E77C6B709774";

    public static BlueprintFeature Configure()
    {
      Main.Logger.Info($"Configuring {nameof(BattleArdor)}");

      BlueprintFeature stanceMastery = FeatureConfigurator.New("StanceMastery", Guid)
        .SetDisplayName("StanceMastery.Name")
        .SetDescription("StanceMastery.Desc")
        .SetIsClassFeature()
        .AddIncreaseActivatableAbilityGroupSize(Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityGroup.CombatStyle)
        .Configure();

      return stanceMastery;
    }
  }
}