using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Utility;
using System;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.DesertWind;

namespace VoidHeadWOTRNineSwords.Counters
{
    internal class LeapingFlameCounter : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
    {
        static readonly Feet maxRange = new Feet(100);
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            Helpers.WriteCombatLogMessage("Leaping Flame debug", GameLogStrings.Instance.DefaultColor, Owner);
        }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            Helpers.WriteCombatLogMessage("Leaping Flame Trigger", GameLogStrings.Instance.DefaultColor, Owner);
            if (Owner.HasFact(FireRiposte.OnFact))
            {
                try
                {
                    Helpers.WriteCombatLogMessage("Leaping Flame already triggered?", GameLogStrings.Instance.DefaultColor, Owner);
                    if (Owner.HasFact(LeapingFlame.ActiveFact)) //only trigger once per turn
                        return;
                    Helpers.WriteCombatLogMessage("Leaping Flame max range?", GameLogStrings.Instance.DefaultColor, Owner);
                    if (evt.Initiator.DistanceTo(Owner) > maxRange.Meters)
                        return;

                    Helpers.WriteCombatLogMessage("Leaping Flame resource?", GameLogStrings.Instance.DefaultColor, Owner);
                    Blueprint<BlueprintAbilityResourceReference> maneuverResource = ManeuverResources.ManeuverResourceGuid;
                    if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 1))
                    {
                        AbilityData spell = new AbilityData(AbilityRefs.MantisZealotTeleportToEnemyAbility.Reference, Owner);
                        RuleCastSpell jump = new RuleCastSpell(spell, evt.Initiator);
                        Rulebook.Trigger(jump);

                        Blueprint<BlueprintBuffReference> activeBuff = LeapingFlame.ActiveBuffGuid;
                        Owner.AddBuff(activeBuff.Reference, Owner, new TimeSpan(0, 0, 6)); //mark ourselves as already jumped

                        RuleAttackRoll counterAttack = new RuleAttackRoll(Owner, evt.Initiator, Owner.GetThreatHandMelee().Weapon, 0);
                        Context.TriggerRule<RuleAttackRoll>(counterAttack);
                    }
                }
                catch (Exception e)
                {
                    Main.Log(e.Message);
                }
            }
        }
    }
}