using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Warblade
{
  internal class BattleCunning
  {
    public const string Guid = "F8FC61BE-91B3-4101-ADB6-DCDA0C921711";
    const string name = "BattleCunning.Name";
    const string desc = "BattleCunning.Desc";

    public static BlueprintFeature Configure()
    {
      Main.Logger.Info($"Configuring {nameof(BattleCunning)}");

      BlueprintFeature battleCunning = FeatureConfigurator.New("BattleCunning", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIsClassFeature()
        .AddRecalculateOnStatChange(stat: Kingmaker.EntitySystem.Stats.StatType.Intelligence)
        //.AddFlankedAttackBonus() //only allows flat bonus
        .AddComponent(new IntelligenceFlankedAttackBonus { Descriptor = ModifierDescriptor.Insight})
        .Configure();

      return battleCunning;
    }
  }

  [ComponentName("Intelligence-based Bonus to attack against flanked opponents")]
  [AllowedOn(typeof(BlueprintUnitFact), false)]
  [AllowMultipleComponents]
  [TypeId("089B032C-7916-4E54-AC33-E0B4DCEA1D67")]
  public class IntelligenceFlankedAttackBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>, ISubscriber, IInitiatorRulebookSubscriber
  {
    public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;

    public void OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
    {
      bool isFlatFooted = Rulebook.Trigger(new RuleCheckTargetFlatFooted(evt.Initiator, evt.Target)).IsFlatFooted;
      if (evt.Target.CombatState.IsFlanked || isFlatFooted || evt.TargetIsFlanked)
      {
        evt.AddModifier(Math.Max(0, evt.Initiator.Stats.Intelligence.Bonus), base.Fact, Descriptor);
      }
    }

    public void OnEventDidTrigger(RuleCalculateAttackBonus evt)
    { }
  }
}