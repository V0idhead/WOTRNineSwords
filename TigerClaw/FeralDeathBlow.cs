using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.TigerClaw
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/feral-death-blow--3735/
  static class FeralDeathBlow
  {
    public const string Guid = "811C70F5-60EE-4646-AF72-2F6758BBDB24";
    const string name = "FeralDeathBlow.Name";
    const string desc = "FeralDeathBlow.Desc";
    const string icon = Helpers.IconPrefix + "feraldeathblow.png";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(FeralDeathBlow)}");

      var successBuff = BuffConfigurator.New("FeralDeathBlowSuccessBuff", "E4B13234-1239-43D4-860A-DEE2F9C90396")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddCriticalConfirmationBonus(4)
        .AddOutflankAttackBonus(4)
        .Configure();

      var ability = AbilityConfigurator.New(name, "39010FB4-0102-43D8-9969-EA669024665B")
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
            a.Success = ActionsBuilder.New().ApplyBuff(successBuff, ContextDuration.Fixed(1), toCaster: true).Add<ContextMeleeAttackRolledBonusDamage>(bd =>
            {
              bd.ExtraDamage = new DiceFormula(20, DiceType.D6);
              bd.OnHit = ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 19 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength, TigerBlooded.TigerClawFocusFactGuid),
                onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().Kill(Kingmaker.UnitLogic.UnitState.DismemberType.LimbsApart))).Build();
            }).Build();
            a.Failure = ActionsBuilder.New().MeleeAttack().Build();
          })
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("FeralDeathBlow", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl9Guid)
        .AddPrerequisiteFeaturesFromList(amount: 4, features: AllManeuversAndStances.TigerClawGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}