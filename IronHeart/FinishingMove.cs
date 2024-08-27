using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.AI;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.StoneDragon;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.IronHeart
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/finishing-move--3647/
  static class FinishingMove
  {
    public const string Guid = "A0945E6D-388D-42F0-9C47-819AF03D9646";
    const string name = "FinishingMove.Name";
    const string desc = "FinishingMove.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(FinishingMove)}");

      UnityEngine.Sprite icon = AbilityRefs.HolySmite.Reference.Get().Icon;

      var ability = AbilityConfigurator.New(name, "37518CBF-613F-4F01-B5EF-57AA1B9E5E16")
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
          ActionsBuilder.New().Add<FinishingMoveAttack>(fma => { })
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("FinishingMove", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl7Guid)
        .AddPrerequisiteFeaturesFromList(amount: 3, features: AllManeuversAndStances.IronHeartGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }

  public class FinishingMoveAttack : ContextActionMeleeAttack
  {
    public override string GetCaption()
    {
      return "does variable damage based on current target health";
    }

    public override void RunAction()
    {
      try
      {
        var target = Context.MainTarget.Unit;
        if (target == null) return;

        DiceFormula dmg;
        if (target.HPLeft < target.MaxHP / 2)
          dmg = new DiceFormula(14, DiceType.D6);
        else if (target.HPLeft < target.MaxHP)
          dmg = new DiceFormula(6, DiceType.D6);
        else
          dmg = new DiceFormula(4, DiceType.D6);

        //realistically I'd need to convert the DamageType of the main weapon to Physical Damage Form. But I'd rather err on the side of making this too powerful, so I'm using DirectDamage
        //Game.Instance.Rulebook.TriggerEvent<RuleDealDamage>(new RuleDealDamage(caster, target, new PhysicalDamage(new ModifiableDiceFormula(dmg), 0, caster.GetFirstWeapon().Blueprint.DamageType.Type.)));

        base.RunAction();
        var attack = AbilityContext.RulebookContext?.LastEvent<RuleAttackWithWeapon>();
        if(attack != null && attack.AttackRoll.IsHit)
          Game.Instance.Rulebook.TriggerEvent<RuleDealDamage>(new RuleDealDamage(attack.Initiator, target, new DirectDamage(dmg)));
      }
      catch (Exception e)
      {
        Main.Logger.Error($"{nameof(FinishingMoveAttack)} error: {e.Message}");
      }
    }
  }
}