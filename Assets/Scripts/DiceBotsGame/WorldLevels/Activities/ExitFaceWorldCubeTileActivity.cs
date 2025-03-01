using UnityEngine;

namespace DiceBotsGame.WorldLevels.Activities {
   public class ExitFaceWorldCubeTileActivity : MonoBehaviour, IWorldCubeTileActivity {
      public bool CanRevert => true;
      public string PromptText => "Move on to the next face of the cube? This face will become inaccessible.";
      public string ContinueLabel => "Continue";
      public string CancelLabel => "Cancel";
      public bool Solved { get; set; }
      public bool IsTraversable => false;
   }
}