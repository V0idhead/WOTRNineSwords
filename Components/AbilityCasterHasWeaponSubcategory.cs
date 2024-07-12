using BlueprintCore.Utils;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityModManagerNet.UnityModManager.ModEntry;

namespace VoidHeadWOTRNineSwords.Components
{
  //from https://github.com/WittleWolfie/CharacterOptionsPlus/blob/main/CharacterOptionsPlus/CharacterOptionsPlus/Components/AbilityCasterHasWeaponSubcategory.cs
  [AllowedOn(typeof(BlueprintAbility))]
  [TypeId("446b614c-fbd4-4c25-98f6-94c3cc0f5eae")]
  internal class AbilityCasterHasWeaponSubcategory : BlueprintComponent, IAbilityCasterRestriction
  {
    private static readonly LogWrapper log = LogWrapper.Get("VoidHeadWOTRNineSwords");

    private readonly List<WeaponSubCategory> SubCategories = new();

    internal AbilityCasterHasWeaponSubcategory(params WeaponSubCategory[] subCategories)
    {
      SubCategories.AddRange(subCategories);
    }

    public string GetAbilityCasterRestrictionUIText()
    {
      var subCategories =
        SubCategories.Select(subCategory => LocalizedTexts.Instance.WeaponSubCategories.GetText(subCategory));
      return string.Format(
        LocalizationTool.GetString("AbilityCasterHasWeaponSubcategory.Restriction"),
        string.Join(", ", subCategories));
    }

    public bool IsCasterRestrictionPassed(UnitEntityData caster)
    {
      try
      {
        if (caster is null)
        {
          log.Info("No target");
          return false;
        }

        var weapon = caster.GetFirstWeapon();
        if (weapon is null)
          return false;

        foreach (var subCategory in SubCategories)
        {
          if (weapon.Blueprint.Category.HasSubCategory(subCategory))
            return true;
        }
      }
      catch (Exception e)
      {
        log.Error("AbilityCasterHasWeaponSubcategory.IsCasterRestrictionPassed: " + e.Message);
      }
      return false;
    }
  }
}