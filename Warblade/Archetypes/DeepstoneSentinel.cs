using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Warblade.Archetypes
{
    static class DeepstoneSentinel
    {
        public const string Guid = "{5560F891-8282-4229-8495-DB53BD19677D}";
        public static void Configure()
        {
            var maneuverSelector = DeepstoneSentinelManeuverSelection.Configure();
            var stanceSelector = DeepstoneSentinelStanceSelection.Configure();

            var mountainFortressBuff2 = BuffConfigurator.New("MountainFortressBuff2", "9301216D-0296-4A46-8827-55AFBA069453")
                .SetDisplayName("MountainFortress.Name")
                .SetDescription("MountainFortress.BuffDesc")
                .SetIcon(FeatureRefs.ArmoredHulkIndomitableStance.Reference.Get().Icon)
                .AddACBonusAgainstAttacks(value: ContextValues.Constant(2), descriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable)
                .Configure();

            var mountainFortressBuff5 = BuffConfigurator.New("MountainFortressBuff5", "33E0FFB8-AA88-4EC4-A51C-66FD5C45522C")
                .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
                .AddSpellResistanceAgainstSpellSchool(spellSchool: Kingmaker.Blueprints.Classes.Spells.SpellSchool.Evocation, value: ContextValues.Constant(10))
                .Configure();

            var mountainFortressBuff8 = BuffConfigurator.New("MountainFortressBuff8", "C568F049-CD82-4FE3-9EE3-4C5DECCCBD20")
                .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
                .AddDamageResistancePhysical(value: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusConstitution, true))
                .Configure();

            var mountainFortressBuff11 = BuffConfigurator.New("MountainFortressBuff11", "80B059C2-E8A4-417E-8DD3-2C185D08644D")
                .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
                .AddACBonusAgainstAttacks(value: ContextValues.Constant(2), descriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable)
                .Configure();

            var mountainFortressBuff14 = BuffConfigurator.New("MountainFortressBuff14", "5D43A984-FB8C-492A-914F-4882B577DF89")
                .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
                .AddFortification(50)
                .Configure();

            var mountainFortressBuff17 = BuffConfigurator.New("MountainFortressBuff17", "950C27FD-4C8D-47F2-A06D-52508536A045")
                .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
                .AddACBonusAgainstAttacks(value: ContextValues.Constant(2), descriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable)
                .Configure();

            var mountainFortressDefense = FeatureConfigurator.New("mountainFortressDefense", "D1F4B19B-5182-439E-B8DE-B6B09DFC180A")
                .SetDisplayName("MountainFortress.Name")
                .SetDescription("MountainFortress.Desc")
                .SetIcon(FeatureRefs.ArmoredHulkIndomitableStance.Reference.Get().Icon)
                .AddNewRoundTrigger(newRoundActions:
                    ActionsBuilder.New().Conditional(ConditionsBuilder.New().IsShieldEquipped(true), ActionsBuilder.New()
                        .ApplyBuff(mountainFortressBuff2, ContextDuration.Fixed(1))
                        .Conditional(ConditionsBuilder.New().CharacterClass(true, WarbladeC.Guid, 5),
                            ActionsBuilder.New().ApplyBuff(mountainFortressBuff5, ContextDuration.Fixed(1)))
                        .Conditional(ConditionsBuilder.New().CharacterClass(true, WarbladeC.Guid, 8),
                            ActionsBuilder.New().ApplyBuff(mountainFortressBuff8, ContextDuration.Fixed(1)))
                        .Conditional(ConditionsBuilder.New().CharacterClass(true, WarbladeC.Guid, 11),
                            ActionsBuilder.New().ApplyBuff(mountainFortressBuff11, ContextDuration.Fixed(1)))
                        .Conditional(ConditionsBuilder.New().CharacterClass(true, WarbladeC.Guid, 14),
                            ActionsBuilder.New().ApplyBuff(mountainFortressBuff14, ContextDuration.Fixed(1)))
                        .Conditional(ConditionsBuilder.New().CharacterClass(true, WarbladeC.Guid, 17),
                            ActionsBuilder.New().ApplyBuff(mountainFortressBuff17, ContextDuration.Fixed(1)))
                    )
                )
                .Configure();

            ArchetypeConfigurator.New("DeepstoneSentinel", Guid, WarbladeC.Guid)
                .SetLocalizedName("DeepstoneSentinel.Name")
                .SetLocalizedDescription("DeepstoneSentinel.Desc")
                .AddPrerequisiteFeature(RaceRefs.DwarfRace.Reference.Get())
                .AddToRemoveFeatures(1, WarbladeManeuverSelection.Guid, WarbladeManeuverSelection.Guid, WarbladeManeuverSelection.Guid, WarbladeStanceSelection.Guid)
                .AddToRemoveFeatures(2, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(3, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(4, WarbladeStanceSelection.Guid)
                .AddToRemoveFeatures(5, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(7, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(9, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(10, WarbladeStanceSelection.Guid)
                .AddToRemoveFeatures(11, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(13, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(15, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(16, WarbladeStanceSelection.Guid)
                .AddToRemoveFeatures(17, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(19, WarbladeManeuverSelection.Guid)
                .AddToAddFeatures(1, maneuverSelector, maneuverSelector, maneuverSelector, stanceSelector)
                .AddToAddFeatures(2, FeatureRefs.ShieldFocus.Reference.Get(), mountainFortressDefense)
                .AddToAddFeatures(2, maneuverSelector)
                .AddToAddFeatures(3, maneuverSelector)
                .AddToAddFeatures(4, stanceSelector)
                .AddToAddFeatures(5, maneuverSelector)
                .AddToAddFeatures(7, maneuverSelector)
                .AddToAddFeatures(8, FeatureRefs.ShieldFocusGreater.Reference.Get())
                .AddToAddFeatures(9, maneuverSelector)
                .AddToAddFeatures(10, stanceSelector)
                .AddToAddFeatures(11, maneuverSelector)
                .AddToAddFeatures(13, maneuverSelector)
                .AddToAddFeatures(15, maneuverSelector)
                .AddToAddFeatures(16, stanceSelector)
                .AddToAddFeatures(17, maneuverSelector)
                .AddToAddFeatures(19, maneuverSelector)
                .Configure();
        }
    }
}