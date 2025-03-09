using System.Collections.Generic;
using DiceBotsGame.BotUpgrades;
using DiceBotsGame.UI;

namespace DiceBotsGame.GameSates.WorldStates {
   public class ForgeState : GameState {
      private PartyUpgrade UpgradeData { get; }

      private HashSet<IUpgrade> SelectedUpgrades { get; } = new HashSet<IUpgrade>();

      public ForgeState(PartyUpgrade upgradeData) {
         UpgradeData = upgradeData;
      }

      protected override void Enable() {
         MainUi.Log.SetTexts(UpgradeData.DisplayTitle, $"You can select up to {UpgradeData.UpgradeCountAllowed} upgrades");

         MainUi.DiceBots.Hide();
         MainUi.Forge.Open(UpgradeData);

         MainUi.Forge.OnUpgradeClicked.AddListener(HandleUpgradeClicked);
         MainUi.Forge.OnValidated.AddListener(HandleForgeValidated);
      }

      private void HandleForgeValidated() {
         if (SelectedUpgrades.Count < UpgradeData.UpgradeCountAllowed) {
            MainUi.Forge.SetInteractable(false);
            MainUi.Prompt.ShowPrompt("You can select up to " + UpgradeData.UpgradeCountAllowed + " upgrades. If you continue, you will not be able to select these upgrades anymore. Continue?",
               ("Continue", ConfirmValidated),
               ("Cancel", CancelValidated));
         }
         else {
            ConfirmValidated();
         }
      }

      private static void CancelValidated() {
         MainUi.Prompt.Hide();
         MainUi.Forge.SetInteractable(true);
      }

      private void ConfirmValidated() {
         MainUi.Prompt.Hide();
         foreach (var upgrade in SelectedUpgrades) {
            upgrade.Apply();
         }

         ChangeState(WorldState.Instance);
      }

      private void HandleUpgradeClicked(IUpgrade upgrade) => ToggleUpgradeSelection(upgrade);

      private void ToggleUpgradeSelection(IUpgrade upgrade) {
         if (SelectedUpgrades.Count < UpgradeData.UpgradeCountAllowed && SelectedUpgrades.Add(upgrade)) {
            upgrade.SetSelected(true);
         }
         else {
            SelectedUpgrades.Remove(upgrade);
            upgrade.SetSelected(false);
         }

         MainUi.Log.SetTexts("Level up!", $"Selected upgrades: {SelectedUpgrades.Count} / {UpgradeData.UpgradeCountAllowed}");
      }

      protected override void Disable() {
         MainUi.Forge.OnUpgradeClicked.RemoveListener(HandleUpgradeClicked);
         MainUi.Forge.OnValidated.RemoveListener(HandleForgeValidated);
         MainUi.DiceBots.Show();
         MainUi.Forge.Hide();
      }

      protected override void Update() {
         GameInfo.PlayerParty.UpdateBotsWorldPosition();
      }
   }
}