using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Counters;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/wall-blades--3661/
  static class WallOfBlades
  {
    public const string Guid = "C5A6FA90-91C5-417A-8926-CD19A4E3416F";
    public const string ActiveBuffGuid = "69E45CF2-533F-4150-B957-70D29609F59B";
    const string FactGuid = "10B5E376-A515-4D00-A77E-EF0493761DA4";
    public static BlueprintUnitFact Fact { get; private set; }
    public static BlueprintActivatableAbility Activatable { get; private set; }
    const string name = "WallOfBlades.Name";
    const string desc = "WallOfBlades.Desc";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.BladeBarrier.Reference.Get().Icon;

      log.Info($"Configuring {nameof(WallOfBlades)}");

      Fact = UnitFactConfigurator.New("WallOfBladesActiveFact", FactGuid)
        .Configure();

      var activeBuff = BuffConfigurator.New("WallOfBladesActiveBuff", ActiveBuffGuid)
        .AddFacts(new() { Fact })
        .SetDisplayName(name)
        .SetDescription("WallOfBladesBuff.Desc")
        .SetIcon(icon)
        .Configure();

      var toggleBuff = BuffConfigurator.New("WallOfBladesOn", "64C038CE-61B4-4762-9F77-7623C863CC62")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddComponent<WallOfBladesCounter>()
        .Configure();

      Activatable = ActivatableAbilityConfigurator.New("WallOfBladesActivatable", "4F7886ED-D3BB-4EBD-BE8D-42B6A9F38223")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetActivationType(AbilityActivationType.Immediately)
        .SetDeactivateIfOwnerDisabled()
        .SetDeactivateIfOwnerUnconscious()
        .SetDoNotTurnOffOnRest()
        .SetBuff(toggleBuff)
        .Configure();

      var feat = FeatureConfigurator.New("WallOfBladesFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Defense | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { Activatable })
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl2Guid)
#endif
        .Configure(true);
    }
  }
}