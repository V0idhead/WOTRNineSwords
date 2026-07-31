using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.ShadowHand;

namespace VoidHeadWOTRNineSwords.DiamondMind
{
    static class ElementalShell
    {
        public const string Guid = "62399D2F-2A8F-4753-9915-0A559FFB6ED6";
        const string name = "ElementalShell.Name";
        const string desc = "ElementalShell.Desc";
        //const string icon = Helpers.IconPrefix + "elementalshell.png";
        static Sprite icon = AbilityRefs.Flare.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(ElementalShell)}");

            var buff = BuffConfigurator.New("ElementalShellBuff", "525D8B3D-1024-4E47-881A-7C4199EB5556")
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddNotDispelable()
              .AddDamageResistanceEnergy(type: Kingmaker.Enums.Damage.DamageEnergyType.Fire, value: ContextValues.Constant(5))
              .AddDamageResistanceEnergy(type: Kingmaker.Enums.Damage.DamageEnergyType.Sonic, value: ContextValues.Constant(5))
              .AddDamageResistanceEnergy(type: Kingmaker.Enums.Damage.DamageEnergyType.Electricity, value: ContextValues.Constant(5))
              .AddDamageResistanceEnergy(type: Kingmaker.Enums.Damage.DamageEnergyType.Cold, value: ContextValues.Constant(5))
              .AddDamageResistanceEnergy(type: Kingmaker.Enums.Damage.DamageEnergyType.Acid, value: ContextValues.Constant(5))
              .Configure();

            var ability = AbilityConfigurator.New("ElementalShellAbility", "A25759E5-869D-4671-B97C-4145F30FE717")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
              .SetCanTargetEnemies(false)
              .SetCanTargetFriends(false)
              .SetCanTargetSelf()
              .SetRange(AbilityRange.Personal)
              .SetActionType(UnitCommand.CommandType.Swift)
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityEffectRunAction
              (
                ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true)
              )
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var feat = FeatureConfigurator.New("ElementalShellFeat", Guid, AllManeuversAndStances.featureGroup)
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