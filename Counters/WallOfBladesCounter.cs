using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Actions;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.IronHeart;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Counters
{
  internal class WallOfBladesCounter : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
  {
    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public void OnEventAboutToTrigger(RuleAttackRoll evt)
    {
      log.Info("WallOfBlades AboutTo");

      if (!Owner.HasFact(WallOfBlades.Fact))
      {
        log.Info("WallOfBlades fact not found");
        Blueprint<BlueprintAbilityResourceReference> maneuverResource = WarbladeC.ManeuverResourceGuid;
        if (Owner.Resources.HasEnoughResource(maneuverResource.Reference, 1)) //TODO: switch Resource implementation
        {
          Blueprint<BlueprintBuffReference> wallOfBladesBuff = WallOfBlades.ActiveBuffGuid;
          Owner.AddBuff(wallOfBladesBuff.Reference, Owner, new TimeSpan(0, 0, 6));
          Owner.Resources.Spend(maneuverResource.Reference, 1);
          log.Info("WallOfBlades resource spent");
        }
        else
        {
          log.Info("WallOfBlades resource ran out");
          return;
        }
      }
      log.Info("Parrying");
      evt.TryParry(Owner, Owner.Body.PrimaryHand.Weapon, 0);
    }

    public void OnEventDidTrigger(RuleAttackRoll evt)
    { }
  }
}