using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.DiamondMind
{
    static class DiamondShimmerStance
    {
        public const string Guid = "F416BEC6-35EC-45DB-B8C8-C4D8C35C310C";
        const string name = "DiamondShimmerStance.Name";
        const string desc = "DiamondShimmerStance.Desc";
        const string icon = Helpers.IconPrefix + "diamonshimmerstance.png";

        public static void Configure()
        {
            Main.Log($"Configuring {nameof(DiamondShimmerStance)}");

            var buff = BuffConfigurator.New("DiamondShimmerStanceBuff", "ED3B179F-B788-4538-9F6B-C6AFB733B12E")
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddNotDispelable()
              .AddSpellResistance(allSpellResistancePenaltyDoNotUse: false, value: ContextValues.Constant(21))
              .Configure();

            var activatable = ActivatableAbilityConfigurator.New("DiamondShimmerStanceActivatable", "389456A1-1295-461C-A78E-0A3761AB94CF")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetBuff(buff)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetGroup(ActivatableAbilityGroup.CombatStyle)
              .SetWeightInGroup(1)
              .Configure();

            var feat = FeatureConfigurator.New("DiamondShimmerStanceFeat", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Defense | FeatureTag.Melee)
              .SetRanks(1)
              .AddFacts(new() { activatable })
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.DiamondMindProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl7Guid)
              .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.DiamondMindGuids.Except([Guid]).ToList())
#endif
              .Configure(true);
        }
    }
}