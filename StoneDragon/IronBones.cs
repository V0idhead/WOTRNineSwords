using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Warblade;
using BlueprintCore.Actions.Builder.ContextEx;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.StoneDragon
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/iron-bones--3719/
  static class IronBones
  {
    public const string Guid = "91D4E335-F250-45F9-B055-D8A77DDE9B68";
    const string name = "IronBones.Name";
    const string desc = "IronBones.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.StonestrikeAbility.Reference.Get().Icon;

      Main.Logger.Info($"Configuring {nameof(IronBones)}");

      var buff = BuffConfigurator.New("IronBonesBuff", "36E99481-F26F-41D4-A566-01C671154097")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddDamageResistancePhysical(bypassedByMaterial: true, material: PhysicalDamageMaterial.Adamantite, value: 10)
        .Configure();

      var triggerBuff = BuffConfigurator.New("IronBonesTriggerBuff", "21C25574-3682-4EDC-921D-E5C06FB9575A")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddInitiatorAttackRollTrigger(
          onlyHit: true,
          action: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1, DurationRate.Rounds), toCaster: true))
        .Configure();

      var ability = AbilityConfigurator.New("IronBonesAbility", "23FA4AD7-D03A-4F80-978F-1BB27517AF3F")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
        .SetCanTargetEnemies()
        .SetCanTargetFriends(false)
        .SetCanTargetSelf(false)
        .SetRange(AbilityRange.Weapon)
        .SetUseCurrentWeaponAsReasonItem()
        .SetActionType(UnitCommand.CommandType.Standard)
        .SetShouldTurnToTarget()
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(
          actions: ActionsBuilder.New().ApplyBuff(triggerBuff, ContextDuration.Fixed(1), toCaster: true).Add<MeleeAttackExtended>(mae => mae.OnHit = EnduranceOfStone.GetEffectAction())
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("IronBones", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl6Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.StoneDragonGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}