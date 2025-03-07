using DiceBotsGame.CombatActions;
using DiceBotsGame.CombatGrids;
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
      private CombatGrid combatGrid;

      public UnityEvent<DiceBot, CombatActionDefinition> OnPlayerBotActionHoverStarted { get; } = new UnityEvent<DiceBot, CombatActionDefinition>();
      public UnityEvent<DiceBot, CombatActionDefinition> OnPlayerBotActionHoverStopped { get; } = new UnityEvent<DiceBot, CombatActionDefinition>();
      public UnityEvent<DiceBot, CombatActionDefinition> OnPlayerBotActionClicked { get; } = new UnityEvent<DiceBot, CombatActionDefinition>();
      public UnityEvent<DiceBot, CombatActionDefinition> OnOpponentActionHoverStarted { get; } = new UnityEvent<DiceBot, CombatActionDefinition>();
      public UnityEvent<DiceBot, CombatActionDefinition> OnOpponentActionHoverStopped { get; } = new UnityEvent<DiceBot, CombatActionDefinition>();

      private void Start() {
         playerBotsList.SetInteractable(false);
         playerBotsList.OnBotActionHoverStarted.AddListener(OnPlayerBotActionHoverStarted.Invoke);
         playerBotsList.OnBotActionHoverStopped.AddListener(OnPlayerBotActionHoverStopped.Invoke);
         playerBotsList.OnBotActionClicked.AddListener(OnPlayerBotActionClicked.Invoke);
         encounterBotsList.OnBotActionHoverStarted.AddListener(OnOpponentActionHoverStarted.Invoke);
         encounterBotsList.OnBotActionHoverStopped.AddListener(OnOpponentActionHoverStopped.Invoke);

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

      public void Show() => animator.Enter();

      private void CleanUp() {
         playerBotsList.CleanUpAllBots();
         encounterBotsList.CleanUpAllBots();
         playerParty?.OnBotAdded.RemoveListener(HandlePlayerPartyBotAdded);
      }

      private void HandlePlayerPartyBotAdded(DiceBot newBot) => playerBotsList.AddBot(newBot);

      public void SetupEncounter(CombatGrid combatGrid) {
         this.combatGrid = combatGrid;
         encounterBotsList.CleanUpAllBots();
         foreach (var encounterBot in combatGrid.ListOfOpponentBots) {
            encounterBotsList.AddBot(encounterBot);
         }

         encounterBotsList.SetActionsVisible(true);
         playerBotsList.SetActionsVisible(true);
         combatGrid.OnNewOpponent.AddListener(HandleNewOpponent);
      }

      private void HandleNewOpponent(DiceBot newOpponent) {
         encounterBotsList.AddBot(newOpponent);
         encounterBotsList.SetActionsVisible(true);
      }

      public void EndEncounter() {
         if (combatGrid) combatGrid.OnNewOpponent.RemoveListener(HandleNewOpponent);
         encounterBotsList.CleanUpAllBots();
         encounterBotsList.SetActionsVisible(false);
         playerBotsList.SetActionsVisible(false);
      }

      private void OnDestroy() {
         playerBotsList.SetInteractable(false);
         playerBotsList.OnBotActionHoverStarted.RemoveListener(OnPlayerBotActionHoverStarted.Invoke);
         playerBotsList.OnBotActionHoverStopped.RemoveListener(OnPlayerBotActionHoverStopped.Invoke);
         playerBotsList.OnBotActionClicked.RemoveListener(OnPlayerBotActionClicked.Invoke);
         encounterBotsList.OnBotActionHoverStarted.RemoveListener(OnOpponentActionHoverStarted.Invoke);
         encounterBotsList.OnBotActionHoverStopped.RemoveListener(OnOpponentActionHoverStopped.Invoke);
      }

      public void Hide() => animator.Exit();

      public void SetPlayerActionsInteractable(bool interactable) => playerBotsList.SetInteractable(interactable);
   }
}