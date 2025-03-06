using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.CombatActions.Conditions {
   public class TargetIsAliveCombatActionCondition : MonoBehaviour, ICombatActionCondition {
      public bool Check(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value) {
         var botAtPosition = combatGrid.GetDiceBotAtPosition(targetTile);
         return botAtPosition && botAtPosition.HealthSystem.IsAlive;
      }
   }
}