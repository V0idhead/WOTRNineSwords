using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Owlcat.Runtime.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Warblade;
using static UnityModManagerNet.UnityModManager.ModEntry;

namespace VoidHeadWOTRNineSwords.StoneDragon
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/stone-bones--3726/
  static class StoneBones
  {
    public const string Guid = "6B21AE73-E0B8-4889-A930-575893AA8DCB";
    const string name = "StoneBones.Name";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.StonestrikeAbility.Reference.Get().Icon;

      log.Info($"Configuring {nameof(StoneBones)}");

      var buff = BuffConfigurator.New("StoneBonesBuff", "3E19D1E7-1396-4F55-AA0F-32492F733E21")
        .SetDisplayName(name)
        .SetDescription("StoneBones.Desc")
        .SetIcon(icon)
        .AddDamageResistancePhysical(bypassedByMaterial: true, material: PhysicalDamageMaterial.Adamantite, value: 5)
        .Configure();

      var triggerBuff = BuffConfigurator.New("StoneBonesTriggerBuff", "38A670E9-0B03-4BCD-87A1-3CD459BECCF8")
        .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
        .AddInitiatorAttackRollTrigger(
          onlyHit: true,
          action: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1, DurationRate.Rounds), toCaster: true))
        .Configure();

      var ability = AbilityConfigurator.New("StoneBonesAbility", "4E0B3C16-334A-4B0D-939E-F26812D862DA")
        .SetDisplayName(name)
        .SetDescription("StoneBones.Desc")
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
          actions: ActionsBuilder.New().ApplyBuff(triggerBuff,ContextDuration.Fixed(1), toCaster: true).MeleeAttack()
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("StoneBones", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription("StoneBones.Desc")
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
        .Configure();
    }
  }
}