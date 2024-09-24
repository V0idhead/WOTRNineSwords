using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityModManagerNet.UnityModManager.ModEntry;
using VoidHeadWOTRNineSwords.StoneDragon;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Actions.Builder;
using Kingmaker.ElementsSystem;
using BlueprintCore.Actions.Builder.ContextEx;
using VoidHeadWOTRNineSwords.Common;

namespace VoidHeadWOTRNineSwords.Warblade
{
  //https://dndtools.net/classes/warblade/
  public static class WarbladeC
  {
    public const string Guid = "9FD9151C-B985-4B70-81BF-D8D9C4D21E15";
    public const string ManeuverResourceGuid = "06B1EFA3-CF54-47EA-9BA8-D788F239FE1C";
    public const string ManeuverResourceFactGuid = "B77E4216-1AC6-4E58-92E7-0550C8ACA4EE";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static void ConfigureClass()
    {
      log.Info($"{nameof(WarbladeC)} configuring resource");
      var warbladeManeuverResource = AbilityResourceConfigurator.New("WarbladeManeuvers.Resource", ManeuverResourceGuid)
      //.SetMaxAmount(new BlueprintAbilityResource.Amount { BaseValue=3, IncreasedByLevel=true, StartingLevel=0, LevelStep=4, PerStepIncrease=1 })
      .SetMaxAmount(ResourceAmountBuilder.New(3).IncreaseByLevelStartPlusDivStep(classes: new string[] { Guid }, startingLevel: 1, levelsPerStep: 4, bonusPerStep: 1))
      //.SetUseMax()
      //.AddPlayerLeaveCombatTrigger(ActionsBuilder.New().RestoreResource(ManeuverResourceGuid, 3)) //doesn't seem to do anything, which is a shame because now I need an end combat trigger on every maneuver
      .Configure();

      var manResFact = UnitFactConfigurator.New("WarbladeManeuvers.Resource.Fact", ManeuverResourceFactGuid)
        .AddAbilityResources(resource: warbladeManeuverResource, restoreAmount: true)
        .Configure();

      log.Info($"{nameof(WarbladeC)} configuring");
      BlueprintProgression progression = ConfigureProgression();

      BlueprintCharacterClass warbladeC = CharacterClassConfigurator.New("Warblade", Guid)
      .SetLocalizedName("WarbladeC.Name")
      .SetLocalizedDescription("WarbladeC.Desc")
      .SetSkillPoints(3)
      .SetHitDie(DiceType.D12)
      .SetBaseAttackBonus(StatProgressionRefs.BABFull.Reference.Get())
      .SetFortitudeSave(StatProgressionRefs.SavesHigh.Reference.Get())
      .SetReflexSave(StatProgressionRefs.SavesLow.Reference.Get())
      .SetWillSave(StatProgressionRefs.SavesLow.Reference.Get())
      .AddToClassSkills(StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillPersuasion, StatType.SkillPerception)
      .SetProgression(progression)
      .AddToRecommendedAttributes(StatType.Strength, StatType.Constitution, StatType.Intelligence)
      .AddToNotRecommendedAttributes(StatType.Wisdom, StatType.Charisma)
      .SetStartingGold(200)
      .SetStartingItems(ItemWeaponRefs.ColdIronBattleaxe.Reference.Get(), ItemArmorRefs.ScalemailStandard.Reference.Get(), ItemEquipmentUsableRefs.PotionOfCureLightWounds.Reference.Get())
      .SetPrimaryColor(11)
      .SetSecondaryColor(47)
      .SetDifficulty(4)
      .AddToMaleEquipmentEntities("65e7ae8b40be4d64ba07d50871719259", "04244d527b8a1f14db79374bc802aaaa")
      .AddToFemaleEquipmentEntities("11266d19b35cb714d96f4c9de08df48e", "64abd9c4d6565de419f394f71a2d496f")
      .Configure();

      BlueprintCharacterClassReference classRef = warbladeC.ToReference<BlueprintCharacterClassReference>();
      BlueprintRoot root = BlueprintTool.Get<BlueprintRoot>("2d77316c72b9ed44f888ceefc2a131f6");
      root.Progression.m_CharacterClasses = CommonTool.Append(root.Progression.m_CharacterClasses, classRef);

      log.Info($"{nameof(WarbladeC)} done");
    }

