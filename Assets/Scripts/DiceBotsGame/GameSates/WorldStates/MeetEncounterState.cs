using DiceBotsGame.Cameras;
using DiceBotsGame.GameSates.CombatStates;
using DiceBotsGame.WorldLevels;

namespace DiceBotsGame.GameSates.WorldStates {
   public class MeetEncounterState : GameState {
      private readonly WorldCubeTileActivity activity;
      private readonly WorldCubeTileEncounter encounter;

      public MeetEncounterState(WorldCubeTileActivity activity) {
         this.activity = activity;
         encounter = this.activity.GetComponent<WorldCubeTileEncounter>();
      }

      protected override void Enable() {
         MainCameraController.ActivateCamera(activity.ActivityCamera);
      }

      protected override void Disable() { }

      protected override void Update() {
         while (!MainCameraController.AtAnchorPosition) return;
         ChangeState(new StartCombatState(encounter));
      }
   }
}