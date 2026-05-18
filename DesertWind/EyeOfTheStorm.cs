using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Utils.Types;
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

namespace VoidHeadWOTRNineSwords.DesertWind
{
    static class EyeOfTheStorm
    {
        public const string Guid = "ECABD9C8-49D6-47ED-B93E-83C8C24CCC7D";
        const string name = "EyeOfTheStorm.Name";
        //const string icon = Helpers.IconPrefix + "eyeofthestorm.png";
        static UnityEngine.Sprite icon = AbilityRefs.CausticEruption.Reference.Get().Icon;

        public static void Configure()
        {
            var area = AbilityAreaEffectConfigurator.New("EyeOfTheStormArea", "1998EB41-6604-4796-8B7B-135B2D58DB8F")
               .AddAbilityAreaEffectRunAction(round: ActionsBuilder.New().DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Fire), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D6, ContextValues.Constant(2))))
              .SetShape(Kingmaker.UnitLogic.Abilities.Blueprints.AreaEffectShape.Cylinder)
              .SetSize(new Feet(20))
              .SetAffectEnemies()
              .SetTargetType(Kingmaker.UnitLogic.Abilities.Blueprints.BlueprintAbilityAreaEffect.TargetType.Enemy)
              .Configure();

            var self = BuffConfigurator.New("EyeOfTheStormSelf", "BA3C2369-4DA8-4FA1-AAC2-319103750304")
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddNotDispelable()
              .AddAreaEffect(area)
              .Configure();

            var activatable = ActivatableAbilityConfigurator.New("EyeOfTheStormActivatable", "2F150601-CF55-452A-AE5C-A29C4C1EE438")
              .SetDisplayName(name)
              .SetDescription("EyeOfTheStorm.Desc")
              .SetIcon(icon)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetBuff(self)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetGroup(ActivatableAbilityGroup.CombatStyle)
              .SetWeightInGroup(1)
              .Configure();

            FeatureConfigurator.New("EyeOfTheStormFeat", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription("EyeOfTheStorm.Desc")
              .SetIcon(icon)
              .SetRanks(1)
              .AddFacts(new() { activatable })
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl5Guid)
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.DesertWindGuids.Except([Guid]).ToList())
#endif
              .Configure(true);
        }
    }
}