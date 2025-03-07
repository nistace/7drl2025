using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.Cameras;
using DiceBotsGame.GameSates.CombatStates.CombatSubStates;
using DiceBotsGame.WorldLevels;

namespace DiceBotsGame.GameSates.CombatStates {
   public class PlayCombatState : GameState {
      private readonly WorldCubeTileEncounter encounter;
      private float lerp;
      private int turn;

      private readonly List<ICombatSubState> subStatesSequence;
      private readonly SuddenDeathCombatSubState suddenDeathSubState;
      private int currentSubStateIndex;
      private ICombatSubState currentSubState => subStatesSequence[currentSubStateIndex];

      public PlayCombatState(WorldCubeTileEncounter encounter) {
         this.encounter = encounter;
         var allBots = GameInfo.PlayerParty.DiceBotsInParty.Union(this.encounter.DiceBots).ToArray();
         subStatesSequence = new List<ICombatSubState> { new RollCombatSubState(allBots), new PlayerCombatSubState(), new OpponentCombatSubState() };

         suddenDeathSubState = new SuddenDeathCombatSubState(allBots);
      }

      protected override void Enable() {
         MainCameraController.ActivateCamera(GameInfo.CombatGrid.CinemachineCamera);

         currentSubStateIndex = 0;
         currentSubState.StartState();
         CombatInfoHelper.EnableTooltips();

         CombatInputUtils.ActionMap.Enable();
      }

      protected override void Disable() {
         CombatInputUtils.ActionMap.Disable();
         CombatInfoHelper.DisableTooltips();
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

         if (currentSubStateIndex == subStatesSequence.Count - 1) {
            turn++;
            if (turn == 5) {
               subStatesSequence.Add(suddenDeathSubState);
            }
            else if (turn > 5 && (turn - 5) % 4 == 0) {
               suddenDeathSubState.IncreaseEffect();
            }
         }

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