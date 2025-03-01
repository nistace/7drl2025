using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DiceBotsGame.UI {
   [RequireComponent(typeof(Button))]
   public class TextualButton : MonoBehaviour {
      [SerializeField] protected Button button;
      [SerializeField] protected TMP_Text text;

      private UnityAction callback;

      private void Start() => button.onClick.AddListener(HandleClick);
      private void OnDestroy() => button.onClick.RemoveListener(HandleClick);

      private void HandleClick() => callback?.Invoke();

      public void Setup(string label, UnityAction callback) {
         text.text = label;
         this.callback = callback;
      }
   }
}