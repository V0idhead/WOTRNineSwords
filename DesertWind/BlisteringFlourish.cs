using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Mechanics;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.DesertWind
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/blistering-flourish--3567/
  static class BlisteringFlourish
  {
    public const string Guid = "A4F1E5D7-0B2C-4F8A-8C3D-6E9F1B5A0E3F";
    const string name = "BlisteringFlourish.Name";
    const string desc = "BlisteringFlourish.Desc";
    //const string icon = Helpers.IconPrefix + "blisteringflourish.png";
    static UnityEngine.Sprite icon = AbilityRefs.CausticEruption.Reference.Get().Icon;

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(BlisteringFlourish)}");

      var ability = AbilityConfigurator.New("BlisteringFlourishAbility", "7A38ACF7-7D8F-4B87-912F-B44350465509")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
        .SetCanTargetSelf(true)
        .SetEffectOnAlly(AbilityEffectOnUnit.Harmful)
        .SetEffectOnEnemy(AbilityEffectOnUnit.Harmful)
        .SetRange(AbilityRange.Personal)
        .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityTargetsAround(radius: new Kingmaker.Utility.Feet(30))
        .AddAbilitySpawnFx(AbilitySpawnFxAnchor.Caster, prefabLink: AbilityRefs.FlareBurst.Reference.Get().GetComponent<AbilitySpawnFx>().PrefabLink, time: AbilitySpawnFxTime.OnApplyEffect)
        .AddAbilityEffectRunAction
        (
          ActionsBuilder.New()
          .SavingThrow(SavingThrowType.Will, customDC: new ContextValue { Value = 11 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, RelentlessSirocco.DesertWindFocusFactGuid),
          onResult: ActionsBuilder.New()
            .ConditionalSaved
            (
              failed: ActionsBuilder.New().ApplyBuff(BuffRefs.DazzledBuff.Reference.Get(), ContextDuration.Fixed(1, DurationRate.Minutes))
            )
          )
        )
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("BlisteringFlourish", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl1Guid)
#endif
        .Configure();

      Main.Logger.Info($"Configured {nameof(BlisteringFlourish)}");
    }
  }
}