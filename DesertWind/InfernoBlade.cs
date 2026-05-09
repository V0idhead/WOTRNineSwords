using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.DesertWind
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/inferno-blade--3581/
    static class InfernoBlade
    {
        public const string Guid = "40998DE4-F5CB-4B67-AADC-A5F7B8EC7006";
        const string name = "InfernoBlade.Name";
        const string desc = "InfernoBlade.Desc";
        const string icon = Helpers.IconPrefix + "infernoblade.png";

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(InfernoBlade)}");

            var selfBuff = BuffConfigurator.New("InfernoBladeBuff", "E97129EF-FA2A-44E2-89E8-7BEB46506747")
              .SetDisplayName(name)
              .SetDescription("InfernoBladeBuff.Desc")
              .SetIcon(icon)
              //.AddOutgoingDamageTriggerFixed
              .AddInitiatorAttackRollTrigger(onlyHit: true, action:
                  ActionsBuilder.New().DealDamage(new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Fire }, new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 3, BonusValue = 7 })
                  .Build())
              .Configure();

            var ability = AbilityConfigurator.New("InfernoBladeAbility", "FF4B30E9-5FD1-462D-8ABC-7FD7C61B443E")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetCanTargetEnemies(false)
              .SetCanTargetFriends(false)
              .SetCanTargetSelf()
              .SetRange(AbilityRange.Personal)
              .SetActionType(UnitCommand.CommandType.Swift)
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(selfBuff, ContextDuration.Fixed(1), toCaster: true))
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("InfernoBlade", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl7Guid)
#endif
              .Configure();
        }
    }
}