using System;
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
      [SerializeField] protected Sprite noActionIcon;
      [SerializeField] protected Sprite rollingIcon;

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
         icon.sprite = combatAction.IsValidAction ? combatAction.Action.Sprite : noActionIcon;
         button.interactable = combatAction.IsValidAction;
      }

      public void MarkAsRolling() {
         icon.sprite = rollingIcon;
         button.interactable = false;
      }

      private void HandleClick() => OnClicked.Invoke(this);
      public void OnPointerEnter(PointerEventData eventData) => OnPointerEntered.Invoke(this);
      public void OnPointerExit(PointerEventData eventData) => OnPointerExited.Invoke(this);
   }
}