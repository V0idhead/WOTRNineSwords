using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Common
{
  static class InitiatorLevels
  {
    public const string Lvl1Guid = "076F1F9D-B695-4F69-93A2-9743507B7361";
    public const string Lvl2Guid = "076F1F9D-B695-4F69-93A2-9743507B7362";
    public const string Lvl3Guid = "076F1F9D-B695-4F69-93A2-9743507B7363";
    public const string Lvl4Guid = "076F1F9D-B695-4F69-93A2-9743507B7364";
    public const string Lvl5Guid = "076F1F9D-B695-4F69-93A2-9743507B7365";
    public const string Lvl6Guid = "076F1F9D-B695-4F69-93A2-9743507B7366";
    public const string Lvl7Guid = "076F1F9D-B695-4F69-93A2-9743507B7367";
    public const string Lvl8Guid = "076F1F9D-B695-4F69-93A2-9743507B7368";
    public const string Lvl9Guid = "076F1F9D-B695-4F69-93A2-9743507B7369";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(InitiatorLevels)}");

      FeatureConfigurator.New("ManeuverInitiatorLvl1", Lvl1Guid)
        .SetDisplayName("InitiatorLevel1.Name")
        .SetDescription("InitiatorLevel1.Desc")
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("ManeuverInitiatorLvl2", Lvl2Guid)
        .SetDisplayName("InitiatorLevel2.Name")
        .SetDescription("InitiatorLevel2.Desc")
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("ManeuverInitiatorLvl3", Lvl3Guid)
        .SetDisplayName("InitiatorLevel3.Name")
        .SetDescription("InitiatorLevel3.Desc")
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("ManeuverInitiatorLvl4", Lvl4Guid)
        .SetDisplayName("InitiatorLevel4.Name")
        .SetDescription("InitiatorLevel4.Desc")
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("ManeuverInitiatorLvl5", Lvl5Guid)
        .SetDescription("InitiatorLevel5.Desc")
        .SetDisplayName("InitiatorLevel5.Name")
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("ManeuverInitiatorLvl6", Lvl6Guid)
        .SetDisplayName("InitiatorLevel6.Name")
        .SetDescription("InitiatorLevel6.Desc")
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("ManeuverInitiatorLvl7", Lvl7Guid)
        .SetDisplayName("InitiatorLevel7.Name")
        .SetDescription("InitiatorLevel7.Desc")
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("ManeuverInitiatorLvl8", Lvl8Guid)
        .SetDisplayName("InitiatorLevel8.Name")
        .SetDescription("InitiatorLevel8.Desc")
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("ManeuverInitiatorLvl9", Lvl9Guid)
        .SetDisplayName("InitiatorLevel9.Name")
        .SetDescription("InitiatorLevel9.Desc")
        .SetIsClassFeature()
        .Configure();
    }
  }
}