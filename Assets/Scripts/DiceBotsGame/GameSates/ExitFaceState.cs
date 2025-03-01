using DiceBotsGame.WorldLevels.Activities;

namespace DiceBotsGame.GameSates {
   public class ExitFaceState : GameState {
      private readonly WorldCubeTileActivity exitActivity;

      public ExitFaceState(WorldCubeTileActivity exitActivity) {
         this.exitActivity = exitActivity;
      }

      protected override void Enable() {
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