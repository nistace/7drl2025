using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.CombatActions.Conditions {
   public class AtRangeCombatActionCondition : MonoBehaviour, ICombatActionCondition {
      [SerializeField] private int range = 1;

      public bool Check(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value) {
         var actorTile = combatGrid.GetDiceBotPosition(actor);
         var distance = Mathf.Abs(actorTile.x - targetTile.X) + Mathf.Abs(actorTile.y - targetTile.Y);
         return distance <= range;
      }
   }
}