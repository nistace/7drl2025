using DiceBotsGame.Cameras;
using DiceBotsGame.WorldLevels;
using UnityEngine;

namespace DiceBotsGame.GameSates.CombatStates {
   public class StartCombatState : GameState {
      private readonly WorldCubeTileEncounter encounter;
      private float lerp;

      public StartCombatState(WorldCubeTileEncounter encounter) {
         this.encounter = encounter;
      }

      protected override void Enable() {
         MainCameraController.ActivateCamera(GameInfo.CombatGrid.CinemachineCamera);
         GameInfo.CombatGrid.PrepareCombat(encounter.transform, GameInfo.PlayerParty.DiceBotsInParty, encounter.DiceBots);
         GameInfo.CombatGrid.StartBattle();
      }

      protected override void Disable() { }

      protected override void Update() {
         lerp = Mathf.Clamp01(lerp + Time.deltaTime * GameInfo.CombatGrid.TransitionLerpSpeed);

         GameInfo.CombatGrid.Lerp(lerp);
         GameInfo.WorldCube.Lerp(1 - lerp);
         GameInfo.CombatGrid.UpdateAllBotsPosition();
         if (Mathf.Approximately(lerp, 1) && MainCameraController.AtAnchorPosition && GameInfo.CombatGrid.AreAllBotsAtTheirPosition) {
            ChangeState(new PlayCombatState(encounter));
         }
      }
   }
}