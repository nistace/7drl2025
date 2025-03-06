using System;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.CombatActions.Conditions {
   public class TargetHealthCombatActionCondition : MonoBehaviour, ICombatActionCondition {
      private enum ECheck {
         Alive = 0,
         AliveDamaged = 1,
         DamagedOrDead = 2,
         Dead = 3
      }

      [SerializeField] private ECheck check = ECheck.Alive;

      public bool Check(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value) {
         var botAtPosition = combatGrid.GetDiceBotAtPosition(targetTile);
         if (!botAtPosition) return true;
         return check switch {
            ECheck.Alive => botAtPosition.HealthSystem.IsAlive,
            ECheck.AliveDamaged => botAtPosition.HealthSystem.IsAlive && botAtPosition.HealthSystem.MissingHealth > 0,
            ECheck.DamagedOrDead => botAtPosition.HealthSystem.MissingHealth > 0,
            ECheck.Dead => botAtPosition.HealthSystem.IsDead,
            _ => throw new ArgumentOutOfRangeException()
         };
      }
   }
}