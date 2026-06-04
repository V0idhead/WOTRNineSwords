using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.DesertWind;

namespace VoidHeadWOTRNineSwords.Counters
{
    internal class UnifiedCounterAttackCounter : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
    {
        static int accumulatedPenalty = 0;
        static readonly Feet MeleeRange = new Feet(10);

        static UnifiedCounterAttackCounter()
        {
            Singleton = new UnifiedCounterAttackCounter();
        }

        public static UnifiedCounterAttackCounter Singleton { get; private set; }

        private enum Mode
        {
            FireRiposte
        }

        Mode? mode;

        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        { }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            //Helpers.WriteCombatLogMessage("FireRiposte Trigger", GameLogStrings.Instance.DefaultColor, Owner);
            if (Owner.HasFact(FireRiposte.OnFact))
            {
                //Helpers.WriteCombatLogMessage("FireRiposte: range Check; " + evt.Initiator.DistanceTo(Owner) + "|" + MeleeRange.Meters, GameLogStrings.Instance.DefaultColor, Owner);
                if (evt.IsHit && evt.Initiator.DistanceTo(Owner) <= MeleeRange.Meters)
                {
                    Blueprint<BlueprintAbilityResourceReference> maneuverResource = ManeuverResources.ManeuverResourceGuid;

                    //Helpers.WriteCombatLogMessage("FireRiposte: active? resource?", GameLogStrings.Instance.DefaultColor, Owner);
                    if (Owner.HasFact(FireRiposte.ActiveFact))
                        mode = Mode.FireRiposte;
                    else if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 2))
                    {
                        //Helpers.WriteCombatLogMessage("FireRiposte: activating!", GameLogStrings.Instance.DefaultColor, Owner);
                        Blueprint<BlueprintBuffReference> counterAttackBuff = Common.Common.CounterAttackActiveFact;
                        Owner.AddBuff(counterAttackBuff.Reference, Owner, new TimeSpan(0, 0, 6));
                        Owner.Resources.Spend(maneuverResource.Reference, 2);
                        accumulatedPenalty = 0;
                        Helpers.WriteCombatLogMessage("FireRiposte.LogMsg", GameLogStrings.Instance.DefaultColor, Owner);
                        mode = Mode.FireRiposte;
                    }

                    if (mode == Mode.FireRiposte)
                    {
                        //Helpers.WriteCombatLogMessage("FireRiposte: making attack", GameLogStrings.Instance.DefaultColor, Owner);
                        accumulatedPenalty++;
                        RuleAttackRoll counterAttack = new RuleAttackRoll(Owner, evt.Initiator, Owner.GetThreatHandMelee().Weapon, -accumulatedPenalty);
                        //RuleAttackRoll counterAttack = new RuleAttackRoll(Owner, evt.Initiator, , -accumulatedPenalty);
                        counterAttack.ACRule = new RuleCalculateAC(Owner, evt.Initiator, Kingmaker.RuleSystem.AttackType.Touch);
                        Context.TriggerRule<RuleAttackRoll>(counterAttack);

                        if (counterAttack.Result == AttackResult.Hit || counterAttack.Result == AttackResult.CriticalHit)
                        {
                            RuleDealDamage counterAttackDamage = new RuleDealDamage(Owner, evt.Initiator, new DamageBundle(new EnergyDamage(new Kingmaker.RuleSystem.DiceFormula(4, Kingmaker.RuleSystem.DiceType.D6), Kingmaker.Enums.Damage.DamageEnergyType.Fire)));
                            Context.TriggerRule<RuleDealDamage>(counterAttackDamage);
                        }
                    }
                }
            }
        }
    }
}