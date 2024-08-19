using BlueprintCore.Utils;
using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Components
{
  internal class MeleeAttackTargetsAround : ContextAction
  {
    public int? TargetLimit;
    public Feet Range;

    public override string GetCaption()
    {
      return "Attack targets around";
    }

    public override void RunAction()
    {
      int limit = TargetLimit ?? 99; //even though the game creates a new instance of this class, somehow the TargetLimit is cached, causing the Steel Wind ability to do nothing after using it the first time; capturing this locally fixes the problem
      var caster = Context.MaybeCaster;
      var targets = GameHelper.GetTargetsAround(caster.Position, Range).Where(unit => unit.IsEnemy(caster));

      foreach (var target in targets)
      {
        if(limit > 0)
        {
          var attack = new RuleAttackWithWeapon(caster, target, caster.GetFirstWeapon(), 0);
          Context.TriggerRule(attack);

          limit--;
        }
      }
    }
  }
}