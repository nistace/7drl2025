using System.Collections;
using DiceBotsGame.Cameras;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using DiceBotsGame.GameSates;
using DiceBotsGame.GameSates.WorldStates;
using DiceBotsGame.UI;
using DiceBotsGame.WorldLevels;
using UnityEngine;

namespace DiceBotsGame {
   public class GameController : MonoBehaviour {
      [SerializeField] private DiceBotPattern playerDicePattern;
      [SerializeField] private WorldCubePattern worldCubePattern;
      [SerializeField] private CombatGridPattern combatGridPattern;

      private void Start() {
         StartCoroutine(StartNewGame());
      }

      private IEnumerator StartNewGame() {
         var playerMainDiceBot = DiceBotFactory.Instantiate(playerDicePattern);
         playerMainDiceBot.transform.position = Vector3.zero;

         var worldCube = WorldCubeFactory.InstantiateWorldCube(worldCubePattern);
         var combatGrid = CombatGridFactory.InstantiateCombatGrid(combatGridPattern);
         var playerParty = DiceBotFactory.InstantiateParty(worldCube.SpawnTile, true, playerMainDiceBot);

         MainCameraController.worldTarget = playerMainDiceBot.transform;
         MainCameraController.worldCenterPosition = new Vector3(0, worldCube.InnerSphereRadius);

         worldCube.Lerp(1);
         combatGrid.Lerp(0);

         GameInfo.SetupGameData(worldCube, playerParty, combatGrid);

         yield return null;

         MainUi.DiceBots.SetUp(playerParty);
         MainUi.Log.Show();

         GameState.ChangeState(WorldState.Instance);
      }

      private void OnDestroy() {
         GameState.ChangeState(default);
      }

      private void Update() => GameState.UpdateCurrentState();
   }
}