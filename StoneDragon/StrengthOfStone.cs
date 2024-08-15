using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.StoneDragon
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/strength-stone--3730/
  static class StrengthOfStone
  {
    public const string Guid = "3FB94C21-D461-4075-AD83-3E1725CE6C40";
    const string name = "StrengthOfStone.Name";
    const string desc = "StrengthOfStone.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.Stoneskin.Reference.Get().Icon;

      var buff = BuffConfigurator.New("StrengthOfStoneBuff", "974E119B-9315-4D16-8A4B-C988191CAD7B")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddNotDispelable()
        .AddImmunityToCriticalHits()
        .Configure();

      var activatable = ActivatableAbilityConfigurator.New("StrengthOfStoneActivatable", "E556CA60-5256-421B-95E6-D25DA564D59E")
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

      var punishingStanceFeat = FeatureConfigurator.New("StrengthOfStoneFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { activatable })
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl8Guid)
        .AddPrerequisiteFeaturesFromList(amount: 3, features: AllManeuversAndStances.StoneDragonGuids.Except([Guid]).ToList())
#endif
        .Configure(true);
    }
  }
}