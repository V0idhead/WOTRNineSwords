using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.StoneDragon
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/ancient-mountain-hammer--3708/
  static class AncientMountainHammer
  {
    public const string Guid = "38A78FF3-86A9-47C4-BC43-430718F0DFF8";
    const string name = "AncientMountainHammer.Name";
    const string desc = "AncientMountainHammer.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(MountainHammer)}");

      UnityEngine.Sprite icon = AbilityRefs.ChaosHammer.Reference.Get().Icon;

      var buff = BuffConfigurator.New("AncientMountainHammerBuff", "27B8F5A3-C892-4D1E-9C4E-8ADD26EDB976")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddIgnoreDamageReductionOnAttack(/*onlyOnFirstAttack: true*/)
        .Configure();

      var ability = AbilityConfigurator.New(name, "2ABB6CB5-8E9B-4FB4-AD02-384E7D8A28E2")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
        .SetCanTargetEnemies()
        .SetCanTargetFriends(false)
        .SetCanTargetSelf(false)
        .SetRange(AbilityRange.Weapon)
        .SetActionType(UnitCommand.CommandType.Standard)
        .SetShouldTurnToTarget()
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction
        (
          ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true).Add<ContextMeleeAttackRolledBonusDamage>(bd => { bd.ExtraDamage = new DiceFormula(12, DiceType.D6); bd.OnHit = EnduranceOfStone.GetEffectAction(); })
        )
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("AncientMountainHammer", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl7Guid)
        .AddPrerequisiteFeaturesFromList(amount: 3, features: AllManeuversAndStances.StoneDragonGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}