using UnityEngine;

namespace DiceBotsGame.UI {
   public class WorldUi : MonoBehaviour, IMainScreenUi {
      [SerializeField] protected PromptUi prompt;

      public PromptUi Prompt => prompt;

      public void Hide() => prompt.Hide();
   }
}