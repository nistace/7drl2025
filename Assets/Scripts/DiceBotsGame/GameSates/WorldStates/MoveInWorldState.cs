using System.Collections.Generic;
using DiceBotsGame.Cameras;
using DiceBotsGame.UI;
using DiceBotsGame.WorldLevels;

namespace DiceBotsGame.GameSates.WorldStates {
   public class MoveInWorldState : GameState {
      private readonly Queue<WorldCubeTile> path;

      public MoveInWorldState(IReadOnlyList<WorldCubeTile> path) {
         this.path = new Queue<WorldCubeTile>(path);
      }

      protected override void Enable() {
         MainUi.Log.SetTexts(WorldActionName, "On the way!");
         MainCameraController.ActivateWorldCamera();
      }

      protected override void Disable() { }

      protected override void Update() {
         GameInfo.PlayerParty.UpdateBotsWorldPosition();

         if (GameInfo.PlayerParty.AllBotsAtWorldTarget) {
            if (path.Count > 0) {
               GameInfo.PlayerParty.SetWorldPosition(path.Dequeue());
            }
            else {
               if (GameInfo.PlayerParty.CurrentTile.Activity == null || GameInfo.PlayerParty.CurrentTile.Activity.Solved) {
                  ChangeState(WorldState.Instance);
               }
               else if (GameInfo.PlayerParty.CurrentTile.Activity.IsOptional(out _)) {
                  ChangeState(new PromptWorldActivityState(GameInfo.PlayerParty.CurrentTile.Activity));
               }
               else {
                  ChangeState(GetWorldActivityState(GameInfo.PlayerParty.CurrentTile.Activity));
               }
            }
         }
      }
   }
}