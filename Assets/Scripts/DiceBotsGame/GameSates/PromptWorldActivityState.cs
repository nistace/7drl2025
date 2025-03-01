using DiceBotsGame.UI;
using DiceBotsGame.WorldLevels.Activities;

namespace DiceBotsGame.GameSates {
   public class PromptWorldActivityState : GameState {
      private readonly WorldCubeTileActivity activity;

      public PromptWorldActivityState(WorldCubeTileActivity activity) {
         this.activity = activity;
      }

      protected override void Enable() {
         if (activity.IsOptional(out var optionalInfo)) {
            WorldUi.Prompt.ShowPrompt(optionalInfo.PromptText, (optionalInfo.ContinueLabel, HandleContinueClicked), (optionalInfo.CancelLabel, HandleCancelClicked));
         }
         else {
            ChangeToWorldActivity(activity);
         }
      }

      private void HandleContinueClicked() => ChangeState(GetWorldActivityState(activity));
      private static void HandleCancelClicked() => ChangeState(WorldState.Instance);
      protected override void Disable() => WorldUi.Prompt.Hide();
      protected override void Update() { }
   }
}