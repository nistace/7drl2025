using UnityEngine;

namespace DiceBotsGame.UI {
   public class MainUi : MonoBehaviour {
      private static MainUi instance { get; set; }

      [SerializeField] protected WorldUi world;
      [SerializeField] protected DiceBotsScreenUi diceBots;
      [SerializeField] protected GameOverUi gameOver;
      [SerializeField] protected LogUI log;

      public static WorldUi World => instance.world;
      public static DiceBotsScreenUi DiceBots => instance.diceBots;
      public static GameOverUi GameOver => instance.gameOver;
      public static LogUI Log => instance.log;

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