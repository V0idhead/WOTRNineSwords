using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Kingmaker.EntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace VoidHeadWOTRNineSwords.Components
{
  internal class PushTargetAction : ContextAction
  {
    public Func<Feet> CalcDistance = () => new Feet(1);

    public override string GetCaption()
    {
      return "Pushes the target";
    }

    public override void RunAction()
    {
      try
      {
        var caster = Context.MaybeCaster;
        var target = Context.MainTarget.Unit;
        if (caster == null || target == null) return;

        Vector3 normalized2 = (target.Position - caster.Position).normalized;
        target.Ensure<UnitPartForceMove>().Push(normalized2, CalcDistance().Meters, false);
      }
      catch(Exception e)
      {
        Main.Logger.Error($"{nameof(PushTargetAction)} error: {e.Message}");
      }
    }
  }
}