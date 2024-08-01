using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
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
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/giants-stance--3718/
  static class GiantsStance
  {
    public const string Guid = "90435742-816D-455D-9D57-D35C7F77FC96";
    const string name = "GiantsStance.Name";
    const string desc = "GiantsStance.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.EnlargePerson.Reference.Get().Icon;

      var buff = BuffConfigurator.New("GiantsStanceBuff", "C7ECF16E-F69A-4743-B80E-A099A2E8A307")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddNotDispelable()
        .AddIncreaseDiceSizeOnAttack(1)
        .Configure();

      var activatable = ActivatableAbilityConfigurator.New("GiantsStanceActivatable", "683CACA2-C33C-43CD-AA79-F68CD81560A2")
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

      var punishingStanceFeat = FeatureConfigurator.New("GiantsStanceFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { activatable })
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.StoneDragonGuids.Except([Guid]).ToList())
#endif
        .Configure(true);
    }
  }
}