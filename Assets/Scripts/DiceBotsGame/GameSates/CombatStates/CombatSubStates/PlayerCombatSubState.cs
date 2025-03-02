using DiceBotsGame.CombatGrids;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DiceBotsGame.GameSates.CombatStates.CombatSubStates {
   public class PlayerCombatSubState : ICombatSubState {
      private readonly InputAction interactAction = InputSystem.actions.FindAction("Interact");
      private CombatGridTile hoveredTile;

      public bool IsOver { get; private set; }

      public void StartState() {
         interactAction.performed += HandleInteractPerformed;
         IsOver = false;
      }

      private void HandleInteractPerformed(InputAction.CallbackContext obj) {
         Debug.Log("Clicked");
         IsOver = true;
      }

      public void Update() {
         CombatInputUtils.TryGetHitCombatTile(out var newHoveredTile);

         if (newHoveredTile != hoveredTile) {
            if (hoveredTile) hoveredTile.SetHighlight(CombatGridTileConfig.HighlightType.None);
            hoveredTile = newHoveredTile;
            if (hoveredTile) hoveredTile.SetHighlight(CombatGridTileConfig.HighlightType.HoveredDefaultSelectable);
         }
      }

      public void EndState() {
         interactAction.performed -= HandleInteractPerformed;
         IsOver = false;
         if (hoveredTile) {
            hoveredTile.SetHighlight(CombatGridTileConfig.HighlightType.None);
            hoveredTile = null;
         }
      }
   }
}