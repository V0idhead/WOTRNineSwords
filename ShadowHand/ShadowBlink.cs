using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Utility;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.ShadowHand
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/shadow-blink--3699/
    static class ShadowBlink
    {
        public const string Guid = "BDA0794C-FDA0-4A3B-8FC8-AE51E6488FD8";
        const string name = "ShadowBlink.Name";
        const string desc = "ShadowBlink.Desc";
        const string icon = Helpers.IconPrefix + "shadowblink.png";
        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(ShadowBlink)}");

            var ability = AbilityConfigurator.New("ShadowBlinkAbility", "82BCBBC9-7640-4557-B23A-29F2B7A244F7")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
              .SetRange(AbilityRange.Long)
              .SetCanTargetPoint()
              .SetActionType(UnitCommand.CommandType.Free)
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityCustomDimensionDoor(1, false, dissapearTime: 1, landingTime: 1, radius: new Feet(1))
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("ShadowBlink", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.ShadowHandProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl7Guid)
#endif
              .Configure();
        }
    }
}