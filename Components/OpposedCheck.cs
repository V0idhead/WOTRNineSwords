using BlueprintCore.Utils;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VoidHeadWOTRNineSwords.DiamondMind.SapphireNightmareBlade;

namespace VoidHeadWOTRNineSwords.Components
{
  public class OpposedCheck : ContextAction
  {
    public OpposedCheck() { }
    public OpposedCheck(Func<UnitEntityData, int> casterValue, Func<UnitEntityData, int> targetValue, ActionList onSuccess = null, ActionList onFail = null, int dice = 20)
    {
      CasterValue = casterValue;
      TargetValue = targetValue;
      OnSuccess = OnSuccess ?? Constants.Empty.Actions;
      OnFail = onFail ?? Constants.Empty.Actions;
      Dice = dice;
    }

    Random r = new Random();

    public ActionList OnSuccess;
    public ActionList OnFail;
    public Func<UnitEntityData, int> CasterValue;
    public Func<UnitEntityData, int> TargetValue;
    public int Dice;

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

        int rollVal = r.Next(1, Dice); //TODO: replace with Pathfinder-Roll?
        if (rollVal + CasterValue(caster) >= TargetValue(target))
          OnSuccess.Run();
        else
          OnFail.Run();
      }
      catch (Exception ex)
      {
        Main.Logger.Error($"{nameof(OpposedCheck)} failed: {ex.Message}");
      }
    }
  }
}