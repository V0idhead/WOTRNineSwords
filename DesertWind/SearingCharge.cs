using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using VoidHeadWOTRNineSwords.Common;

namespace VoidHeadWOTRNineSwords.DesertWind
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/searing-charge--3589/
    static class SearingCharge
    {
        public const string Guid = "0BE7E0E7-C4A3-447F-823C-0831108A5C09";
        const string name = "SearingCharge.Name";
        const string desc = "SearingCharge.Desc";

        //const string icon = Helpers.IconPrefix + "burningblade.png";
        static UnityEngine.Sprite icon = AbilityRefs.CausticEruption.Reference.Get().Icon;

        private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

        public static void Configure()
        {
            log.Info($"Configuring {nameof(SearingCharge)}");

            var buff = BuffConfigurator.New("SearingChargeBuff", "B6E16D43-9C6E-4058-BAE4-920CEAC442D3")
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddMechanicsFeature(Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature.MechanicsFeatureType.DisengageWithoutAttackOfOpportunity)
              .AddInitiatorAttackRollTrigger(onlyHit: true, action:
                  ActionsBuilder.New().DealDamage(DamageTypes.Energy(DamageEnergyType.Fire), ContextDice.Value(DiceType.D6, ContextValues.Constant(5))))
              .Configure();

            var chargeBuff = BuffConfigurator.New("SearingChargeChargeBuff", "1D5777E0-8C6F-4078-A08B-48B41EFD1F58")
              .SetDisplayName(name)
              .SetDescription(desc)
              .AddBuffExtraEffects(BuffRefs.ChargeBuff.Reference.Guid, extraEffectBuff: buff)
              .Configure();

            var ability = AbilityConfigurator.New("SearingChargeAbility", "E8C7AA21-2484-4008-B821-B2717DC1E8C5")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetCanTargetEnemies(false)
              .SetCanTargetFriends(false)
              .SetCanTargetSelf()
              .SetRange(AbilityRange.Personal)
              .SetActionType(UnitCommand.CommandType.Free)
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(chargeBuff, ContextDuration.Fixed(1), toCaster: true))
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("SearingCharge", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl4Guid)
#endif
              .Configure();
        }
    }
}