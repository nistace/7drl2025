using System;
using System.Collections;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.CombatActions.Effects {
   [RequireComponent(typeof(CombatAction))]
   public class TeleportCombatActionEffect : MonoBehaviour, ICombatActionEffect {
      private enum EOutputType {
         Unchanged = 0,
         Distance = 1
      }

      [SerializeField] private EOutputType outputType = EOutputType.Unchanged;

      public IEnumerator Execute(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value, UnityAction<int> outputValueCallback) {
         var actorInitialPosition = combatGrid.GetDiceBotPosition(actor);
         combatGrid.SetBotPosition(actor, targetTile);
         actor.SnapTo(targetTile.transform);
         yield return null;

         outputValueCallback?.Invoke(outputType switch {
            EOutputType.Unchanged => value,
            EOutputType.Distance => CombatGrid.Distance(targetTile.Coordinates, actorInitialPosition),
            _ => throw new ArgumentOutOfRangeException()
         });
      }
   }
}