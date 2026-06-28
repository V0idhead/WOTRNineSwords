using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoidHeadWOTRNineSwords.Components;

namespace VoidHeadWOTRNineSwords.Swordsage.Archetypes.RimeRavagerManeuvers
{
    static class IceMirrorStance
    {
        public const string Guid = "4950A778-ACD3-4A43-B629-91912AAFE8C8";
        const string name = "IceMirrorStance.Name";
        //const string icon = Helpers.IconPrefix + "icemirrorstance.png";
        static Sprite icon = AbilityRefs.Flare.Reference.Get().Icon;

        public static BlueprintFeature Configure()
        {
            var stanceSelf = BuffConfigurator.New("IceMirrorStanceSelf", "4329E528-6627-40BF-BF84-19285A255B14")
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddNotDispelable()
              .AddTargetAttackRollTrigger(onlyMelee: true, onlyHit: true, affectFriendlyTouchSpells: false,
                actionsOnAttacker: ActionsBuilder.New()
                    .DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Cold), ContextDice.Value(Kingmaker.RuleSystem.DiceType.D10, ContextValues.Constant(2)))
                    .SavingThrow(Kingmaker.EntitySystem.Stats.SavingThrowType.Reflex, customDC: new ContextValue { Value = 16 }, conditionalDCModifiers: Helpers.GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom),
                        onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().DealDamageToAbility(Kingmaker.EntitySystem.Stats.StatType.Strength, ContextDice.Value(Kingmaker.RuleSystem.DiceType.D4, ContextValues.Constant(1), ContextValues.Constant(-1))))
                    )
              )
              .Configure();

            var stanceActivatable = ActivatableAbilityConfigurator.New("IceMirrorStanceActivatable", "5750CEAB-C856-49E3-B968-624F30A2171E")
              .SetDisplayName(name)
              .SetDescription("BorealisStance.Desc")
              .SetIcon(icon)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetBuff(stanceSelf)
              .SetDeactivateIfOwnerDisabled()
              .SetDeactivateIfOwnerUnconscious()
              .SetDoNotTurnOffOnRest()
              .SetGroup(ActivatableAbilityGroup.CombatStyle)
              .SetWeightInGroup(1)
              .Configure();

            return FeatureConfigurator.New("IceMirrorStance", Guid, AllManeuversAndStances.featureGroup)
              .SetDisplayName(name)
              .SetDescription("IceMirrorStance.Desc")
              .SetIcon(icon)
              .SetRanks(1)
              .AddFacts(new() { stanceActivatable })
              .Configure();
        }
    }
}