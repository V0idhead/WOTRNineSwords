using BlueprintCore.Utils;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Components
{
  public class NightmareBladeAction : ContextAction
  {
    Random r = new Random();

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

        int rollVal = r.Next(1, 20); //TODO: replace with Pathfinder-Roll?
        if (rollVal + caster.Stats.SkillPerception.ModifiedValue >= target.Stats.AC)
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
