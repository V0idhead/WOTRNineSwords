using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using VoidHeadWOTRNineSwords.Common;
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
    const string icon = Helpers.IconPrefix + "punishingstance.png";

    public static void Configure()
    {
      var punishingStanceBuff = BuffConfigurator.New("PunishingStanceBuff", "58BBA9F5-13B5-4878-8C67-D31608AF58BE")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddNotDispelable()
        .AddInitiatorAttackRollTrigger(onlyHit: true, action:
            ActionsBuilder.New().DealDamage(new DamageTypeDescription { Type = DamageType.Untyped }, new ContextDiceValue { DiceType = DiceType.D6, DiceCountValue = 1, BonusValue = 0 })
            .Build())
        .AddACBonusAgainstAttacks(armorClassBonus: -2, descriptor: ModifierDescriptor.Penalty)
        .AddAreaEffect(IronHeartAura.IronHeartAuraArea)
        .Configure();

      var punishingStanceActivatable = ActivatableAbilityConfigurator.New("PunishingStanceActivatable", "7990BAB5-27BC-4422-829F-8D9B411A691C")
        .SetDisplayName(name)
        .SetDescription("PunishingStance.Description")
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
        .AddFacts(new() { punishingStanceActivatable })
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.IronHeartProficencyGuid, hideInUI: true)
#endif
        .Configure(true);
    }
  }
}