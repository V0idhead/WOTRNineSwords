using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
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
using VoidHeadWOTRNineSwords.DiamondMind;
using VoidHeadWOTRNineSwords.Warblade;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using VoidHeadWOTRNineSwords.Components;
using Kingmaker.RuleSystem;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.StoneDragon
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/mountain-hammer--3722/
  static class MountainHammer
  {
    public const string Guid = "0197D57F-E670-4042-8437-2DBE06A271B5";
    const string name = "MountainHammer.Name";
    const string desc = "MountainHammer.Desc";
    const string icon = Helpers.IconPrefix + "mountainhammer.png";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(MountainHammer)}");

      //UnityEngine.Sprite icon = AbilityRefs.ChallengeEvil.Reference.Get().Icon;

      var buff = BuffConfigurator.New("MountainHammerBuff", "AFA88986-91D1-4608-A062-7B7043FAC946")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddIgnoreDamageReductionOnAttack(onlyOnFirstAttack: true)
        .Configure();

      var ability = AbilityConfigurator.New(name, "F8481A26-023C-4120-8FC9-39275A2CC28A")
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
          ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true).Add<ContextMeleeAttackRolledBonusDamage>(bd => { bd.ExtraDamage = new DiceFormula(2, DiceType.D6); bd.OnHit = EnduranceOfStone.GetEffectAction(); })
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("MountainHammer", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl2Guid)
#endif
        .Configure();
    }
  }
}