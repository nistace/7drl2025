using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.CombatActions.Conditions {
   public class SelfCombatActionCondition : MonoBehaviour, ICombatActionCondition {
      public bool Check(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value) {
         var botAtPosition = combatGrid.GetDiceBotAtPosition(targetTile);
         if (!botAtPosition) return false;
         return botAtPosition == actor;
      }
   }
}