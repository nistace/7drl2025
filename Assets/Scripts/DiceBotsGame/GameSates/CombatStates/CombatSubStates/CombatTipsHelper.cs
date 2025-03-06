using DiceBotsGame.CombatActions;
using DiceBotsGame.DiceBots;
using DiceBotsGame.UI;

namespace DiceBotsGame.GameSates.CombatStates.CombatSubStates {
   public static class CombatTipsHelper {
      public static void EnableTips() {
         MainUi.DiceBots.OnPlayerBotActionHoverStarted.AddListener(HandleBotActionHoverStarted);
         MainUi.DiceBots.OnPlayerBotActionHoverStopped.AddListener(HandleBotActionHoverStopped);
         MainUi.DiceBots.OnOpponentActionHoverStarted.AddListener(HandleBotActionHoverStarted);
         MainUi.DiceBots.OnOpponentActionHoverStopped.AddListener(HandleBotActionHoverStopped);
      }

      public static void DisableTips() {
         MainUi.DiceBots.OnPlayerBotActionHoverStarted.RemoveListener(HandleBotActionHoverStarted);
         MainUi.DiceBots.OnPlayerBotActionHoverStopped.RemoveListener(HandleBotActionHoverStopped);
         MainUi.DiceBots.OnOpponentActionHoverStarted.RemoveListener(HandleBotActionHoverStarted);
         MainUi.DiceBots.OnOpponentActionHoverStopped.RemoveListener(HandleBotActionHoverStopped);
      }

      private static void HandleBotActionHoverStopped(DiceBot bot, CombatActionDefinition action) {
         MainUi.Log.SetTexts(ICombatSubState.BattleTitle, string.Empty);
      }

      private static void HandleBotActionHoverStarted(DiceBot bot, CombatActionDefinition action) {
         MainUi.Log.SetTexts(ICombatSubState.BattleTitle, $"{bot.DisplayName} considers {action.DisplayName}");
      }
   }
}