using System.Collections.Generic;
using DiceBotsGame.CombatActions;
using DiceBotsGame.DiceBots;
using DiceBotsGame.DiceBots.Dices.Faces;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DiceBotsGame.UI {
   public class DiceBotBoxUi : MonoBehaviour {
      [SerializeField] protected InOutUiAnimator boxAnimator;
      [SerializeField] protected TMP_Text botName;
      [SerializeField] protected Graphic[] itemsColoredWithBotColor;
      [SerializeField] protected Transform healthPointsParent;
      [SerializeField] protected Transform actionsParent;
      [SerializeField] protected InOutUiAnimator actionsAnimator;
      [SerializeField] protected DiceBotActionUi diceBotActionPrefab;
      [SerializeField] protected DiceBotActionUi diceRollAction;

      public DiceBot Bot { get; private set; }
      private readonly List<HealthPointUi> healthPoints = new List<HealthPointUi>();
      private readonly List<DiceBotActionUi> coreActions = new List<DiceBotActionUi>();

      public UnityEvent<DiceBot, CombatActionDefinition> OnBotActionHoverStarted { get; } = new UnityEvent<DiceBot, CombatActionDefinition>();
      public UnityEvent<DiceBot, CombatActionDefinition> OnBotActionHoverStopped { get; } = new UnityEvent<DiceBot, CombatActionDefinition>();
      public UnityEvent<DiceBot, CombatActionDefinition> OnBotActionClicked { get; } = new UnityEvent<DiceBot, CombatActionDefinition>();

      private void Start() {
         diceRollAction.OnClicked.AddListener(HandleActionClicked);
         diceRollAction.OnPointerEntered.AddListener(HandleActionPointerEntered);
         diceRollAction.OnPointerExited.AddListener(HandleActionPointerExited);
      }

      public void SetUp(DiceBot bot) {
         CleanUp();
         this.Bot = bot;

         botName.text = bot.DisplayName;

         foreach (var itemColoredWithBotColor in itemsColoredWithBotColor) {
            itemColoredWithBotColor.color = bot.Color;
         }

         RefreshHealthBar();
         RebuildActions();
         actionsAnimator.SnapOut();

         this.Bot.HealthSystem.OnChanged.AddListener(HandleHealthChanged);
         this.Bot.HealthSystem.OnMaxHealthChanged.AddListener(HandleMaxHealthChanged);
         this.Bot.Dice.OnStartedRolling.AddListener(HandleDiceStartedRolling);
         this.Bot.Dice.OnSavedRolledFace.AddListener(HandleDiceSavedRolledFace);
      }

      public void SetVisible(bool visible) => boxAnimator.ChangeTargetPosition(visible);
      public void SetActionsVisible(bool visible) => actionsAnimator.ChangeTargetPosition(visible);

      private void HandleDiceSavedRolledFace(CharacterDiceFace savedFace) => diceRollAction.SetAction(savedFace.Data.CombatAction);
      private void HandleDiceStartedRolling() => diceRollAction.MarkAsRolling();

      public void CleanUp() {
         if (!Bot) return;
         Bot.HealthSystem.OnChanged.RemoveListener(HandleHealthChanged);
         Bot.HealthSystem.OnMaxHealthChanged.RemoveListener(HandleMaxHealthChanged);
         Bot.Dice.OnStartedRolling.RemoveListener(HandleDiceStartedRolling);
         Bot.Dice.OnSavedRolledFace.RemoveListener(HandleDiceSavedRolledFace);
         Bot = null;
      }

      private void HandleMaxHealthChanged(int _) => RefreshHealthBar();
      private void HandleHealthChanged(int _) => RefreshHealthBar();

      private void RebuildActions() {
         while (coreActions.Count < Bot.Dice.CoreActions.Count) {
            var action = Instantiate(diceBotActionPrefab, actionsParent);
            action.OnClicked.AddListener(HandleActionClicked);
            action.OnPointerEntered.AddListener(HandleActionPointerEntered);
            action.OnPointerExited.AddListener(HandleActionPointerExited);
            coreActions.Add(action);
         }

         for (var i = 0; i < Bot.Dice.CoreActions.Count; ++i) {
            coreActions[i].SetAction(Bot.Dice.CoreActions[i]);
            coreActions[i].gameObject.SetActive(true);
         }

         for (var i = Bot.Dice.CoreActions.Count; i < coreActions.Count; ++i) {
            coreActions[i].gameObject.SetActive(false);
         }

         diceRollAction.transform.SetSiblingIndex(diceRollAction.transform.parent.childCount - 1);
      }

      private void HandleActionClicked(DiceBotActionUi actionUi) => InvokeEventForAction(OnBotActionClicked, actionUi);
      private void HandleActionPointerEntered(DiceBotActionUi actionUi) => InvokeEventForAction(OnBotActionHoverStarted, actionUi);
      private void HandleActionPointerExited(DiceBotActionUi actionUi) => InvokeEventForAction(OnBotActionHoverStopped, actionUi);

      private void InvokeEventForAction(UnityEvent<DiceBot, CombatActionDefinition> eventToInvoke, DiceBotActionUi actionUi) {
         if (TryGetAction(actionUi, out var action)) {
            eventToInvoke.Invoke(Bot, action);
         }
      }

      private bool TryGetAction(DiceBotActionUi actionUi, out CombatActionDefinition action) {
         if (diceRollAction == actionUi) {
            action = Bot.Dice.LastRolledFace.Data.CombatAction;
            return true;
         }
         var actionIndex = coreActions.IndexOf(actionUi);
         if (actionIndex > -1) {
            action = Bot.Dice.CoreActions[actionIndex];
            return true;
         }
         action = default;
         return false;
      }

      private void RefreshHealthBar() {
         while (healthPoints.Count > Bot.HealthSystem.MaxHealth) {
            HealthBarManager.Release(healthPoints[0]);
            healthPoints.RemoveAt(0);
         }
         while (healthPoints.Count < Bot.HealthSystem.MaxHealth) {
            healthPoints.Add(HealthBarManager.Get(healthPointsParent));
         }

         for (var i = 0; i < healthPoints.Count; ++i) {
            HealthBarManager.SetFull(healthPoints[i], i < Bot.HealthSystem.CurrentHealth);
         }
      }
   }
}
