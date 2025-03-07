using System.Linq;
using DiceBotsGame.UI;
using UnityEngine;

namespace DiceBotsGame.GameSates.CombatStates.CombatSubStates {
   public class SuddenDeathCombatSubState : ICombatSubState {
      private int effect = 1;

      private float StateTime { get; set; }
      public bool IsOver => StateTime > 3;

      public void StartState() {
         MainUi.Log.SetTexts(ICombatSubState.BattleTitle, $"Sudden death! All bots take {effect} damage!");
         StateTime = 0;
         foreach (var bot in GameInfo.PlayerParty.DiceBotsInParty.Union(GameInfo.CombatGrid.ListOfOpponentBots).Where(t => t.HealthSystem.IsAlive)) {
            bot.HealthSystem.Damage(effect);
         }
      }

      public void Update() => StateTime += Time.deltaTime;

      public void EndState() { }

      public void IncreaseEffect() => effect++;
   }
}