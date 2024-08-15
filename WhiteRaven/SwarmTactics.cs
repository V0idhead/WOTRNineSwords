using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.WhiteRaven
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/swarm-tactics--3765/
  static class SwarmTactics
  {
    public const string Guid = "F5793148-0E96-457F-A84D-7053D832CAAD";
    const string name = "SwarmTactics.Name";
    const string desc = "SwarmTactics.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.ReducePersonMass.Reference.Get().Icon;

      var buff = BuffConfigurator.New("SwarmTacticsBuff", "6D75350C-DF32-4115-8BE5-B819725BBF5D")
        .SetDisplayName(name)
        .SetDescription("SwarmTactics.BuffDesc")
        .SetIcon(icon)
        .AddACBonusAgainstAttacks(armorClassBonus: -5)
        .Configure();

      var area = AbilityAreaEffectConfigurator.New("SwarmTacticsArea", "2E10670F-5140-42E6-920D-AEFF0A2336E9")
        .AddAbilityAreaEffectBuff(buff, false, ConditionsBuilder.New().IsEnemy())
        .SetShape(Kingmaker.UnitLogic.Abilities.Blueprints.AreaEffectShape.Cylinder)
        .SetSize(new Feet(5))
        .Configure();

      var selfBuff = BuffConfigurator.New("SwarmTacticsSelf", "9D642DEE-618B-4BF2-A295-D2596A247F19")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddNotDispelable()
        .AddAreaEffect(area)
        .Configure();

      var activatable = ActivatableAbilityConfigurator.New("SwarmTacticsActivatable", "C9356C6A-592D-4820-8948-AC710241F4D8")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        //.AddComponent(new AbilityCasterHasWeaponSubcategory(WeaponSubCategory.Melee)) // doesn't work
        .SetActivationType(AbilityActivationType.Immediately)
        .SetBuff(selfBuff)
        .SetDeactivateIfOwnerDisabled()
        .SetDeactivateIfOwnerUnconscious()
        .SetDoNotTurnOffOnRest()
        .SetGroup(ActivatableAbilityGroup.CombatStyle)
        .SetWeightInGroup(1)
        .Configure();

      var punishingStanceFeat = FeatureConfigurator.New("SwarmTacticsFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { activatable })
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl8Guid)
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.WhiteRavenGuids.Except([Guid]).ToList())
#endif
        .Configure(true);
    }
  }
}