using DiceBotsGame.WorldLevels.Activities;

namespace DiceBotsGame.GameSates {
   public abstract class GameState {
      public static GameState CurrentState { get; private set; }

      public static void ChangeState(GameState nextState) {
         CurrentState?.Disable();
         CurrentState = nextState;
         CurrentState?.Enable();
      }

      public static void UpdateCurrentState() => CurrentState?.Update();

      protected abstract void Enable();
      protected abstract void Disable();
      protected abstract void Update();

      protected static void StartWorldActivityState(IWorldCubeTileActivity activity) {
         if (activity is ExitFaceWorldCubeTileActivity exitFace) {
            ChangeState(new ExitFaceState());
         }
      }
   }
}