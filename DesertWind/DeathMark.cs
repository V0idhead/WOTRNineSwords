using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.DesertWind
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/death-mark--3570/
    static class DeathMark
    {
        public const string Guid = "E1C97259-A7D0-4E10-8809-03AAE28DCF80";
        private const string subAbilityGuid = "7D694B9E-6F0A-41CD-B6E4-0DAAF6122961";
        const string name = "DeathMark.Name";
        const string desc = "DeathMark.Desc";
        //const string icon = Helpers.IconPrefix + "burningblade.png";
        static UnityEngine.Sprite icon = AbilityRefs.FlareBurst.Reference.Get().Icon;
        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(DeathMark)}");

            var subAbility = AbilityConfigurator.New("DeathMarkEffect", subAbilityGuid)
                .SetHidden()
                .SetCanTargetEnemies()
                .SetRange(AbilityRange.Weapon)
                .SetUseCurrentWeaponAsReasonItem()
                .SetActionType(UnitCommand.CommandType.Free)
                .AddAbilityAoERadius(radius: new Kingmaker.Utility.Feet(20))
                .AddAbilityTargetsAround(radius: new Kingmaker.Utility.Feet(20), targetType: Kingmaker.UnitLogic.Abilities.Components.TargetType.Enemy | Kingmaker.UnitLogic.Abilities.Components.TargetType.Ally)
                .AddAbilityEffectRunAction(ActionsBuilder.New().
                    SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Reflex, customDC: new ContextValue { Value = 13 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom, DesertWindDodge.DesertWindFocusFactGuid)).
                        DealDamage(
                            new DamageTypeDescription { Type = DamageType.Energy, Energy = Kingmaker.Enums.Damage.DamageEnergyType.Fire },
                            new ContextDiceValue { DiceCountValue = 6, DiceType = Kingmaker.RuleSystem.DiceType.D6 },
                            halfIfSaved: true, isAoE: true)
                )
                .Configure();

            var ability = AbilityConfigurator.New("DeathMarkAbility", "3148D96F-CE18-48AE-962F-2423BF901C94")
              .SetDisplayName(name)
              .SetDescription(desc)
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
                actions: ActionsBuilder.New().
                    Add<MeleeAttackExtended>(mae => mae.OnHit = ActionsBuilder.New()
                        .CastSpell(subAbilityGuid, markAsChild: true, spendAction: false)
                        .Build()
                    )
               )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("DeathMark", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl3Guid)
#endif
              .Configure();
        }
    }
}