using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.StoneDragon
{
    static class StoneshellStance
    {
        public const string Guid = "77AC2973-7BD7-4F92-B9C6-F831844A42F3";
        const string name = "StoneshellStance.Name";
        const string desc = "StoneshellStance.Desc";
        //const string icon = Helpers.IconPrefix + "stoneshellstance.png";
        static Sprite icon = AbilityRefs.Flare.Reference.Get().Icon;

        public static void Configure()
        {
            var buff = BuffConfigurator.New("StoneshellStanceBuff", "C29841BA-D091-431F-8E0B-65D7AFE4FDAA")
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddNotDispelable()
              .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.NaturalArmorEnhancement, stat: Kingmaker.EntitySystem.Stats.StatType.AC, value: 2)
              .AddACBonusAgainstAttackOfOpportunity(ContextValues.Constant(2))
              .Configure();

            var activatable = ActivatableAbilityConfigurator.New("StoneshellStanceActivatable", "78F6C959-BCED-4886-AB98-B61FEE02D236")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              //.AddComponent(new AbilityCasterHasWeaponSubcategory(WeaponSubCategory.Melee)) // doesn't work
              .SetActivationType(AbilityActivationType.Immediately)
              .SetBuff(buff)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetGroup(ActivatableAbilityGroup.CombatStyle)
              .SetWeightInGroup(1)
              .Configure();

            var punishingStanceFeat = FeatureConfigurator.New("StoneshellStanceFeat", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .SetRanks(1)
              .AddFacts(new() { activatable })
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.StoneDragonProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl2Guid)
              .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.StoneDragonGuids.Except([Guid]).ToList())
#endif
              .Configure(true);
        }
    }
}