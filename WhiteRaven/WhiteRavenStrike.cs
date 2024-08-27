using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.ElementsSystem;
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
using VoidHeadWOTRNineSwords.StoneDragon;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.WhiteRaven
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/white-raven-strike--3772/
  static class WhiteRavenStrike
  {
    public const string Guid = "B1BCB605-A65D-4426-AB47-ECFB67204963";
    const string name = "WhiteRavenStrike.Name";
    const string desc = "WhiteRavenStrike.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(WhiteRavenStrike)}");

      UnityEngine.Sprite icon = AbilityRefs.Eaglesoul.Reference.Get().Icon;

      var buff = BuffConfigurator.New("WhiteRavenStrikeTargetBuff", "421E37AA-E477-49FD-8901-13D823C01AD1")
        .AddCondition(Kingmaker.UnitLogic.UnitCondition.LoseDexterityToAC)
        .Configure();

      /*var buff = BuffConfigurator.New("WhiteRavenStrikeBuff", "B1BCB605-A65D-4426-AB47-ECFB67204964")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddDamageBonusConditional(bonus: new ContextValue { Value = 16 }, descriptor: ModifierDescriptor.UntypedStackable) //TODO: damage bonus should be 4d6; also should flat-foot the enemy
        .AddInitiatorAttackRollTrigger(onlyHit: true,
          action: ActionsBuilder.New().ApplyBuff(targetBuff, ContextDuration.Fixed(1))
        )
        .Configure();*/

      var ability = AbilityConfigurator.New(name, "130E3211-E6F8-4CC6-9F6B-FDC0CFD63F64")
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
          ActionsBuilder.New().Add<ContextMeleeAttackRolledBonusDamage>(marb => { marb.ExtraDamage = new DiceFormula(4, DiceType.D6); marb.OnHit = ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1)).Build(); })
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("WhiteRavenStrike", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl4Guid)
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.WhiteRavenGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}