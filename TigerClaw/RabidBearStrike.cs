using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
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
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.DiamondMind;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.TigerClaw
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/rabid-bear-strike--3744/
  static class RabidBearStrike
  {
    public const string Guid = "BB403B0F-5BE7-4D0C-9B0C-E3DFC76CDFFF";
    const string name = "RabidBearStrike.Name";
    const string desc = "RabidBearStrike.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(RabidBearStrike)}");

      UnityEngine.Sprite icon = AbilityRefs.AspectOfTheBear.Reference.Get().Icon;

      var buff = BuffConfigurator.New("RabidBearStrikeBuff", "293389FB-0189-451D-B057-533F02A98EAE")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddAttackBonus(4)
        .AddACBonusAgainstAttacks(armorClassBonus: -4)
        .Configure();

      var ability = AbilityConfigurator.New(name, "96BAF5D9-080E-4E08-825A-EB8218D9F401")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
        .SetCanTargetEnemies()
        .SetCanTargetFriends(false)
        .SetCanTargetSelf(false)
        .SetRange(AbilityRange.Weapon)
        .SetActionType(UnitCommand.CommandType.Standard)
        .SetShouldTurnToTarget()
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction
        (
          ActionsBuilder.New().AddAll(TigerBlooded.GetEffectAction()).ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true).Add<ContextMeleeAttackRolledBonusDamage>(bd => bd.ExtraDamage = new DiceFormula(10, DiceType.D6))
        )
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("RabidBearStrike", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl6Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.TigerClawGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}