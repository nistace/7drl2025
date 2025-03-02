using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DiceBotsGame.UI {
   [RequireComponent(typeof(InOutUiAnimator))]
   public class GameOverUi : MonoBehaviour, IMainScreenUi {
      [SerializeField] protected InOutUiAnimator animator;
      [SerializeField] protected TMP_Text text;
      [SerializeField] protected Button continueButton;

      public UnityEvent OnContinueClicked => continueButton.onClick;

      private void Start() {
         animator.SnapOut();
      }

      public void Show(bool victory) {
         text.text = victory ? "Victory" : "Defeat";
         animator.Enter();
      }

      public void Hide() => animator.Exit();
   }
}