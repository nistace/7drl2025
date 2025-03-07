using DiceBotsGame.BotUpgrades;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DiceBotsGame.UI.Forge {
   [RequireComponent(typeof(InOutUiAnimator))]
   public class ForgeUi : MonoBehaviour, IMainScreenUi {
      [SerializeField] protected InOutUiAnimator uiAnimator;
      [SerializeField] private BotForgeUi[] botPanels;
      [SerializeField] private Button validateButton;

      public UnityEvent<IUpgrade> OnUpgradeClicked { get; } = new UnityEvent<IUpgrade>();
      public UnityEvent OnValidated { get; } = new UnityEvent();

      private void Reset() => uiAnimator = GetComponent<InOutUiAnimator>();

      private void Start() {
         uiAnimator.SnapOut();

         validateButton.onClick.AddListener(HandleValidateClicked);
         foreach (var botPanel in botPanels) {
            botPanel.OnUpgradeClicked.AddListener(OnUpgradeClicked.Invoke);
         }
      }

      private void OnDestroy() {
         validateButton.onClick.RemoveListener(HandleValidateClicked);
         foreach (var botPanel in botPanels) {
            botPanel.OnUpgradeClicked.RemoveListener(OnUpgradeClicked.Invoke);
         }
      }

      private void HandleValidateClicked() {
         OnValidated.Invoke();
      }

      public void Open(PartyUpgrade partyUpgrade) {
         for (var botIndex = 0; botIndex < partyUpgrade.BotUpgrades.Count; botIndex++) {
            botPanels[botIndex].SetUp(partyUpgrade.BotUpgrades[botIndex]);
            botPanels[botIndex].gameObject.SetActive(true);
         }

         for (var botIndex = partyUpgrade.BotUpgrades.Count; botIndex < botPanels.Length; botIndex++) {
            botPanels[botIndex].gameObject.SetActive(false);
         }

         uiAnimator.Enter();
      }

      public void Hide() => uiAnimator.Exit();
   }
}