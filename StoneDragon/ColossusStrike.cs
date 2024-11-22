using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.StoneDragon
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/colossus-strike--3713/
  static class ColossusStrike
  {
    public const string Guid = "F9FFD3DB-B446-4D9E-9E9D-7755E5FC2D50";
    const string name = "ColossusStrike.Name";
    const string desc = "ColossusStrike.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(ColossusStrike)}");

      UnityEngine.Sprite icon = AbilityRefs.LegendaryProportions.Reference.Get().Icon;

      var ability = AbilityConfigurator.New(name, "938A43D0-9041-4A5C-9983-6227650A3CDB")
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
          ActionsBuilder.New().Add<ContextMeleeAttackRolledBonusDamage>(bd =>
          {
            bd.ExtraDamage = new DiceFormula(6, DiceType.D6);
            bd.OnHit = ActionsBuilder.New().AddAll(EnduranceOfStone.GetEffectAction()).SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, customDC: new ContextValue { Value = 17 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength, EnduranceOfStone.StoneDragonFocusFactGuid),
              onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(BuffRefs.Prone.Reference.Get(), ContextDuration.Fixed(1)).Add<PushTargetAction>(pta => { pta.CalcDistance = () => { Random r = new Random(); return new Kingmaker.Utility.Feet(r.Next(5, 20)); }; }))
            ).Build(); }
        ))
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("ColossusStrike", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl7Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.StoneDragonGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}