using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;

namespace VoidHeadWOTRNineSwords.DesertWind
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/burning-blade--3568/
  static class BurningBlade
  {
    public const string Guid = "F30C3415-CCE2-441F-98F8-F87689275DE9";
    const string name = "BurningBlade.Name";
    const string desc = "BurningBlade.Desc";
    //const string icon = Helpers.IconPrefix + "burningblade.png";
    static UnityEngine.Sprite icon = AbilityRefs.BlueFlameBlastBladeDamage.Reference.Get().Icon;

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(BurningBlade)}");

      var selfBuff = BuffConfigurator.New("BurningBladeBuff", "1B68A9CB-3F63-4AE3-97DB-D0BE16176A9B")
        .SetDisplayName(name)
        .SetDescription("BurningBladeBuff.Desc")
        .SetIcon(icon)
        .AdditionalDamageOnHit(DamageEnergyType.Fire, new DiceFormula(1, DiceType.D6), onlyMelee: true)
        .AdditionalDamageOnHit(DamageEnergyType.Fire, DiceFormula.One, onlyMelee: true) //TODO: should be 1 per initiator level
        .Configure();

      var ability = AbilityConfigurator.New("BurningBladeAbility", "DAB777FC-2AEE-49AD-9C89-68F808AF743F")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetCanTargetEnemies(false)
        .SetCanTargetFriends(false)
        .SetCanTargetSelf()
        .SetRange(AbilityRange.Personal)
        .SetActionType(UnitCommand.CommandType.Swift)
        .SetType(AbilityType.CombatManeuver)
        .AddAbilityRequirementHasItemInHands(type: Kingmaker.UnitLogic.Abilities.Components.AbilityRequirementHasItemInHands.RequirementType.HasMeleeWeapon)
        .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(selfBuff, ContextDuration.Fixed(1), toCaster: true))
        .AddAbilityResourceLogic(1, requiredResource: ManeuverResources.ManeuverResourceGuid, isSpendResource: true)
        .Configure();

      var spell = FeatureConfigurator.New("BurningBlade", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { ability })
        .AddCombatStateTrigger(ActionsBuilder.New().RestoreResource(ManeuverResources.ManeuverResourceGuid))
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
        .AddPrerequisiteFeature(InitiatorLevels.Lvl1Guid)
#endif
        .Configure();
    }
  }
}