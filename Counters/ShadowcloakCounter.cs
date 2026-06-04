using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.DesertWind;
using VoidHeadWOTRNineSwords.ShadowHand;

namespace VoidHeadWOTRNineSwords.Counters
{
    internal class ShadowcloakCounter : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (Owner.HasFact(Shadowcloak.OnFact))
            {
                if (Owner.HasFact(Shadowcloak.Active2Fact)) //no further buffs
                    return;

                Blueprint<BlueprintAbilityResourceReference> maneuverResource = ManeuverResources.ManeuverResourceGuid;
                if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 1))
                {
                    if (Owner.HasFact(Shadowcloak.Active1Fact)) //upgrade blur --> displacement
                    {
                        Owner.AddBuff(BuffRefs.DisplacementBuff.Reference, Owner, new TimeSpan(0, 0, 6));
                        Blueprint<BlueprintBuffReference> activeBuff = Shadowcloak.Active2BuffGuid;
                        Owner.AddBuff(activeBuff.Reference, Owner, new TimeSpan(0, 0, 6));
                    }
                    else //give blur
                    {
                        Owner.AddBuff(BuffRefs.BlurBuff.Reference, Owner, new TimeSpan(0, 0, 6));
                        Blueprint<BlueprintBuffReference> activeBuff = Shadowcloak.Active1BuffGuid;
                        Owner.AddBuff(activeBuff.Reference, Owner, new TimeSpan(0, 0, 6));
                    }
                    Owner.Resources.Spend(maneuverResource.Reference, 1);
                }
            }
        }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        { }
    }
}