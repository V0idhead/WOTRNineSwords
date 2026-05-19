using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;

namespace VoidHeadWOTRNineSwords.RivenHourglass
{
    //https://www.d20pfsrd.com/ALTERNATIVE-RULE-SYSTEMS/3RD-PARTY-RULES-SYSTEMS/PATH-OF-WAR/DISCIPLINES-AND-MANEUVERS/RIVEN-HOURGLASS-MANEUVERS/#TOC-Riven-Hourglass-Stance
    static class HourglassStance
    {
        public const string Guid = "30F491EF-E038-4D87-B3D0-0891DE679016";
        const string name = "HourglassStance.Name";
        const string desc = "HourglassStance.Desc";
        //const string icon = Helpers.IconPrefix + "hourglassstance.png";
        static UnityEngine.Sprite icon = AbilityRefs.FlareBurst.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(HourglassStance)}");

            var buff = BuffConfigurator.New("HourglassStanceBuff", "FAB60CD5-9F8D-4250-8478-D4D94561204F")
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.Dodge, stat: Kingmaker.EntitySystem.Stats.StatType.AC, value: 4)
              .AddStatBonus(stat: Kingmaker.EntitySystem.Stats.StatType.Initiative, value: 4)
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

            var activatable = ActivatableAbilityConfigurator.New("HourglassStanceActivatable", "5433B2C6-0DE0-4429-83FE-C4CAEEBCDAF8")
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

            var feat = FeatureConfigurator.New("HourglassStanceFeat", Guid)
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