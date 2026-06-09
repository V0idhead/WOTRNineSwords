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

            var mountainFortressBuff = BuffConfigurator.New("MountainFortressRampartBuff", "{9301216D-0296-4A46-8827-55AFBA069453}")
                .SetDisplayName("MountainFortressRampart.Name")
                .SetDescription("MountainFortressRampart.BuffDesc")
                .SetIcon(BuffRefs.SacredArmorEnchantFortification50Buff.Reference.Get().Icon)
                .AddFortification(50)
                .AddACBonusAgainstAttacks(value: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusIntelligence, true), descriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable)
                .AddDamageResistancePhysical(value: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusConstitution, true))
                .Configure();

            var mountainFortressAbility = AbilityConfigurator.New("MountainFortressRampartAbility", "{8FEC2B3F-0B8C-4A23-83A8-1B09D3204A48}")
                .AddMovementDistanceTrigger(limitTiggerCountInOneRound: true)
                .Configure();

            var mountainFortressRampart = FeatureConfigurator.New("MountainFortressRampart", "{D1F4B19B-5182-439E-B8DE-B6B09DFC180A}")
                .SetDisplayName("MountainFortressRampart.Name")
                .SetDescription("MountainFortressRampart.Desc")
                .SetIcon(BuffRefs.SacredArmorEnchantFortification50Buff.Reference.Get().Icon)
                .AddNewRoundTrigger(newRoundActions: ActionsBuilder.New().ApplyBuff(mountainFortressBuff, ContextDuration.Fixed(1)))
                .AddMovementDistanceTrigger(ActionsBuilder.New().RemoveBuff(mountainFortressBuff, true, toCaster:true))
                //.AddFacts(new() { mountainFortressAbility })
                .Configure();

            /*var crashingMountainJuggernautAura = AbilityAreaEffectConfigurator.New("CrashingMountainJuggernautAura", "")
                .Configure();

            var crashingMountainJuggernautBuff = BuffConfigurator.New("CrashingMountainJuggernautBuff", "{9878D0A5-635B-4BC3-8092-3637576B2D0D}")
                .SetDisplayName("CrashingMountainJuggernaut.Name")
                .SetDescription("CrashingMountainJuggernaut.BuffDesc")
                .SetIcon(FeatureRefs.IncredibleHeftFeature.Reference.Get().Icon)
                .Configure();

            var crashingMountainJuggernautAbility = AbilityConfigurator.New("CrashingMountainJuggernautAbility", "")
                .AddAbilityTargetsAround(radius: new Feet(10))
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(BuffRefs.Prone.Reference, context)*/

            //make it work like grease?
            var crashingMountainJuggernaut = FeatureConfigurator.New("CrashingMountainJuggernaut", "{7284A5C0-DD9D-4EB1-A386-FB47E213002B}")
                .SetDisplayName("CrashingMountainJuggernaut.Name")
                .SetDescription("CrashingMountainJuggernaut.Desc")
                .SetIcon(FeatureRefs.IncredibleHeftFeature.Reference.Get().Icon)
                .Configure();

            //TODO: need one more stance to be able to choose at lvl 4 to also restrict stances
            ArchetypeConfigurator.New("DeepstoneSentinel", Guid, WarbladeC.Guid)
                .SetLocalizedName("DeepstoneSentinel.Name")
                .SetLocalizedDescription("DeepstoneSentinel.Desc")
                .AddPrerequisiteFeature(RaceRefs.DwarfRace.Reference.Get())
                .AddToRemoveFeatures(1, WarbladeManeuverSelection.Guid, WarbladeManeuverSelection.Guid, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(2, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(3, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(5, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(7, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(9, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(11, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(13, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(15, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(17, WarbladeManeuverSelection.Guid)
                .AddToRemoveFeatures(19, WarbladeManeuverSelection.Guid)
                .AddToAddFeatures(1, maneuverSelector, maneuverSelector, maneuverSelector)
                .AddToAddFeatures(2, FeatureRefs.ShieldFocus.Reference.Get())
                .AddToAddFeatures(2, maneuverSelector)
                .AddToAddFeatures(3, maneuverSelector)
                .AddToAddFeatures(5, maneuverSelector)
                .AddToAddFeatures(6, mountainFortressRampart)
                .AddToAddFeatures(7, maneuverSelector)
                .AddToAddFeatures(8, FeatureRefs.ShieldFocusGreater.Reference.Get())
                .AddToAddFeatures(9, maneuverSelector)
                .AddToAddFeatures(11, crashingMountainJuggernaut)
                .AddToAddFeatures(11, maneuverSelector)
                .AddToAddFeatures(13, maneuverSelector)
                .AddToAddFeatures(15, maneuverSelector)
                .AddToAddFeatures(17, maneuverSelector)
                .AddToAddFeatures(19, maneuverSelector)
                .Configure();
        }
    }
}