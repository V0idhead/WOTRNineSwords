using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Components
{
  public class OpposedSkillCheck : ContextActionSkillCheck
  {
    public Func<UnitEntityData, int> TargetValue;

    public override string GetCaption()
    {
      return "Makes a skill check";
    }

    public override void RunAction()
    {
      try
      {
        var caster = Context.MaybeCaster;
        var target = Context.MainTarget.Unit;

        var check = Game.Instance.Rulebook.TriggerEvent<RuleSkillCheck>(new RuleSkillCheck(caster, Stat, TargetValue(target)));
        check.RollD20();

        if (check.Success)
          Success.Run();
        else
          Failure.Run();
      }
      catch (Exception ex)
      {
        Main.Logger.Error($"{nameof(OpposedSkillCheck)} failed: {ex.Message}");
      }
    }
  }
}