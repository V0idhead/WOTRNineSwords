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
using static VoidHeadWOTRNineSwords.DiamondMind.SapphireNightmareBlade;
using VoidHeadWOTRNineSwords.DiamondMind;
using VoidHeadWOTRNineSwords.Warblade;
using VoidHeadWOTRNineSwords.Components;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.RuleSystem;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Feats;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;

namespace VoidHeadWOTRNineSwords.TigerClaw
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/claw-moon--3732/
  internal class ClawAtTheMoon
  {
    public const string Guid = "1F7823C2-B34A-4E6F-AED7-AE979E530C9D";
    const string name = "ClawAtTheMoon.Name";
    const string desc = "ClawAtTheMoon.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(ClawAtTheMoon)}");

      UnityEngine.Sprite icon = AbilityRefs.BeastShapeIII.Reference.Get().Icon;

      var successBuff = BuffConfigurator.New("ClawAtTheMoonSuccessBuff", "CBB1732A-6564-4C90-A1A2-84EBEF62F93C")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddCriticalConfirmationBonus(4)
        .Configure();

      var ability = AbilityConfigurator.New(name, "08B0338B-7491-4443-B4D9-C1A98B15E515")
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
          ActionsBuilder.New()
          .AddAll(TigerBlooded.GetEffectAction())
          .Add<OpposedSkillCheck>(a =>
          {
            a.Stat = Kingmaker.EntitySystem.Stats.StatType.SkillAthletics;
            a.TargetValue = t => t.Stats.AC;
            a.Success = ActionsBuilder.New().ApplyBuff(successBuff, ContextDuration.Fixed(1), toCaster: true).Add<ContextMeleeAttackRolledBonusDamage>(bd => bd.ExtraDamage = new DiceFormula(2, DiceType.D6)).Build();
            a.Failure = ActionsBuilder.New().MeleeAttack().Build();
          })
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("ClawAtTheMoon", Guid, AllManeuversAndStances.featureGroup)
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