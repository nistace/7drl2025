using System.Collections.Generic;
using DiceBotsGame.DiceBots;
using UnityEngine;

namespace DiceBotsGame.UI {
   [RequireComponent(typeof(InOutUiAnimator))]
   public class DiceBotsScreenUi : MonoBehaviour, IMainScreenUi {
      [SerializeField] private InOutUiAnimator animator;
      [SerializeField] private DiceBotsListUi playerBotsList;
      [SerializeField] private DiceBotsListUi encounterBotsList;

      private DiceBotsParty playerParty;

      private void Start() {
         playerBotsList.SetInteractable(false);
         encounterBotsList.SetInteractable(false);

         playerBotsList.SetVisible(false);
         encounterBotsList.SetVisible(false);
      }

      public void SetUp(DiceBotsParty playerParty) {
         CleanUp();

         this.playerParty = playerParty;
         foreach (var diceBot in this.playerParty.DiceBotsInParty) {
            playerBotsList.AddBot(diceBot);
         }

         this.playerParty.OnBotAdded.AddListener(HandlePlayerPartyBotAdded);
         animator.Enter();
      }

      private void CleanUp() {
         playerBotsList.CleanUpAllBots();
         encounterBotsList.CleanUpAllBots();
         playerParty?.OnBotAdded.RemoveListener(HandlePlayerPartyBotAdded);
      }

      private void HandlePlayerPartyBotAdded(DiceBot newBot) => playerBotsList.AddBot(newBot);

      public void SetupEncounter(IReadOnlyList<DiceBot> encounterBots) {
         encounterBotsList.CleanUpAllBots();
         foreach (var encounterBot in encounterBots) {
            encounterBotsList.AddBot(encounterBot);
         }

         encounterBotsList.SetActionsVisible(true);
         playerBotsList.SetActionsVisible(true);
      }

      public void EndEncounter() {
         encounterBotsList.CleanUpAllBots();
         encounterBotsList.SetActionsVisible(false);
         playerBotsList.SetActionsVisible(false);
      }

      public void Hide() => animator.Exit();

      public void SetPlayerActionsInteractable(bool interactable) => playerBotsList.SetInteractable(interactable);
   }
}