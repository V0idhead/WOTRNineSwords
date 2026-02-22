using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;

namespace VoidHeadWOTRNineSwords.DesertWind
{
    static class WindStride
    {
        public const string Guid = "E610DA1B-F49F-4F56-9CE8-E88FEE824BC0";
        const string name = "WindStride.Name";
        const string desc = "WindStride.Desc";
        //const string icon = Helpers.IconPrefix + "burningblade.png";
        static UnityEngine.Sprite icon = AbilityRefs.BlueFlameBlastBladeDamage.Reference.Get().Icon;
        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(WindStride)}");

            var selfBuff = BuffConfigurator.New("WindStrideBuff", "EA16FAC8-558C-4ADD-8990-43CC8FA5FD74")
              .SetDisplayName(name)
              .SetDescription("WindStrideBuff.Desc")
              .SetIcon(icon)
              .AddBuffMovementSpeed(value: 20)
              .Configure();

            var ability = AbilityConfigurator.New("WindStrideAbility", "7FA67E37-FC7E-4A67-8EC3-5A3E9FC5B342")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetCanTargetEnemies(false)
              .SetCanTargetFriends(false)
              .SetCanTargetSelf()
              .SetRange(AbilityRange.Personal)
              .SetActionType(UnitCommand.CommandType.Swift)
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(selfBuff, ContextDuration.Fixed(1), toCaster: true))
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("WindStride", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl1Guid)
#endif
              .Configure();
        }
    }
}
