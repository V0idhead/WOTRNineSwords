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

namespace VoidHeadWOTRNineSwords.DiamondMind
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/hearing-air--3626/
  static class HearingTheAir
  {
    public const string Guid = "3737CB58-3622-48A9-9C88-D3A2557E3E9E";
    const string name = "HearingTheAir.Name";
    const string desc = "HearingTheAir.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.Echolocation.Reference.Get().Icon;

      var buff = BuffConfigurator.New("HearingTheAirBuff", "44A094FB-2C1F-42DF-9D12-E0729EE71D1E")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddNotDispelable()
        .AddComponent(new Blindsense { Range=new Kingmaker.Utility.Feet(30), Blindsight=true})
        .AddBuffSkillBonus(Kingmaker.EntitySystem.Stats.StatType.SkillPerception, 5, Kingmaker.Enums.ModifierDescriptor.Insight)
        .Configure();

      var activatable = ActivatableAbilityConfigurator.New("HearingTheAirActivatable", "A660EF7B-09BB-48CD-8D1B-D1A33381DB05")
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

      var punishingStanceFeat = FeatureConfigurator.New("HearingTheAirFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { activatable })
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.DiamondMindGuids.Except([Guid]).ToList())
#endif
        .Configure(true);
    }
  }
}