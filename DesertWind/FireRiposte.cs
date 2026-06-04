using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System.Collections.Generic;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Counters;

namespace VoidHeadWOTRNineSwords.DesertWind
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/fire-riposte--3575/
  static class FireRiposte
  {
    public const string Guid = "195364DE-9692-4219-839A-CD9B5DCF0BAF";
    public const string ActiveBuffGuid = "71AE88CF-DC45-4FF6-AF3E-EA082BFE2100";
    const string ActiveFactGuid = "F5E37C64-1507-4EBC-AF6C-A13B28399F39";
    public static BlueprintUnitFact ActiveFact { get; private set; }
    const string OnFactGuid = "F62B3FE1-81BB-4660-BC30-A93C90749178";
    public static BlueprintUnitFact OnFact { get; private set; }
    public static BlueprintActivatableAbility Activatable { get; private set; }
    const string name = "FireRiposte.Name";
    const string desc = "FireRiposte.Desc";
    const string icon = Helpers.IconPrefix + "firereposte.png";

    public static void Configure()
    {
      Main.Log($"Configuring {nameof(FireRiposte)}");

      ActiveFact = UnitFactConfigurator.New("FireRiposteActiveFact", ActiveFactGuid).Configure();
      OnFact = UnitFactConfigurator.New("FireRiposteOnFact", OnFactGuid).Configure();

      var activeBuff = BuffConfigurator.New("FireRiposteActiveBuff", ActiveBuffGuid)
        .AddFacts(new() { ActiveFact })
        .SetDisplayName(name)
        .SetDescription("FireRiposteBuff.Desc")
        .SetIcon(icon)
        .Configure();

      var toggleBuff = BuffConfigurator.New("FireRiposteOn", "A9AF5517-11EC-4CB2-B0CF-4267AB1E1363")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddComponent<UnifiedCounterAttackCounter>()
        .AddFacts(new List<Blueprint<Kingmaker.Blueprints.BlueprintUnitFactReference>> { OnFact })
        .Configure();

      Activatable = ActivatableAbilityConfigurator.New("FireRiposteActivatable", "BD511214-0FC0-4AE7-89DD-DDE0717C1CC8")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetActivationType(AbilityActivationType.Immediately)
        .SetDeactivateIfOwnerDisabled()
        .SetDeactivateIfOwnerUnconscious()
        .SetDoNotTurnOffOnRest()
        .SetBuff(toggleBuff)
        .Configure();

      var feat = FeatureConfigurator.New("FireRiposteFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .SetRanks(1)
        .AddFacts(new() { Activatable })
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl2Guid)
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.DesertWindGuids.Except([Guid]).ToList())
#endif
        .Configure(true);
    }
  }
}