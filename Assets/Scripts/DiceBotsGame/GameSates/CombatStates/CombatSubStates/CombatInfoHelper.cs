using DiceBotsGame.CombatActions;
using DiceBotsGame.DiceBots;
using DiceBotsGame.UI;

namespace DiceBotsGame.GameSates.CombatStates.CombatSubStates {
   public static class CombatInfoHelper {
      public static void EnableLog() {
         MainUi.DiceBots.OnPlayerBotActionHoverStarted.AddListener(ShowLog);
         MainUi.DiceBots.OnPlayerBotActionHoverStopped.AddListener(HideLog);
         MainUi.DiceBots.OnOpponentActionHoverStarted.AddListener(ShowLog);
         MainUi.DiceBots.OnOpponentActionHoverStopped.AddListener(HideLog);
      }

      public static void DisableLog() {
         MainUi.DiceBots.OnPlayerBotActionHoverStarted.RemoveListener(ShowLog);
         MainUi.DiceBots.OnPlayerBotActionHoverStopped.RemoveListener(HideLog);
         MainUi.DiceBots.OnOpponentActionHoverStarted.RemoveListener(ShowLog);
         MainUi.DiceBots.OnOpponentActionHoverStopped.RemoveListener(HideLog);
      }

      public static void EnableTooltips() {
         MainUi.DiceBots.OnPlayerBotActionHoverStarted.AddListener(ShowTooltip);
         MainUi.DiceBots.OnPlayerBotActionHoverStopped.AddListener(HideTooltip);
         MainUi.DiceBots.OnOpponentActionHoverStarted.AddListener(ShowTooltip);
         MainUi.DiceBots.OnOpponentActionHoverStopped.AddListener(HideTooltip);
      }

      public static void DisableTooltips() {
         MainUi.DiceBots.OnPlayerBotActionHoverStarted.RemoveListener(ShowTooltip);
         MainUi.DiceBots.OnPlayerBotActionHoverStopped.RemoveListener(HideTooltip);
         MainUi.DiceBots.OnOpponentActionHoverStarted.RemoveListener(ShowTooltip);
         MainUi.DiceBots.OnOpponentActionHoverStopped.RemoveListener(HideTooltip);
      }

      private static void HideTooltip(DiceBot bot, CombatActionDefinition action) => MainUi.Tooltip.Hide(action);

      private static void ShowTooltip(DiceBot bot, CombatActionDefinition action) {
         if (action.IsValidAction) {
            MainUi.Tooltip.Show($"{action.Action.ActionName} ({action.ConstantStrength})\n"
                                + $"  > {action.Action.GetDisplayConditions(action.ConstantStrength)}\n"
                                + $"  > {action.Action.GetDisplayEffects(action.ConstantStrength)}",
               action);
         }
         else {
            MainUi.Tooltip.Hide();
         }
      }

      private static void HideLog(DiceBot bot, CombatActionDefinition action) => HideLog();
      private static void ShowLog(DiceBot bot, CombatActionDefinition action) => ShowLog($"{bot.DisplayName} considers {action.DisplayName}");

      public static void ShowLog(string text) => MainUi.Log.SetTexts(ICombatSubState.BattleTitle, text);
      public static void HideLog() => MainUi.Log.SetTexts(ICombatSubState.BattleTitle, string.Empty);
   }
}