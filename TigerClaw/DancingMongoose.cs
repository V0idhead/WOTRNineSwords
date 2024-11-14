using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.Warblade;
using VoidHeadWOTRNineSwords.WhiteRaven;

namespace VoidHeadWOTRNineSwords.TigerClaw
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/dancing-mongoose--3733/
  static class DancingMongoose
  {
    public const string Guid = "378D0D7C-C012-41D3-9079-DEC7BA8A47AF";
    const string name = "DancingMongoose.Name";
    const string desc = "DancingMongoose.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(DancingMongoose)}");

      UnityEngine.Sprite icon = AbilityRefs.BlessWeapon.Reference.Get().Icon;

      var oneBuff = BuffConfigurator.New("DancingMongooseOneBuff", "486042ED-F130-4EAF-A0CC-A34C290EDF8B")
        //.SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddBuffExtraAttack(number: 1, penalized: false)
        .Configure();

      var twoBuff = BuffConfigurator.New("DancingMongooseTwoBuff", "B61FB3B2-B58D-4FC3-93C2-A2AFA1D53603")
        //.SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddBuffExtraAttack(number: 2, penalized: false)
        .Configure();

      var ability = AbilityConfigurator.New(name, "F6AC9C57-DABD-4E44-AF58-E7DF16475483")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
        .SetCanTargetEnemies(false)
        .SetCanTargetFriends(false)
        .SetCanTargetSelf()
        .SetRange(AbilityRange.Personal)
        .SetActionType(UnitCommand.CommandType.Swift)
        .SetShouldTurnToTarget()
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction
        (
          ActionsBuilder.New().ApplyBuff(TigerBlooded.TigerBloodedBuff, ContextDuration.Fixed(1), toCaster: true).Conditional(ConditionsBuilder.New().CasterWeaponInTwoHands(), //TODO: true for Two-Handed-Weapons and even wielding a single One-Handed-Weapons in two hands. There doesn't seem to be a way to check if there are two weapons equiped
            ifTrue: ActionsBuilder.New().ApplyBuff(twoBuff, ContextDuration.Fixed(1), toCaster: true),
            ifFalse: ActionsBuilder.New().ApplyBuff(oneBuff, ContextDuration.Fixed(1), toCaster: true)
          )
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("DancingMongoose", Guid, AllManeuversAndStances.featureGroup)
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