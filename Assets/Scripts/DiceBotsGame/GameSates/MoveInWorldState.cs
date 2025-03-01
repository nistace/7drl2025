using System.Collections.Generic;
using DiceBotsGame.WorldLevels;
using UnityEngine;

namespace DiceBotsGame.GameSates {
   public class MoveInWorldState : GameState {
      private readonly Queue<WorldCubeTile> path;

      public MoveInWorldState(IReadOnlyList<WorldCubeTile> path) {
         this.path = new Queue<WorldCubeTile>(path);
      }

      protected override void Enable() { }

      protected override void Disable() { }

      protected override void Update() {
         GameInfo.PlayerParty.UpdateBotsWorldPosition();

         if (GameInfo.PlayerParty.AllBotsAtWorldTarget) {
            if (path.Count > 0) {
               var nextNode = path.Dequeue();
               GameInfo.PlayerParty.transform.rotation = Quaternion.LookRotation(nextNode.transform.position - GameInfo.PlayerParty.CurrentTile.transform.position, Vector3.up);
               GameInfo.PlayerParty.transform.position = nextNode.transform.position;
               GameInfo.PlayerParty.CurrentTile = nextNode;
            }
            else {
               if (GameInfo.PlayerParty.CurrentTile.Activity == null || GameInfo.PlayerParty.CurrentTile.Activity.Solved) {
                  ChangeState(WorldState.Instance);
               }
               else if (GameInfo.PlayerParty.CurrentTile.Activity.CanRevert) {
                  ChangeState(new PromptWorldActivityState(GameInfo.PlayerParty.CurrentTile.Activity));
               }
               else {
                  StartWorldActivityState(GameInfo.PlayerParty.CurrentTile.Activity);
               }
            }
         }
      }
   }
}