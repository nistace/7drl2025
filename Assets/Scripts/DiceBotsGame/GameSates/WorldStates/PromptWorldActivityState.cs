using DiceBotsGame.Cameras;
using DiceBotsGame.UI;
using DiceBotsGame.WorldLevels;

namespace DiceBotsGame.GameSates.WorldStates {
   public class PromptWorldActivityState : GameState {
      private readonly WorldCubeTileActivity activity;

      public PromptWorldActivityState(WorldCubeTileActivity activity) {
         this.activity = activity;
      }

      protected override void Enable() {
         MainUi.Log.SetTexts(WorldActionName, $"Pondering over this {activity.DisplayName}...");
         if (activity.IsOptional(out var optionalInfo)) {
            MainCameraController.ActivateCamera(activity.ActivityCamera);
            MainUi.World.Prompt.ShowPrompt(optionalInfo.PromptText, (optionalInfo.ContinueLabel, HandleContinueClicked), (optionalInfo.CancelLabel, HandleCancelClicked));
         }
         else {
            ChangeToWorldActivity(activity);
         }
      }

      private void HandleContinueClicked() => ChangeState(GetWorldActivityState(activity));
      private static void HandleCancelClicked() => ChangeState(WorldState.Instance);
      protected override void Disable() => MainUi.World.Prompt.Hide();
      protected override void Update() { }
   }
}