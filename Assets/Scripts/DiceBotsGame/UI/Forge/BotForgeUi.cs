using System;
using System.Linq;
using DiceBotsGame.BotUpgrades;
using DiceBotsGame.UI.Health;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DiceBotsGame.UI.Forge {
   public class BotForgeUi : MonoBehaviour {
      [SerializeField] protected Image[] coloredImages;
      [SerializeField] protected Image portrait;
      [SerializeField] private TMP_Text botName;
      [SerializeField] private HealthBarUi currentHealthBar;
      [SerializeField] private UpgradeArrow healthUpgradeArrow;
      [SerializeField] private HealthBarUi upgradeHealthBar;
      [SerializeField] protected DiceBotActionUi[] currentActions;
      [SerializeField] protected UpgradeArrow[] upgradeArrows;
      [SerializeField] protected DiceBotActionUi[] upgradeActions;

      private FaceUpgrade[] FaceUpgrades { get; set; }
      public UnityEvent<IUpgrade> OnUpgradeClicked { get; } = new UnityEvent<IUpgrade>();

      private void Start() {
         foreach (var currentAction in currentActions) {
            currentAction.OnClicked.AddListener(HandleCurrentActionClicked);
         }
         foreach (var upgradeArrow in upgradeArrows) {
            upgradeArrow.OnClick.AddListener(HandleActionUpgradeClicked);
         }
         foreach (var action in upgradeActions) {
            action.OnClicked.AddListener(HandleUpgradeActionClicked);
         }
         healthUpgradeArrow.OnClick.AddListener(HandleHealthUpgradeClicked);
      }

      private void HandleActionUpgradeClicked(IUpgrade actionUpgrade) => OnUpgradeClicked.Invoke(actionUpgrade);
      private void HandleHealthUpgradeClicked(IUpgrade healthUpgrade) => OnUpgradeClicked.Invoke(healthUpgrade);

      private void HandleCurrentActionClicked(DiceBotActionUi action) {
         var index = Array.IndexOf(currentActions, action);
         OnUpgradeClicked.Invoke(FaceUpgrades[index]);
      }

      private void HandleUpgradeActionClicked(DiceBotActionUi action) {
         var index = Array.IndexOf(upgradeActions, action);
         OnUpgradeClicked.Invoke(FaceUpgrades[index]);
      }

      public void SetUp(BotUpgrade botUpgrade) {
         FaceUpgrades = botUpgrade.FaceUpgrades.Values.OrderBy(t => botUpgrade.Bot.Dice.IndexOfFace(t.Face)).ToArray();
         foreach (var coloredImage in coloredImages) {
            coloredImage.color = botUpgrade.Bot.Color;
         }

         botName.text = botUpgrade.Bot.name;
         currentHealthBar.Refresh(botUpgrade.Bot.HealthSystem.CurrentHealth, botUpgrade.Bot.HealthSystem.MaxHealth);
         if (botUpgrade.HasHealthUpgrade) {
            upgradeHealthBar.Refresh(botUpgrade.HealthUpgrade.NewHealth, botUpgrade.HealthUpgrade.NewHealth);
            healthUpgradeArrow.Show(botUpgrade.HealthUpgrade);
         }
         else {
            upgradeHealthBar.Refresh(0, 0);
            healthUpgradeArrow.Hide();
         }

         for (var faceIndex = 0; faceIndex < botUpgrade.Bot.Dice.Faces.Count; faceIndex++) {
            var face = botUpgrade.Bot.Dice.Faces[faceIndex];
            currentActions[faceIndex].SetAction(face.Data.CombatAction);

            if (botUpgrade.FaceUpgrades.TryGetValue(face, out var faceUpgrade)) {
               currentActions[faceIndex].SetInteractable(true);
               upgradeArrows[faceIndex].Show(faceUpgrade);
               upgradeActions[faceIndex].gameObject.SetActive(true);
               upgradeActions[faceIndex].SetAction(faceUpgrade.Upgrade);
            }
            else {
               currentActions[faceIndex].SetInteractable(false);
               upgradeArrows[faceIndex].Hide();
               upgradeActions[faceIndex].gameObject.SetActive(false);
            }
         }
      }

#if UNITY_EDITOR

      [ContextMenu("Fix names")] private void FixeNames() {
         var parent = transform.Find("DiceUpgrades");
         if (parent) {
            currentActions = parent.GetComponentsInChildren<DiceBotActionUi>().Take(6).ToArray();
            upgradeArrows = parent.GetComponentsInChildren<UpgradeArrow>().Take(6).ToArray();
            upgradeActions = parent.GetComponentsInChildren<DiceBotActionUi>().Skip(6).Take(6).ToArray();
         }

         for (var index = 0; index < currentActions.Length; index++) {
            var currentAction = currentActions[index];
            if (currentAction) {
               currentAction.name = $"CurrentAction_{index}";
               currentAction.transform.parent.name = $"CurrentActionParent_{index}";
               EditorUtility.SetDirty(currentAction);
               EditorUtility.SetDirty(currentAction.transform.parent);
            }
         }

         for (var index = 0; index < upgradeArrows.Length; index++) {
            var upgradeArrow = upgradeArrows[index];
            if (upgradeArrow) {
               upgradeArrow.name = $"UpgradeArrow_{index}";
               EditorUtility.SetDirty(upgradeArrow);
            }
         }

         for (var index = 0; index < upgradeActions.Length; index++) {
            var upgradeAction = upgradeActions[index];
            if (upgradeAction) {
               upgradeAction.name = $"UpgradeAction_{index}";
               upgradeAction.transform.parent.name = $"UpgradeActionParent_{index}";
               EditorUtility.SetDirty(upgradeAction);
               EditorUtility.SetDirty(upgradeAction.transform.parent);
            }
         }
      }

#endif
   }
}