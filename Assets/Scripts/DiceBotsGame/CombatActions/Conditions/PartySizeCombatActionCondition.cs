using System;
using DiceBotsGame.CombatGrids;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.CombatActions.Conditions {
   public class PartyCombatActionCondition : MonoBehaviour, ICombatActionCondition {
      protected enum EOperation {
         AtLeast = 0,
         More = 1,
         AtMost = 2,
         Less = 3
      }

      [SerializeField] protected EOperation operation = EOperation.AtLeast;
      [SerializeField] protected ETarget target = ETarget.Self;
      [SerializeField] protected int amount;

      public bool Check(CombatGrid combatGrid, DiceBot actor, CombatGridTile targetTile, int value) {
         var bot = target switch {
            ETarget.Self => actor,
            ETarget.BotOnTile => combatGrid.GetDiceBotAtPosition(targetTile),
            _ => throw new ArgumentOutOfRangeException()
         };
         if (!bot) return false;

         var team = combatGrid.GetBotTeam(bot);

         return operation switch {
            EOperation.AtLeast => team.Count >= amount,
            EOperation.More => team.Count > amount,
            EOperation.AtMost => team.Count <= amount,
            EOperation.Less => team.Count < amount,
            _ => throw new ArgumentOutOfRangeException()
         };
      }
   }
}