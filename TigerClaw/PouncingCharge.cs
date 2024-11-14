using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Warblade;
using VoidHeadWOTRNineSwords.WhiteRaven;
using BlueprintCore.Actions.Builder.ContextEx;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.TigerClaw
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/pouncing-charge--3742/
  static class PouncingCharge
  {
    public const string Guid = "AEFE94E9-A497-44C6-9864-65ADA5B55446";
    const string name = "PouncingCharge.Name";
    const string desc = "PouncingCharge.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.ArmyChargeAbility.Reference.Get().Icon;

      Main.Logger.Info($"Configuring {nameof(PouncingCharge)}");

      var buff = BuffConfigurator.New("PouncingChargeBuff", "3D27D176-E98B-4DCE-8976-2C5ABAA3E481")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        //.AddInitiatorAttackRollTrigger(ActionsBuilder.New().MeleeAttack(fullAttack: true))
        .AddMechanicsFeature(Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature.MechanicsFeatureType.Pounce)
        .Configure();

      var chargeBuff = BuffConfigurator.New("PouncingChargeChargeBuff", "BE1EEB3A-3D68-4403-B345-C8DC05C66E68")
        .SetDisplayName(name)
        .SetDescription(desc)
        .AddBuffExtraEffects(BuffRefs.ChargeBuff.Reference.Guid, extraEffectBuff: buff)
        .Configure();

      var ability = AbilityConfigurator.New("PouncingChargeAbility", "A0542B19-E1FE-4271-80C6-0D34FB2FB3D4")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetCanTargetEnemies(false)
        .SetCanTargetFriends(false)
        .SetCanTargetSelf()
        .SetRange(AbilityRange.Personal)
        .SetActionType(UnitCommand.CommandType.Free)
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(TigerBlooded.TigerBloodedBuff, ContextDuration.Fixed(1), toCaster: true).ApplyBuff(chargeBuff, ContextDuration.Fixed(1), toCaster: true))
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("PouncingCharge", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.TigerClawGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}