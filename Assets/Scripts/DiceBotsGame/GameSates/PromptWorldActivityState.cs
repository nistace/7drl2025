using DiceBotsGame.UI;
using DiceBotsGame.WorldLevels.Activities;

namespace DiceBotsGame.GameSates {
   public class PromptWorldActivityState : GameState {
      private readonly IWorldCubeTileActivity activity;

      public PromptWorldActivityState(IWorldCubeTileActivity activity) {
         this.activity = activity;
      }

      protected override void Enable() => WorldUi.Prompt.ShowPrompt(activity.PromptText, (activity.ContinueLabel, HandleContinueClicked), (activity.CancelLabel, HandleCancelClicked));
      private void HandleContinueClicked() => StartWorldActivityState(activity);
      private static void HandleCancelClicked() => ChangeState(WorldState.Instance);
      protected override void Disable() => WorldUi.Prompt.Hide();

      protected override void Update() { }
   }
}