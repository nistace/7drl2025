﻿using System.Collections.Generic;
using DiceBotsGame.DiceBots;

namespace DiceBotsGame.GameSates.CombatStates.CombatSubStates {
   public class RollCombatSubState : ICombatSubState {
      private readonly DiceBot[] allBots;

      private List<DiceBot> rollingBots { get; } = new List<DiceBot>();
      public bool IsOver => rollingBots.Count == 0;

      public RollCombatSubState(DiceBot[] allBots) {
         this.allBots = allBots;
      }

      public void StartState() {
         rollingBots.Clear();
         foreach (var bot in allBots) {
            if (bot.HealthSystem.IsAlive) {
               bot.Roll();
               rollingBots.Add(bot);
            }
         }
      }

      public void Update() {
         for (var i = rollingBots.Count - 1; i >= 0; i--) {
            var bot = rollingBots[i];
            if (bot.IsRolling()) continue;
            rollingBots.RemoveAt(i);
         }
      }

      public void EndState() { }
   }
}