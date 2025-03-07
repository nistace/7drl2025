using System.Linq;
using DiceBotsGame.DiceBots;
using DiceBotsGame.UI;
using UnityEngine;

namespace DiceBotsGame.GameSates.CombatStates.CombatSubStates {
   public class SuddenDeathCombatSubState : ICombatSubState {
      private readonly DiceBot[] allBots;
      private int effect;

      public SuddenDeathCombatSubState(DiceBot[] allBots) {
         this.allBots = allBots;
         effect = 1;
      }

      private float StateTime { get; set; }
      public bool IsOver => StateTime > 3;

      public void StartState() {
         MainUi.Log.SetTexts(ICombatSubState.BattleTitle, $"Sudden death! All bots take {effect} damage!");
         StateTime = 0;
         foreach (var bot in allBots.Where(t => t.HealthSystem.IsAlive)) {
            bot.HealthSystem.Damage(effect);
         }
      }

      public void Update() => StateTime += Time.deltaTime;

      public void EndState() { }

      public void IncreaseEffect() => effect++;
   }
}