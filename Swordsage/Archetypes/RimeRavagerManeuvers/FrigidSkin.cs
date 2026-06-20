using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.Counters;
using VoidHeadWOTRNineSwords.DesertWind;

namespace VoidHeadWOTRNineSwords.Swordsage.Archetypes.RimeRavagerManeuvers
{
    static class FrigidSkin
    {
        public const string Guid = "BCD84E88-9CF6-4262-9E17-1350EAC8CA9D";
        public const string ActiveBuffGuid = "5FE18FDC-A198-4293-9348-BE8D4CB8CDF0";
        const string ActiveFactGuid = "244EF70C-875D-4C98-AD41-F0E66FD27215";
        public static BlueprintUnitFact ActiveFact { get; private set; }
        const string OnFactGuid = "5C69890F-956D-4FE5-B30B-4AE1D8C8E5BC";
        public static BlueprintUnitFact OnFact { get; private set; }
        public static BlueprintActivatableAbility Activatable { get; private set; }
        const string name = "FrigidSkin.Name";
        const string desc = "FrigidSkin.Desc";
        //const string icon = Helpers.IconPrefix + "frigidskin.png";
        static Sprite icon = AbilityRefs.Flare.Reference.Get().Icon;

        public static BlueprintFeature Configure()
        {
            Main.Log($"Configuring {nameof(FrigidSkin)}");

            ActiveFact = UnitFactConfigurator.New("FrigidSkinActiveFact", ActiveFactGuid).Configure();
            OnFact = UnitFactConfigurator.New("FrigidSkinOnFact", OnFactGuid).Configure();

            var activeBuff = BuffConfigurator.New("FrigidSkinActiveBuff", ActiveBuffGuid)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddFacts(new() { ActiveFact })
              .Configure();

            var toggleBuff = BuffConfigurator.New("FrigidSkinOn", "A648760A-05AC-4466-8800-B21F117206EA")
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddComponent<UnifiedDebuffCounter>()
              .AddFacts(new List<Blueprint<Kingmaker.Blueprints.BlueprintUnitFactReference>> { OnFact })
              .Configure();

            Activatable = ActivatableAbilityConfigurator.New("FrigidSkinActivatable", "9D95AE19-4505-4396-AB75-9CA0AD6C76BD")
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetBuff(toggleBuff)
              .Configure();

            return FeatureConfigurator.New("FrigidSkinFeat", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .SetRanks(1)
              .AddFacts(new() { Activatable })
              .Configure(true);
        }
    }
}