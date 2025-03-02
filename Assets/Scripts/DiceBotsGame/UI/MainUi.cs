using UnityEngine;

namespace DiceBotsGame.UI {
   public class MainUi : MonoBehaviour {
      private static MainUi instance { get; set; }

      [SerializeField] protected WorldUi world;
      [SerializeField] protected GameOverUi gameOver;

      public static WorldUi World => instance.world;
      public static GameOverUi GameOver => instance.gameOver;

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