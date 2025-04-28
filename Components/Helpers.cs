using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.Models.Log;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem.LogThreads.Common;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoidHeadWOTRNineSwords.Feats;

namespace VoidHeadWOTRNineSwords.Components
{
  static class Helpers
  {
    public const string IconPrefix = "assets/icons/";

    public static List<(ConditionsBuilder conditions, ContextValue modifier)> GetManeuverDCModifier(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty property, string disciplineFocusGuid)
    {
      return new List<(ConditionsBuilder conditions, ContextValue modifier)>
      {
        (ConditionsBuilder.New().AddTrue(), new ContextValue { Property = property, ValueType = ContextValueType.CasterProperty }),
        (ConditionsBuilder.New().CasterHasFact(ManeuverFocus.Guid), new ContextValue{Value = 1}),
        (ConditionsBuilder.New().CasterHasFact(disciplineFocusGuid), new ContextValue{Value = 1}),
        (ConditionsBuilder.New().CasterHasFact(MythicManeuverFocus.Guid), new ContextValue{Property = Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.MythicLevel, ValueType = ContextValueType.CasterProperty})
      };
    }

    public static void WriteCombatLogMessage(LocalString messageText, Color color, UnitEntityData source_entity = null, UnitEntityData target = null, string text = "", string description = "", string text_with_tags = "")
    {
      using (ProfileScope.New("Build Simple Combat Log Message", (SimpleBlueprint)null))
      {
        using (GameLogContext.Scope)
        {
          if (source_entity != null)
            GameLogContext.SourceUnit = source_entity;

          GameLogContext.Target = target;
          GameLogContext.Message = messageText?.LocalizedString;
          GameLogContext.Text = text;
          GameLogContext.TextWithTags = text_with_tags;
          GameLogContext.Description = description;
          string message_text = GameLogContext.Message.ToString();
          TooltipTemplateCombatLogMessage template = null;

          CombatLogMessage message = new CombatLogMessage(message_text, color, GameLogContext.GetIcon(), template, true);


          var messageLog = LogThreadService.Instance.m_Logs[LogChannelType.Common].Last(x => x is MessageLogThread);
          messageLog.AddMessage(message);
        }
      }
    }
  }
}