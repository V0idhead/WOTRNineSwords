using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.KingdomEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/absolute-steel--3641/
  static class AbsoluteSteel
  {
    public const string Guid = "435ED43C-7A2C-49F6-B0C6-774B779F6D48";
    const string name = "AbsoluteSteel.Name";
    const string desc = "AbsoluteSteel.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = FeatureRefs.FastMovement.Reference.Get().Icon;

      var triggeredBuff = BuffConfigurator.New("AbsoluteSteelMoveBuff", "68EBC7FD-C250-4DC7-B2AD-B5A4562D469B")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddACBonusAgainstAttacks(armorClassBonus: 2, descriptor: ModifierDescriptor.Dodge)
        .Configure();

      var buff = BuffConfigurator.New("AbsoluteSteelBuff", "A11F1829-D5D5-48E6-AA72-C7E6691F9EBC")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddNotDispelable()
        .AddBuffMovementSpeed(value: 10)
        .AddMovementDistanceTrigger(distanceInFeet: 10,
          action: ActionsBuilder.New().ApplyBuff(triggeredBuff, ContextDuration.Fixed(1), toCaster: true)
        )
        .Configure();

      var activatable = ActivatableAbilityConfigurator.New("AbsoluteSteelActivatable", "215664FE-47B9-40F1-B763-9416BC79C549")
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

      var feat = FeatureConfigurator.New("AbsoluteSteelFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { activatable })
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl3Guid)
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.IronHeartGuids.Except([Guid]).ToList())
#endif
        .Configure(true);
    }
  }
}