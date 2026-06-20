using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Swordsage.Archetypes;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Swordsage
{
  //https://dndtools.net/classes/swordsage/
  public static class SwordsageC
  {
    public const string Guid = "600E0E53-9A37-4D59-89ED-2234908D283B";
    public const string SwordsageProficienciesGuid = "E6DEFCF2-0C19-45F4-A047-13F71B52DD9A";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static void ConfigureClass()
    {
      log.Info($"{nameof(SwordsageC)} configuring");
      BlueprintProgression progression = ConfigureProgression();

      BlueprintStatProgression babProg = StatProgressionConfigurator.New("BABSwordsage", "B50BF310-D86D-41E9-BA7B-9D3328FD553F") //as Medium, but +1, so we start with 1 at level 1
        .SetBonuses(0, 1, 2, 3, 4, 4, 5, 6, 7, 7, 8, 9, 10, 10, 11, 12, 13, 13, 14, 15, 16, 16, 17, 18, 19, 19, 20, 21, 22, 22, 23, 24, 25, 26, 26, 27, 28, 29, 29)
        .Configure();

      BlueprintCharacterClass swordsageC = CharacterClassConfigurator.New("Swordsage", Guid)
      .SetLocalizedName("SwordsageC.Name")
      .SetLocalizedDescription("SwordsageC.Desc")
      .SetSkillPoints(4)
      .SetHitDie(DiceType.D8)
      .SetIsArcaneCaster(false).SetIsDivineCaster(false)
      .SetBaseAttackBonus(babProg)
      .SetFortitudeSave(StatProgressionRefs.SavesLow.Reference.Get())
      .SetReflexSave(StatProgressionRefs.SavesHigh.Reference.Get())
      .SetWillSave(StatProgressionRefs.SavesHigh.Reference.Get())
      .AddToClassSkills(StatType.SkillMobility, StatType.SkillStealth, StatType.SkillPersuasion, StatType.SkillKnowledgeWorld, StatType.SkillLoreNature)
      .SetProgression(progression)
      .AddToRecommendedAttributes(StatType.Strength, StatType.Constitution, StatType.Wisdom)
      .AddToNotRecommendedAttributes(StatType.Intelligence, StatType.Charisma)
      .AddPrerequisiteNoClassLevel(WarbladeC.Guid, hideInUI: true) //no multiclassing with Warblade
      .AddPrerequisiteIsPet(not: true)
      .SetStartingGold(200)
      .SetStartingItems(ItemWeaponRefs.ColdIronBattleaxe.Reference.Get(), ItemArmorRefs.ScalemailStandard.Reference.Get(), ItemEquipmentUsableRefs.PotionOfCureLightWounds.Reference.Get())
      .SetPrimaryColor(11)
      .SetSecondaryColor(47)
      .SetDifficulty(5)
      .AddToMaleEquipmentEntities("65e7ae8b40be4d64ba07d50871719259", "04244d527b8a1f14db79374bc802aaaa")
      .AddToFemaleEquipmentEntities("11266d19b35cb714d96f4c9de08df48e", "64abd9c4d6565de419f394f71a2d496f")
      .Configure();

      BlueprintCharacterClassReference classRef = swordsageC.ToReference<BlueprintCharacterClassReference>();
      BlueprintRoot root = BlueprintTool.Get<BlueprintRoot>("2d77316c72b9ed44f888ceefc2a131f6");
      root.Progression.m_CharacterClasses = CommonTool.Append(root.Progression.m_CharacterClasses, classRef);

      log.Info($"{nameof(SwordsageC)} done");

      VoidHeadWOTRNineSwords.Swordsage.Archetypes.RimeRavager.Configure();

      log.Info($"{nameof(SwordsageC)} Archetypes done");
    }

    public static BlueprintProgression ConfigureProgression()
    {
      SwordsageRecoverManeuvers.Configure();
      var quickToAct = QuickToAct.Configure();
      var bonusWeaponFocus = BonusWeaponFocus.Configure();
      var swordsageACBonus = SwordsageACBonus.Configure();
      var swordInsight = SwordInsight.Configure();
      var swordsageDamageBonus = SwordsageDamageBonus.Configure();
      var swordsageDefensiveStance = SwordsageDefensiveStance.Configure();
      var maneuverSelector = SwordsageManeuverSelection.Configure();
      var stanceSelector = SwordsageStanceSelection.Configure();
      var disciplineFocusSelection = SwordsageDisciplineFocusSelection.Configure();

      var swordsageProficiencies = FeatureConfigurator.New("SwordsageProficiencies", SwordsageProficienciesGuid)
        .SetDisplayName("SwordsageProficiencies.Name")
        .SetDescription("SwordsageProficiencies.Desc")
        .AddFacts(new() { FeatureRefs.SimpleWeaponProficiency.Reference.Get(), FeatureRefs.MartialWeaponProficiency.Reference.Get(), FeatureRefs.LightArmorProficiency.Reference.Get(), DisciplineProficencies.DesertWindProficencyGuid, DisciplineProficencies.DiamondMindProficencyGuid, DisciplineProficencies.IronHeartProficencyGuid, DisciplineProficencies.RivenHourglassProficencyGuid, DisciplineProficencies.ShadowHandProficencyGuid, DisciplineProficencies.StoneDragonProficencyGuid, DisciplineProficencies.TigerClawProficencyGuid })
        .SetIsClassFeature()
        .SetRanks(1)
        .Configure();

      var entries = LevelEntryBuilder.New()
        .AddEntry(1, swordsageProficiencies.AssetGuid, SwordsageRecoverManeuvers.Guid, quickToAct.AssetGuid, bonusWeaponFocus.AssetGuid, disciplineFocusSelection.AssetGuid, maneuverSelector.AssetGuid, maneuverSelector.AssetGuid, maneuverSelector.AssetGuid, maneuverSelector.AssetGuid, maneuverSelector.AssetGuid, maneuverSelector.AssetGuid, stanceSelector.AssetGuid, InitiatorLevels.Lvl1Guid, ManeuverResources.IncreaseManeuverUsesGuid) //6 maneuvers, 1 stance
        .AddEntry(2, swordsageACBonus.AssetGuid, maneuverSelector.AssetGuid, stanceSelector.AssetGuid)
        .AddEntry(3, maneuverSelector.AssetGuid, ManeuverResources.IncreaseManeuverUsesGuid, InitiatorLevels.Lvl2Guid)
        .AddEntry(4, swordInsight.AssetGuid, maneuverSelector.AssetGuid) //Discipline Focus: replaced by attack bonus
        .AddEntry(5, quickToAct.AssetGuid, maneuverSelector.AssetGuid, ManeuverResources.IncreaseManeuverUsesGuid, stanceSelector.AssetGuid, InitiatorLevels.Lvl3Guid)
        .AddEntry(6, maneuverSelector.AssetGuid)
        .AddEntry(7, maneuverSelector.AssetGuid, swordsageDamageBonus.AssetGuid, InitiatorLevels.Lvl4Guid) //Sense Magic: replaced by damage bonus
        .AddEntry(8, swordsageDefensiveStance.AssetGuid, maneuverSelector.AssetGuid, ManeuverResources.IncreaseManeuverUsesGuid) //Defensive Stance: replaced by general saving throw bonus
        .AddEntry(9, FeatureRefs.Evasion.Reference.Get(), maneuverSelector.AssetGuid, stanceSelector.AssetGuid, InitiatorLevels.Lvl5Guid)
        .AddEntry(10, quickToAct.AssetGuid, maneuverSelector.AssetGuid, stanceSelector.AssetGuid, ManeuverResources.IncreaseManeuverUsesGuid)
        .AddEntry(11, maneuverSelector.AssetGuid, InitiatorLevels.Lvl6Guid)
        .AddEntry(12, maneuverSelector.AssetGuid)
        .AddEntry(13, maneuverSelector.AssetGuid, ManeuverResources.IncreaseManeuverUsesGuid, InitiatorLevels.Lvl7Guid)
        .AddEntry(14, maneuverSelector.AssetGuid, stanceSelector.AssetGuid)
        .AddEntry(15, quickToAct.AssetGuid, maneuverSelector.AssetGuid, InitiatorLevels.Lvl8Guid, ManeuverResources.IncreaseManeuverUsesGuid)
        .AddEntry(16, maneuverSelector.AssetGuid)
        .AddEntry(17, FeatureRefs.ImprovedEvasion.Reference.Get(), maneuverSelector.AssetGuid, InitiatorLevels.Lvl9Guid)
        .AddEntry(18, maneuverSelector.AssetGuid, ManeuverResources.IncreaseManeuverUsesGuid)
        .AddEntry(19, maneuverSelector.AssetGuid)
        .AddEntry(20, quickToAct.AssetGuid, maneuverSelector.AssetGuid, ManeuverResources.IncreaseManeuverUsesGuid, stanceSelector.AssetGuid);

      var progression = ProgressionConfigurator.New("SwordsageProgression", "6AD2A307-54B1-4421-8C67-CF84F3B98B98")
        .SetRanks(1)
        .SetLevelEntries(entries)
        .SetClasses(Guid)
        .Configure();

      return progression;
    }
  }
}