using System.Collections.Generic;
using DiceBotsGame.DiceBots;
using DiceBotsGame.DiceBots.Dices.Faces;
using TMPro;
using UnityEngine;
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

      public void SetUp(DiceBot bot) {
         CleanUp();
         this.bot = bot;

         botName.name = bot.DisplayName;

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
            coreActions.Add(Instantiate(diceBotActionPrefab, actionsParent));
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