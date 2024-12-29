using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.WhiteRaven
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/lions-roar--3762/
  static class LionsRoar
  {
    public const string Guid = "363B6079-339D-499E-86BE-13A26A743639";
    const string name = "LionsRoar.Name";
    const string desc = "LionsRoar.Desc";
    const string icon = Helpers.IconPrefix + "lionsroar.png";

    public static void Configure()
    {
      var buff = BuffConfigurator.New("LionsRoarBuff", "08518743-F415-4367-89FD-B582456BABE4")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddDamageBonusConditional(new ContextValue {Value = 5 }, descriptor: ModifierDescriptor.Morale)
        .Configure();

      var area = AbilityAreaEffectConfigurator.New("LionsRoarBuffArea", "B85D16C4-DA6C-44F4-A7DA-AD1760B6CBE7")
        .AddAbilityAreaEffectBuff(buff, false, ConditionsBuilder.New().IsAlly())
        .SetShape(Kingmaker.UnitLogic.Abilities.Blueprints.AreaEffectShape.Cylinder)
        .SetSize(new Feet(60))
        .Configure();

      var selfBuff = BuffConfigurator.New("LionsRoarSelfBuff", "AFD8B191-BB6B-427A-9912-354E3492A636")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddNotDispelable()
        .AddAreaEffect(area)
        .Configure();

      var ability = AbilityConfigurator.New("LionsRoarAbility", "09365084-D3CA-420A-A901-5D1575483C21")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetCanTargetEnemies(false)
        .SetCanTargetFriends(false)
        .SetCanTargetSelf()
        .SetRange(AbilityRange.Personal)
        .SetActionType(UnitCommand.CommandType.Move)
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(selfBuff, ContextDuration.Fixed(1), toCaster: true))
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      FeatureConfigurator.New("LionsRoarFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl3Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.TigerClawGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}