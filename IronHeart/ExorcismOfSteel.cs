using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.StoneDragon;
using VoidHeadWOTRNineSwords.Warblade;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.Buffs;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/exorcism-steel--3646/
  static class ExorcismOfSteel
  {
    public const string Guid = "DE327BA3-B409-45EC-96DC-5C58EC4F1CEF";
    const string name = "ExorcismOfSteel.Name";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.ChaosHammer.Reference.Get().Icon;

      log.Info($"Configuring {nameof(ExorcismOfSteel)}");

      var buffSaveFailed = BuffConfigurator.New("ExorcismOfSteelBuffSaveFailed", "63798F63-90B2-4B4E-92AE-1E7A937BFB50")
        .SetDisplayName(name)
        .SetDescription("ExorcismOfSteel.Desc")
        .SetIcon(icon)
        .AddDamageBonusConditional(new ContextValue { Value = -4})
        .Configure();

      var buffSaveSucced = BuffConfigurator.New("ExorcismOfSteelBuffSaveSucceed", "44A0EA1A-5D46-4387-B37B-747C961A5564")
        .SetDisplayName(name)
        .SetDescription("ExorcismOfSteel.Desc")
        .SetIcon(icon)
        .AddDamageBonusConditional(new ContextValue { Value = -2 })
        .Configure();

      var triggerBuff = BuffConfigurator.New("ExorcismOfSteelTriggerBuff", "AAFB2E3E-4DFD-4488-8EF3-FF58F50C7C69")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddInitiatorAttackRollTrigger(
          onlyHit: true,
          action: ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Will, customDC: new ContextValue { Value = 13 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength, IronHeartAura.IronHeartFocusFactGuid),
            onResult: ActionsBuilder.New().ConditionalSaved
              (
                failed: ActionsBuilder.New().ApplyBuff(buffSaveFailed, ContextDuration.Fixed(1, DurationRate.Minutes)),
                succeed: ActionsBuilder.New().ApplyBuff(buffSaveSucced, ContextDuration.Fixed(1, DurationRate.Minutes))
              )
          ))
        .Configure();

      var ability = AbilityConfigurator.New("ExorcismOfSteelAbility", "D1DA7EF9-0854-492F-A7D2-64BF45AC889A")
        .SetDisplayName(name)
        .SetDescription("ExorcismOfSteel.Desc")
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
        .SetCanTargetEnemies()
        .SetCanTargetFriends(false)
        .SetCanTargetSelf(false)
        .SetRange(AbilityRange.Weapon)
        .SetUseCurrentWeaponAsReasonItem()
        .SetActionType(UnitCommand.CommandType.Standard)
        .SetShouldTurnToTarget()
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(
          actions: ActionsBuilder.New().ApplyBuff(triggerBuff, ContextDuration.Fixed(1), toCaster: true).MeleeAttack()
        )
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("ExorcismOfSteel", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription("ExorcismOfSteel.Desc")
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl3Guid)
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.IronHeartGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}