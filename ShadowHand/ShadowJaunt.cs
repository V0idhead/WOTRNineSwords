using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Utility;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.ShadowHand
{
    //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/shadow-jaunt--3701/
    static class ShadowJaunt
    {
        public const string Guid = "221F8701-227D-4339-BD58-C0DC8AD87ACE";
        const string name = "ShadowJaunt.Name";
        const string desc = "ShadowJaunt.Desc";
        //const string icon = Helpers.IconPrefix + "shadowjaunt.png";
        static UnityEngine.Sprite icon = AbilityRefs.CausticEruption.Reference.Get().Icon;
        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(ShadowJaunt)}");

            var ability = AbilityConfigurator.New("ShadowJauntAbility", "8A1827EB-97C2-4619-9611-E119BEFE6006")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
              .SetRange(AbilityRange.Long)
              .SetCanTargetPoint()
              .SetActionType(UnitCommand.CommandType.Move)
              .SetType(AbilityType.CombatManeuver)
              .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
              .AddAbilityCustomDimensionDoor(1, false, dissapearTime: 1, landingTime: 1, radius: new Feet(1))
              .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
              .Configure();

            var spell = FeatureConfigurator.New("ShadowJaunt", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
              .AddPrerequisiteFeature(DisciplineProficencies.ShadowHandProficencyGuid, hideInUI: true)
              .AddPrerequisiteFeature(InitiatorLevels.Lvl2Guid)
#endif
              .Configure();
        }
    }
}