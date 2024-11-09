using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using VoidHeadWOTRNineSwords.WhiteRaven;

namespace VoidHeadWOTRNineSwords.Feats
{
  static class DisciplineFoci
  {
    public const string TigerClawFocusGuid = "69DC31E3-78F9-4FBA-AB65-8850D15B00CA";
    public const string WhiteRavenFocusGuid = "5E307CEC-E03E-4104-B1E6-61C021B81833";
    /*
    public static void Configure()
    {
      Main.Logger.Info($"Configuring Discipline Focus");

      var diamondMindFocusFact = UnitFactConfigurator.New("DiamondMindFocusFact", diamondMindFocusGuid)
        .Configure();
      var diamondMindFocus = FeatureConfigurator.New("DiamondMindFocus", "6E37F470-72F6-4A52-8278-2553C17B7DBE")
        .SetDisplayName("DiamondMindFocus.Name")
        .SetDescription("DiamondMindFocus.Desc")
        .SetIcon(FeatureRefs.SpellFocusAbjuration.Reference.Get().Icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { diamondMindFocusFact })
        .Configure();

      var ironHeartFocusFact = UnitFactConfigurator.New("IronHeartFocusFact", ironHeartFocusGuid)
        .Configure();
      var ironHeartFocus = FeatureConfigurator.New("IronHeartFocus", "408EB54C-5A39-4E31-BE31-DB5EF3112B2C")
        .SetDisplayName("IronHeartFocus.Name")
        .SetDescription("IronHeartFocus.Desc")
        .SetIcon(FeatureRefs.SpellFocusAbjuration.Reference.Get().Icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ironHeartFocusFact })
        .Configure();

      FeatureSelectionConfigurator.New("DisciplineFocusSelection", "1DE906DE-C23B-4938-8D90-5495B0720DA7", Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
        .SetDisplayName("DisciplineFocus.Name")
        .SetDescription("DisciplineFocus.Desc")
        .SetIcon(FeatureRefs.SpellFocusAbjuration.Reference.Get().Icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .SetMode(SelectionMode.Default)
        .SetIsClassFeature()
        //.AddPrerequisiteFeature(ManeuverFocus.FeatGuid)
        .SetAllFeatures(
          diamondMindFocus.AssetGuid,
          ironHeartFocus.AssetGuid
        )
        .Configure();

      Main.Logger.Info($"Discipline Focus configured");
    }*/
  }
}