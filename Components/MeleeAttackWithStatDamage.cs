using BlueprintCore.Utils;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;

namespace VoidHeadWOTRNineSwords.Components
{
  internal class MeleeAttackWithStatDamage : ContextActionMeleeAttack
  {
    public StatType statType;
    public DiceFormula damageAmount;
    public ActionList OnHit = Constants.Empty.Actions;

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
        {
          Context.TriggerRule<RuleDealStatDamage>(new(Context.MaybeCaster, Context.MainTarget.Unit, statType, damageAmount, 0));
          OnHit.Run();
        }
      }
      catch (Exception e)
      {
        Main.Logger.Error("MeleeAttackExtended.RunAction", e);
      }
    }
  }
}