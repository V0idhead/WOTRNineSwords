using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
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
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.DiamondMind
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/ruby-nightmare-blade--3636/
  static class RubyNightmareBlade
  {
    public const string Guid = "2322ACE6-FAF3-4CB6-B9FD-34E955EACE61";
    const string name = "RubyNightmareBlade.Name";
    const string desc = "RubyNightmareBlade.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(RubyNightmareBlade)}");

      UnityEngine.Sprite icon = AbilityRefs.DivineFavor.Reference.Get().Icon;

      var successBuff = BuffConfigurator.New("RubyNightmareBladeSuccessBuff", "A15BE749-EC5C-4E5B-9601-D269AC252802")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddDoubleDamageDiceOnAttack()
        .Configure();

      var failBuff = BuffConfigurator.New("RubyNightmareBladeFailBuff", "FC9787EB-0741-40A5-A74A-E60ABFEAB427")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddAttackBonus(-2)
        .Configure();

      var ability = AbilityConfigurator.New(name, "564CFB2C-FCAE-419A-BC71-E297B15FB26D")
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
          ActionsBuilder.New()
          .Add<NightmareBladeAction>(a =>
          {
            a.OnLow = ActionsBuilder.New().ApplyBuff(failBuff, ContextDuration.Fixed(1), toCaster: true).Build();
            a.OnHigh = ActionsBuilder.New().ApplyBuff(successBuff, ContextDuration.Fixed(1), toCaster: true).Build();
          })
          .MeleeAttack()
        )
        .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("RubyNightmareBlade", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl4Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.DiamondMindGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}