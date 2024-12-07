using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.DiamondMind;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Counters
{
  internal class RapidCounterCounter : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IInitiatorRulebookSubscriber
  {
    public void OnEventAboutToTrigger(RuleAttackRoll evt)
    { }

    public void OnEventDidTrigger(RuleAttackRoll evt)
    {
      if (evt.RuleAttackWithWeapon?.IsAttackOfOpportunity == true && Owner.CombatState.AttackOfOpportunityCount == 0)
      {
        Blueprint<BlueprintAbilityResourceReference> maneuverResource = WarbladeC.ManeuverResourceGuid; //TODO: switch Resource implementation
        if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 1) || Owner.HasFact(RapidCounter.Fact))
        {
          if (!Owner.HasFact(RapidCounter.Fact))
          {
            Owner.Resources.Spend(maneuverResource.Reference, 1);
            Helpers.WriteCombatLogMessage("RapidCounter.LogMsg", GameLogStrings.Instance.DefaultColor, Owner);
          }
          Blueprint<BlueprintBuffReference> rapidCounterBuff = RapidCounter.ActiveBuffGuid;
          Owner.AddBuff(rapidCounterBuff.Reference, Owner, new TimeSpan(0, 0, 6));
          Owner.CombatState.AttackOfOpportunityCount++;
          Owner.CombatState.Cooldown.AttackOfOpportunity = 0;
        }
      }
    }
  }
}