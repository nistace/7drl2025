using DiceBotsGame.WorldLevels;

namespace DiceBotsGame.GameSates {
   public class EnterFaceState : GameState {
      private readonly WorldCubeTile entryFace;

      public EnterFaceState(WorldCubeTile entryFace) {
         this.entryFace = entryFace;
      }

      protected override void Enable() {
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