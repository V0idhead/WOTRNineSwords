using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using System;
using System.Linq;
using VoidHeadWOTRNineSwords.IronHeart;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Counters
{
  internal class ManticoreParryCounter : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
  {
    public void OnEventAboutToTrigger(RuleAttackRoll evt)
    {
      if (evt.IsTargetFlatFooted || evt.RuleAttackWithWeapon.Weapon.Blueprint.IsNatural || evt.RuleAttackWithWeapon.Weapon.Blueprint.IsRanged)
        return;

      if (!Owner.HasFact(ManticoreParry.Fact))
      {
        Blueprint<BlueprintAbilityResourceReference> maneuverResource = WarbladeC.ManeuverResourceGuid;
        if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 2)) //TODO: switch Resource implementation
        {
          Blueprint<BlueprintBuffReference> manticoreParryBuff = ManticoreParry.ActiveBuffGuid;
          Owner.AddBuff(manticoreParryBuff.Reference, Owner, new TimeSpan(0, 0, 6));
          Owner.Resources.Spend(maneuverResource.Reference, 2);
        }
        else
        {
          return;
        }
      }
      evt.TryParry(Owner, Owner.Body.PrimaryHand.Weapon, 0);
    }

    public void OnEventDidTrigger(RuleAttackRoll evt)
    {
      if(evt.Result == AttackResult.Parried && Owner.HasFact(ManticoreParry.Fact))
      {
        var target = GameHelper.GetTargetsAround(evt.Initiator.Position, evt.Weapon.AttackRange).Where(unit => unit.IsEnemy(Owner)).FirstOrDefault();
        if (target != null)
        {
          var damage = new RuleDealDamage(evt.Initiator, target, evt.RuleAttackWithWeapon.CreateDamage(true));
          Context.TriggerRule(damage);
        }
      }
    }
  }
}