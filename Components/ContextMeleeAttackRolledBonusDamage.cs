using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.ElementsSystem;
using BlueprintCore.Utils;

namespace VoidHeadWOTRNineSwords.Components
{
  public class ContextMeleeAttackRolledBonusDamage : ContextActionMeleeAttack //TODO: convert all instances of averaged damage to instances of a class like this
  {
    public DiceFormula ExtraDamage;
    internal ActionList OnHit = Constants.Empty.Actions;

    public override string GetCaption()
    {
      return "Melee Attack that deals extra damage based on dice rolls";
    }

    public override void RunAction()
    {
      try
      {
        //realistically I'd need to convert the DamageType of the main weapon to Physical Damage Form (see DoubleDamageDiceOnAttack for how this might work). But I'd rather err on the side of making this too powerful, so I'm using DirectDamage
        //Game.Instance.Rulebook.TriggerEvent<RuleDealDamage>(new RuleDealDamage(caster, target, new PhysicalDamage(new ModifiableDiceFormula(dmg), 0, caster.GetFirstWeapon().Blueprint.DamageType.Type.)));

        base.RunAction();
        var attack = AbilityContext.RulebookContext?.LastEvent<RuleAttackWithWeapon>();
        if (attack != null && attack.AttackRoll.IsHit)
        {
          Game.Instance.Rulebook.TriggerEvent<RuleDealDamage>(new RuleDealDamage(attack.Initiator, attack.Target, new DirectDamage(ExtraDamage)));
          OnHit?.Run();
        }
      }
      catch (Exception e)
      {
        Main.Logger.Error($"{nameof(ContextMeleeAttackRolledBonusDamage)} error: {e.Message}");
      }
    }
  }
}