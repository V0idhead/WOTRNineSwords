using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.ActivatableAbilities;
using VoidHeadWOTRNineSwords.Common;

namespace VoidHeadWOTRNineSwords.DesertWind
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/flames-blessing--3577/
  static class FlamesBlessing
  {
    public const string Guid = "4F759304-3119-420D-AEFD-7B8909DE41AC";
    const string name = "FlamesBlessing.Name";
    const string desc = "FlamesBlessing.Desc";
    static UnityEngine.Sprite icon = AbilityRefs.FlareBurst.Reference.Get().Icon;

    public static void Configure()
    {
      Main.Logger.Info($"Configuring {nameof(FlamesBlessing)}");

      var buff = BuffConfigurator.New("FlamesBlessingBuff", "ADB73934-FA2B-4F40-9F97-B8C7EADF7924")
        //.SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .AddContextRankConfig(ContextRankConfigs.BaseStat(Kingmaker.EntitySystem.Stats.StatType.SkillMobility))
        .AddDamageResistanceEnergy(type: Kingmaker.Enums.Damage.DamageEnergyType.Fire,  value: ContextValues.Rank(), useValueMultiplier: true, valueMultiplier: ContextValues.Constant(2))
        .Configure();

      var activatable = ActivatableAbilityConfigurator.New("FlamesBlessingActivatable", "2AA72E15-599E-40B6-A432-90EE93592167")
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetActivationType(AbilityActivationType.Immediately)
        .SetBuff(buff)
        .SetDeactivateIfOwnerDisabled()
        .SetDeactivateIfOwnerUnconscious()
        .SetDoNotTurnOffOnRest()
        .SetGroup(ActivatableAbilityGroup.CombatStyle)
        .SetWeightInGroup(1)
        .Configure();

      var feat = FeatureConfigurator.New("FlamesBlessingFeat", Guid)
        .SetDisplayName(name)
        .SetDescription(desc)
        .SetIcon(icon)
        .SetRanks(1)
        .AddFacts(new() { activatable })
#if !DEBUG
        .AddPrerequisiteFeature(DisciplineProficencies.DesertWindProficencyGuid, hideInUI: true)
#endif
        .Configure(true);
    }
  }
}