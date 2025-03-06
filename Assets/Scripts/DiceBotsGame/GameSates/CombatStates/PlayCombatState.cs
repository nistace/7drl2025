using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.Cameras;
using DiceBotsGame.GameSates.CombatStates.CombatSubStates;
using DiceBotsGame.WorldLevels;

namespace DiceBotsGame.GameSates.CombatStates {
   public class PlayCombatState : GameState {
      private readonly WorldCubeTileEncounter encounter;
      private float lerp;

      private readonly IReadOnlyList<ICombatSubState> subStatesSequence;
      private int currentSubStateIndex;
      private ICombatSubState currentSubState => subStatesSequence[currentSubStateIndex];

      public PlayCombatState(WorldCubeTileEncounter encounter) {
         this.encounter = encounter;
         subStatesSequence = new ICombatSubState[] {
            new RollCombatSubState(GameInfo.PlayerParty.DiceBotsInParty.Union(this.encounter.DiceBots).ToArray()), new PlayerCombatSubState(), new OpponentCombatSubState()
         };
      }

      protected override void Enable() {
         MainCameraController.ActivateCamera(GameInfo.CombatGrid.CinemachineCamera);

         currentSubStateIndex = 0;
         currentSubState.StartState();
         CombatTipsHelper.EnableTips();

         CombatInputUtils.CombatActionMap.Enable();
      }

      protected override void Disable() {
         CombatInputUtils.CombatActionMap.Disable();
         CombatTipsHelper.DisableTips();
      }

      protected override void Update() {
         currentSubState.Update();

         if (TryChangeState()) return;

         if (currentSubState.IsOver) {
            NextSubState();
         }
      }

      private void NextSubState() {
         currentSubState.EndState();
         currentSubStateIndex = (currentSubStateIndex + 1) % subStatesSequence.Count;
         currentSubState.StartState();
      }

      private bool TryChangeState() {
         if (GameInfo.PlayerParty.DiceBotsInParty.All(t => t.HealthSystem.IsDead)) {
            ChangeState(new GameOverState(false));
            return true;
         }
         if (encounter.DiceBots.All(t => t.HealthSystem.IsDead)) {
            ChangeState(new EndCombatState(encounter));
            return true;
         }
         return false;
      }
   }
}