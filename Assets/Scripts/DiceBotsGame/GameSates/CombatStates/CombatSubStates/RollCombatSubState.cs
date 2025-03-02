using System.Collections.Generic;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.GameSates.CombatStates.CombatSubStates {
   public class RollCombatSubState : ICombatSubState {
      private readonly DiceBot[] allBots;

      private List<DiceBot> rollingBots { get; } = new List<DiceBot>();
      public bool IsOver => rollingBots.Count == 0;
      private float remainingMinTime = 1;

      public RollCombatSubState(DiceBot[] allBots) {
         this.allBots = allBots;
      }

      public void StartState() {
         remainingMinTime = 1;
         rollingBots.Clear();
         foreach (var bot in allBots) {
            if (bot.HealthSystem.IsAlive) {
               bot.Roll();
               rollingBots.Add(bot);
            }
         }
      }

      public void Update() {
         remainingMinTime -= Time.deltaTime;
         if (remainingMinTime > 0) return;

         for (var i = rollingBots.Count - 1; i >= 0; i--) {
            var bot = rollingBots[i];
            if (bot.IsRolling()) {
               continue;
            }
            if (bot.IsStuckWhileRolling()) {
               bot.Roll(.2f);
               continue;
            }
            bot.SaveRolledFace();
            Debug.Log(bot.Dice.LastRolledFace.Data.CombatAction);
            rollingBots.RemoveAt(i);
         }
      }

      public void EndState() { }
   }
}