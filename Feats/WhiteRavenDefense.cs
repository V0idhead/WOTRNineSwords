using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.AVEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace VoidHeadWOTRNineSwords.Feats
{
  static class WhiteRavenDefense
  {
    public const string WhiteRavenDefenseGuid = "66C9C1BE-23F7-4055-932D-58EF671BB1F2";
    public const string WhiteRavenFocusFactGuid = "5E307CEC-E03E-4104-B1E6-61C021B81833";
    public const string WhiteRavenDefenseEffectGuid = "A5FDC75D-DB62-4409-9406-E85861C797F3";

    public static void Configure()
    {
      var whiteRavenFocusFact = UnitFactConfigurator.New("WhiteRavenFocusFact", WhiteRavenFocusFactGuid)
        .Configure();
      FeatureConfigurator.New("WhiteRavenDefense", WhiteRavenDefenseGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
        .SetDisplayName("WhiteRavenDefense.Name")
        .SetDescription("WhiteRavenDefense.Desc")
        .SetIcon(FeatureRefs.SpellFocusEnchanctment.Reference.Get().Icon)
        .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
        .AddFacts(new() { whiteRavenFocusFact })
        .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.WhiteRavenGuids.ToList())
        .Configure();
      AbilityConfigurator.New("WhiteRavenDefenseEffect", WhiteRavenDefenseEffectGuid)
        .SetCanTargetFriends(true)
        .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
        .AddAbilitySpawnFx(Kingmaker.UnitLogic.Abilities.Components.Base.AbilitySpawnFxAnchor.Caster, prefabLink: "d119d19888a8f964b8acc5dfce6ea9e9", time: Kingmaker.UnitLogic.Abilities.Components.Base.AbilitySpawnFxTime.OnStart) //Bless Effect
        .AddAbilityTargetsAround(radius: new Feet(30), targetType: Kingmaker.UnitLogic.Abilities.Components.TargetType.Ally)
        .AddAbilityEffectRunAction(ActionsBuilder.New()
          .RemoveBuffsByDescriptor(new SpellDescriptorWrapper(SpellDescriptor.Confusion))
          .RemoveBuffsByDescriptor(new SpellDescriptorWrapper(SpellDescriptor.Stun))
          .RemoveBuffsByDescriptor(new SpellDescriptorWrapper(SpellDescriptor.Fear))
          .RemoveBuffsByDescriptor(new SpellDescriptorWrapper(SpellDescriptor.Compulsion))
          .RemoveBuffsByDescriptor(new SpellDescriptorWrapper(SpellDescriptor.Daze))
          .RemoveBuffsByDescriptor(new SpellDescriptorWrapper(SpellDescriptor.Shaken))
          .RemoveBuffsByDescriptor(new SpellDescriptorWrapper(SpellDescriptor.Staggered))
        ).Configure();
    }

    public static ActionList GetEffectAction()
    {
      return ActionsBuilder.New().Conditional(ConditionsBuilder.New().CasterHasFact(WhiteRavenFocusFactGuid), ActionsBuilder.New()
        .CastSpell(WhiteRavenDefenseEffectGuid))
        .Build();
    }
  }
}