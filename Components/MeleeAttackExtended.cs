using BlueprintCore.Utils;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;

namespace VoidHeadWOTRNineSwords.Components
{
  //taken from https://github.com/WittleWolfie/CharacterOptionsPlus
  internal class MeleeAttackExtended : ContextActionMeleeAttack
  {
    internal ActionList OnHit = Constants.Empty.Actions;

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
          OnHit.Run();
      }
      catch (Exception e)
      {
        Main.Logger.Error("MeleeAttackExtended.RunAction", e);
      }
    }
  }
}