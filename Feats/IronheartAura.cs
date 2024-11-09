using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using System.Linq;

namespace VoidHeadWOTRNineSwords.Feats
{
  static class IronHeartAura //Iron Heart Focus
  {
    public const string IronHeartAuraGuid = "9D7D8480-3AFD-493D-913E-A751FD216072";
    public const string IronHeartFocusFactGuid = "96E34B27-F96B-42E6-B709-DC7D495AF3CA";

    private static BlueprintAbilityAreaEffect _ironHeartAuraArea;
    public static BlueprintAbilityAreaEffect IronHeartAuraArea
    {
      get
      {
        if (_ironHeartAuraArea == null)
        {
          var ironHeartAuraEffect = BuffConfigurator.New("IronHeartAuraEffect", "F3B1CFE7-C4FA-42BA-867F-13E392F47A4D")
            .SetDisplayName("IronHeartAura.Name")
            .SetDescription("IronHeartAuraEffect.Desc")
            .AddBuffAllSavesBonus(Kingmaker.Enums.ModifierDescriptor.Morale, 2)
            .Configure();

          _ironHeartAuraArea = AbilityAreaEffectConfigurator.New("IronHeartAuraArea", "C784F511-1490-44A1-9A9C-14318BDB5604")
            .AddAbilityAreaEffectBuff(ironHeartAuraEffect, false, ConditionsBuilder.New().CasterHasFact(IronHeartFocusFactGuid).IsAlly().IsCaster(negate: true)) //allies, but not self
            .SetShape(AreaEffectShape.Cylinder)
            .SetSize(new Feet(20))
            .Configure();
        }
        return _ironHeartAuraArea;
      }
    }

    public static void Configure()
    {
      var ironHeartFocusFact = UnitFactConfigurator.New("IronHeartFocusFactGuid", IronHeartFocusFactGuid)
        .Configure();

      var ironHeart = FeatureConfigurator.New("IronHeartAura", IronHeartAuraGuid)
        .SetDisplayName("IronHeartAura.Name")
        .SetDescription("IronHeartAura.Desc")
        .SetIcon(FeatureRefs.SpellFocusTransmutation.Reference.Get().Icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ironHeartFocusFact })
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.IronHeartGuids.ToList())
        .Configure();
    }
  }
}