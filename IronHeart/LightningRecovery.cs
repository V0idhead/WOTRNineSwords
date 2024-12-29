using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Counters;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/lightning-recovery--3651/
  static class LightningRecovery
  {
    public const string Guid = "3EACEFEA-C1E5-45E0-87EF-FAACB5D5BFC4";
    public static BlueprintActivatableAbility Activatable { get; private set; }
    const string name = "LightningRecovery.Name";
    const string desc = "LightningRecovery.Desc";
    const string icon = Helpers.IconPrefix + "lightningrecovery.png";

    public static BlueprintBuff AttackBuff;

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static void Configure()
    {
      Main.Log($"Configuring {nameof(LightningRecovery)}");

      AttackBuff = BuffConfigurator.New("LightningRecoveryAttackBuff", "2D96D2BF-A1A5-46D2-9184-879931B5824D")
        //.SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .SetIcon(icon)
        .SetDisplayName(name)
        .SetDescription(desc)
        .AddAttackBonus(2)
        .Configure();

      var toggleBuff = BuffConfigurator.New("LightningRecoveryOn", "046586FD-E970-43D5-8448-F6A8C44C433B")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddComponent<LightningRecoveryCounter>()
        .Configure();

      Activatable = ActivatableAbilityConfigurator.New("LightningRecoveryActivatable", "71A0E0F3-DCDA-48EF-878B-2A956436A2D1")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetActivationType(AbilityActivationType.Immediately)
        .SetDeactivateIfOwnerDisabled()
        .SetDeactivateIfOwnerUnconscious()
        .SetDoNotTurnOffOnRest()
        .SetBuff(toggleBuff)
        .Configure();

      var feat = FeatureConfigurator.New("LightningRecoveryFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { Activatable })
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl4Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.IronHeartGuids.Except([Guid]).ToList())
#endif
        .Configure(true);
    }
  }
}