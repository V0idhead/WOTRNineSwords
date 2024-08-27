using Kingmaker.Designers;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace VoidHeadWOTRNineSwords.Components
{
  internal class MeleeAttackMultiplyDamage : ContextActionMeleeAttack
  {
    public int Multiplicator = 2;

    public override void RunAction()
    {
      try
      {
        base.RunAction();
        var attack = AbilityContext.RulebookContext?.LastEvent<RuleAttackWithWeapon>();
        if (attack is null)
        {
          Main.Logger.Warn("MeleeAttackExtended.RunAction: No attack triggered");
          return;
        }

        Main.Logger.Verbose($"MeleeAttackExtended.RunAction Result: {attack.AttackRoll.IsHit}");
        if (attack.AttackRoll.IsHit)
          GameHelper.DealDirectDamage(AbilityContext.Caster, AbilityContext.MainTarget.Unit, attack.MeleeDamage.DamageWithoutReduction * (Multiplicator-1));
      }
      catch (Exception e)
      {
        Main.Logger.Error("MeleeAttackExtended.RunAction", e);
      }
    }
  }
}