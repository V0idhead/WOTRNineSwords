using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.ShadowHand
{
    internal class ThickShadows
    {
        public const string Guid = "83612CDB-BE5D-4D11-85C1-46E08A66811A";
        const string name = "ThickShadows.Name";
        const string icon = Helpers.IconPrefix + "thickshadows.png";

        public static void Configure()
        {
            var deBuff = BuffConfigurator.New("ThickShadowsDeBuff", "E897B54B-A72C-42E6-B1AA-68583CAFC28D")
              .SetDisplayName(name)
              .SetDescription("ThickShadows.BuffDesc")
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Penalty, null, Kingmaker.EntitySystem.Stats.StatType.Strength, -2)
              .AddStatBonus(ModifierDescriptor.Penalty, null, Kingmaker.EntitySystem.Stats.StatType.Dexterity, -2)
              .Configure();

            var area = AbilityAreaEffectConfigurator.New("ThickShadowsArea", "BF6E2E78-2CED-4D8B-A720-44AE62834F5C")
              //.AddAbilityAreaEffectBuff(deBuff, false, ConditionsBuilder.New().IsEnemy())
              .AddAbilityAreaEffectRunAction(round: ActionsBuilder.New().ApplyBuff(deBuff, ContextDuration.Fixed(1)))
              .SetShape(Kingmaker.UnitLogic.Abilities.Blueprints.AreaEffectShape.Cylinder)
              .SetSize(new Feet(40))
              .SetTargetType(Kingmaker.UnitLogic.Abilities.Blueprints.BlueprintAbilityAreaEffect.TargetType.Enemy)
              .Configure();

            var self = BuffConfigurator.New("ThickShadowsSelf", "E24A9FC9-6134-42FE-9706-8990FDD1D94B")
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddNotDispelable()
              .AddAreaEffect(area)
              .AddStatBonus(ModifierDescriptor.Penalty, null, Kingmaker.EntitySystem.Stats.StatType.Strength, 2)
              .AddStatBonus(ModifierDescriptor.Penalty, null, Kingmaker.EntitySystem.Stats.StatType.Dexterity, 2)
              .AddStatBonusIfHasFactFixed(new BlueprintCore.Blueprints.Components.Replacements.AddStatBonusIfHasFactFixed(Kingmaker.EntitySystem.Stats.StatType.AC, ContextValues.Constant(2), [ShadowPresence.ShadowHandFocusFactGuid], descriptor: Kingmaker.Enums.ModifierDescriptor.NaturalArmorEnhancement))
              .Configure();

            var activatable = ActivatableAbilityConfigurator.New("ThickShadowsActivatable", "D50A81F6-AA99-4A4B-BEC8-C96F25A567F5")
              .SetDisplayName(name)
              .SetDescription("ThickShadows.Desc")
              .SetIcon(icon)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetBuff(self)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetGroup(ActivatableAbilityGroup.CombatStyle)
              .SetWeightInGroup(1)
              .Configure();

            FeatureConfigurator.New("ThickShadowsFeat", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription("ThickShadows.Desc")
              .SetIcon(icon)
              .SetRanks(1)
              .AddFacts(new() { activatable })
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.ShadowHandProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl3Guid)
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.ShadowHandGuids.Except([Guid]).ToList())
#endif
              .Configure(true);
        }
    }
}