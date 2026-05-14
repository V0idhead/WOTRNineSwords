using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoidHeadWOTRNineSwords.Common;
using VoidHeadWOTRNineSwords.Components;
using VoidHeadWOTRNineSwords.DesertWind;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.RivenHourglass
{
    //https://www.d20pfsrd.com/ALTERNATIVE-RULE-SYSTEMS/3RD-PARTY-RULES-SYSTEMS/PATH-OF-WAR/DISCIPLINES-AND-MANEUVERS/RIVEN-HOURGLASS-MANEUVERS/#TOC-Sands-of-Time-Stance
    static class SandsOfTime
    {
        public const string Guid = "8482F0D8-CE7C-4C6F-AAB1-9BF2020E32A6";
        const string name = "SandsOfTime.Name";
        const string desc = "SandsOfTime.Desc";
        //const string icon = Helpers.IconPrefix + "sandsoftime.png";
        static UnityEngine.Sprite icon = AbilityRefs.FlareBurst.Reference.Get().Icon;

        public static void Configure()
        {
            Main.Logger.Info($"Configuring {nameof(SandsOfTime)}");

            var buff = BuffConfigurator.New("SandsOfTimeBuff", "8A57AF8D-6554-4631-9184-B304AAB00400")
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .SetDisplayName(name)
              .SetDescription(desc)
              .SetIcon(icon)
              .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.Inherent, stat: StatType.SaveReflex, value: 3)
              .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.Inherent, stat: StatType.Initiative, value: 3)
              .Configure();

            var activatable = ActivatableAbilityConfigurator.New("SandsOfTimeActivatable", "CCB96603-00E0-400F-9091-936BB70FB441")
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

            var feat = FeatureConfigurator.New("SandsOfTimeFeat", Guid)
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