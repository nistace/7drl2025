using DiceBotsGame.UI;
using UnityEngine.SceneManagement;

namespace DiceBotsGame.GameSates {
   public class GameOverState : GameState {
      private readonly bool victory;

      public GameOverState(bool victory) {
         this.victory = victory;
      }

      protected override void Enable() {
         MainUi.HideAll();
         MainUi.GameOver.Show(victory);
         MainUi.GameOver.OnContinueClicked.AddListener(HandleContinueClicked);
      }

      private static void HandleContinueClicked() {
         SceneManager.LoadScene(0);
      }

      protected override void Disable() {
         MainUi.GameOver.Hide();
         MainUi.GameOver.OnContinueClicked.RemoveListener(HandleContinueClicked);
      }

      protected override void Update() { }
   }
}