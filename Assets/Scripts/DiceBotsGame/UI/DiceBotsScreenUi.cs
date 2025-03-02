using System.Collections.Generic;
using DiceBotsGame.CombatActions;
using DiceBotsGame.DiceBots;
using UnityEngine;
using UnityEngine.Events;

namespace DiceBotsGame.UI {
   [RequireComponent(typeof(InOutUiAnimator))]
   public class DiceBotsScreenUi : MonoBehaviour, IMainScreenUi {
      [SerializeField] private InOutUiAnimator animator;
      [SerializeField] private DiceBotsListUi playerBotsList;
      [SerializeField] private DiceBotsListUi encounterBotsList;

      private DiceBotsParty playerParty;

      public UnityEvent<DiceBot, CombatActionDefinition> OnPlayerBotActionHoverStarted { get; } = new UnityEvent<DiceBot, CombatActionDefinition>();
      public UnityEvent<DiceBot, CombatActionDefinition> OnPlayerBotActionHoverStopped { get; } = new UnityEvent<DiceBot, CombatActionDefinition>();
      public UnityEvent<DiceBot, CombatActionDefinition> OnPlayerBotActionClicked { get; } = new UnityEvent<DiceBot, CombatActionDefinition>();

      private void Start() {
         playerBotsList.SetInteractable(false);
         playerBotsList.OnBotActionHoverStarted.AddListener(OnPlayerBotActionHoverStarted.Invoke);
         playerBotsList.OnBotActionHoverStopped.AddListener(OnPlayerBotActionHoverStopped.Invoke);
         playerBotsList.OnBotActionClicked.AddListener(OnPlayerBotActionClicked.Invoke);

         encounterBotsList.SetInteractable(false);
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