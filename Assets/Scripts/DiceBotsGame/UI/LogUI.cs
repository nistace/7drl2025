using TMPro;
using UnityEngine;

namespace DiceBotsGame.UI {
   public class LogUI : MonoBehaviour, IMainScreenUi {
      [SerializeField] private InOutUiAnimator inOutUiAnimator;
      [SerializeField] protected TMP_Text mainText;
      [SerializeField] protected TMP_Text secondaryText;

      public void Show() => inOutUiAnimator.Enter();
      public void Hide() => inOutUiAnimator.Exit();

      public void SetTexts(string main, string secondary) {
         mainText.text = main;
         secondaryText.text = secondary;
      }
   }
}