using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Components
{
  class SavingThrowAgainstAttackRoll : ContextActionSavingThrow
  {
    public override void RunAction()
    {
      try
      {
        Random r = new Random();

        var caster = Context.MaybeCaster;


        if (caster != null)
        {
        int dc = r.Next(1, 20) + caster.Stats.AdditionalAttackBonus + caster.Stats.BaseAttackBonus;

          Main.Logger.Error("SavingThrowAgainstAttackRoll rolling");
          /*var roll = new RuleAttackRoll(caster, caster, caster.GetFirstWeapon(), 0);
          Context.TriggerRule<RuleAttackRoll>(roll);

          Main.Logger.Error("SavingThrowAgainstAttackRoll dc = " + roll.Roll);*/
          CustomDC = new Kingmaker.UnitLogic.Mechanics.ContextValue { Value = dc };
        }
        else
          CustomDC = new Kingmaker.UnitLogic.Mechanics.ContextValue { Value = 42 };
        HasCustomDC = true;

        base.RunAction();
      }
      catch (Exception ex)
      {
        Main.Logger.Error("SavingThrowAgainstAttackRoll error : " + ex.Message);
      }
    }
  }
}