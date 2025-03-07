using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.DiceBots;
using DiceBotsGame.UI;
using UnityEngine;

namespace DiceBotsGame.GameSates.CombatStates.CombatSubStates {
   public class RollCombatSubState : ICombatSubState {
      private List<DiceBot> rollingBots { get; } = new List<DiceBot>();
      public bool IsOver => rollingBots.Count == 0;
      private float remainingMinTime = 1;

      public void StartState() {
         MainUi.Log.SetTexts("Battle continues", "Dice are rolling");

         remainingMinTime = 1;
         rollingBots.Clear();
         foreach (var bot in GameInfo.PlayerParty.DiceBotsInParty.Union(GameInfo.CombatGrid.ListOfOpponentBots)) {
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
            rollingBots.RemoveAt(i);
         }
      }

      public void EndState() { }
   }
}