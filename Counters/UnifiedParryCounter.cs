using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.Designers;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using System;
using System.Linq;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.IronHeart;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Counters
{
    internal class UnifiedParryCounter : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
  {
    private enum Mode
    {
      WallOfBlades,
      ManticoreParry
    }

    Mode? mode;
    public void OnEventAboutToTrigger(RuleAttackRoll evt)
    {
      try
      {
        if (evt.IsTargetFlatFooted)
          return;

        Blueprint<BlueprintAbilityResourceReference> maneuverResource = WarbladeC.ManeuverResourceGuid;

        if (!evt.RuleAttackWithWeapon.Weapon.Blueprint.IsNatural && !evt.RuleAttackWithWeapon.Weapon.Blueprint.IsRanged && Owner.HasFact(ManticoreParry.OnFact))
        {
          //we are eligible for ManticoreParry, check if we can afford it
          if (Owner.HasFact(Common.Common.ParryActiveFact))
            mode = Mode.ManticoreParry;
          else if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 2))
          {
            Blueprint<BlueprintBuffReference> parryBuff = Common.Common.ParryActiveBuffGuid;
            Owner.AddBuff(parryBuff.Reference, Owner, new TimeSpan(0, 0, 6));
            Owner.Resources.Spend(maneuverResource.Reference, 2);
            Helpers.WriteCombatLogMessage("ManticoreParry.LogMsg", GameLogStrings.Instance.DefaultColor, Owner);
            mode = Mode.ManticoreParry;
          }
        }

        if (mode == null && Owner.HasFact(WallOfBlades.OnFact))
        {
          //if ManticoreParry hasn't activated try Wall Of Blades
          if (Owner.HasFact(Common.Common.ParryActiveFact))
            mode = Mode.WallOfBlades;
          else if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 2))
          {
            Blueprint<BlueprintBuffReference> parryBuff = Common.Common.ParryActiveBuffGuid;
            Owner.AddBuff(parryBuff.Reference, Owner, new TimeSpan(0, 0, 6));
            Owner.Resources.Spend(maneuverResource.Reference, 2);
            Helpers.WriteCombatLogMessage("WallOfBlades.LogMsg", GameLogStrings.Instance.DefaultColor, Owner);
            mode = Mode.WallOfBlades;
          }
        }

        //attempt parry if eny parry ability was activated
        if (mode != null)
        {
          evt.TryParry(Owner, Owner.Body.PrimaryHand.Weapon, 0);
        }
      }
      catch (Exception e)
      {
        Main.Log(e.Message);
      }
    }

    public void OnEventDidTrigger(RuleAttackRoll evt)
    {
      try
      {
        //deal damage if ManticoreParry was triggered
        if (mode == Mode.ManticoreParry && evt.Result == AttackResult.Parried)
        {

          var target = GameHelper.GetTargetsAround(evt.Initiator.Position, evt.Weapon.AttackRange*2).Where(unit => unit.IsEnemy(Owner) && unit != evt.Initiator).FirstOrDefault(); //double range to improve chances to actually hit things
          /*var allTargets = GameHelper.GetTargetsAround(evt.Initiator.Position, evt.Weapon.AttackRange*2);
          Main.Log($"UnifiedParryCounter: {allTargets.Count()} targets in {evt.Weapon.AttackRange} range");
          var enemyTargets = allTargets.Where(unit => unit.IsEnemy(Owner));
          Main.Log($"UnifiedParryCounter: {enemyTargets.Count()} enemy targets");
          var targets = enemyTargets.Where(unit => unit != evt.Initiator);
          Main.Log($"UnifiedParryCounter: {targets.Count()} that are not attacker");
          var target = targets.FirstOrDefault();
          Main.Log($"UnifiedParryCounter: selected one target: {target.CharacterName}");*/
          if (target != null)
          {
            var damage = new RuleDealDamage(evt.Initiator, target, evt.RuleAttackWithWeapon.CreateDamage(true));
            Context.TriggerRule(damage);
          }
        }

        mode = null; //reset state
      }
      catch (Exception e)
      {
        Main.Log(e.Message);
      }
    }
  }
}