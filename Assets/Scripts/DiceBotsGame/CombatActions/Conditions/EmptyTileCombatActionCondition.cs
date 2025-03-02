using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.CombatActions.Conditions {
   public class EmptyTileCombatActionCondition : MonoBehaviour, ICombatActionCondition {
      public bool Check(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value) => !combatGrid.GetDiceBotAtPosition(targetTile);
   }
}