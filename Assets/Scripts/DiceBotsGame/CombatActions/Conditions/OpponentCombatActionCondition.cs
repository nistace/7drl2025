using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.CombatActions.Conditions {
   public class OpponentCombatActionCondition : MonoBehaviour, ICombatActionCondition {
      public bool Check(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value) {
         var botAtPosition = combatGrid.GetDiceBotAtPosition(targetTile);
         if (!botAtPosition) return false;
         if (botAtPosition == actor) return false;
         return !combatGrid.AreInSameTeam(actor, botAtPosition);
      }
   }
}