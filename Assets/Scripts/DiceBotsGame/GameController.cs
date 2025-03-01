using DiceBotsGame.DiceBots;
using DiceBotsGame.DiceBots.Dices;
using DiceBotsGame.GameSates;
using DiceBotsGame.WorldLevels;
using UnityEngine;

namespace DiceBotsGame {
   public class GameController : MonoBehaviour {
      [SerializeField] private CharacterDicePattern dicePattern;
      [SerializeField] private WorldCubePattern worldCubePattern;

      private void Start() {
         var diceBot = DiceBotFactory.Instantiate(dicePattern, Color.cyan);
         diceBot.transform.position = Vector3.zero;

         var worldCube = WorldCubeFactory.InstantiateWorldCube(worldCubePattern);
         var playerParty = DiceBotFactory.InstantiateParty(worldCube.SpawnTile, true, diceBot);

         GameInfo.SetupGameData(worldCube, playerParty);

         GameState.ChangeState(WorldState.Instance);
      }

      private void OnDestroy() {
         GameState.ChangeState(default);
      }

      private void Update() => GameState.UpdateCurrentState();
   }
}