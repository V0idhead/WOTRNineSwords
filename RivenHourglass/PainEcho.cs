using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Counters;
using VoidHeadWOTRNineSwords.DesertWind;

namespace VoidHeadWOTRNineSwords.RivenHourglass
{
    static class PainEcho
    {
        public const string Guid = "0E93E17E-CD62-49B8-B8B1-21573D486D77";
        public const string ActiveBuffGuid = "C47351BD-D702-4CFF-9FBB-9D1BE4168C81";
        const string ActiveFactGuid = "47B064FC-61F0-4351-8723-139FE75C3E02";
        public static BlueprintUnitFact ActiveFact { get; private set; }
        const string OnFactGuid = "A8E08DAE-D587-4473-8B76-A6F707D8EE00";
        public static BlueprintUnitFact OnFact { get; private set; }
        public static BlueprintActivatableAbility Activatable { get; private set; }
        public const string ImageBuffGuid = "1668858E-A1D7-48B5-A102-8E33FFD9281B";
        const string name = "PainEcho.Name";
        const string desc = "PainEcho.Desc";
        //const string icon = Helpers.IconPrefix + "painecho.png";
        static Sprite icon = AbilityRefs.CausticEruption.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Log($"Configuring {nameof(PainEcho)}");

            ActiveFact = UnitFactConfigurator.New("PainEchoActiveFact", ActiveFactGuid).Configure();
            OnFact = UnitFactConfigurator.New("PainEchoOnFact", OnFactGuid).Configure();

            var mirrorBuff = BuffConfigurator.New("PainEchoImageBuff", ImageBuffGuid)
              .SetDisplayName(name)
              .SetDescription("PainEcho.BuffDesc")
              .SetIcon(icon)
              .AddMirrorImage(ContextDice.Value(Kingmaker.RuleSystem.DiceType.One, ContextValues.Constant(1)))
              .Configure();

            var activeBuff = BuffConfigurator.New("PainEchoeActiveBuff", ActiveBuffGuid)
              .AddFacts(new() { ActiveFact })
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              /*.SetDisplayName(name)
              .SetDescription("PainEchoe.BuffDesc")
              .SetIcon(icon)*/
              .Configure();

            var toggleBuff = BuffConfigurator.New("PainEchoeOn", "FA7A69F7-3213-4966-BDF0-6E55C359E636")
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddComponent<DamageTakenCounter>()
              .AddFacts(new List<Blueprint<Kingmaker.Blueprints.BlueprintUnitFactReference>> { OnFact })
              .Configure();

            Activatable = ActivatableAbilityConfigurator.New("PainEchoeActivatable", "B13C5EBC-76EB-4456-96E0-B3217E6842F1")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetBuff(toggleBuff)
              .Configure();

            var feat = FeatureConfigurator.New("PainEchoeFeat", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .SetRanks(1)
              .AddFacts(new() { Activatable })
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.RivenHourglassProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.RivenHourglassGuids.Except([Guid]).ToList())
#endif
              .Configure(true);
        }
    }
}