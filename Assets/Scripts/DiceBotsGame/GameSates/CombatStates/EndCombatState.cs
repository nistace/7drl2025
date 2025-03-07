using DiceBotsGame.BotUpgrades;
using DiceBotsGame.Cameras;
using DiceBotsGame.GameSates.WorldStates;
using DiceBotsGame.UI;
using DiceBotsGame.WorldLevels;
using UnityEngine;

namespace DiceBotsGame.GameSates.CombatStates {
   public class EndCombatState : GameState {
      private readonly WorldCubeTileEncounter encounter;
      private float lerp = 1;

      public EndCombatState(WorldCubeTileEncounter encounter) {
         this.encounter = encounter;
      }

      protected override void Enable() {
         MainCameraController.ActivateCamera(GameInfo.CombatGrid.CinemachineCamera);
         MainUi.Log.SetTexts("Battle is won!", "");

         MainUi.Combat.Hide();
         MainUi.DiceBots.EndEncounter();
      }

      protected override void Disable() { }

      protected override void Update() {
         lerp = Mathf.Clamp01(lerp - Time.deltaTime * GameInfo.CombatGrid.TransitionLerpSpeed);

         GameInfo.CombatGrid.Lerp(lerp);
         GameInfo.WorldCube.Lerp(1 - lerp);
         if (lerp > .1f) {
            GameInfo.CombatGrid.SnapAllBotsPosition();
         }

         if (Mathf.Approximately(lerp, 0)) {
            encounter.Activity.Solved = true;
            GameInfo.CombatGrid.EndBattle();
            encounter.SnapAllBotsToWorldSlots();
            ChangeState(new LevelUpState(PartyUpgrade.GenerateForLevelUp(GameInfo.PlayerParty)));
         }
      }
   }
}