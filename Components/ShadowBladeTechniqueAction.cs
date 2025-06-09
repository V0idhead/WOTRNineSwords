using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace VoidHeadWOTRNineSwords.Components
{
  internal class ShadowBladeTechniqueAction : ContextActionMeleeAttack
  {
    public override void RunAction()
    {
      RuleAttackRoll bonusAttackRoll = new RuleAttackRoll(base.Context.MaybeCaster, base.Context.MainTarget.Unit, base.Context.MaybeCaster.GetThreatHandMelee().Weapon, 0);
      Context.TriggerRule<RuleAttackRoll>(bonusAttackRoll);
      var bonusAttack = AbilityContext.RulebookContext?.LastEvent<RuleAttackRoll>();

      base.RunAction();
      var attack = AbilityContext.RulebookContext?.LastEvent<RuleAttackWithWeapon>();

      if (attack.AttackRoll.IsHit && bonusAttack.IsHit) //both hit
        Context.TriggerRule(new RuleDealDamage(attack.Initiator, attack.Target, new DirectDamage(new DiceFormula(1, DiceType.D6))));
      //Game.Instance.Rulebook.TriggerEvent<RuleDealDamage>(new RuleDealDamage(attack.Initiator, attack.Target, new DirectDamage(new DiceFormula(1, DiceType.D6))));
      else if (!attack.AttackRoll.IsHit && !bonusAttack.IsHit) //real attack didn't hit, but bonus one did, create the damage it would have dealt and apply
      {
        var damage = new RuleDealDamage(attack.Initiator, attack.Target, attack.CreateDamage(true));
        Context.TriggerRule(damage);
      }
    }
  }
}