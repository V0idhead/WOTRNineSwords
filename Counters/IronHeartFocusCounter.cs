using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.IronHeart;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Counters
{
  internal class IronHeartFocusCounter : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
  {
    static bool active = false; //blocks trigger during our processing. I don't think we need a more complicated semaphore because there should be no incoming saving throw during our sequence of events

    public void OnEventAboutToTrigger(RuleSavingThrow evt)
    {
      if (!active && !evt.AutoPass) //skip if this save was caused by ourselves or it will auto-pass anyway
      {
        Blueprint<BlueprintAbilityResourceReference> maneuverResource = WarbladeC.ManeuverResourceGuid; //TODO: switch Resource implementation
        if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 2) || Owner.HasFact(IronHeartFocus.Fact)) //do we have enough resource or is our buff already active?
        {
          active = true;
          RuleSavingThrow first = new RuleSavingThrow(evt.Initiator, evt); //copy the saving throw
          Context.TriggerRule(first);
          if (first.Success)
            evt.AutoPass = true; //on success auto-pass the original save, otherwise the original save acts as our second try
          else if (!Owner.HasFact(IronHeartFocus.Fact)) //is our buff already active?
          {
            Blueprint<BlueprintBuffReference> ironHeartFocusBuff = IronHeartFocus.ActiveBuffGuid;
            Owner.AddBuff(ironHeartFocusBuff.Reference, Owner, new TimeSpan(0, 0, 6)); //give ourselves the one round buff
            Helpers.WriteCombatLogMessage("IronHeartFocus.LogMsg", GameLogStrings.Instance.DefaultColor, Owner);
            Owner.Resources.Spend(maneuverResource.Reference, 2); //and spend our resource
          }
          active = false;
        }
      }
    }

    public void OnEventDidTrigger(RuleSavingThrow evt)
    { }
  }
}