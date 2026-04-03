using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System.Collections.Generic;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Counters;
using VoidHeadWOTRNineSwords.Swordsage;

namespace VoidHeadWOTRNineSwords.DesertWind
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/leaping-flame--3583/
    static class LeapingFlame
    {
        public const string Guid = "7F4B15B8-1E1E-475C-9799-6AC22D46F319";
        public const string ActiveBuffGuid = "C018D16A-0536-4BE6-8B52-706F3ADB322F";
        const string ActiveFactGuid = "D92D3639-81E5-48DA-A6DE-4A6BA26DCF74";
        public static BlueprintUnitFact ActiveFact { get; private set; }
        const string OnFactGuid = "32BD50BC-0E55-45C9-A4B5-A1E572333D0F";
        public static BlueprintUnitFact OnFact { get; private set; }
        public static BlueprintActivatableAbility Activatable { get; private set; }
        const string name = "LeapingFlame.Name";
        const string desc = "LeapingFlame.Desc";
        //const string icon = Helpers.IconPrefix + "manticoreparry.png";
        static UnityEngine.Sprite icon = AbilityRefs.FlareBurst.Reference.Get().Icon;

        private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

        public static void Configure()
        {
            log.Info($"Configuring {nameof(LeapingFlame)}");

            ActiveFact = UnitFactConfigurator.New("LeapingFlameActiveFact", ActiveFactGuid).Configure();
            OnFact = UnitFactConfigurator.New("LeapingFlameOnFact", OnFactGuid).Configure();

            var activeBuff = BuffConfigurator.New("LeapingFlameActiveBuff", ActiveBuffGuid)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddFacts(new() { ActiveFact })
              .SetIcon(icon)
              .Configure();

            var toggleBuff = BuffConfigurator.New("LeapingFlameOn", "A019CC30-0D60-4186-8D3C-79D2A3E7F655")
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddComponent<LeapingFlameCounter>()
              .AddFacts(new List<Blueprint<Kingmaker.Blueprints.BlueprintUnitFactReference>> { OnFact })
              .Configure();

            Activatable = ActivatableAbilityConfigurator.New("LeapingFlameActivatable", "CA200903-2A26-403C-9B07-3F228AB8A9AF")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetBuff(toggleBuff)
              .Configure();

            var feat = FeatureConfigurator.New("LeapingFlameFeat", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Defense | FeatureTag.Melee)
              .SetRanks(1)
              .AddPrerequisiteClassLevel(SwordsageC.Guid, 1, hideInUI: true)
              .AddFacts(new() { Activatable })
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
              .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.DesertWindGuids.Except([Guid]).ToList())
#endif
              .Configure(true);
        }
    }
}