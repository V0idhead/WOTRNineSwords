﻿using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Counters
{
  internal class LightningRecoveryCounter : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
  {
    static bool active = false; //blocks trigger during the processing for the re-roll. I don't think we need a more complicated semaphore because AFAIK it's not possible for another creatures attack to interrupt our sequence of events

    public void OnEventAboutToTrigger(RuleAttackRoll evt)
    { }

    public void OnEventDidTrigger(RuleAttackRoll evt)
    {
      if (!active && (evt.Result != AttackResult.Hit || evt.Result != AttackResult.CriticalHit))
      {
        Blueprint<BlueprintAbilityResourceReference> maneuverResource = WarbladeC.ManeuverResourceGuid;
        if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 1)) //TODO: switch Resource implementation
        {
          active = true;
          Owner.Resources.Spend(maneuverResource.Reference, 1);
          var attack = new RuleAttackWithWeapon(Owner, evt.Target, evt.Weapon, -2);
          Context.TriggerRule<RuleAttackWithWeapon>(attack);
          active = false;
        }
      }
    }
  }
}