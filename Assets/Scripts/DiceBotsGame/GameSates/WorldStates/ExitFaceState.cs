using DiceBotsGame.Cameras;
using DiceBotsGame.UI;
using DiceBotsGame.WorldLevels;

namespace DiceBotsGame.GameSates.WorldStates {
   public class ExitFaceState : GameState {
      private readonly WorldCubeTileActivity exitActivity;

      public ExitFaceState(WorldCubeTileActivity exitActivity) {
         this.exitActivity = exitActivity;
      }

      protected override void Enable() {
         MainUi.Log.SetTexts(WorldActionName, "Moving on to the next face of the cube!");
         MainCameraController.ActivateWorldCamera();
         GameInfo.WorldCube.RotateToFace(GameInfo.WorldCube.CurrentFaceIndex + 1, HandleCubeRotated);
      }

      private void HandleCubeRotated() {
         GameInfo.PlayerParty.SetWorldPosition(GameInfo.WorldCube.GetFaceEntryInCurrentFace());
         exitActivity.Solved = true;

         ChangeState(new EnterFaceState(GameInfo.WorldCube.GetFaceEntryInCurrentFace()));
      }

      protected override void Disable() { }
      protected override void Update() { }
   }
}