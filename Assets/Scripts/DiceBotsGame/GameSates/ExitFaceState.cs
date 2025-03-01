namespace DiceBotsGame.GameSates {
   public class ExitFaceState : GameState {
      protected override void Enable() {
         GameInfo.WorldCube.RotateToFace(GameInfo.WorldCube.CurrentFaceIndex + 1, HandleCubeRotated);
      }

      private static void HandleCubeRotated() {
         ChangeState(WorldState.Instance);
      }

      protected override void Disable() { }

      protected override void Update() { }
   }
}