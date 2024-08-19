using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Commands;
using Kingmaker;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Items.Slots;
using Kingmaker.UnitLogic;

namespace VoidHeadWOTRNineSwords.Components
{
  class MeleeAttackAvalanche : ContextActionMeleeAttack
  {
    public override void RunAction()
    {
      UnitEntityData maybeCaster = base.Context.MaybeCaster;
      if (maybeCaster == null)
      {
        PFLog.Default.Error("Caster is missing");
        return;
      }

      WeaponSlot threatHandMelee = maybeCaster.GetThreatHandMelee();
      if (threatHandMelee == null)
      {
        PFLog.Default.Error("Caster can't make melee attack");
        return;
      }

      UnitEntityData unitEntityData = SelectTarget(base.Context.MaybeCaster, threatHandMelee.Weapon.AttackRange.Meters, SelectNewTarget, base.Target?.Unit);
      if (unitEntityData != null)
      {
        int attackPenalty = 0;

        while (true)
        {
          RunAttackRule(maybeCaster, unitEntityData, threatHandMelee, attackPenalty);
          var attack = AbilityContext.RulebookContext?.LastEvent<RuleAttackWithWeapon>();
          if (attack == null || !attack.AttackRoll.IsHit)
            break;
          attackPenalty += 4;
        }
      }
    }
  }
}