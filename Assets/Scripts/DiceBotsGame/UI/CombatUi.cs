using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DiceBotsGame.UI {
   [RequireComponent(typeof(InOutUiAnimator))]
   public class CombatUi : MonoBehaviour, IMainScreenUi {
      [SerializeField] protected InOutUiAnimator uiAnimator;
      [SerializeField] protected Button endTurnButton;
      [SerializeField] protected InOutUiAnimator endTurnButtonAnimator;

      public UnityEvent OnEndTurnClicked => endTurnButton.onClick;

      private void Reset() {
         uiAnimator = GetComponent<InOutUiAnimator>();
      }

      private void Start() {
         uiAnimator.SnapOut();
         endTurnButtonAnimator.SnapOut();
      }

      public void Show() => uiAnimator.Enter();
      public void Hide() => uiAnimator.Exit();
      public void SetEndTurnButtonVisible(bool visible) => endTurnButtonAnimator.ChangeTargetPosition(visible);
   }
}