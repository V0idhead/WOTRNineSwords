using BlueprintCore.Blueprints.Configurators.Facts;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using System.Linq;

namespace VoidHeadWOTRNineSwords.Feats
{
    static class RelentlessSirocco //Desert Wind Focus
    {
        public const string RelentlessSiroccoGuid = "7FA0F05D-F4D1-45C3-AFC9-ED62C76C9A50";
        public const string DesertWindFocusFactGuid = "272EF7E8-54E7-4681-BD33-64E7A7ECC242";
        static UnityEngine.Sprite icon = FeatureRefs.SpellFocusEvocation.Reference.Get().Icon;

        public static void Configure()
        {
            var buff = BuffConfigurator.New("RelentlessSiroccoDeBuff", "C624F50A-D116-456E-A278-034316A01A74")
              .SetDisplayName("RelentlessSirocco.Name")
              .SetDescription("RelentlessSirocco.DeBuffDesc")
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.Harmful)
              .AddEnergyVulnerability(Kingmaker.Enums.Damage.DamageEnergyType.Fire)
              .Configure();

            var enchant = WeaponEnchantmentConfigurator.New("RelentlessSiroccoEnchant", "044465BD-8E2A-4B6E-8DA0-0B7E898B9E92")
                .SetEnchantName("RelentlessSirocco.Name")
                .SetDescription("RelentlessSirocco.EnchantDesc")
                .AddWeaponDebuffOnAttack(buff, saveType: Kingmaker.EntitySystem.Stats.SavingThrowType.Will, dC: 17, duration: new Kingmaker.Utility.Rounds(10))
                .Configure();

            var desertWindFocusFact = UnitFactConfigurator.New("RelentlessSiroccoFact", DesertWindFocusFactGuid)
              .Configure();
            FeatureConfigurator.New("RelentlessSirocco", RelentlessSiroccoGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
              .SetDisplayName("RelentlessSirocco.Name")
              .SetDescription("RelentlessSirocco.Desc")
              .SetIcon(icon)
              .AddFeatureTagsComponent(FeatureTag.Attack | FeatureTag.Melee)
              .AddFacts(new() { desertWindFocusFact })
              .AddBuffEnchantAnyWeapon(enchant)
              .AddPrerequisiteFeaturesFromList(amount: 1, features: AllManeuversAndStances.DesertWindGuids.ToList())
              .Configure();
        }
    }
}