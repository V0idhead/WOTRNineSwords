using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
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
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/inferno-blast--3582/
    static class InfernoBlast
    {
        public const string Guid = "8332FFAA-C073-43B0-A926-CED7A67E384B";
        const string name = "InfernoBlast.Name";
        const string desc = "InfernoBlast.Desc";
        //const string icon = Helpers.IconPrefix + "blisteringflourish.png";
        static UnityEngine.Sprite icon = AbilityRefs.CausticEruption.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(InfernoBlast)}");

            var ability = AbilityConfigurator.New("InfernoBlastAbility", "4245FF96-2198-44CE-B170-32024CD92AE4")
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
              .AddAbilityTargetsAround(radius: new Kingmaker.Utility.Feet(30), targetType: Kingmaker.UnitLogic.Abilities.Components.TargetType.Any, condition: ConditionsBuilder.New().IsCaster(true).Build())
              .AddAbilitySpawnFx(AbilitySpawnFxAnchor.Caster, prefabLink: AbilityRefs.FlareBurst.Reference.Get().GetComponent<AbilitySpawnFx>().PrefabLink, time: AbilitySpawnFxTime.OnApplyEffect)
              .AddAbilityEffectRunAction
              (
                ActionsBuilder.New()
                .SavingThrow(SavingThrowType.Reflex, customDC: new ContextValue { Value = 19 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, RelentlessSirocco.DesertWindFocusFactGuid),
                onResult: ActionsBuilder.New()
                  .ConditionalSaved
                  (
                    failed: ActionsBuilder.New().DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Fire), ContextDice.Value(Kingmaker.RuleSystem.DiceType.One, 100)),
                    succeed: ActionsBuilder.New().DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Fire), ContextDice.Value(Kingmaker.RuleSystem.DiceType.One, 50), half: true)
                  )
                )
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("InfernoBlast", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl9Guid)
        .AddPrerequisiteFeaturesFromList(amount: 5, features: AllManeuversAndStances.DesertWindGuids.Except([Guid]).ToList())
#endif
              .Configure();

            Main.Logger.Info($"Configured {nameof(InfernoBlast)}");
        }
    }
}