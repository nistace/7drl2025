namespace DiceBotsGame.GameSates.CombatStates.CombatSubStates {
   public interface ICombatSubState {
      public const string BattleTitle = "Battle continues";
      public void StartState();
      public void Update();
      public bool IsOver { get; }
      public void EndState();
   }
}