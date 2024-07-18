using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.DiamondMind
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/sapphire-nightmare-blade--3637/
  static class SapphireNightmareBlade
  {
    public const string Guid = "65FD7F51-E1CB-49B8-858A-92FB4DFBB8F7";
    const string name = "SapphireNightmareBlade.Name";
    const string desc = "SapphireNightmareBlade.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(SapphireNightmareBlade)}");

      UnityEngine.Sprite icon = FeatureRefs.SwordlordDefensiveParryFeature.Reference.Get().Icon;

      var failBuff = BuffConfigurator.New("SapphireNightmareBladeFailBuff", "E71F778A-9B14-4A5F-A6CD-910D5B097CCB")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddAttackBonus(-2)
        .Configure();

      var ability = AbilityConfigurator.New(name, "4D5FB994-B83C-4E36-B120-D1B18388E49A")
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
        /*.AddAbilityEffectRunAction
        (
          ActionsBuilder.New()
          .Add<NightmareBladeAction>(a =>
          {
            a.OnLow = ActionsBuilder.New().ApplyBuff(failBuff, ContextDuration.Fixed(1), toCaster: true).MeleeAttack().Build();
            a.OnHigh = ActionsBuilder.New().Add<ContextMeleeAttackRolledBonusDamage>(bd => { bd.ExtraDamage = new DiceFormula(1, DiceType.D6); }).Build();
          })
          .MeleeAttack()
        )*/
        .AddAbilityEffectRunAction
        (
          ActionsBuilder.New()
          .Add<NightmareBladeAction2>(a =>
          {
            a.OnLow = ActionsBuilder.New().ApplyBuff(failBuff, ContextDuration.Fixed(1), toCaster: true).MeleeAttack().Build();
            a.OnHigh = ActionsBuilder.New().Add<ContextMeleeAttackRolledBonusDamage>(bd => { bd.ExtraDamage = new DiceFormula(1, DiceType.D6); }).Build();
          })
        )
        /*.AddAbilityEffectRunAction
        (
          ActionsBuilder.New()
          .SkillCheck(Kingmaker.EntitySystem.Stats.StatType.SkillPerception, checkForCaster: true, customDC: new ContextValue { Property = Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.AC },
            failure: ActionsBuilder.New().ApplyBuff(failBuff, ContextDuration.Fixed(1), toCaster: true).MeleeAttack(),
            success: ActionsBuilder.New().Add<ContextMeleeAttackRolledBonusDamage>(bd => { bd.ExtraDamage = new DiceFormula(1, DiceType.D6); }))
        )*/
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("SapphireNightmareBlade", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
        .Configure();
    }
  }
}