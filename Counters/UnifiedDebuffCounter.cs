using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.DesertWind;
using VoidHeadWOTRNineSwords.Swordsage.Archetypes.RimeRavagerManeuvers;

namespace VoidHeadWOTRNineSwords.Counters
{
    internal class UnifiedDebuffCounter : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
    {
        static UnifiedDebuffCounter()
        {
            Singleton = new UnifiedDebuffCounter();
        }
        public static UnifiedDebuffCounter Singleton { get; private set; }

        private enum Mode
        {
            FrigidSkin
        }
        Mode? mode;
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        { }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            //Helpers.WriteCombatLogMessage("FireRiposte Trigger", GameLogStrings.Instance.DefaultColor, Owner);
            if (Owner.HasFact(FrigidSkin.OnFact) && evt.Initiator.IsPlayersEnemy)
            {
                //Helpers.WriteCombatLogMessage("FireRiposte: range Check; " + evt.Initiator.DistanceTo(Owner) + "|" + MeleeRange.Meters, GameLogStrings.Instance.DefaultColor, Owner);
                if (evt.IsHit && evt.Weapon.Blueprint.IsMelee)
                {
                    Blueprint<BlueprintAbilityResourceReference> maneuverResource = ManeuverResources.ManeuverResourceGuid;

                    //Helpers.WriteCombatLogMessage("FireRiposte: active? resource?", GameLogStrings.Instance.DefaultColor, Owner);
                    if (Owner.HasFact(FrigidSkin.ActiveFact))
                        mode = Mode.FrigidSkin;
                    else if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 2))
                    {
                        //Helpers.WriteCombatLogMessage("FireRiposte: activating!", GameLogStrings.Instance.DefaultColor, Owner);
                        Blueprint<BlueprintBuffReference> frigidSkinActiveBuff = FrigidSkin.ActiveBuffGuid;
                        Owner.AddBuff(frigidSkinActiveBuff.Reference, Owner, new TimeSpan(0, 0, 6));
                        Owner.Resources.Spend(maneuverResource.Reference, 2);
                        Helpers.WriteCombatLogMessage("FrigSkin.LogMsg", GameLogStrings.Instance.DefaultColor, Owner);
                        mode = Mode.FrigidSkin;
                    }

                    if (mode == Mode.FrigidSkin)
                    {
                        //Helpers.WriteCombatLogMessage("FireRiposte: making attack", GameLogStrings.Instance.DefaultColor, Owner);
                        RuleSavingThrow attackerSave = new RuleSavingThrow(evt.Initiator, Kingmaker.EntitySystem.Stats.SavingThrowType.Reflex, 14 + Owner.Stats.Wisdom.Bonus);
                        Context.TriggerRule(attackerSave);
                        if (!attackerSave.Success)
                        {
                            evt.Initiator.AddBuff(BuffRefs.DisarmMainHandBuff.Reference, Owner, new TimeSpan(0, 0, 6));
                            evt.Initiator.AddBuff(BuffRefs.DisarmOffHandBuff.Reference, Owner, new TimeSpan(0, 0, 6));
                        }
                    }
                }
            }
        }
    }
}