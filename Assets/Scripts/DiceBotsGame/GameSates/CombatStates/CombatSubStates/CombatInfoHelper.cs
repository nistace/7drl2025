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
      }

      public static void DisableTooltips() {
      }


      private static void HideLog(DiceBot bot, CombatActionDefinition action) => HideLog();
      private static void ShowLog(DiceBot bot, CombatActionDefinition action) => ShowLog($"{bot.DisplayName} considers {action.DisplayName}");

      public static void ShowLog(string text) => MainUi.Log.SetTexts(ICombatSubState.BattleTitle, text);
      public static void HideLog() => MainUi.Log.SetTexts(ICombatSubState.BattleTitle, string.Empty);
   }
}