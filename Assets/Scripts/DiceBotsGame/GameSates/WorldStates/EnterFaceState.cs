﻿using DiceBotsGame.Cameras;
using DiceBotsGame.UI;
using DiceBotsGame.WorldLevels;

namespace DiceBotsGame.GameSates.WorldStates {
   public class EnterFaceState : GameState {
      private readonly WorldCubeTile entryFace;

      public EnterFaceState(WorldCubeTile entryFace) {
         this.entryFace = entryFace;
      }

      protected override void Enable() {
         MainUi.Log.SetTexts(WorldActionName, "Entering the face");
         MainCameraController.ActivateCamera(entryFace.Activity.ActivityCamera);
         GameInfo.PlayerParty.SetWorldPosition(entryFace);
         GameInfo.PlayerParty.SnapBotsToWorldPosition();
      }

      protected override void Disable() { }

      protected override void Update() {
         entryFace.Activity.Solved = true;
         ChangeState(WorldState.Instance);
      }
   }
}