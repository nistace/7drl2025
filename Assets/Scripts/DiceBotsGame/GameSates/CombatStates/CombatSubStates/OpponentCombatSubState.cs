using UnityEngine;

namespace DiceBotsGame.GameSates.CombatStates.CombatSubStates {
   public class OpponentCombatSubState : ICombatSubState {
      private float time = 0;
      public bool IsOver => time >= 1;

      public void StartState() => time = 0;
      public void Update() => time += Time.deltaTime;
      public void EndState() => time = 0;
   }
}