    public static BlueprintProgression ConfigureProgression()
    {
      WarbladeRecoverManeuvers.Configure();
      InitiatorLevels.Configure();
      var weaponAptitude = WeaponAptitude.Configure();
      var battleClarity = BattleClarityReflex.Configure();
      var battleArdor = BattleArdor.Configure();
      var battleCunning = BattleCunning.Configure();
      var battleSkill = BattleSkill.Configure();
      var battleMastery = BattleMastery.Configure();
      var stanceMastery = StanceMastery.Configure();
      var stanceSelector = WarbladeStanceSelection.Configure();
      var maneuverSelector = WarbladeManeuverSelection.Configure();

      var warbladeProficiencies = FeatureConfigurator.New("WarbladeProficiencies", "A68BE9ED-C7D6-45CE-8334-7D0551D6F971")
        .SetDisplayName("WarbladeProficiencies.Name")
        .SetDescription("WarbladeProficiencies.Desc")
        .AddFacts(new() { FeatureRefs.SimpleWeaponProficiency.Reference.Get(), FeatureRefs.MartialWeaponProficiency.Reference.Get(), FeatureRefs.LightArmorProficiency.Reference.Get(), FeatureRefs.MediumArmorProficiency.Reference.Get(), FeatureRefs.ShieldsProficiency.Reference.Get() })
        .SetIsClassFeature()
        .SetRanks(1)
        .Configure();

      var bonusFeatSelector = FeatureSelectionRefs.FighterFeatSelection.Reference.Guid; //RAW Bonus Feat selection is pathetic, using fighter list instead

      var entries = LevelEntryBuilder.New()
        .AddEntry(1, warbladeProficiencies.AssetGuid, WarbladeRecoverManeuvers.Guid, weaponAptitude.AssetGuid, battleClarity.AssetGuid, stanceSelector.AssetGuid, maneuverSelector.AssetGuid, maneuverSelector.AssetGuid, maneuverSelector.AssetGuid, InitiatorLevels.Lvl1Guid) //3 maneuvers, 1 stance; Weapon Aptitude not implemented
        .AddEntry(2, FeatureRefs.UncannyDodge.Reference.Guid, maneuverSelector.AssetGuid)
        .AddEntry(3, battleArdor.AssetGuid, maneuverSelector.AssetGuid, InitiatorLevels.Lvl2Guid) //implementing Weapon Aptitude would be a lot of work for the relatively small benefit of allowing selection of Greater Weapon Focus, Weapon Sepcialization and Greater Shield Focus
        .AddEntry(4, stanceSelector.AssetGuid)
        .AddEntry(5, bonusFeatSelector, maneuverSelector, InitiatorLevels.Lvl3Guid)
        .AddEntry(6, FeatureRefs.ImprovedUncannyDodge.Reference.Guid)
        .AddEntry(7, battleCunning.AssetGuid, maneuverSelector.AssetGuid, InitiatorLevels.Lvl4Guid)
        .AddEntry(9, bonusFeatSelector, maneuverSelector, InitiatorLevels.Lvl5Guid)
        .AddEntry(10, stanceSelector.AssetGuid)
        .AddEntry(11, battleSkill.AssetGuid, maneuverSelector.AssetGuid, InitiatorLevels.Lvl6Guid)
        .AddEntry(13, bonusFeatSelector, maneuverSelector.AssetGuid, InitiatorLevels.Lvl7Guid)
        .AddEntry(15, battleMastery.AssetGuid, maneuverSelector.AssetGuid, InitiatorLevels.Lvl8Guid)
        .AddEntry(16, stanceSelector.AssetGuid)
        .AddEntry(17, bonusFeatSelector, maneuverSelector.AssetGuid, InitiatorLevels.Lvl9Guid)
        .AddEntry(19, maneuverSelector.AssetGuid)
        .AddEntry(20, stanceMastery.AssetGuid);

      var progression = ProgressionConfigurator.New("WarbladeProgression", "5BD44661-AEF3-48E1-8B11-3740A0BA9A31")
        .SetRanks(1)
        .SetLevelEntries(entries)
        .SetClasses(Guid)
        .Configure();

      return progression;
    }
  }
}