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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Warblade;

namespace VoidHeadWOTRNineSwords.Swordsage
{
  //https://dndtools.net/classes/swordsage/
  public static class SwordsageC
  {
    public const string Guid = "600E0E53-9A37-4D59-89ED-2234908D283B";

    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    public static void ConfigureClass()
    {
      log.Info($"{nameof(SwordsageC)} configuring");
      BlueprintProgression progression = ConfigureProgression();

      BlueprintCharacterClass swordsageC = CharacterClassConfigurator.New("Swordsage", Guid)
      .SetLocalizedName("SwordsageC.Name")
      .SetLocalizedDescription("SwordsageC.Desc")
      .SetSkillPoints(0) //should be 5, lowered to speed up char creation for testing
      .SetHitDie(DiceType.D8)
      .SetIsArcaneCaster(false).SetIsDivineCaster(false)
      .SetBaseAttackBonus(StatProgressionRefs.BABMedium.Reference.Get())
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
    }

    public static BlueprintProgression ConfigureProgression()
    {
      SwordsageRecoverManeuvers.Configure();
      var quickToAct = QuickToAct.Configure();
      var bonusWeaponFocus = BonusWeaponFocus.Configure();
      var swordsageACBonus = SwordsageACBonus.Configure();
      var swordsageDamageBonus = SwordsageDamageBonus.Configure();
      /*var battleCunning = BattleCunning.Configure();
      var battleSkill = BattleSkill.Configure();
      var battleMastery = BattleMastery.Configure();
      var stanceMastery = StanceMastery.Configure();*/
      var maneuverSelector = SwordsageManeuverSelection.Configure();
      var stanceSelector = SwordsageStanceSelection.Configure();

      var swordsageProficiencies = FeatureConfigurator.New("SwordsageProficiencies", "E6DEFCF2-0C19-45F4-A047-13F71B52DD9A")
        .SetDisplayName("SwordsageProficiencies.Name")
        .SetDescription("SwordsageProficiencies.Desc")
        .AddFacts(new() { FeatureRefs.SimpleWeaponProficiency.Reference.Get(), FeatureRefs.MartialWeaponProficiency.Reference.Get(), FeatureRefs.LightArmorProficiency.Reference.Get(), DisciplineProficencies.DesertWindProficencyGuid, DisciplineProficencies.DiamondMindProficencyGuid, DisciplineProficencies.IronHeartProficencyGuid, DisciplineProficencies.SettingSunProficencyGuid, DisciplineProficencies.ShadowHandProficencyGuid, DisciplineProficencies.StoneDragonProficencyGuid, DisciplineProficencies.TigerClawProficencyGuid, DisciplineProficencies.StoneDragonProficencyGuid })
        .SetIsClassFeature()
        .SetRanks(1)
        .Configure();

      var entries = LevelEntryBuilder.New()
        .AddEntry(1, swordsageProficiencies.AssetGuid, SwordsageRecoverManeuvers.Guid, quickToAct.AssetGuid, bonusWeaponFocus.AssetGuid, maneuverSelector.AssetGuid, maneuverSelector.AssetGuid, maneuverSelector.AssetGuid, maneuverSelector.AssetGuid, maneuverSelector.AssetGuid, maneuverSelector.AssetGuid, stanceSelector.AssetGuid, InitiatorLevels.Lvl1Guid, ManeuverResources.IncreaseManeuverUsesGuid) //6 maneuvers, 1 stance
        .AddEntry(2, swordsageACBonus.AssetGuid, maneuverSelector.AssetGuid, stanceSelector.AssetGuid)
        .AddEntry(3, maneuverSelector.AssetGuid, ManeuverResources.IncreaseManeuverUsesGuid, InitiatorLevels.Lvl2Guid)
        .AddEntry(4, swordsageDamageBonus.AssetGuid, ManeuverResources.IncreaseManeuverUsesGuid)
        .AddEntry(5, quickToAct.AssetGuid, maneuverSelector, InitiatorLevels.Lvl3Guid) //implement
        /*.AddEntry(6, )
        .AddEntry(7, , maneuverSelector.AssetGuid, InitiatorLevels.Lvl4Guid)
        .AddEntry(9, , maneuverSelector, InitiatorLevels.Lvl5Guid)
        .AddEntry(10, quickToAct.AssetGuid, stanceSelector.AssetGuid, ManeuverResources.IncreaseManeuverUsesGuid)
        .AddEntry(11, , maneuverSelector.AssetGuid, InitiatorLevels.Lvl6Guid)
        .AddEntry(13, , maneuverSelector.AssetGuid, InitiatorLevels.Lvl7Guid)
        .AddEntry(15, quickToAct.AssetGuid, maneuverSelector.AssetGuid, InitiatorLevels.Lvl8Guid, ManeuverResources.IncreaseManeuverUsesGuid)
        .AddEntry(16, stanceSelector.AssetGuid)
        .AddEntry(17, , maneuverSelector.AssetGuid, InitiatorLevels.Lvl9Guid)
        .AddEntry(19, maneuverSelector.AssetGuid)*/
        .AddEntry(20, quickToAct.AssetGuid, ManeuverResources.IncreaseManeuverUsesGuid);

      var progression = ProgressionConfigurator.New("SwordsageProgression", "6AD2A307-54B1-4421-8C67-CF84F3B98B98")
        .SetRanks(1)
        .SetLevelEntries(entries)
        .SetClasses(Guid)
        .Configure();

      return progression;
    }
  }
}