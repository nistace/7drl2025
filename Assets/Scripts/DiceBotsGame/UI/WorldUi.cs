using UnityEngine;

namespace DiceBotsGame.UI {
   public class WorldUi : MonoBehaviour {
      private static WorldUi instance { get; set; }

      [SerializeField] protected PromptUi prompt;

      public static PromptUi Prompt => instance.prompt;

      private void Awake() {
         instance = this;
      }
   }
}