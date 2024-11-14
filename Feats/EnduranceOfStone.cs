using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Feats
{
  static class EnduranceOfStone //Stone Dragon Focus
  {
    public const string EnduranceOfStoneGuid = "FEEB4C40-749A-4AB7-A33A-FD42CBC257DB";
    public const string StoneDragonFocusFactGuid = "29DCA772-20B4-4850-A9D1-E8A33B31BC2B";

    public static void Configure()
    {
      var stoneDragonFocusFact = UnitFactConfigurator.New("StoneDragonFocusFact", StoneDragonFocusFactGuid)
        .Configure();
      FeatureConfigurator.New("EnduranceOfStone", EnduranceOfStoneGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
        .SetDisplayName("EnduranceOfStone.Name")
        .SetDescription("EnduranceOfStone.Desc")
        .SetIcon(FeatureRefs.SpellFocusAbjuration.Reference.Get().Icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { stoneDragonFocusFact })
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.StoneDragonGuids.ToList())
        .Configure();
    }

    public static ActionList GetEffectAction()
    {
      return ActionsBuilder.New().Conditional(ConditionsBuilder.New().CasterHasFact(StoneDragonFocusFactGuid), ActionsBuilder.New().OnContextCaster(ActionsBuilder.New().HealTarget(new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = ContextValues.Constant(1) }))).Build();
    }
  }
}