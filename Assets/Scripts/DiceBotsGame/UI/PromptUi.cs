using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.UI {
   [RequireComponent(typeof(InOutUiAnimator))]
   public class PromptUi : MonoBehaviour {
      [SerializeField] protected InOutUiAnimator animator;
      [SerializeField] protected TMP_Text promptText;
      [SerializeField] protected Transform buttonsContainer;
      [SerializeField] protected TextualButton buttonPrefab;

      private readonly List<TextualButton> buttons = new List<TextualButton>();

      private void Start() {
         animator.SnapOut();
      }

      public void ShowPrompt(string prompt, params (string label, UnityAction callback)[] buttonInfos) {
         promptText.text = prompt;

         for (var i = 0; i < buttonInfos.Length; ++i) {
            if (buttons.Count <= i) {
               buttons.Add(Instantiate(buttonPrefab, buttonsContainer));
            }
            buttons[i].Setup(buttonInfos[i].label, buttonInfos[i].callback);
            buttons[i].gameObject.SetActive(true);
         }

         for (var i = buttonInfos.Length; i < buttons.Count; ++i) {
            buttons[i].gameObject.SetActive(false);
         }

         animator.Enter();
      }

      public void Hide() => animator.Exit();
   }
}