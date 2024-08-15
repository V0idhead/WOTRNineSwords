using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/supreme-blade-parry--3660/
  static class SupremeBladeParry
  {
    public const string Guid = "04BA699C-F2C7-4D29-946E-34EBFC3EA0DF";
    const string name = "SupremeBladeParry.Name";
    const string desc = "SupremeBladeParry.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = ActivatableAbilityRefs.FightDefensivelyToggleAbility.Reference.Get().Icon;

      var buff = BuffConfigurator.New("SupremeBladeParryBuff", "3A201493-D880-4458-8D5B-1D614DF423C3")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddNotDispelable()
        .AddDamageResistancePhysical(bypassedByMaterial: true, material: PhysicalDamageMaterial.Adamantite, value: 5)
        .Configure();

      var activatable = ActivatableAbilityConfigurator.New("SupremeBladeParryActivatable", "257A9F54-6D60-44BE-8617-50302324AD8D")
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

      var punishingStanceFeat = FeatureConfigurator.New("SupremeBladeParryFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { activatable })
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl8Guid)
        .AddPrerequisiteFeaturesFromList(amount: 3, features: AllManeuversAndStances.IronHeartGuids.Except([Guid]).ToList())
#endif
        .Configure(true);
    }
  }
}