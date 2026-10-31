using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Components
{
    internal class IslandInTime : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        { }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            Helpers.WriteCombatLogMessage("IslandInTime: triggered", GameLogStrings.Instance.DefaultColor, Owner);
            if (evt.IsAttackOfOpportunity)
            {
                Helpers.WriteCombatLogMessage("IslandInTime: is AOP", GameLogStrings.Instance.DefaultColor, Owner);
                if (evt.Target.HPLeft > 0)
                {
                    Helpers.WriteCombatLogMessage("IslandInTime: HPLeft1", GameLogStrings.Instance.DefaultColor, Owner);
                    RuleAttackWithWeapon atk1 = new RuleAttackWithWeapon(evt.Initiator, evt.Target, evt.Weapon, 5);
                    Context.TriggerRule(atk1);
                    if (evt.Target.HPLeft > 0)
                    {
                        Helpers.WriteCombatLogMessage("IslandInTime: HPLeft2", GameLogStrings.Instance.DefaultColor, Owner);
                        RuleAttackWithWeapon atk2 = new RuleAttackWithWeapon(evt.Initiator, evt.Target, evt.Weapon, 10);
                        Context.TriggerRule(atk2);
                    }
                }
            }
        }
    }
}