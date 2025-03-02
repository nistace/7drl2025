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

      private DiceBot bot;
      public DiceBot Bot => bot;
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
         this.bot = bot;

         botName.text = bot.DisplayName;

         foreach (var itemColoredWithBotColor in itemsColoredWithBotColor) {
            itemColoredWithBotColor.color = bot.Color;
         }

         RefreshHealthBar();
         RebuildActions();
         actionsAnimator.SnapOut();

         this.bot.HealthSystem.OnChanged.AddListener(HandleHealthChanged);
         this.bot.HealthSystem.OnMaxHealthChanged.AddListener(HandleMaxHealthChanged);
         this.bot.Dice.OnStartedRolling.AddListener(HandleDiceStartedRolling);
         this.bot.Dice.OnSavedRolledFace.AddListener(HandleDiceSavedRolledFace);
      }

      public void SetVisible(bool visible) => boxAnimator.ChangeTargetPosition(visible);
      public void SetActionsVisible(bool visible) => actionsAnimator.ChangeTargetPosition(visible);

      private void HandleDiceSavedRolledFace(CharacterDiceFace savedFace) => diceRollAction.SetAction(savedFace.Data.CombatAction);
      private void HandleDiceStartedRolling() => diceRollAction.MarkAsRolling();

      public void CleanUp() {
         if (!bot) return;
         bot.HealthSystem.OnChanged.RemoveListener(HandleHealthChanged);
         bot.HealthSystem.OnMaxHealthChanged.RemoveListener(HandleMaxHealthChanged);
         bot.Dice.OnStartedRolling.RemoveListener(HandleDiceStartedRolling);
         bot.Dice.OnSavedRolledFace.RemoveListener(HandleDiceSavedRolledFace);
         bot = null;
      }

      private void HandleMaxHealthChanged(int _) => RefreshHealthBar();
      private void HandleHealthChanged(int _) => RefreshHealthBar();

      private void RebuildActions() {
         while (coreActions.Count < bot.Dice.CoreActions.Count) {
            var action = Instantiate(diceBotActionPrefab, actionsParent);
            action.OnClicked.AddListener(HandleActionClicked);
            action.OnPointerEntered.AddListener(HandleActionPointerEntered);
            action.OnPointerExited.AddListener(HandleActionPointerExited);
            coreActions.Add(action);
         }

         for (var i = 0; i < bot.Dice.CoreActions.Count; ++i) {
            coreActions[i].SetAction(bot.Dice.CoreActions[i]);
            coreActions[i].gameObject.SetActive(true);
         }

         for (var i = bot.Dice.CoreActions.Count; i < coreActions.Count; ++i) {
            coreActions[i].gameObject.SetActive(false);
         }

         diceRollAction.transform.SetSiblingIndex(diceRollAction.transform.parent.childCount - 1);
      }

      private void HandleActionClicked(DiceBotActionUi actionUi) => InvokeEventForAction(OnBotActionClicked, actionUi);
      private void HandleActionPointerEntered(DiceBotActionUi actionUi) => InvokeEventForAction(OnBotActionHoverStarted, actionUi);
      private void HandleActionPointerExited(DiceBotActionUi actionUi) => InvokeEventForAction(OnBotActionHoverStopped, actionUi);

      private void InvokeEventForAction(UnityEvent<DiceBot, CombatActionDefinition> eventToInvoke, DiceBotActionUi actionUi) {
         if (TryGetAction(actionUi, out var action)) {
            eventToInvoke.Invoke(bot, action);
         }
      }

      private bool TryGetAction(DiceBotActionUi actionUi, out CombatActionDefinition action) {
         if (diceRollAction == actionUi) {
            action = bot.Dice.LastRolledFace.Data.CombatAction;
            return true;
         }
         var actionIndex = coreActions.IndexOf(actionUi);
         if (actionIndex > -1) {
            action = bot.Dice.CoreActions[actionIndex];
            return true;
         }
         action = default;
         return false;
      }

      private void RefreshHealthBar() {
         while (healthPoints.Count > bot.HealthSystem.MaxHealth) {
            HealthBarManager.Release(healthPoints[0]);
            healthPoints.RemoveAt(0);
         }
         while (healthPoints.Count < bot.HealthSystem.MaxHealth) {
            healthPoints.Add(HealthBarManager.Get(healthPointsParent));
         }

         for (var i = 0; i < healthPoints.Count; ++i) {
            HealthBarManager.SetFull(healthPoints[i], i < bot.HealthSystem.CurrentHealth);
         }
      }
   }
}