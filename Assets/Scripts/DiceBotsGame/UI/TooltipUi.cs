using DiceBotsGame.CombatActions;
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

         SharedUiEvents.OnActionHoverStarted.AddListener(ShowTooltip);
         SharedUiEvents.OnActionHoverStopped.AddListener(HideTooltip);
      }

      private void OnDestroy() {
         SharedUiEvents.OnActionHoverStarted.RemoveListener(ShowTooltip);
         SharedUiEvents.OnActionHoverStopped.RemoveListener(HideTooltip);
      }

      private void HideTooltip(CombatActionDefinition action) => Hide(action);

      private void ShowTooltip(CombatActionDefinition action) {
         if (action.IsValidAction) {
            Show($"{action.Action.ActionName} ({action.ConstantStrength})\n"
                 + $"  > {action.Action.GetDisplayConditions(action.ConstantStrength)}\n"
                 + $"  > {action.Action.GetDisplayEffects(action.ConstantStrength)}",
               action);
         }
         else {
            Hide();
         }
      }

      private void Show(string text, object source = null) {
         tooltipLabel.text = text;
         uiAnimator.Enter();
         tooltipSource = source;
      }

      private void Hide(object fromSource = null) {
         if (fromSource == null || tooltipSource == null || tooltipSource == fromSource) {
            uiAnimator.Exit();
         }
      }
   }
}