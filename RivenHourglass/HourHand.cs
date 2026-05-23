using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.RivenHourglass
{
    //https://www.d20pfsrd.com/ALTERNATIVE-RULE-SYSTEMS/3RD-PARTY-RULES-SYSTEMS/PATH-OF-WAR/DISCIPLINES-AND-MANEUVERS/RIVEN-HOURGLASS-MANEUVERS/#TOC-Hour-Hand
    static class HourHand
    {
        public const string Guid = "289633E9-4383-4D26-8E7E-D2ED9976D3BD";
        const string name = "HourHand.Name";
        const string desc = "HourHand.Desc";
        const string icon = Helpers.IconPrefix + "hourhand.png";

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(HourHand)}");

            var buff = BuffConfigurator.New("HourHandBuff", "C4F2A13F-601C-42C5-83AB-534620A957D3")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddAttackBonus(-4)
              .Configure();

            var ability = AbilityConfigurator.New(name, "B56FD070-15E3-4825-BCA0-4614A73611C2")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
              .SetCanTargetEnemies()
              .SetCanTargetFriends(false)
              .SetCanTargetSelf(false)
              .SetRange(AbilityRange.Weapon)
              .SetActionType(UnitCommand.CommandType.Swift)
              .SetShouldTurnToTarget()
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityEffectRunAction(ActionsBuilder.New()
                .ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true)
                .Add<ContextMeleeAttackRolledBonusDamage>(marb => { marb.ExtraDamage = new Kingmaker.RuleSystem.DiceFormula(4, Kingmaker.RuleSystem.DiceType.D6); marb.OnHit = ActionsBuilder.New().AddAll(EternalMoment.GetEffectAction()).Build(); } )
                .RemoveBuff(buff, onlyFromCaster: true, toCaster: true)
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("HourHand", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.RivenHourglassProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl4Guid)
              .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.RivenHourglassGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }
}