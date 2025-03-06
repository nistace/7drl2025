using UnityEngine;

namespace DiceBotsGame.UI {
   public class MainUi : MonoBehaviour {
      private static MainUi instance { get; set; }

      [SerializeField] protected PromptUi prompt;
      [SerializeField] protected CombatUi combat;
      [SerializeField] protected DiceBotsScreenUi diceBots;
      [SerializeField] protected GameOverUi gameOver;
      [SerializeField] protected LogUI log;
      [SerializeField] protected TooltipUi tooltip;

      public static PromptUi Prompt => instance.prompt;
      public static CombatUi Combat => instance.combat;
      public static DiceBotsScreenUi DiceBots => instance.diceBots;
      public static GameOverUi GameOver => instance.gameOver;
      public static LogUI Log => instance.log;
      public static TooltipUi Tooltip => instance.tooltip;

      private void Awake() {
         instance = this;
      }

      public static void HideAll() {
         foreach (var screen in instance.GetComponentsInChildren<IMainScreenUi>()) {
            screen.Hide();
         }
      }
   }
}