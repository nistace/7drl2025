using TMPro;
using UnityEngine;

namespace DiceBotsGame.UI {
   public class TooltipUi : MonoBehaviour {
      [SerializeField] protected TMP_Text tooltipLabel;
      [SerializeField] protected InOutUiAnimator uiAnimator;

      private object tooltipSource { get; set; }

      private void Reset() {
         uiAnimator = GetComponent<InOutUiAnimator>();
      }

      private void Start() {
         uiAnimator.SnapOut();
         Hide();
      }

      public void Show(string text, object source = null) {
         tooltipLabel.text = text;
         uiAnimator.Enter();
         tooltipSource = source;
      }

      public void Hide(object fromSource = null) {
         if (fromSource == null || tooltipSource == null || tooltipSource == fromSource) {
            uiAnimator.Exit();
         }
      }
   }
}