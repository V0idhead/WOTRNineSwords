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
  //TODO: switch to IronHeartFocus implementation to work properly with maneuver. Current implementation just makes a new normal attack
  internal class LightningRecoveryCounter : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IInitiatorRulebookSubscriber
  {
    static bool active = false; //blocks trigger during the processing for the re-roll. I don't think we need a more complicated semaphore because AFAIK it's not possible for another creatures attack to interrupt our sequence of events (including two warblades in real-time mode)

    public void OnEventAboutToTrigger(RuleAttackRoll evt)
    {
      if (!active && !evt.AutoHit) //skip if this save was caused by ourselves or it will auto-pass anyway
      {
        Blueprint<BlueprintAbilityResourceReference> maneuverResource = WarbladeC.ManeuverResourceGuid; //TODO: switch Resource implementation
        if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 1)) //do we have enough resource?
        {
          active = true;

          RuleAttackRoll first = new RuleAttackRoll(evt.Initiator, evt.Target, evt.Weapon, evt.AttackBonusPenalty); //copy the saving throw
          Context.TriggerRule(first);
          if (first.IsHit)
          {
            evt.AutoHit = true; //on success auto-pass the original save, otherwise the original save acts as our second try
            if (first.IsCriticalRoll)
            {
              evt.AutoCriticalThreat = true;
              if(first.IsCriticalConfirmed)
                evt.IsCriticalConfirmed = true;
            }
          }
          else
          {
            Owner.AddBuff(LightningRecovery.AttackBuff, Owner, new TimeSpan(0,0,1));
            Owner.Resources.Spend(maneuverResource.Reference, 1); //and spend our resource
            Helpers.WriteCombatLogMessage("LightningRecovery.LogMsg", GameLogStrings.Instance.DefaultColor, Owner);
          }
          active = false;
        }
      }
    }

    public void OnEventDidTrigger(RuleAttackRoll evt)
    {
      Owner.SetBuffDuration(LightningRecovery.AttackBuff, 0);
    }
  }
}