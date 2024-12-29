using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.WhiteRaven
{
  //https://dndtools.net/spells/tome-of-battle-the-book-of-nine-swords--88/bolstering-voice--3755/
  static class BolsteringVoice
  {
    public const string Guid = "7B7E2BE8-7AF7-48C7-A29F-6DEC77DA5C3F";
    const string name = "BolsteringVoice.Name";
    const string icon = Helpers.IconPrefix + "bolsteringvoice.png";

    public static void Configure()
    {
      var bolsteringVoiceBuff = BuffConfigurator.New("BolsteringVoiceBuff", "E17D69EE-3DAA-4FC9-83A2-BA4AE33AAD59")
        .SetDisplayName(name)
        .SetDescription("BolsteringVoice.BuffDesc")
        .SetIcon(icon)
        .AddStatBonus(ModifierDescriptor.Morale, null, Kingmaker.EntitySystem.Stats.StatType.SaveWill, 2)
        .AddSavingThrowBonusAgainstDescriptor(null, null, ModifierDescriptor.Morale, null, SpellDescriptor.Fear, 4)
        .Configure();

      var bolsteringVoiceArea = AbilityAreaEffectConfigurator.New("BolsteringVoiceArea", "E7DB3EE9-70F8-44DA-89EE-32759AAE30EF")
        .AddAbilityAreaEffectBuff(bolsteringVoiceBuff, false, ConditionsBuilder.New().IsAlly())
        .SetShape(Kingmaker.UnitLogic.Abilities.Blueprints.AreaEffectShape.Cylinder)
        .SetSize(new Feet(60))
        .Configure();

      var bolsteringVoiceSelf = BuffConfigurator.New("BolsteringVoiceSelf", "80BE2941-FD66-424D-8A51-F81130C28ACF")
        .SetFlags(BlueprintBuff.Flags.HiddenInUi)
        .AddNotDispelable()
        .AddAreaEffect(bolsteringVoiceArea)
        .Configure();

      var bolsteringVoiceActivatable = ActivatableAbilityConfigurator.New("BolsteringVoiceActivatable", "07A63282-B03F-40BF-B6C1-7921EB8C4175")
        .SetDisplayName(name)
        .SetDescription("BolsteringVoice.Desc")
        .SetIcon(icon)
        .SetActivationType(AbilityActivationType.Immediately)
        .SetBuff(bolsteringVoiceSelf)
        .SetDeactivateIfOwnerDisabled()
        .SetDeactivateIfOwnerUnconscious()
        .SetDoNotTurnOffOnRest()
        .SetGroup(ActivatableAbilityGroup.CombatStyle)
        .SetWeightInGroup(1)
        .Configure();

      var bolsteringVoiceFeat = FeatureConfigurator.New("BolsteringVoiceFeat", Guid, AllManeuversAndStances.featureGroup)
        .SetDisplayName(name)
        .SetDescription("BolsteringVoice.Desc")
        .SetIcon(icon)
        .AddFeatureTagsComponent(FeatureTag.Defense)
        .SetRanks(1)
        .AddPrerequisiteClassLevel(WarbladeC.Guid, 1, hideInUI: true)
        .AddFacts(new() { bolsteringVoiceActivatable })
        .Configure(true);
    }
  }
}