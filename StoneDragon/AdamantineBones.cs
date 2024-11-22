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
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.StoneDragon
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/adamantine-bones--3707/
  internal class AdamantineBones
  {
    public const string Guid = "E842CA48-D474-489C-BD7D-D1DC82EA475A";
    const string name = "AdamantineBones.Name";
    const string desc = "AdamantineBones.Desc";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.IronBody.Reference.Get().Icon;

      log.Info($"Configuring {nameof(AdamantineBones)}");

      var buff = BuffConfigurator.New("AdamantineBonesBuff", "54CAA099-27E7-44BD-A238-A59E3F5F5628")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddDamageResistancePhysical(bypassedByMaterial: true, material: PhysicalDamageMaterial.Adamantite, value: 20)
        .Configure();

      var ability = AbilityConfigurator.New("AdamantineBonesAbility", "18FD4289-3368-4DB1-AF01-0E851D3A596C")
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
          actions: ActionsBuilder.New().Add<MeleeAttackExtended>(attack => { attack.OnHit = ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true).AddAll(EnduranceOfStone.GetEffectAction()).Build(); })
        )
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("AdamantineBones", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription("AdamantineBones.Desc")
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl8Guid)
        .AddPrerequisiteFeaturesFromList(amount: 3, features: AllManeuversAndStances.StoneDragonGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}