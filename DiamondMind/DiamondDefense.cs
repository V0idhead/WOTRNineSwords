using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.ActivatableAbilities;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.DiamondMind
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/diamond-defense--3622/
  static class DiamondDefense
  {
    public const string Guid = "1DB045E7-2C60-44AD-8A51-40130DCDEB00";
    public const string ActiveBuffGuid = "302D1897-7423-42AB-A0B7-C37903212BF8";
    const string name = "DiamondDefense.Name";
    const string desc = "DiamondDefense.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = FeatureRefs.KiDiamondBodyFeature.Reference.Get().Icon;

      Main.Log($"Configuring {nameof(DiamondDefense)}");

      var selfBuff = BuffConfigurator.New("DiamondDefenseBuff", "1B68A9CB-3F63-4AE3-97DB-D0BE16176A9B")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddBuffAllSavesBonus(Kingmaker.Enums.ModifierDescriptor.UntypedStackable, 9)
        .Configure();

      var activatable = ActivatableAbilityConfigurator.New("DiamondDefenseActivatable", "E1074884-3038-4656-9774-FB868C3D3DA6")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetActivationType(AbilityActivationType.Immediately)
        .SetDeactivateIfOwnerDisabled()
        .SetDeactivateIfOwnerUnconscious()
        .SetDeactivateIfCombatEnded()
        .SetOnlyInCombat()
        .SetBuff(selfBuff)
        .AddActivatableAbilityResourceLogic(requiredResource: WarbladeC.ManeuverResourceGuid, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.NewRound)
        .Configure();

      var feat = FeatureConfigurator.New("DiamondDefenseFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Defense | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { activatable })
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl8Guid)
#endif
        .Configure(true);
    }
  }
}