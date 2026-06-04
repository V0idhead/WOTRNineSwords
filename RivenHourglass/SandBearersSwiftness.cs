using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.RivenHourglass
{
    //https://www.d20pfsrd.com/ALTERNATIVE-RULE-SYSTEMS/3RD-PARTY-RULES-SYSTEMS/PATH-OF-WAR/DISCIPLINES-AND-MANEUVERS/RIVEN-HOURGLASS-MANEUVERS/#TOC-Sand-Bearer-s-Swiftness
    static class SandBearersSwiftness
    {
        public const string Guid = "DB3373AF-8765-40E5-A322-8927FA9DE4E8";
        const string name = "SandBearersSwiftness.Name";
        const string desc = "SandBearersSwiftness.Desc";
        const string icon = Helpers.IconPrefix + "sandbearersswiftness.png";

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(SandBearersSwiftness)}");

            var buff = BuffConfigurator.New("SandBearersSwiftnessBuff", "BAD75554-AE1B-4BCC-9D06-A58217AA38B3")
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.Dodge, stat: Kingmaker.EntitySystem.Stats.StatType.AC, value: 1)
              .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.Dodge, stat: Kingmaker.EntitySystem.Stats.StatType.SaveReflex, value: 1)
              .AddStatBonus(stat: Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus, value: 1)
              .AddBuffExtraAttack(haste: true, number: 1)
              .AddBuffMovementSpeed(value: 30)
              .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.Paralyzed)
              .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.Slowed)
              .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.CantMove)
              .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.MovementImpairing)
              .AddSpecificBuffImmunity(buff: BuffRefs.AeonBlackHoleSlowBuff.Reference.Get())
              .AddSpecificBuffImmunity(buff: BuffRefs.SlowBuff.Reference.Get())
              .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.Staggered)
              //.AddAttackerSpellFailureChance(20) //failure chance works, but also for buffs and heals
              .AddSpellResistance(value: ContextValues.Constant(20))
              .Configure();

            var activatable = ActivatableAbilityConfigurator.New("SandBearersSwiftnessActivatable", "C32B90A1-C03D-42A5-97D9-99F0AF7B0F6B")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetBuff(buff)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetGroup(ActivatableAbilityGroup.CombatStyle)
              .SetWeightInGroup(1)
              .Configure();

            var feat = FeatureConfigurator.New("SandBearersSwiftnessFeat", Guid)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetRanks(1)
              .AddFacts(new() { activatable })
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.RivenHourglassProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl3Guid)
              .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.RivenHourglassGuids.Except([Guid]).ToList())
#endif
              .Configure(true);
        }
    }
}