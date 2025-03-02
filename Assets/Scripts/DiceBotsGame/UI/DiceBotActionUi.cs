using DiceBotsGame.CombatActions;
using UnityEngine;
using UnityEngine.UI;

namespace DiceBotsGame.UI {
   [RequireComponent(typeof(Button))]
   public class DiceBotActionUi : MonoBehaviour {
      [SerializeField] protected Button button;
      [SerializeField] protected Image icon;
      [SerializeField] protected Sprite noActionIcon;
      [SerializeField] protected Sprite rollingIcon;

      public void SetAction(CombatActionDefinition combatAction) {
         icon.sprite = combatAction.IsValidAction ? combatAction.Action.Sprite : noActionIcon;
         button.interactable = combatAction.IsValidAction;
      }

      public void MarkAsRolling() {
         icon.sprite = rollingIcon;
         button.interactable = false;
      }
   }
}