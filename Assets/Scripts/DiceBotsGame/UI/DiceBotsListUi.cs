using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.CombatActions;
using DiceBotsGame.DiceBots;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.UI {
   public class DiceBotsListUi : MonoBehaviour {
      [SerializeField] protected CanvasGroup canvasGroup;
      [SerializeField] protected Transform container;
      [SerializeField] protected DiceBotBoxUi boxPrefab;

      private readonly List<DiceBotBoxUi> boxes = new List<DiceBotBoxUi>();
      private int activeBoxes;

      public UnityEvent<DiceBot, CombatActionDefinition> OnBotActionHoverStarted { get; } = new UnityEvent<DiceBot, CombatActionDefinition>();
      public UnityEvent<DiceBot, CombatActionDefinition> OnBotActionHoverStopped { get; } = new UnityEvent<DiceBot, CombatActionDefinition>();
      public UnityEvent<DiceBot, CombatActionDefinition> OnBotActionClicked { get; } = new UnityEvent<DiceBot, CombatActionDefinition>();

      public void AddBot(DiceBot bot) {
         if (boxes.Any(t => t.Bot == bot)) return;

         if (boxes.Count <= activeBoxes) {
            boxes.Add(Instantiate(boxPrefab, container));
         }
         var box = boxes[activeBoxes];
         box.SetUp(bot);
         box.SetVisible(true);
         box.OnBotActionHoverStarted.AddListener(OnBotActionHoverStarted.Invoke);
         box.OnBotActionHoverStopped.AddListener(OnBotActionHoverStopped.Invoke);
         box.OnBotActionClicked.AddListener(OnBotActionClicked.Invoke);

         activeBoxes++;
      }

      public void RemoveBot(DiceBot bot) {
         var box = boxes.FirstOrDefault(t => t.Bot == bot);
         if (!box) return;

         box.CleanUp();
         boxes.Remove(box);
         boxes.Add(box);
         box.SetVisible(false);
         box.OnBotActionHoverStarted.RemoveListener(OnBotActionHoverStarted.Invoke);
         box.OnBotActionHoverStopped.RemoveListener(OnBotActionHoverStopped.Invoke);
         box.OnBotActionClicked.RemoveListener(OnBotActionClicked.Invoke);

         activeBoxes--;
      }

      public void CleanUpAllBots() {
         for (var i = 0; i < activeBoxes; i++) {
            boxes[i].CleanUp();
            boxes[i].SetVisible(false);
         }
         activeBoxes = 0;
      }

      public void SetVisible(bool visible) {
         for (var i = 0; i < activeBoxes; i++) {
            boxes[i].SetVisible(visible);
         }
      }

      public void SetActionsVisible(bool visible) {
         for (var i = 0; i < activeBoxes; i++) {
            boxes[i].SetActionsVisible(visible);
         }
      }

      public void SetInteractable(bool interactable) => canvasGroup.interactable = interactable;
   }
}