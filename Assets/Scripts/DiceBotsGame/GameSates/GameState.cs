using System;
using DiceBotsGame.GameSates.WorldStates;
using DiceBotsGame.WorldLevels;

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

      protected static void ChangeToWorldActivity(WorldCubeTileActivity activity) => ChangeState(GetWorldActivityState(activity));

      protected static GameState GetWorldActivityState(WorldCubeTileActivity activity) {
         return activity.Type switch {
            WorldCubeTileActivity.EType.None => WorldState.Instance,
            WorldCubeTileActivity.EType.ExitFace => new ExitFaceState(activity),
            WorldCubeTileActivity.EType.MeetEncounter => new MeetEncounterState(activity),
            WorldCubeTileActivity.EType.EnterFace => throw new ArgumentException($"Activities of type {activity.GetType().Name} are not supported."),
            _ => throw new ArgumentException($"Activities of type {activity.GetType().Name} are not supported.")
         };
      }
   }
}