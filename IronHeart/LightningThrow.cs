using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.DiamondMind;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/lightning-throw--3652/
  static class LightningThrow
  {
    public const string Guid = "D1037F44-3B30-46B7-8ADE-9ED1D517E4CA";
    const string name = "LightningThrow.Name";
    const string desc = "LightningThrow.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(LightningThrow)}");

      UnityEngine.Sprite icon = AbilityRefs.LightningBolt.Reference.Get().Icon;

      var ability = AbilityConfigurator.New("LightningThrowAbility", "8293DA51-9F9E-47FA-819D-88275D8341F6")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
        .SetCanTargetEnemies()
        .SetCanTargetFriends(false)
        .SetCanTargetSelf(false)
        .SetCanTargetPoint()
        .SetRange(AbilityRange.Medium)
        .SetActionType(UnitCommand.CommandType.Standard)
        .SetShouldTurnToTarget()
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityDeliverProjectile(attackRollBonusStat: Kingmaker.EntitySystem.Stats.StatType.Strength, length: new Kingmaker.Utility.Feet(30), lineWidth: new Kingmaker.Utility.Feet(2), type: Kingmaker.UnitLogic.Abilities.Components.AbilityProjectileType.Line)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(ActionsBuilder.New().DealDamage(damageType: DamageTypes.Physical(), value: new Kingmaker.UnitLogic.Mechanics.ContextDiceValue { DiceCountValue = 12, DiceType = DiceType.D6}, halfIfSaved: true)) //TODO: see if this works before creating custom action to handle full RAW effect
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("LightningThrow", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl8Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.IronHeartGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}