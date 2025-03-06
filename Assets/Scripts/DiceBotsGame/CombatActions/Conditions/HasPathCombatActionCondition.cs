using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.CombatActions.Conditions {
   public class HasPathCombatActionCondition : MonoBehaviour, ICombatActionCondition {
      public bool Check(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value) {
         if (CombatGrid.Distance(combatGrid.GetDiceBotPosition(actor), targetTile.Coordinates) > value) return false;
         return combatGrid.TryGetPath(combatGrid.GetDiceBotPosition(actor), targetTile.Coordinates, out var path) && path.Count - 1 <= value;
      }
   }
}