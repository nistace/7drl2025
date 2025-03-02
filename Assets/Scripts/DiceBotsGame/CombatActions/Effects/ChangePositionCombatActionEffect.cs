using System;
using System.Collections;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.CombatActions {
   [RequireComponent(typeof(CombatAction))]
   public class ChangePositionCombatActionEffect : MonoBehaviour, ICombatActionEffect {
      private enum Type {
         Walk = 0,
         Fly = 1,
         Teleport = 2
      }

      [SerializeField] private Type type = Type.Walk;

      public IEnumerator Execute(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value) => type switch {
         Type.Walk => Walk(combatGrid, actor, targetTile),
         Type.Fly => Fly(combatGrid, actor, targetTile),
         Type.Teleport => Teleport(combatGrid, actor, targetTile),
         _ => throw new ArgumentOutOfRangeException()
      };

      private static IEnumerator Teleport(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile) {
         combatGrid.SetBotPosition(actor, targetTile);
         actor.SnapTo(targetTile.transform);
         yield return null;
      }

      private static IEnumerator Fly(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile) {
         combatGrid.SetBotPosition(actor, targetTile);
         while (actor.transform.position != targetTile.transform.position) {
            actor.MoveTowards(targetTile.transform);
            yield return null;
         }
      }

      private static IEnumerator Walk(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile) {
         if (combatGrid.TryGetPath(combatGrid.GetDiceBotPosition(actor), targetTile.Coordinates, out var path)) {
            for (var indexInPath = 1; indexInPath < path.Count; ++indexInPath) {
               var pathStep = combatGrid[path[indexInPath]].transform;
               while (actor.transform.position != pathStep.position) {
                  actor.MoveTowards(pathStep);
                  yield return null;
               }
            }
            combatGrid.SetBotPosition(actor, targetTile);
         }
      }
   }
}