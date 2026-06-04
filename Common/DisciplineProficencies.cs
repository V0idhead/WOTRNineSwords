using BlueprintCore.Blueprints.CustomConfigurators.Classes;

namespace VoidHeadWOTRNineSwords.Common
{
  static class DisciplineProficencies
  {
    public const string DesertWindProficencyGuid = "D7445426-4FF1-4F46-9042-63795D8442CF";
    public const string DiamondMindProficencyGuid = "1043B53B-DF44-445E-AD21-C758B6C72A19";
    public const string IronHeartProficencyGuid = "68E36D4E-022A-47F6-A4B5-BC849C71AC5D";
    public const string ShadowHandProficencyGuid = "396580B7-B8EF-4E93-AC63-74895581E667";
    public const string StoneDragonProficencyGuid = "A5B95365-F5A4-4376-8213-A39358B37012";
    public const string TigerClawProficencyGuid = "16C57F8C-FD74-4A6F-AB02-AF0BD833B38E";
    public const string WhiteRavenProficencyGuid = "AC18A78E-77C9-46AE-9872-B253CAA25A78";
    public const string RivenHourglassProficencyGuid = "10BF38C3-75A8-4D09-8E96-20C1357DFE40";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(DisciplineProficencies)}");

      FeatureConfigurator.New("DesertWindProficency", DesertWindProficencyGuid)
        .SetHideInUI()
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("DiamondMindProficency", DiamondMindProficencyGuid)
        .SetHideInUI()
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("IronHeartProficency", IronHeartProficencyGuid)
        .SetHideInUI()
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("ShadowHandProficency", ShadowHandProficencyGuid)
        .SetHideInUI()
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("StoneDragonProficency", StoneDragonProficencyGuid)
        .SetHideInUI()
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("TigerClawProficency", TigerClawProficencyGuid)
        .SetHideInUI()
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("WhiteRavenProficency", WhiteRavenProficencyGuid)
        .SetHideInUI()
        .SetIsClassFeature()
        .Configure();
      FeatureConfigurator.New("RivenHourglassProficency", RivenHourglassProficencyGuid)
          .SetHideInUI()
          .SetIsClassFeature()
          .Configure();

      Main.Logger.Info($"Configured {nameof(DisciplineProficencies)}");
    }
  }
}