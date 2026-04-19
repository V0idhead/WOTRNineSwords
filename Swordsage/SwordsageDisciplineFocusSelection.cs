using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.Swordsage
{
    static class SwordsageDisciplineFocusSelection
    {
        public const string Guid = "261D30A7-85F1-457B-8AAA-8C25A4B36EF5";

        public static BlueprintFeatureSelection Configure()
        {
            BlueprintFeatureSelection swordsageManeuverSelection = FeatureSelectionConfigurator.New("SwordsageDisciplineFocusSelection", Guid)
              .SetDisplayName("SwordsageDisciplineFocusSelection.Name")
              .SetDescription("SwordsageDisciplineFocusSelection.Desc")
              .SetIsClassFeature()
              .SetMode(SelectionMode.OnlyNew)
              .SetAllFeatures(
                DesertWindDodge.DesertWindDodgeGuid,
                UnnervingCalm.UnnervingCalmGuid,
                ShadowPresence.ShadowPresenceGuid,
                EnduranceOfStone.EnduranceOfStoneGuid,
                TigerBlooded.TigerBloodedGuid
              ).Configure();

            return swordsageManeuverSelection;
        }
    }
}