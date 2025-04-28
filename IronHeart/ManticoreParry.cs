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
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/manticore-parry--3653/
  static class ManticoreParry
  {
    public const string Guid = "605A4C0D-AE90-44D6-AC4F-781C0DE20A7B";
    public const string ActiveBuffGuid = "4D09B867-9CF1-479A-9AB0-3EEBA06315C4";
    const string ActiveFactGuid = "F257A392-6FB1-4DF7-93B0-51A25E7C701C";
    public static BlueprintUnitFact ActiveFact { get; private set; }
    const string OnFactGuid = "4BE82298-30A3-4169-94C1-21C6AA0570F6";
    public static BlueprintUnitFact OnFact { get; private set; }
    public static BlueprintActivatableAbility Activatable { get; private set; }
    const string name = "ManticoreParry.Name";
    const string desc = "ManticoreParry.Desc";
    const string icon = Helpers.IconPrefix + "manticoreparry.png";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static void Configure()
    {
      log.Info($"Configuring {nameof(ManticoreParry)}");

      ActiveFact = UnitFactConfigurator.New("ManticoreParryActiveFact", ActiveFactGuid).Configure();
      OnFact = UnitFactConfigurator.New("MaticoreParryOnFact", OnFactGuid) .Configure();

      var activeBuff = BuffConfigurator.New("ManticoreParryActiveBuff", ActiveBuffGuid)
        .AddFacts(new() { ActiveFact })
        .SetDisplayName(name)
        .SetDescription("ManticoreParryBuff.Desc")
        .SetIcon(icon)
        .Configure();

      var toggleBuff = BuffConfigurator.New("ManticoreParryOn", "2E106DCE-4B2F-4BCA-B69B-9B0E056D4DB7")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        //.AddComponent<UnifiedParryCounter>()
        .AddComponent(UnifiedParryCounter.Singleton)
        //.AddUniqueComponent(UnifiedParryCounter.Singleton, BlueprintCore.Blueprints.CustomConfigurators.ComponentMerge.Fail)
        .AddFacts(new List<Blueprint<Kingmaker.Blueprints.BlueprintUnitFactReference>> { OnFact })
        .Configure();

      Activatable = ActivatableAbilityConfigurator.New("ManticoreParryActivatable", "8427162E-BFBB-4576-9948-D7C3FDA4868B")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetActivationType(AbilityActivationType.Immediately)
        .SetDeactivateIfOwnerDisabled()
        .SetDeactivateIfOwnerUnconscious()
        .SetDoNotTurnOffOnRest()
        .SetBuff(toggleBuff)
        .Configure();

      var feat = FeatureConfigurator.New("ManticoreParryFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Defense | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { Activatable })
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl6Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.IronHeartGuids.Except([Guid]).ToList())
#endif
        .Configure(true);
    }
  }
}