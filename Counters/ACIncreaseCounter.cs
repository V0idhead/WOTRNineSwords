using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.Counters
{
    internal class ACIncreaseCounter : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        { }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            Blueprint<BlueprintAbilityResourceReference> maneuverResource = ManeuverResources.ManeuverResourceGuid;
            if (!Owner.Resources.HasEnoughResource(maneuverResource.Reference, 1))
                return;

            Helpers.WriteCombatLogMessage("--Zephyr Dance--", GameLogStrings.Instance.DefaultColor, Owner);
            int atkRoll = evt.Roll;
            int bonus = evt.TotalBonusValue;
            Helpers.WriteCombatLogMessage("atk Roll: " + atkRoll + " + " + bonus + "; AC: " + evt.TargetAC, GameLogStrings.Instance.DefaultColor, Owner);

            if (evt.IsHit && atkRoll < evt.TargetAC + 4)
            {
                Helpers.WriteCombatLogMessage("triggering", GameLogStrings.Instance.DefaultColor, Owner);
                evt.AutoMiss = true;
                Owner.Resources.Spend(maneuverResource.Reference, 1);
            }
            else
                Helpers.WriteCombatLogMessage("not triggering", GameLogStrings.Instance.DefaultColor, Owner);
        }
    }
}