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
using Kingmaker.Utility;
using System.Linq;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.WhiteRaven
{
  //massive buffs for all allies in 60ft radius
  static class WhiteRavenCall
  {
    public const string Guid = "6B5047F2-61DC-429B-B808-C245F6005033";
    const string name = "WhiteRavenCall.Name";
    const string description = "WhiteRavenCall.Desc";
    const string icon = Helpers.IconPrefix + "whiteravencall.png";

    public static void Configure()
    {
      var buff = BuffConfigurator.New("WhiteRavenCallBuff", "F582E0A5-6FA1-460D-A0F6-3656697A2BF8")
        .SetDisplayName(name)
        .SetDescription("WhiteRavenCall.BuffDesc")
        .SetIcon(icon)
        .AddStatBonus(ModifierDescriptor.Morale, null, Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus, 5)
        .AddStatBonus(ModifierDescriptor.Morale, null, Kingmaker.EntitySystem.Stats.StatType.AdditionalDamage, 5)
        .AddStatBonus(ModifierDescriptor.Morale, null, Kingmaker.EntitySystem.Stats.StatType.AC, 5)
        .AddBuffAllSavesBonus(ModifierDescriptor.Morale, 5)
        .AddStatBonus(ModifierDescriptor.Morale, null, Kingmaker.EntitySystem.Stats.StatType.AdditionalCMD, 5)
        .Configure();

      var area = AbilityAreaEffectConfigurator.New("WhiteRavenCallArea", "86B86387-404B-40D9-96BF-D9B817FB015E")
        .AddAbilityAreaEffectBuff(buff, false, ConditionsBuilder.New().IsAlly())
        .SetShape(Kingmaker.UnitLogic.Abilities.Blueprints.AreaEffectShape.Cylinder)
        .SetSize(new Feet(60))
        .Configure();

      var buffSelf = BuffConfigurator.New("WhiteRavenCallBuffSelf", "5CB828F0-8945-40E8-947B-9C09496D1BD8")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddNotDispelable()
        .AddAreaEffect(area)
        .Configure();

      var ability = AbilityConfigurator.New("WhiteRavenCallAbility", "DDFFA7DA-EF21-415E-8AE3-D0DEA0DE5758")
        .SetDisplayName(name)
        .SetDescription(description)
        .SetIcon(icon)
        .SetCanTargetEnemies(false)
        .SetCanTargetFriends(false)
        .SetCanTargetSelf()
        .SetRange(AbilityRange.Personal)
        .SetActionType(UnitCommand.CommandType.Move)
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buffSelf, ContextDuration.Fixed(1), toCaster: true))
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var whiteRavenCallFeat = FeatureConfigurator.New("WhiteRavenCallFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(description)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Defense)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { ability })
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl9Guid)
        .AddPrerequisiteFeaturesFromList(amount: 4, features: AllManeuversAndStances.WhiteRavenGuids.Except([Guid]).ToList())
#endif
        .Configure(true);
    }
  }
}