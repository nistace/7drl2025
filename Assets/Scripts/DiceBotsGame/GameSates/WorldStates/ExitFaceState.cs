using DiceBotsGame.Cameras;
using DiceBotsGame.UI;
using DiceBotsGame.WorldLevels;

namespace DiceBotsGame.GameSates.WorldStates {
   public class ExitFaceState : GameState {
      private readonly WorldCubeTileActivity exitActivity;
      private bool cubeRotated;

      public ExitFaceState(WorldCubeTileActivity exitActivity) {
         this.exitActivity = exitActivity;
      }

      protected override void Enable() {
         MainUi.Log.SetTexts(WorldActionName, "Moving on to the next face of the cube!");
         MainCameraController.ActivateWorldCamera();
         GameInfo.WorldCube.RotateToFace(GameInfo.WorldCube.CurrentFaceIndex + 1, HandleCubeRotated);
         GameInfo.PlayerParty.SetWorldPosition(GameInfo.WorldCube.GetFaceEntryInNextFace());
      }

      private void HandleCubeRotated() => cubeRotated = true;

      protected override void Disable() { }

      protected override void Update() {
         GameInfo.PlayerParty.UpdateBotsWorldPosition();
         if (GameInfo.PlayerParty.AllBotsAtWorldTarget && cubeRotated) {
            exitActivity.SetSolved(true);
            ChangeState(new EnterFaceState(GameInfo.WorldCube.GetFaceEntryInCurrentFace()));
         }
      }
   }
}