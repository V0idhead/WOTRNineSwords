using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Warblade.Archetypes
{
    static class Warlord
    {
        public const string Guid = "{7F9843D1-2923-4BFE-96B2-37A4783BCD71}";
        public static void Configure()
        {
            #region Tactitian
            var tactitianResource = AbilityResourceConfigurator.New("WarlordTactitianResource", "5DA60312-E0C0-4439-AE02-F4DB2293400B")
                .SetMaxAmount(ResourceAmountBuilder.New(1).IncreaseByLevelStartPlusDivStep(new string[] { WarbladeC.Guid }, 0, 5, 1, 5, 1).Build())
                .Configure();

            var origAbility = AbilityRefs.CavalierTacticianAbility.Reference.Get();
            var tactitianAbility = AbilityConfigurator.New("WarlordTactitianAbility", "29DFCCEA-7678-4FDD-A104-C47982FF66D4")
                .SetIcon(origAbility.Icon)
                .SetDisplayName(origAbility.m_DisplayName)
                .SetDescription(origAbility.m_Description)
                .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.Extraordinary)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetCanTargetSelf()
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .AddAbilityTargetsAround(ConditionsBuilder.New().TargetIsYourself(negate: true), radius: new Feet(30), targetType: Kingmaker.UnitLogic.Abilities.Components.TargetType.Ally)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { WarbladeC.Guid }))
                .AddContextCalculateSharedValue(1, ContextDice.Value(Kingmaker.RuleSystem.DiceType.Zero, bonus: new ContextValue { Value = 3, PropertyName = Kingmaker.Enums.ContextPropertyName.Value1 }), Kingmaker.UnitLogic.Abilities.AbilitySharedValue.Duration)
                .AddAbilityApplyFact(
                    ContextDuration.Variable(ContextValues.Shared(Kingmaker.UnitLogic.Abilities.AbilitySharedValue.Duration)),
                    new List<Blueprint<Kingmaker.Blueprints.BlueprintUnitFactReference>> { BuffRefs.CavalierTacticianAlliedSpellcasterBuff.Reference.Get(), BuffRefs.CavalierTacticianBackToBackBuff.Reference.Get(), BuffRefs.CavalierTacticianCoordinatedDefenseBuff.Reference.Get(), BuffRefs.CavalierTacticianCoordinatedManeuversBuff.Reference.Get(), BuffRefs.CavalierTacticianOutflankBuff.Reference.Get(), BuffRefs.CavalierTacticianPreciseStrikeBuff.Reference.Get(), BuffRefs.CavalierTacticianShakeItOffBuff.Reference.Get(), BuffRefs.CavalierTacticianShieldedCasterBuff.Reference.Get(), BuffRefs.CavalierTacticianShieldWallBuff.Reference.Get(), BuffRefs.CavalierTacticianSiezeTheMomentBuff.Reference.Get(), BuffRefs.CavalierTacticianTandemTripBuff.Reference.Get(), BuffRefs.CavalierTacticianVolleyFireBuff.Reference.Get() },
                    true,
                    restriction: Kingmaker.UnitLogic.Abilities.Components.AbilityApplyFact.FactRestriction.CasterHasFact)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: tactitianResource)
                .AddAbilitySpawnFx(Kingmaker.UnitLogic.Abilities.Components.Base.AbilitySpawnFxAnchor.Caster, prefabLink: "76e2066f35b1a564cb03bb618869e91f") //same id as original
                .Configure();

            var origFeat = FeatureRefs.CavalierTacticianFeature.Reference.Get();
            var tactitianFeature = FeatureConfigurator.New("WarlordTactitian", "9F45A05A-0C01-472B-AA8A-07847EC0FB7D")
                .SetIcon(origFeat.Icon)
                .SetDisplayName(origFeat.m_DisplayName)
                .SetDescription(origFeat.m_Description)
                .AddFacts(new List<Blueprint<BlueprintUnitFactReference>> { tactitianAbility, FeatureRefs.CavalierTacticianSupportFeature.Reference.Get() })
                .AddAbilityResources(0, tactitianResource)
                .Configure();

            var origGAbility = AbilityRefs.CavalierTacticianAbilitySwift.Reference.Get();
            var tactitianGreaterAbility = AbilityConfigurator.New("WarlordGreaterTactitianAbility", "CDD3243E-90D1-4EAC-B9F8-5E25D153FE7B")
                .SetIcon(origAbility.Icon)
                .SetDisplayName(origAbility.m_DisplayName)
                .SetDescription(origAbility.m_Description)
                .SetType(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.Extraordinary)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .SetCanTargetSelf()
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AddAbilityTargetsAround(ConditionsBuilder.New().TargetIsYourself(negate: true), radius: new Feet(30), targetType: Kingmaker.UnitLogic.Abilities.Components.TargetType.Ally)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { WarbladeC.Guid }))
                .AddContextCalculateSharedValue(1, ContextDice.Value(Kingmaker.RuleSystem.DiceType.Zero, bonus: new ContextValue { Value = 3, PropertyName = Kingmaker.Enums.ContextPropertyName.Value1 }), Kingmaker.UnitLogic.Abilities.AbilitySharedValue.Duration)
                .AddAbilityApplyFact(
                    ContextDuration.Variable(ContextValues.Shared(Kingmaker.UnitLogic.Abilities.AbilitySharedValue.Duration)),
                    new List<Blueprint<Kingmaker.Blueprints.BlueprintUnitFactReference>> { BuffRefs.CavalierTacticianAlliedSpellcasterBuff.Reference.Get(), BuffRefs.CavalierTacticianBackToBackBuff.Reference.Get(), BuffRefs.CavalierTacticianCoordinatedDefenseBuff.Reference.Get(), BuffRefs.CavalierTacticianCoordinatedManeuversBuff.Reference.Get(), BuffRefs.CavalierTacticianOutflankBuff.Reference.Get(), BuffRefs.CavalierTacticianPreciseStrikeBuff.Reference.Get(), BuffRefs.CavalierTacticianShakeItOffBuff.Reference.Get(), BuffRefs.CavalierTacticianShieldedCasterBuff.Reference.Get(), BuffRefs.CavalierTacticianShieldWallBuff.Reference.Get(), BuffRefs.CavalierTacticianSiezeTheMomentBuff.Reference.Get(), BuffRefs.CavalierTacticianTandemTripBuff.Reference.Get(), BuffRefs.CavalierTacticianVolleyFireBuff.Reference.Get() },
                    true,
                    restriction: Kingmaker.UnitLogic.Abilities.Components.AbilityApplyFact.FactRestriction.CasterHasFact)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: tactitianResource)
                .AddAbilitySpawnFx(Kingmaker.UnitLogic.Abilities.Components.Base.AbilitySpawnFxAnchor.Caster, prefabLink: "76e2066f35b1a564cb03bb618869e91f") //same id as original
                .Configure();

            var origGFeat = FeatureRefs.CavalierTacticianGreater.Reference.Get();
            var tactitianGreaterFeature = FeatureConfigurator.New("WarlordGreaterTactitian", "CA9271B7-4FED-4C10-8408-E72FAB5CC6F4")
                .SetIcon(origFeat.Icon)
                .SetDisplayName(origFeat.m_DisplayName)
                .SetDescription(origFeat.m_Description)
                .AddFacts(new List<Blueprint<BlueprintUnitFactReference>> { tactitianGreaterAbility, FeatureRefs.CavalierTacticianSupportFeature.Reference.Get() })
                .AddAbilityResources(0, tactitianResource)
                .Configure();
            #endregion

            BlueprintFeature forceOfPersonality = FeatureConfigurator.New("ForceOfPersonality", "A26B1DE0-73FF-461B-8932-75DBE54F490C")
              .SetDisplayName("ForceOfPersonality.Name")
              .SetDescription("ForceOfPersonality.Desc")
              .SetIsClassFeature()
              .SetReapplyOnLevelUp()
              .AddRecalculateOnStatChange(stat: Kingmaker.EntitySystem.Stats.StatType.Charisma)
              .AddContextStatBonus(Kingmaker.EntitySystem.Stats.StatType.SaveWill, ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusCharisma), Kingmaker.Enums.ModifierDescriptor.Insight)
              .Configure();

            BlueprintBuff battleOrdersEffect = BuffConfigurator.New("BattleOrdersEffect", "9099F371-29A9-4A99-9BBF-962FE321492D")
                .SetDisplayName("BattleOrders.Name")
                .SetDescription("BattleOrders.EffectDesc")
                .SetIcon(AbilityRefs.Flare.Reference.Get().Icon) //TODO: replace
                .AddAttackBonus(1)
                .Configure();

            var battleOrdersAura = AbilityAreaEffectConfigurator.New("BattleOrdersArea", "{647F7C6D-CE55-484F-A469-67DEB3538CB4}")
                .AddAbilityAreaEffectBuff(battleOrdersEffect, condition: ConditionsBuilder.New().IsAlly().IsCaster(false))
                .SetTargetType(Kingmaker.UnitLogic.Abilities.Blueprints.BlueprintAbilityAreaEffect.TargetType.Ally)
                .SetShape(Kingmaker.UnitLogic.Abilities.Blueprints.AreaEffectShape.Cylinder)
                .SetSize(new Feet(30))
                .Configure();

            BlueprintBuff battleOrdersBuff = BuffConfigurator.New("BattleOrdersBuff", "F8DC5F48-E1B0-41BC-BF4C-4F602949C000")
                .SetFlags(BlueprintBuff.Flags.HiddenInUi)
                .AddAreaEffect(battleOrdersAura)
                .Configure();

            BlueprintFeature battleOrders = FeatureConfigurator.New("BattleOrders", "74ECC2E0-5D16-442F-BCB3-070A4D8D369E")
                .SetDisplayName("BattleOrders.Name")
                .SetDescription("BattleOrders.Desc")
                .SetIsClassFeature()
                .SetReapplyOnLevelUp()
                .AddAuraFeatureComponent(battleOrdersBuff)
                .Configure();

            BlueprintBuff intimidatingGlareEffect = BuffConfigurator.New("IntimidatingGlareEffect", "3AD04888-C441-4E05-BCC0-0B34D2570909")
                .SetDisplayName("IntimidatingGlare.Name")
                .SetDescription("IntimidatingGlare.EffectDesc") //TODO: write
                .SetIcon(AbilityRefs.Flare.Reference.Get().Icon) //TODO: replace
                .AddTargetSavingThrowTrigger(ActionsBuilder.New().ApplyBuff(BuffRefs.Shaken.Reference.Get(), ContextDuration.Fixed(3), toCaster: true), onlyFail: true)
                .Configure();

            var intimidatingGlareAura = AbilityAreaEffectConfigurator.New("IntimidatingGlareArea", "8BC41C63-E1F8-484F-933C-B38FBF308A80")
                .AddAbilityAreaEffectBuff(intimidatingGlareEffect)
                .SetTargetType(Kingmaker.UnitLogic.Abilities.Blueprints.BlueprintAbilityAreaEffect.TargetType.Enemy)
                .SetShape(Kingmaker.UnitLogic.Abilities.Blueprints.AreaEffectShape.Cylinder)
                .SetSize(new Feet(20))
                .Configure();

            BlueprintBuff intimidatingGlareBuff = BuffConfigurator.New("IntimidatingGlareBuff", "33E093D8-84F1-4A8D-A70C-71F9563F9363")
                .SetFlags(BlueprintBuff.Flags.HiddenInUi)
                .AddAreaEffect(intimidatingGlareAura)
                .Configure();

            BlueprintFeature intimidatingGlare = FeatureConfigurator.New("IntimidatingGlare", "F92CA6EF-52E2-4A5D-B94D-F6350225F57D")
                .SetDisplayName("IntimidatingGlare.Name")
                .SetDescription("IntimidatingGlare.Desc")
                .AddAuraFeatureComponent(intimidatingGlareBuff)
                .SetIsClassFeature()
                .Configure();

            /*BlueprintBuff tacticalAssistanceBuff = BuffConfigurator.New("TacticalAssistanceBuff", "{5E961FB4-826C-4923-A1B3-FAC49B04AA0B}")
                .SetDisplayName("TacticalAssistance.Name")
                .SetDescription("TacticalAssistance.Desc")
                .SetIcon(FeatureRefs.Mobility.Reference.Get().Icon)
                .Configure();*/

            BlueprintAbility tacticalAssistanceAbility = AbilityConfigurator.New("TacticalAssistanceAbility", "FF16F435-9424-4B1A-979C-270F2D936C15")
                .SetDisplayName("TacticalAssistance.Name")
                .SetDescription("TacticalAssistance.Desc")
                .SetIcon(FeatureRefs.Mobility.Reference.Get().Icon)
                .SetCanTargetEnemies()
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Weapon)
                .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .AddAbilityEffectRunAction(ActionsBuilder.New().DealDamageToAbility(Kingmaker.EntitySystem.Stats.StatType.AC, ContextDice.Value(Kingmaker.RuleSystem.DiceType.One, ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusCharisma, toCaster: true))))
                .Configure();

            BlueprintFeature tacticalAssistance = FeatureConfigurator.New("TacticalAssistance", "1906929A-DB25-4211-A3FF-BD9BFDB755CE")
                .SetDisplayName("TacticalAssistance.Name")
                .SetDescription("TacticalAssistance.Desc")
                .AddFacts([tacticalAssistanceAbility])
                .Configure();

            ArchetypeConfigurator.New("Warlord", Guid, WarbladeC.Guid)
                .SetLocalizedName("Warlord.Name")
                .SetLocalizedDescription("Warlord.Desc")
                .RemoveFromRecommendedAttributes(Kingmaker.EntitySystem.Stats.StatType.Intelligence)
                .AddToRecommendedAttributes(Kingmaker.EntitySystem.Stats.StatType.Charisma)
                .AddToRemoveFeatures(1, BattleClarityReflex.Guid) //replaced by Force of Personality at lvl3
                .AddToRemoveFeatures(2, FeatureRefs.UncannyDodge.Reference.Guid) //moved back to lvl6
                .AddToRemoveFeatures(3, BattleArdor.Guid) //replaced by Battle Orders at lvl7
                .AddToRemoveFeatures(6, FeatureRefs.ImprovedUncannyDodge.Reference.Guid) //removed
                .AddToRemoveFeatures(7, BattleCunning.Guid) //replaced by Intimidating Glare at lvl11
                .AddToRemoveFeatures(11, BattleSkill.Guid) //replaced by Tactical Assistance at lvl15
                .AddToRemoveFeatures(15, BattleMastery.Guid)
                .AddToAddFeatures(1, tactitianFeature, FeatureSelectionRefs.CavalierTacticianFeatSelection.Reference.Get())
                .AddToAddFeatures(3, forceOfPersonality)
                .AddToAddFeatures(6, FeatureRefs.UncannyDodge.Reference.Guid)
                .AddToAddFeatures(7, battleOrders)
                .AddToAddFeatures(9, tactitianGreaterFeature, FeatureSelectionRefs.CavalierTacticianFeatSelection.Reference.Get())
                .AddToAddFeatures(11, intimidatingGlare)
                .AddToAddFeatures(15, tacticalAssistance)
                .AddToAddFeatures(17, FeatureSelectionRefs.CavalierTacticianFeatSelection.Reference.Get())
                .Configure();
        }
    }
}