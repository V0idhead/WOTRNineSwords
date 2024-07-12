﻿using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.WhiteRaven
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/lions-roar--3762/
  static class LionsRoar
  {
    public const string Guid = "363B6079-339D-499E-86BE-13A26A743639";
    const string name = "LionsRoar.Name";
    const string desc = "LionsRoar.Desc";

    public static void Configure()
    {
      UnityEngine.Sprite icon = AbilityRefs.ShifterWildShapeWereTigerAbillity.Reference.Get().Icon;

      var buff = BuffConfigurator.New("LionsRoarBuff", "08518743-F415-4367-89FD-B582456BABE4")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddDamageBonusConditional(new ContextValue {Value = 5 }, descriptor: ModifierDescriptor.Morale)
        .Configure();

      var area = AbilityAreaEffectConfigurator.New("LionsRoarBuffArea", "B85D16C4-DA6C-44F4-A7DA-AD1760B6CBE7")
        .AddAbilityAreaEffectBuff(buff, false, ConditionsBuilder.New().IsAlly())
        .SetShape(Kingmaker.UnitLogic.Abilities.Blueprints.AreaEffectShape.Cylinder)
        .SetSize(new Feet(60))
        .Configure();

      var selfBuff = BuffConfigurator.New("LionsRoarSelfBuff", "AFD8B191-BB6B-427A-9912-354E3492A636")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddNotDispelable()
        .AddAreaEffect(area)
        .Configure();

      var triggerBuff = BuffConfigurator.New("LionsRoarTriggerBuff", "43A5521D-ADB6-4BFE-BAA8-60A6930E3CA6")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddNotDispelable()
        .AddUnitDeathTrigger(radiusInMeters: 10,
          actions: ActionsBuilder.New().ApplyBuff(selfBuff, durationValue: ContextDuration.Fixed(1)).ContextSpendResource(WarbladeC.ManeuverResourceGuid, new ContextValue {Value = 1}))
        .Configure();

      var activatable = ActivatableAbilityConfigurator.New("LionsRoarActivatable", "AA7ED274-B82E-47AF-90F5-64DA82302F4E")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        //.AddComponent(new AbilityCasterHasWeaponSubcategory(WeaponSubCategory.Melee)) // doesn't work
        .SetActivationType(AbilityActivationType.Immediately)
        .SetBuff(triggerBuff)
        .SetDeactivateIfOwnerDisabled()
        .SetDeactivateIfOwnerUnconscious()
        .SetDoNotTurnOffOnRest()
        .SetGroup(ActivatableAbilityGroup.None)
        .SetWeightInGroup(1)
        .Configure();

      FeatureConfigurator.New("LionsRoarFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { activatable })
        .Configure();
    }

    class ResourceLogic : EntityFactComponentDelegate, ITargetRulebookHandler<RuleDealDamage>
    {
      public void OnEventAboutToTrigger(RuleDealDamage evt)
      { }

      public void OnEventDidTrigger(RuleDealDamage evt)
      {
        Main.Logger.Error("damge trigger: " + evt.Target.CharacterName + ": " + evt.Result + "/" + evt.Target.HPLeft);
        evt.Initiator.AddBuff(BuffRefs.AasimarHaloBuff.Reference, evt.Initiator, new TimeSpan(0,1,0));
        evt.Target.AddBuff(BuffRefs.BlessBuff.Reference, null, new TimeSpan(0,1,0));
      }
    }
  }
}