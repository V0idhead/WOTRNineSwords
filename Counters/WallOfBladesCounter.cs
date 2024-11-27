using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using VoidHeadWOTRNineSwords.IronHeart;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Counters
{
  internal class WallOfBladesCounter : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
  {
    public void OnEventAboutToTrigger(RuleAttackRoll evt)
    {
      Main.Log("WallOfBlades: check Flat Footed");
      if (evt.IsTargetFlatFooted)
        return;

      Main.Log("WallOfBlades: check is already active");
      if (!Owner.HasFact(WallOfBlades.Fact))
      {
        Main.Log("WallOfBlades: check resource");
        Blueprint<BlueprintAbilityResourceReference> maneuverResource = WarbladeC.ManeuverResourceGuid;
        if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 2)) //TODO: switch Resource implementation
        {
          Blueprint<BlueprintBuffReference> wallOfBladesBuff = WallOfBlades.ActiveBuffGuid;
          Owner.AddBuff(wallOfBladesBuff.Reference, Owner, new TimeSpan(0, 0, 6));
          Owner.Resources.Spend(maneuverResource.Reference, 2);
        }
        else
        {
          return;
        }
      }
      Main.Log("WallOfBlades: try parry");
      evt.TryParry(Owner, Owner.Body.PrimaryHand.Weapon, 0);
    }

    public void OnEventDidTrigger(RuleAttackRoll evt)
    {
      /*Main.Log("WallOfBlades: check Flat Footed");
      if (evt.IsTargetFlatFooted)
        return;

      Main.Log("WallOfBlades: check is already active");
      if (!Owner.HasFact(WallOfBlades.Fact))
      {
        Main.Log("WallOfBlades: check resource");
        Blueprint<BlueprintAbilityResourceReference> maneuverResource = WarbladeC.ManeuverResourceGuid;
        if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 1)) //TODO: switch Resource implementation
        {
          Blueprint<BlueprintBuffReference> wallOfBladesBuff = WallOfBlades.ActiveBuffGuid;
          Owner.AddBuff(wallOfBladesBuff.Reference, Owner, new TimeSpan(0, 0, 6));
          Owner.Resources.Spend(maneuverResource.Reference, 1);
        }
        else
        {
          return;
        }
      }

      int target = evt.AttackBonusRule.Result + evt.D20.Result;
      Main.Log($"WallOfBlades: target: {target} = {evt.AttackBonusRule.Result} + {evt.D20.Result}");

      var bonus = new RuleCalculateAttackBonusWithoutTarget(Owner, Owner.Body.PrimaryHand.Weapon, 0);
      Context.TriggerRule(bonus);
      var roll = new RuleRollD20(Owner);
      roll.Roll();

      int attempt = bonus.TotalBonusValue + roll.Result;
      Main.Log($"WallOfBlades: attempt: {attempt} = {bonus.TotalBonusValue} + {roll}");

      if (attempt > target)
        evt.Result = AttackResult.Parried;*/
    }
  }
}