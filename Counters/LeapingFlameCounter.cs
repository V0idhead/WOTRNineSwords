using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Utility;
using System;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.DesertWind;

namespace VoidHeadWOTRNineSwords.Counters
{
    internal class LeapingFlameCounter : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
    {
        static readonly Feet maxRange = new Feet(100);
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
        }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (Owner.HasFact(LeapingFlame.OnFact))
            {
                try
                {
                    if (Owner.HasFact(LeapingFlame.ActiveFact)) //only trigger once per turn
                        return;
                    if (evt.Initiator.DistanceTo(Owner) > maxRange.Meters)
                        return;

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