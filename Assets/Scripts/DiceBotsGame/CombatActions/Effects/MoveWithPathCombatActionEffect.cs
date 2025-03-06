using System;
using System.Collections;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.CombatActions.Effects {
   [RequireComponent(typeof(CombatAction))]
   public class MoveWithPathCombatActionEffect : MonoBehaviour, ICombatActionEffect {
      private enum EOutputType {
         Unchanged = 0,
         TilesTravelled = 1,
         TilesTravelledPlusOne = 2
      }

      [SerializeField] private EOutputType outputType = EOutputType.Unchanged;
      [SerializeField] private int stopNodesBeforeEndOfPath = 0;

      public IEnumerator Execute(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value, UnityAction<int> outputValueCallback) {
         if (combatGrid.TryGetPath(combatGrid.GetDiceBotPosition(actor), targetTile.Coordinates, out var path)) {
            for (var indexInPath = 1; indexInPath < path.Count - stopNodesBeforeEndOfPath; ++indexInPath) {
               var pathStep = combatGrid[path[indexInPath]].transform;
               while (actor.transform.position != pathStep.position) {
                  actor.MoveTowards(pathStep);
                  yield return null;
               }
               combatGrid.SetBotPosition(actor, combatGrid.GetTileAtPosition(path[indexInPath]));
            }

            outputValueCallback?.Invoke(outputType switch {
               EOutputType.Unchanged => value,
               EOutputType.TilesTravelled => path.Count - 1 - stopNodesBeforeEndOfPath,
               EOutputType.TilesTravelledPlusOne => path.Count - stopNodesBeforeEndOfPath,
               _ => throw new ArgumentOutOfRangeException()
            });
         }
      }
   }
}