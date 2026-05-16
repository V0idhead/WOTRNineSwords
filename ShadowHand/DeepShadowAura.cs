using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;

namespace VoidHeadWOTRNineSwords.ShadowHand
{
    static class DeepShadowAura
    {
        public const string Guid = "3B625F33-B6CC-46A8-9FD1-2849804CDBBF";
        const string name = "DeepShadowAura.Name";
        //const string icon = Helpers.IconPrefix + "deepshadowaura.png";
        static UnityEngine.Sprite icon = AbilityRefs.FlareBurst.Reference.Get().Icon;

        public static void Configure()
        {
            var deepShadowAuraBuff = BuffConfigurator.New("DeepShadowAuraBuff", "EEBAD411-AD3F-448E-BDEF-6C14F4B06C75")
              .SetDisplayName(name)
              .SetDescription("DeepShadowAura.BuffDesc")
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Penalty, null, Kingmaker.EntitySystem.Stats.StatType.SaveFortitude, -4)
              .AddStatBonus(ModifierDescriptor.Penalty, null, Kingmaker.EntitySystem.Stats.StatType.SaveReflex, -4)
              .AddStatBonus(ModifierDescriptor.Penalty, null, Kingmaker.EntitySystem.Stats.StatType.SaveWill, -4)
              .Configure();

            var deepShadowAuraArea = AbilityAreaEffectConfigurator.New("DeepShadowAuraArea", "6F8A2FFC-1C4E-450F-BA70-74AEF6E5CF34")
              .AddAbilityAreaEffectBuff(deepShadowAuraBuff, false, ConditionsBuilder.New().IsEnemy())
              .SetShape(Kingmaker.UnitLogic.Abilities.Blueprints.AreaEffectShape.Cylinder)
              .SetSize(new Feet(40))
              .Configure();

            var deepShadowAuraSelf = BuffConfigurator.New("DeepShadowAuraSelf", "0FE575EF-5FD2-40FD-A127-389D7BE3B736")
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddNotDispelable()
              .AddAreaEffect(deepShadowAuraArea)
              .Configure();

            var deepShadowAuraActivatable = ActivatableAbilityConfigurator.New("DeepShadowAuraActivatable", "AAB6FE45-4F8A-45DC-BA8D-7A91A896F72B")
              .SetDisplayName(name)
              .SetDescription("DeepShadowAura.Desc")
              .SetIcon(icon)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetBuff(deepShadowAuraSelf)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetGroup(ActivatableAbilityGroup.CombatStyle)
              .SetWeightInGroup(1)
              .Configure();

            var deepShadowAuraFeat = FeatureConfigurator.New("DeepShadowAuraFeat", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription("DeepShadowAura.Desc")
              .SetIcon(icon)
              .SetRanks(1)
              .AddFacts(new() { deepShadowAuraActivatable })
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.ShadowHandProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.ShadowHandGuids.Except([Guid]).ToList())
#endif
              .Configure(true);
        }
    }
}