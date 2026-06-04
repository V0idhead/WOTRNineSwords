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
using VoidHeadWOTRNineSwords.Swordsage;

namespace VoidHeadWOTRNineSwords.ShadowHand
{
    static class Shadowcloak
    {
        public const string Guid = "FB826C0A-1A27-4627-A7FF-869984DC7BD1";

        public const string Active1BuffGuid = "74AAC6AE-688F-454F-BD26-BC3AEC445E90";
        const string Active1FactGuid = "B3B46449-7793-4EEC-8091-8C15A1D8B75E";
        public static BlueprintUnitFact Active1Fact { get; private set; }

        public const string Active2BuffGuid = "1A1588A1-3E75-4950-9258-F9060C3B2126";
        const string Active2FactGuid = "CC23AEFE-FF25-4CB2-9863-6D5EEB76DEE8";
        public static BlueprintUnitFact Active2Fact { get; private set; }

        const string OnFactGuid = "4A51DF8D-0AAF-4854-924B-3512A4102D16";
        public static BlueprintUnitFact OnFact { get; private set; }
        public static BlueprintActivatableAbility Activatable { get; private set; }
        const string name = "Shadowcloak.Name";
        const string desc = "Shadowcloak.Desc";
        const string icon = Helpers.IconPrefix + "shadowcloak.png";

        private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

        public static void Configure()
        {
            log.Info($"Configuring {nameof(Shadowcloak)}");

            Active1Fact = UnitFactConfigurator.New("ShadowcloakActive1Fact", Active1FactGuid).Configure();
            Active2Fact = UnitFactConfigurator.New("ShadowcloakActive2Fact", Active2FactGuid).Configure();
            OnFact = UnitFactConfigurator.New("ShadowcloakOnFact", OnFactGuid).Configure();

            var active1Buff = BuffConfigurator.New("ShadowcloakActive1Buff", Active1BuffGuid)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddFacts(new() { Active1Fact })
              .SetIcon(icon)
              .Configure();

            var active2Buff = BuffConfigurator.New("ShadowcloakActive2Buff", Active2BuffGuid)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddFacts(new() { Active2Fact })
              .SetIcon(icon)
              .Configure();

            var toggleBuff = BuffConfigurator.New("ShadowcloakOn", "547701D2-B793-40E0-97BF-69BA3120430E")
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddComponent<ShadowcloakCounter>()
              .AddFacts(new List<Blueprint<Kingmaker.Blueprints.BlueprintUnitFactReference>> { OnFact })
              .Configure();

            Activatable = ActivatableAbilityConfigurator.New("ShadowcloakActivatable", "68102A64-AFCD-4FD0-BCDB-8C80312DACAE")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetBuff(toggleBuff)
              .Configure();

            var feat = FeatureConfigurator.New("ShadowcloakFeat", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Defense | FeatureTag.Melee)
              .SetRanks(1)
              .AddPrerequisiteClassLevel(SwordsageC.Guid, 1, hideInUI: true)
              .AddFacts(new() { Activatable })
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.ShadowHandProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
              .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.ShadowHandGuids.Except([Guid]).ToList())
#endif
              .Configure(true);
        }
    }
}