using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;

namespace VoidHeadWOTRNineSwords.ShadowHand
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/five-shadow-creeping-ice-enervating-strike--3692/
    static class CreepingIceStrike
    {
        public const string Guid = "0A034C9A-76D8-4CDC-8B43-420FBE85F05A";
        public const string LegsBuffGuid = "EB4A2ECB-4B9E-40D3-B771-582EA1AE9234";
        public const string ArmsBuffGuid = "C3E21F39-A9ED-46B0-A174-700995C5A865";
        const string name = "CreepingIceStrike.Name";
        const string desc = "CreepingIceStrike.Desc";
        //const string icon = Helpers.IconPrefix + "rubynightmareblade.png";
        static UnityEngine.Sprite icon = AbilityRefs.FlareBurst.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(CreepingIceStrike)}");

            var legsBuff = BuffConfigurator.New("LegsBuff", LegsBuffGuid)
              .SetDisplayName(name) //TODO: own name and desc
              .SetIcon(icon)
              .AddBuffMovementSpeed(value: -200)
              .Configure();

            var armsBuff = BuffConfigurator.New("ArmsBuff", ArmsBuffGuid)
              .SetDisplayName(name) //TODO: own name and desc
              .SetIcon(icon)
              .AddAttackBonus(-6)
              .Configure();

            var ability = AbilityConfigurator.New(name, "A8AD3791-C7D5-414A-A540-8BCA9D883A48")
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
                ActionsBuilder.New().DealDamage(DamageTypes.Direct(), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(15)))
                .Add<CreepingIceStrikeAction>()
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("CreepingIceStrike", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.DiamondMindProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl9Guid)
              .AddPrerequisiteFeaturesFromList(amount: 5, features: AllManeuversAndStances.ShadowHandGuids.Except([Guid]).ToList())
#endif
              .Configure();
        }
    }

    public class CreepingIceStrikeAction : ContextActionSavingThrow
    {
        public override string GetCaption()
        {
            return "Five-Shadow Creeping Ice Enervating Strike";
        }
        public override void RunAction()
        {
            try
            {
                var caster = Context.MaybeCaster;
                var target = Context.MainTarget.Unit;

                RuleRollD20 roll = new RuleRollD20(caster);
                Context.TriggerRule(roll);
                int result = roll.Result;

                RuleSavingThrow savingThrow = new RuleSavingThrow(target, Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, 19 + caster.Stats.Wisdom.Bonus);
                Context.TriggerRule(savingThrow);

                if(result < 8) //1 - 7
                {
                    RuleDealStatDamage dexDam = new RuleDealStatDamage(caster, target, Kingmaker.EntitySystem.Stats.StatType.Dexterity, new Kingmaker.RuleSystem.DiceFormula(2, Kingmaker.RuleSystem.DiceType.D6), 0);
                    Context.TriggerRule(dexDam);
                    if(savingThrow.Success == false)
                        ActionsBuilder.New().ApplyBuff(CreepingIceStrike.LegsBuffGuid, ContextDuration.FixedDice(Kingmaker.RuleSystem.DiceType.D6, 2)).Build().Run();
                }
                else if(result < 15) //8 - 14
                {
                    RuleDealStatDamage strDam = new RuleDealStatDamage(caster, target, Kingmaker.EntitySystem.Stats.StatType.Strength, new Kingmaker.RuleSystem.DiceFormula(2, Kingmaker.RuleSystem.DiceType.D6), 0);
                    Context.TriggerRule(strDam);
                    if (savingThrow.Success == false)
                        ActionsBuilder.New().ApplyBuff(CreepingIceStrike.ArmsBuffGuid, ContextDuration.FixedDice(Kingmaker.RuleSystem.DiceType.D6, 2)).Build().Run();
                }
                else //15 - 20
                {
                    RuleDealStatDamage dexDam = new RuleDealStatDamage(caster, target, Kingmaker.EntitySystem.Stats.StatType.Dexterity, new Kingmaker.RuleSystem.DiceFormula(2, Kingmaker.RuleSystem.DiceType.D6), 0);
                    RuleDealStatDamage strDam = new RuleDealStatDamage(caster, target, Kingmaker.EntitySystem.Stats.StatType.Strength, new Kingmaker.RuleSystem.DiceFormula(2, Kingmaker.RuleSystem.DiceType.D6), 0);
                    RuleDealStatDamage conDam = new RuleDealStatDamage(caster, target, Kingmaker.EntitySystem.Stats.StatType.Constitution, new Kingmaker.RuleSystem.DiceFormula(2, Kingmaker.RuleSystem.DiceType.D6), 0);
                    Context.TriggerRule(dexDam);
                    Context.TriggerRule(strDam);
                    Context.TriggerRule(conDam);
                }
            }
            catch (Exception ex)
            {
                Main.Logger.Error($"{nameof(CreepingIceStrikeAction)} failed: {ex.Message}");
            }
        }
    }
}