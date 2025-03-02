using System.Collections;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.CombatActions.Effects {
   [RequireComponent(typeof(CombatAction))]
   public class DamageCombatActionEffect : MonoBehaviour, ICombatActionEffect {
      public IEnumerator Execute(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value) {
         var bot = combatGrid.GetDiceBotAtPosition(targetTile);
         bot.HealthSystem.Damage(value);
         yield return null;
      }
   }
}