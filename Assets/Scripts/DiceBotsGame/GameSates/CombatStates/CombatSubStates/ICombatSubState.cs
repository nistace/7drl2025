namespace DiceBotsGame.GameSates.CombatStates {
   public interface ICombatSubState {
      public abstract void StartState();
      public abstract void Update();
      public abstract bool IsOver { get; }
      public abstract void EndState();
   }
}