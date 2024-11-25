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
    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public void OnEventAboutToTrigger(RuleAttackRoll evt)
    {
      #if DEBUG
        log.Info("WallOfBlades: check Flat Footed");
      #endif
      if (evt.IsTargetFlatFooted)
        return;

      #if DEBUG
      log.Info("WallOfBlades: check is already active");
      #endif
      if (!Owner.HasFact(WallOfBlades.Fact))
      {

        #if DEBUG
        log.Info("WallOfBlades: check resource");
        #endif
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
      #if DEBUG
      log.Info("WallOfBlades: try parry");
      #endif
      evt.TryParry(Owner, Owner.Body.PrimaryHand.Weapon, 0);
    }

    public void OnEventDidTrigger(RuleAttackRoll evt)
    {
      /*log.Info("WallOfBlades: check Flat Footed");
      if (evt.IsTargetFlatFooted)
        return;

      log.Info("WallOfBlades: check is already active");
      if (!Owner.HasFact(WallOfBlades.Fact))
      {
        log.Info("WallOfBlades: check resource");
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
      log.Info($"WallOfBlades: target: {target} = {evt.AttackBonusRule.Result} + {evt.D20.Result}");

      var bonus = new RuleCalculateAttackBonusWithoutTarget(Owner, Owner.Body.PrimaryHand.Weapon, 0);
      Context.TriggerRule(bonus);
      var roll = new RuleRollD20(Owner);
      roll.Roll();

      int attempt = bonus.TotalBonusValue + roll.Result;
      log.Info($"WallOfBlades: attempt: {attempt} = {bonus.TotalBonusValue} + {roll}");

      if (attempt > target)
        evt.Result = AttackResult.Parried;*/
    }
  }
}