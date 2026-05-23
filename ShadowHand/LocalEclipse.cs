using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.ShadowHand
{
    static class LocalEclipse
    {
        public const string Guid = "D44B8DF8-C608-4A2D-A1FF-B17361E22220";
        const string name = "LocalEclipse.Name";
        const string icon = Helpers.IconPrefix + "localeclipse.png";

        public static void Configure()
        {
            var area = AbilityAreaEffectConfigurator.New("LocalEclipseArea", "FA05B6CB-5408-4D5B-86B2-AB51B382B0ED")
              .AddAbilityAreaEffectRunAction(round: ActionsBuilder.New().ApplyBuff(BuffRefs.Blind.Reference.Get(), ContextDuration.Fixed(1)))
              .SetShape(Kingmaker.UnitLogic.Abilities.Blueprints.AreaEffectShape.Cylinder)
              .SetSize(new Feet(20))
              .SetTargetType(Kingmaker.UnitLogic.Abilities.Blueprints.BlueprintAbilityAreaEffect.TargetType.Enemy)
              .Configure();

            var self = BuffConfigurator.New("LocalEclipseSelf", "0EB19250-FFA4-475E-B2C1-5765100E8049")
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddNotDispelable()
              .AddAreaEffect(area)
              .Configure();

            var activatable = ActivatableAbilityConfigurator.New("LocalEclipseActivatable", "B7D42D17-F0C1-4214-B68F-452A7B4A1B36")
              .SetDisplayName(name)
              .SetDescription("LocalEclipse.Desc")
              .SetIcon(icon)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetBuff(self)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetGroup(ActivatableAbilityGroup.CombatStyle)
              .SetWeightInGroup(1)
              .Configure();

            FeatureConfigurator.New("LocalEclipseFeat", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription("LocalEclipse.Desc")
              .SetIcon(icon)
              .SetRanks(1)
              .AddFacts(new() { activatable })
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.ShadowHandProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl6Guid)
              .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.ShadowHandGuids.Except([Guid]).ToList())
#endif
              .Configure(true);
        }
    }
}