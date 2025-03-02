using DiceBotsGame.Cameras;
using DiceBotsGame.UI;
using DiceBotsGame.Utils;
using DiceBotsGame.WorldLevels;
using UnityEngine;

namespace DiceBotsGame.GameSates.CombatStates {
   public class StartCombatState : GameState {
      private static string[] IntroductionSentences { get; } = {
         "This {0} just rolled up for a fight!",
         "Uh-oh! A wild {0} appears… and it looks mad!",
         "This {0} thinks you're just another piece of scrap.",
         "The {0} has activated battle mode!",
         "The {0} is here, and it’s not playing nice!",
         "This {0} is looking for trouble—and you’re on the menu!",
      };

      private readonly WorldCubeTileEncounter encounter;
      private float lerp;

      public StartCombatState(WorldCubeTileEncounter encounter) {
         this.encounter = encounter;
      }

      protected override void Enable() {
         MainCameraController.ActivateCamera(GameInfo.CombatGrid.CinemachineCamera);
         MainUi.Log.SetTexts("Battle is starting...", IntroductionSentences.Roll().Replace("{0}", encounter.DisplayName));

         MainUi.DiceBots.SetupEncounter(encounter.DiceBots);

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