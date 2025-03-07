using DiceBotsGame.BotUpgrades;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DiceBotsGame.UI.Forge {
   public class UpgradeArrow : MonoBehaviour {
      private static readonly int upgradeSelectedAnimParam = Animator.StringToHash("UpgradeSelected");
      [SerializeField] private Button button;
      [SerializeField] private Animator animator;

      private IUpgrade upgrade;

      public UnityEvent<IUpgrade> OnClick { get; } = new UnityEvent<IUpgrade>();

      private void Start() {
         button.onClick.AddListener(HandleButtonClicked);
      }

      private void HandleButtonClicked() => OnClick.Invoke(upgrade);

      public void Show(IUpgrade upgrade) {
         CleanUp();
         this.upgrade = upgrade;
         this.upgrade.OnSelectedChanged.AddListener(HandleUpgradeSelectedChanged);
         RefreshUpgradeSelectedState();
         gameObject.SetActive(true);
      }

      private void CleanUp() {
         upgrade?.OnSelectedChanged.RemoveListener(HandleUpgradeSelectedChanged);
         upgrade = null;
         RefreshUpgradeSelectedState();
      }

      private void OnDestroy() {
         CleanUp();
         button.onClick.RemoveListener(HandleButtonClicked);
      }

      private void HandleUpgradeSelectedChanged() => RefreshUpgradeSelectedState();
      private void RefreshUpgradeSelectedState() => animator.SetBool(upgradeSelectedAnimParam, upgrade?.Selected ?? false);

      public void Hide() {
         CleanUp();
         gameObject.SetActive(false);
      }
   }
}