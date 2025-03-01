using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.UI {
   [RequireComponent(typeof(Animator))]
   public class PromptUi : MonoBehaviour {
      private static readonly int activeAnimParam = Animator.StringToHash("Active");
      [SerializeField] protected Animator animator;
      [SerializeField] protected TMP_Text promptText;
      [SerializeField] protected Transform buttonsContainer;
      [SerializeField] protected TextualButton buttonPrefab;

      private readonly List<TextualButton> buttons = new List<TextualButton>();

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

         animator.SetBool(activeAnimParam, true);
      }

      public void Hide() => animator.SetBool(activeAnimParam, false);
   }
}