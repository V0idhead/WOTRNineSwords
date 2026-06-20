using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Swordsage.Archetypes;
using VoidHeadWOTRNineSwords.Swordsage.Archetypes.RimeRavagerManeuvers;
using VoidHeadWOTRNineSwords.Warblade;
using VoidHeadWOTRNineSwords.Warblade.Archetypes;

namespace VoidHeadWOTRNineSwords.Swordsage.Archetypes
{
    static class RimeRavager
    {
        public const string Guid = "66ABB74E-5BA5-4968-889F-8EAFE47B4FB6";
        public static void Configure()
        {
            //Lvl1 Polaris Strike: add Frost Damage, minor attack bonus penalty to target
            var piercingIcicle = PiercingIcicle.Configure(); //Lvl2 
            var frigidSkin = FrigidSkin.Configure(); //Lvl3 Counter
            var borealisStance = BorealisStance.Configure(); //Lvl3 Stance
            var iceJawStrike = IceJawStrike.Configure(); //Lvl4 
            //Lvl5
            //Lvl5 Stance 
            //Lvl6
            //Lvl7
            //Lvl8
            //Lvl9

            var rimeRavagerProficiencies = FeatureConfigurator.New("RimeRavagerProficiencies", "39B57D69-1EA0-4629-9A1F-A91C2034AE2F")
              .SetDisplayName("RimeRavagerProficiencies.Name")
              .SetDescription("RimeRavagereProficiencies.Desc")
              .AddFacts(new() { FeatureRefs.SimpleWeaponProficiency.Reference.Get(), FeatureRefs.MartialWeaponProficiency.Reference.Get(), FeatureRefs.LightArmorProficiency.Reference.Get(), DisciplineProficencies.DiamondMindProficencyGuid, DisciplineProficencies.IronHeartProficencyGuid, DisciplineProficencies.RivenHourglassProficencyGuid, DisciplineProficencies.ShadowHandProficencyGuid, DisciplineProficencies.StoneDragonProficencyGuid, DisciplineProficencies.TigerClawProficencyGuid })
              .SetIsClassFeature()
              .SetRanks(1)
              .Configure();

            ArchetypeConfigurator.New("RimeRavager", Guid, SwordsageC.Guid)
                .SetLocalizedName("RimeRavager.Name")
                .SetLocalizedDescription("RimeRavager.Desc")
                .AddToRemoveFeatures(1, SwordsageC.SwordsageProficienciesGuid)
                .AddToRemoveFeatures(2, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(4, SwordsageManeuverSelection.Guid)
                .AddToRemoveFeatures(5, Swordsage.SwordsageStanceSelection.Guid)
                .AddToRemoveFeatures(6, SwordsageManeuverSelection.Guid)
                .AddToAddFeatures(1, rimeRavagerProficiencies.AssetGuid)
                .AddToAddFeatures(2, piercingIcicle)
                .AddToAddFeatures(4, frigidSkin)
                .AddToAddFeatures(4, borealisStance)
                .AddToAddFeatures(6, iceJawStrike)
                .Configure();
        }
    }
}