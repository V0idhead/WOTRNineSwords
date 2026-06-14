using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.Swordsage.RimeRavager
{
    static class BorealisStance
    {
        public const string Guid = "0C2ABE80-D308-483C-9FA7-2C09DBC090B7";
        const string name = "BorealisStance.Name";
        //const string icon = Helpers.IconPrefix + "borealisStance.png";
        static Sprite icon = AbilityRefs.Flare.Reference.Get().Icon;

        public static BlueprintFeature Configure()
        {
            var borealisStanceBuff1 = BuffConfigurator.New("BorealisStanceBuff1", "3E8DF091-D5BB-4842-B90C-287D160198EA")
              .SetDisplayName(name)
              .SetDescription("BorealisStance.Buff1Desc")
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Penalty, null, Kingmaker.EntitySystem.Stats.StatType.Speed, -20)
              .Configure();

            var borealisStanceArea1 = AbilityAreaEffectConfigurator.New("BorealisStanceArea1", "9827E42E-226F-4F19-A3E5-D10B87F38F98")
              .AddAbilityAreaEffectRunAction(round: ActionsBuilder.New().ApplyBuff(borealisStanceBuff1, ContextDuration.Fixed(1)))
              .SetShape(Kingmaker.UnitLogic.Abilities.Blueprints.AreaEffectShape.Cylinder)
              .SetSize(new Feet(100))
              .SetTargetType(Kingmaker.UnitLogic.Abilities.Blueprints.BlueprintAbilityAreaEffect.TargetType.Enemy)
              .Configure();

            var borealisStanceBuff2 = BuffConfigurator.New("BorealisStanceBuff2", "E90526FC-D0FD-433C-A44F-ACE7A13531ED")
              .SetDisplayName(name)
              .SetDescription("BorealisStance.Buff2Desc")
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Penalty, null, Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus, -2)
              .Configure();

            var borealisStanceArea2 = AbilityAreaEffectConfigurator.New("BorealisStanceArea2", "76D1FB0C-5187-4A16-B439-4F6CC0BA0DAD")
              .AddAbilityAreaEffectRunAction(round: ActionsBuilder.New().ApplyBuff(borealisStanceBuff2, ContextDuration.Fixed(1)))
              .SetShape(Kingmaker.UnitLogic.Abilities.Blueprints.AreaEffectShape.Cylinder)
              .SetSize(new Feet(10))
              .SetTargetType(Kingmaker.UnitLogic.Abilities.Blueprints.BlueprintAbilityAreaEffect.TargetType.Enemy)
              .Configure();

            var borealisStanceSelf = BuffConfigurator.New("BorealisStanceSelf", "760FE1CF-3853-4DF7-A1FB-56F0DF7BFC88")
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddNotDispelable()
              .AddAreaEffect(borealisStanceArea1)
              .AddAreaEffect(borealisStanceArea2)
              .Configure();

            var borealisStanceActivatable = ActivatableAbilityConfigurator.New("BorealisStanceActivatable", "08131503-87B4-40ED-92C1-C94506207B4D")
              .SetDisplayName(name)
              .SetDescription("BorealisStance.Desc")
              .SetIcon(icon)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetBuff(borealisStanceSelf)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetGroup(ActivatableAbilityGroup.CombatStyle)
              .SetWeightInGroup(1)
              .Configure();

            return FeatureConfigurator.New("BorealisStanceFeat", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription("BorealisStance.Desc")
              .SetIcon(icon)
              .SetRanks(1)
              .AddFacts(new() { borealisStanceActivatable })
              .Configure();
        }
    }
}