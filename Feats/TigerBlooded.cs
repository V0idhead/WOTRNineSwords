using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Epic.OnlineServices;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Feats
{
  static class TigerBlooded
  {
    public const string TigerBloodedGuid = "8BDE220F-1A0B-496E-9FF4-98C1EFCC3821";
    public const string TigerClawFocusFactGuid = "69DC31E3-78F9-4FBA-AB65-8850D15B00CA";

    private static BlueprintBuff _tigerBloodedBuff;
    private static BlueprintBuff TigerBloodedBuff
    {
      get
      {
        if (_tigerBloodedBuff == null)
        {
          _tigerBloodedBuff = BuffConfigurator.New("TigerBloodedBuff", "2F478E0B-1111-4658-93E3-92420A1E3889")
          //.SetFlags(BlueprintBuff.Flags.HiddenInUi)
          .SetIcon(AbilityRefs.BeastShapeIII.Reference.Get().Icon)
          .SetDisplayName("TigerBlooded.Name")
          .SetDescription("TigerBlooded.Desc")
          //.AddAttackBonusConditional(ContextValues.Constant(53), false, ConditionsBuilder.New().HasFact(TigerClawFocusFactGuid), ModifierDescriptor.UntypedStackable)
          .AddAttackBonus(3)
          .Configure();
        }
        return _tigerBloodedBuff;
      }
    }

    public static void Configure()
    {
      var tigerClawFocusFact = UnitFactConfigurator.New("TigerClawFocusFact", TigerClawFocusFactGuid)
        .Configure();
      FeatureConfigurator.New("TigerBlooded", TigerBloodedGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
        .SetDisplayName("TigerBlooded.Name")
        .SetDescription("TigerBlooded.Desc")
        .SetIcon(FeatureRefs.SpellFocusTransmutation.Reference.Get().Icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { tigerClawFocusFact })
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.TigerClawGuids.ToList())
        .Configure();
    }

    public static ActionList GetEffectAction()
    {
      return ActionsBuilder.New().Conditional(ConditionsBuilder.New().CasterHasFact(TigerBlooded.TigerClawFocusFactGuid), ActionsBuilder.New().ApplyBuff(TigerBloodedBuff, ContextDuration.Fixed(1), toCaster: true)).Build();
    }
  }
}