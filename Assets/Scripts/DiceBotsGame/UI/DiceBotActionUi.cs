using DiceBotsGame.CombatActions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DiceBotsGame.UI {
   [RequireComponent(typeof(Button))]
   public class DiceBotActionUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
      [SerializeField] protected Button button;
      [SerializeField] protected Image icon;
      [SerializeField] protected Image value;
      [SerializeField] protected DiceBotActionConfig config;

      private CombatActionDefinition combatAction;

      public UnityEvent<DiceBotActionUi> OnClicked { get; } = new UnityEvent<DiceBotActionUi>();
      public UnityEvent<DiceBotActionUi> OnPointerEntered { get; } = new UnityEvent<DiceBotActionUi>();
      public UnityEvent<DiceBotActionUi> OnPointerExited { get; } = new UnityEvent<DiceBotActionUi>();

      private void Start() {
         button.onClick.AddListener(HandleClick);
      }

      private void OnDestroy() {
         button.onClick.RemoveListener(HandleClick);
      }

      public void SetAction(CombatActionDefinition combatAction) {
         this.combatAction = combatAction;
         icon.sprite = combatAction.IsValidAction ? combatAction.Action.Sprite : config.NoActionSprite;
         value.sprite = config.Digit(combatAction.ConstantStrength);
         value.enabled = combatAction.IsValidAction && value.sprite;
         button.interactable = combatAction.IsValidAction;
      }

      public void MarkAsRolling() {
         icon.sprite = config.RollingActionSprite;
         value.enabled = false;
         button.interactable = false;
      }

      public void SetInteractable(bool interactable) => button.interactable = interactable;

      private void HandleClick() => OnClicked.Invoke(this);

      public void OnPointerEnter(PointerEventData eventData) {
         OnPointerEntered.Invoke(this);
         SharedUiEvents.OnActionHoverStarted.Invoke(combatAction);
      }

      public void OnPointerExit(PointerEventData eventData) {
         OnPointerExited.Invoke(this);
         SharedUiEvents.OnActionHoverStopped.Invoke(combatAction);
      }
   }
}