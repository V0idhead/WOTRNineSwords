using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.ShadowHand
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/clinging-shadow-strike--3686/
  static class ClingingShadowStrike
  {
    public const string Guid = "52E99D3F-8A00-45C0-89A2-B72EB2899B8E";
    const string name = "ClingingShadowStrike.Name";
    const string desc = "ClingingShadowStrike.Desc";
    const string icon = Helpers.IconPrefix + "clingingshadowstrike.png";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(ClingingShadowStrike)}");

      /*var targetBuff = BuffConfigurator.New("ClingingShadowStrikeBuff", "B57D0750-509F-4A9B-B53A-2AB1B588DE8A")
        .SetDisplayName(name)
        .SetDescription(buffDesc)
        .SetIcon(icon)
        //apparently no way to add miss chance to buff
        .Configure();*/

      var ability = AbilityConfigurator.New("ClingingShadowStrikeAbility", "E3112B9F-8127-43EB-973D-C040A0D0A049")
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
          ActionsBuilder.New().Add<ContextMeleeAttackRolledBonusDamage>(marb => { marb.ExtraDamage = new Kingmaker.RuleSystem.DiceFormula(1, Kingmaker.RuleSystem.DiceType.D6);
            marb.OnHit = ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 11 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, ShadowPresence.ShadowHandFocusFactGuid),
            onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(BuffRefs.Blind.Reference.Guid, ContextDuration.Fixed(1))
            )).Build();
          })
        )
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var maneuver = FeatureConfigurator.New("ClingingShadowStrike", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.ShadowHandProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl1Guid)
#endif
        .Configure();
    }
  }
}