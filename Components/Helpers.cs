using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.Components
{
  static class Helpers
  {
    public static List<(ConditionsBuilder conditions, ContextValue modifier)> GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty property)
    {
      return new List<(ConditionsBuilder conditions, ContextValue modifier)>
      {
        (ConditionsBuilder.New().AddTrue(), new ContextValue { Property = property, ValueType = ContextValueType.CasterProperty }),
        (ConditionsBuilder.New().CasterHasFact(ManeuverFocus.Guid), new ContextValue{Value = 1}),
        (ConditionsBuilder.New().CasterHasFact(MythicManeuverFocus.Guid), new ContextValue{Property = Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.MythicLevel, ValueType = ContextValueType.CasterProperty})
      };
    }
  }
}