using DiceBotsGame.CombatGrids;
using DiceBotsGame.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace DiceBotsGame.GameSates.CombatStates.CombatSubStates {
   public class PlayerCombatSubState : ICombatSubState {
      private readonly InputAction interactAction = InputSystem.actions.FindAction("Interact");
      private CombatGridTile hoveredTile;
      private bool overUi;

      public bool IsOver { get; private set; }

      public void StartState() {
         MainUi.DiceBots.SetPlayerActionsInteractable(true);
         interactAction.performed += HandleInteractPerformed;
         IsOver = false;
      }

      private void HandleInteractPerformed(InputAction.CallbackContext obj) {
         if (overUi) return;
         Debug.Log("Clicked");
         IsOver = true;
      }

      public void Update() {
         overUi = EventSystem.current.IsPointerOverGameObject();
         CombatInputUtils.TryGetHitCombatTile(out var newHoveredTile);

         if (newHoveredTile != hoveredTile) {
            if (hoveredTile) hoveredTile.SetHighlight(CombatGridTileConfig.HighlightType.None);
            hoveredTile = newHoveredTile;
            if (hoveredTile) hoveredTile.SetHighlight(CombatGridTileConfig.HighlightType.HoveredDefaultSelectable);
         }
      }

      public void EndState() {
         MainUi.DiceBots.SetPlayerActionsInteractable(false);
         interactAction.performed -= HandleInteractPerformed;
         IsOver = false;
         if (hoveredTile) {
            hoveredTile.SetHighlight(CombatGridTileConfig.HighlightType.None);
            hoveredTile = null;
         }
      }
   }
}