namespace DiceBotsGame.WorldLevels.Activities {
   public interface IWorldCubeTileActivity {
      bool CanRevert { get; }
      string PromptText { get; }
      string ContinueLabel { get; }
      string CancelLabel { get; }
      bool Solved { get; set; }
      bool IsTraversable { get; }
   }
}