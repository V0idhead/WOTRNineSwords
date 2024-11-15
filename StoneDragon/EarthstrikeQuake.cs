using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
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
using VoidHeadWOTRNineSwords.DiamondMind;
using VoidHeadWOTRNineSwords.Feats;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.StoneDragon
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/earthstrike-quake--3716/
  static class EarthstrikeQuake
  {
    public const string Guid = "13E13BA3-FCE5-43D6-863E-810064782415";
    const string name = "EarthstrikeQuake.Name";
    const string desc = "EarthstrikeQuake.Desc";

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(EarthstrikeQuake)}");

      UnityEngine.Sprite icon = AbilityRefs.SlowMud.Reference.Get().Icon;

      var ability = AbilityConfigurator.New(name, "6FBAA642-4044-4EF1-A476-96FCF115AE18")
      .SetDisplayName(name)
      .SetDescription(desc)
      .SetIcon(icon)
      .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special)
      .SetCanTargetEnemies()
      .SetCanTargetFriends(false)
      .SetCanTargetSelf(true)
      .SetRange(AbilityRange.Weapon)
      .SetActionType(UnitCommand.CommandType.Standard)
      .SetShouldTurnToTarget()
      .SetType(AbilityType.CombatManeuver)
      .AddAbilityTargetsAround(radius: new Kingmaker.Utility.Feet(20))
      .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
      .AddAbilityEffectRunAction
      (
        ActionsBuilder.New().SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Reflex, customDC: new ContextValue { Value = 18 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusStrength, EnduranceOfStone.StoneDragonFocusFactGuid),
            onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(BuffRefs.ProneBuff.Reference.Guid, ContextDuration.Fixed(1)))
        )
      )
      .AddAbilityResourceLogic(1, requiredResource: WarbladeC.ManeuverResourceGuid, isSpendResource: true)
      .Configure();

      var spell = FeatureConfigurator.New("EarthstrikeQuake", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(WarbladeC.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(InitiatorLevels.Lvl8Guid)
        .AddPrerequisiteFeaturesFromList(amount: 2, features: AllManeuversAndStances.StoneDragonGuids.Except([Guid]).ToList())
#endif
        .Configure();
    }
  }
}