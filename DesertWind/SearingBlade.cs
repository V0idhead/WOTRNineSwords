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
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.DesertWind
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/searing-blade--3588/
    static class SearingBlade
    {
        public const string Guid = "C9636F87-25ED-4D8D-895C-C380710DC7B9";
        const string name = "SearingBlade.Name";
        const string desc = "SearingBlade.Desc";
        const string icon = Helpers.IconPrefix + "searingblade.png";

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(SearingBlade)}");

            var selfBuff = BuffConfigurator.New("SearingBladeBuff", "B6881604-54AD-4271-AD67-532B76610A84")
              .SetDisplayName(name)
              .SetDescription("SearingBladeBuff.Desc")
              .SetIcon(icon)
              .AddInitiatorAttackRollTrigger(onlyHit: true, action:
                  ActionsBuilder.New().DealDamage(new DamageTypeDescription { Type = DamageType.Energy, Energy = DamageEnergyType.Fire }, new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 2, BonusValue = 4 })
                  .Build())
              .Configure();

            var ability = AbilityConfigurator.New("SearingBladeAbility", "ED9D77B2-02D6-4E5B-AA60-8460415D6A71")
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

            var spell = FeatureConfigurator.New("SearingBlade", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl7Guid)
              .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.DesertWindGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}