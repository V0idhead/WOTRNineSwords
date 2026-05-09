using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System.Collections.Generic;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Counters;

namespace VoidHeadWOTRNineSwords.DesertWind
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/zephyr-dance--3592/
    static class ZephyrDance
    {
        public const string Guid = "CF5A5BE3-CD4C-452A-8992-30B81B2A0592";
        public const string ActiveBuffGuid = "D0B37483-8056-417B-98A2-B1F38AAC8119";
        const string ActiveFactGuid = "4272BB51-A7DB-4C42-927A-CEB6CED1BE5C";
        public static BlueprintUnitFact ActiveFact { get; private set; }
        const string OnFactGuid = "28FDDFE7-89AE-482D-B7A9-9E2C47AF81BC";
        public static BlueprintUnitFact OnFact { get; private set; }
        public static BlueprintActivatableAbility Activatable { get; private set; }
        const string name = "ZephyrDance.Name";
        const string desc = "ZephyrDance.Desc";
        const string icon = Helpers.IconPrefix + "zephyrdance.png";

        public static void Configure()
        {
            Main.Log($"Configuring {nameof(ZephyrDance)}");

            ActiveFact = UnitFactConfigurator.New("ZephyrDanceActiveFact", ActiveFactGuid).Configure();
            OnFact = UnitFactConfigurator.New("ZephyrDanceOnFact", OnFactGuid).Configure();

            var activeBuff = BuffConfigurator.New("ZephyrDanceActiveBuff", ActiveBuffGuid)
              .AddFacts(new() { ActiveFact })
              .SetDisplayName(name)
              .SetDescription("ZephyrDanceBuff.Desc")
              .SetIcon(icon)
              .Configure();

            var toggleBuff = BuffConfigurator.New("ZephyrDanceOn", "46C7BEA7-3A32-42F6-A528-08CA92FEFDAD")
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddComponent<ACIncreaseCounter>()
              .AddFacts(new List<Blueprint<Kingmaker.Blueprints.BlueprintUnitFactReference>> { OnFact })
              .Configure();

            Activatable = ActivatableAbilityConfigurator.New("ZephyrDanceActivatable", "71AC7549-ED13-4E72-84B1-473FD68DC365")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetBuff(toggleBuff)
              .Configure();

            var feat = FeatureConfigurator.New("ZephyrDanceFeat", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .SetRanks(1)
              .AddFacts(new() { Activatable })
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl3Guid)
              .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.DesertWindGuids.Except([Guid]).ToList())
#endif
              .Configure(true);
        }
    }
}