using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Components
{
  public class NightmareBladeAction2 : ContextActionSavingThrow
  {
    public ActionList OnHigh = Constants.Empty.Actions;
    public ActionList OnLow = Constants.Empty.Actions;

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

        Game.Instance.Rulebook.TriggerEvent<RuleSkillCheck>(new RuleSkillCheck(caster, Kingmaker.EntitySystem.Stats.StatType.SkillPerception, target.Stats.AC));

        var skillCheck = AbilityContext.RulebookContext?.LastEvent<RuleSkillCheck>();
        if (skillCheck.Success)
          OnHigh.Run();
        else
          OnLow.Run();
      }
      catch (Exception ex)
      {
        Main.Logger.Error($"{nameof(NightmareBladeAction)} failed: {ex.Message}");
      }
    }
  }
}