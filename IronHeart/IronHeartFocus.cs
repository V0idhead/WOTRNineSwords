using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Counters;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/iron-heart-focus--3649/
  static class IronHeartFocus
  {
    public const string Guid = "2ED160DA-FCDA-4AEA-A836-3007BAF58F7C";
    public const string ActiveBuffGuid = "B581F7DE-6E51-41F1-9BB7-7B3A27FDF758";
    const string FactGuid = "B429F97E-514B-40AD-910C-2E04778D8D9D";
    public static BlueprintUnitFact Fact { get; private set; }
    public static BlueprintActivatableAbility Activatable { get; private set; }
    const string name = "IronHeartFocus.Name";
    const string desc = "IronHeartFocus.Desc";
    const string icon = Helpers.IconPrefix + "ironheartfocus.png";

    public static void Configure()
    {
      Main.Log($"Configuring {nameof(IronHeartFocus)}");

      Fact = UnitFactConfigurator.New("IronHeartFocusActiveFact", FactGuid)
        .Configure();

      var activeBuff = BuffConfigurator.New("IronHeartFocusActiveBuff", ActiveBuffGuid)
        .AddFacts(new() { Fact })
        .SetDisplayName(name)
        .SetDescription("IronHeartFocusBuff.Desc")
        .SetIcon(icon)
        .Configure();

      var toggleBuff = BuffConfigurator.New("IronHeartFocusOn", "F66BCA3C-5FA7-431E-AA1B-0F4AC016648E")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddComponent<IronHeartFocusCounter>()
        .Configure();

      Activatable = ActivatableAbilityConfigurator.New("IronHeartFocusActivatable", "66FF48DD-4BEC-47B9-A00F-977073D6C0C3")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetActivationType(AbilityActivationType.Immediately)
        .SetDeactivateIfOwnerDisabled()
        .SetDeactivateIfOwnerUnconscious()
        .SetDoNotTurnOffOnRest()
        .SetBuff(toggleBuff)
        .Configure();

      var feat = FeatureConfigurator.New("IronHeartFocusFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Defense | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { Activatable })
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.IronHeartGuids.Except([Guid]).ToList())
#endif
        .Configure(true);
    }
  }
}