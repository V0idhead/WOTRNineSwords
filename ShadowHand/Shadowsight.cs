using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.ShadowHand
{
    static class Shadowsight
    {
        public const string Guid = "8E76142A-3158-4B25-B050-074E37A8446F";
        const string name = "Shadowsight.Name";
        const string icon = Helpers.IconPrefix + "shadowsight.png";

        public static void Configure()
        {
            var self = BuffConfigurator.New("ShadowsightSelf", "42E9EDEC-3737-4205-ABF3-1D7D009B59A0")
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddNotDispelable()
              .AddFlatFootedIgnore()
              .AddBlindnessACCompensation()
              .AddRerollConcealment()
              .Configure();

            var activatable = ActivatableAbilityConfigurator.New("ShadowsightActivatable", "A069331D-B5FC-43B7-8C7B-23B313F95753")
              .SetDisplayName(name)
              .SetDescription("Shadowsight.Desc")
              .SetIcon(icon)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetBuff(self)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetGroup(ActivatableAbilityGroup.CombatStyle)
              .SetWeightInGroup(1)
              .Configure();

            FeatureConfigurator.New("ShadowsightFeat", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription("Shadowsight.Desc")
              .SetIcon(icon)
              .SetRanks(1)
              .AddFacts(new() { activatable })
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.ShadowHandProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl2Guid)
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.ShadowHandGuids.Except([Guid]).ToList())
#endif
              .Configure(true);
        }
    }
}