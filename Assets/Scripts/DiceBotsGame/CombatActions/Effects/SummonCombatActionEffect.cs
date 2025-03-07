using System.Collections;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.CombatActions.Effects {
   [RequireComponent(typeof(CombatAction))]
   public class SummonCombatActionEffect : MonoBehaviour, ICombatActionEffect {
      [SerializeField] protected DiceBotPattern botPattern;

      public IEnumerator Execute(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value, UnityAction<int> outputValueCallback) {
         actor.transform.LookAt(targetTile.transform);

         yield return new WaitForSeconds(.5f);

         var newBot = DiceBotFactory.Instantiate(botPattern);
         combatGrid.AddOpponentToBattle(newBot, targetTile);
         newBot.SnapTo(targetTile.transform);
         yield return null;
         newBot.Roll();
         yield return null;
         newBot.Reassemble();
      }
   }
}