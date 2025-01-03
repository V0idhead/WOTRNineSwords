﻿using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/dancing-blade-form--3643/
  static class DancingBladeForm
  {
    public const string Guid = "626C3691-D976-4D2B-AAB3-7E8C3AEB33C9";
    const string name = "DancingBladeForm.Name";
    const string desc = "DancingBladeForm.Desc";
    const string icon = Helpers.IconPrefix + "dancingbladeform.png";

    public static void Configure()
    {
      var buff = BuffConfigurator.New("DancingBladeFormBuff", "185FFE35-7837-4BBC-BED9-652EC68D4657")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddNotDispelable()
        .AddStatBonus(ModifierDescriptor.UntypedStackable, stat: Kingmaker.EntitySystem.Stats.StatType.Reach, value: 5)
        .AddBuffMovementSpeed(value: 10)
        //.AddReachMultiplicator(ModifierDescriptor.Competence, multiplicator: 5) //TODO: doesn't seem to do anything
        .AddAreaEffect(IronHeartAura.IronHeartAuraArea)
        .Configure();

      var activatable = ActivatableAbilityConfigurator.New("DancingBladeFormActivatable", "1B6A50ED-6BD0-4A7E-9231-3B54F90B1F34")
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

      var feat = FeatureConfigurator.New("DancingBladeFormFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { activatable })
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.IronHeartGuids.Except([Guid]).ToList())
#endif
        .Configure(true);
    }
  }
}