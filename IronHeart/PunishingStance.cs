using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UI.MVVM._VM.ServiceWindows.MythicInfo;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/punishing-stance--3655/
  static class PunishingStance
  {
    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public const string Guid = "5708B72C-77AB-4D01-B6FA-6C3C80F3A9BD";
    const string name = "PunishingStance.Name";

    public static void Configure()
    {
      UnityEngine.Sprite icon = FeatureRefs.DamageReduction.Reference.Get().Icon;

      var punishingStanceBuff = BuffConfigurator.New("PunishingStanceBuff", "58BBA9F5-13B5-4878-8C67-D31608AF58BE")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddNotDispelable()
        //.SetDisplayName(name)
        //.SetIcon(icon)
        //.AdditionalDiceOnDamage(diceValue: new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 1 }) //does nothing
        .AddDamageBonusConditional(bonus: new ContextValue {Value = 4 }, descriptor: ModifierDescriptor.UntypedStackable) //TODO: damage bonus should be 1d6 ?AdditionalDiceOnDamage?
        //.AddDamageBonusConditional(bonus: ContextValue., descriptor: ModifierDescriptor.UntypedStackable) //TODO: damage bonus should be 1d6
        .AddACBonusAgainstAttacks(armorClassBonus: -2)
        .AddAreaEffect(IronHeartAura.IronHeartAuraArea)
        .Configure();

      var punishingStanceActivatable = ActivatableAbilityConfigurator.New("PunishingStanceActivatable", "7990BAB5-27BC-4422-829F-8D9B411A691C")
        .SetDisplayName(name)
        .SetDescription("PunishingStanceBuff.Description")
        .SetIcon(icon)
        //.AddComponent(new AbilityCasterHasWeaponSubcategory(WeaponSubCategory.Melee)) // doesn't work
        .SetActivationType(AbilityActivationType.Immediately)
        .SetBuff(punishingStanceBuff)
        .SetDeactivateIfOwnerDisabled()
        .SetDeactivateIfOwnerUnconscious()
        .SetDoNotTurnOffOnRest()
        .SetGroup(ActivatableAbilityGroup.CombatStyle)
        .SetWeightInGroup(1)
        .Configure();

      var punishingStanceFeat = FeatureConfigurator.New("PunishingStanceFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription("PunishingStance.Description")
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { punishingStanceActivatable })
        .Configure(true);
    }
  }
}