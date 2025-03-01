using UnityEngine;

namespace DiceBotsGame.WorldLevels.Activities {
   [CreateAssetMenu]
   public class OptionalActivityInfo : ScriptableObject {
      [SerializeField] protected string promptText = "Do you want to continue?";
      [SerializeField] protected string continueLabel = "Continue";
      [SerializeField] protected string cancelLabel = "Cancel";

      public string PromptText => promptText;
      public string ContinueLabel => continueLabel;
      public string CancelLabel => cancelLabel;
   }
}