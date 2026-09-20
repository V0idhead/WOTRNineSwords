using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoidHeadWOTRNineSwords.Common;

namespace VoidHeadWOTRNineSwords.DiamondMind
{
    static class DiamondLockStance
    {
        public const string Guid = "EC46E70E-720B-4320-B18D-85919EBC4CAC";
        const string name = "DiamondLockStance.Name";
        const string desc = "DiamondLockStance.Desc";
        //const string icon = Helpers.IconPrefix + "diamondlockstance.png";
        static Sprite icon = AbilityRefs.CausticEruption.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Log($"Configuring {nameof(DiamondLockStance)}");

            var buff = BuffConfigurator.New("DiamondLockStanceBuff", "6DEE62B2-B9D6-470E-9E5A-F3F7BD3892EF")
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddNotDispelable()
              .AddSpellImmunity(type: Kingmaker.UnitLogic.Parts.SpellImmunityType.SingleTarget)
              .Configure();

            var activatable = ActivatableAbilityConfigurator.New("DiamondLockStanceActivatable", "A21DC6AA-14B8-4B5C-BF27-999C8F80CF1B")
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

            var feat = FeatureConfigurator.New("DiamondLockStanceFeat", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Defense | FeatureTag.Melee)
              .SetRanks(1)
              .AddFacts(new() { activatable })
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.DiamondMindProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl9Guid)
              .AddPrerequisiteFeaturesFromList(amount: 4, features: AllManeuversAndStances.DiamondMindGuids.Except([Guid]).ToList())
#endif
              .Configure(true);
        }
    }
}