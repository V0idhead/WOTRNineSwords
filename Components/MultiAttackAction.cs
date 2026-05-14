using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items.Slots;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.Components
{
    class MultiAttackAction : ContextActionMeleeAttack
    {
        public int AttackCount = 1;
        private int hitCount = 0;
        public List<ActionList> HitEffects = new List<ActionList>(); //List of Actions to run based on number of hits: at least one hit HitEffects[0] is run; at least two hits HitEffects[1] is run; ...

        public override void RunAction()
        {
            try
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

                UnitEntityData target = SelectTarget(base.Context.MaybeCaster, threatHandMelee.Weapon.AttackRange.Meters, SelectNewTarget, base.Target?.Unit);
                if (target != null)
                {
                    int attackPenalty = 0;
                    for (int i = 0; i < AttackCount; ++i)
                    {
                        RunAttackRule(maybeCaster, target, threatHandMelee, attackPenalty);
                        var attack = AbilityContext.RulebookContext?.LastEvent<RuleAttackWithWeapon>();
                        if (attack != null)
                        {
                            if (attack.AttackRoll.IsHit)
                                hitCount++;
                            if (attack.Target.HPLeft < 1)
                                return;
                        }
                        attackPenalty += 5;
                    }
                    if (target.HPLeft < 1)
                        return;

                    for(int i = 0; i < hitCount; ++i)
                    {
                        if (HitEffects[i] != null)
                            HitEffects[i].Run();
                    }
                }
            }
            catch { }
        }
    }
}