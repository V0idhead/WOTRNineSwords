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
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.DesertWind;

namespace VoidHeadWOTRNineSwords.DiamondMind
{
    static class DiamondFocus
    {
        public const string Guid = "F7FD623C-5FF8-464D-A120-FDD3C968F7F2";
        const string name = "DiamondFocus.Name";
        const string desc = "DiamondFocus.Desc";
        //const string icon = Helpers.IconPrefix + "diamondfocus.png";
        static Sprite icon = AbilityRefs.Flare.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(DiamondFocus)}");

            var selfBuff = BuffConfigurator.New("DiamondFocusBuff", "A99B64EC-6D09-4559-8A2C-D54F44358DE1")
              .SetDisplayName(name)
              .SetDescription("DiamondFocusBuff.Desc")
              .SetIcon(icon)
              .AddAttackBonus(1)
              .Configure();

            var ability = AbilityConfigurator.New("DiamondFocusAbility", "57B17DFD-50C8-4571-BC32-C92A188F26CE")
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

            var spell = FeatureConfigurator.New("DiamondFocus", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.DiamondMindProficencyGuid, hideInUI: true)
#endif
              .Configure();
        }
    }
}