using DiceBotsGame.Cameras;
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
      }

      protected override void Disable() { }

      protected override void Update() {
         GameInfo.PlayerParty.UpdateBotsWorldPosition();
         if (GameInfo.PlayerParty.AllBotsAtWorldTarget) {
            entryFace.Activity.SetSolved(true);
            ChangeState(WorldState.Instance);
         }
      }
   }
}