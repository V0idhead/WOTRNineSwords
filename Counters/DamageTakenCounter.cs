using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.DesertWind;
using VoidHeadWOTRNineSwords.RivenHourglass;

namespace VoidHeadWOTRNineSwords.Counters
{
    internal class DamageTakenCounter : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        { }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (evt.IsHit)
            {
                if (Owner.HasFact(PainEcho.OnFact))
                {
                    try
                    {
                        if (Owner.HasFact(PainEcho.ActiveFact)) //only trigger once per turn
                            return;

                        Blueprint<BlueprintAbilityResourceReference> maneuverResource = ManeuverResources.ManeuverResourceGuid;
                        if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 1))
                        {
                            Blueprint<BlueprintBuffReference> activeBuff = PainEcho.ActiveBuffGuid;
                            Owner.AddBuff(activeBuff.Reference, Owner, new TimeSpan(0, 0, 6)); //mark ourselves as already triggered

                            Blueprint<BlueprintBuffReference> imageBuff = PainEcho.ImageBuffGuid;
                            Owner.AddBuff(imageBuff.Reference, Owner, new TimeSpan(0, 0, 12));

                            Owner.Resources.Spend(maneuverResource.Reference, 1);
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
}