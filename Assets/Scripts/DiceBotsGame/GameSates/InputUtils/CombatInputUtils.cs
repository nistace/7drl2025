using DiceBotsGame.Cameras;
using DiceBotsGame.CombatGrids;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DiceBotsGame.GameSates.CombatStates {
   public static class CombatInputUtils {
      public static InputActionMap CombatActionMap { get; } = InputSystem.actions.FindActionMap("Combat");
      private static InputAction aimAction { get; } = CombatActionMap.FindAction("Aim");

      public static bool TryGetHitCombatTile(out CombatGridTile hitTile) => hitTile =
         aimAction.enabled && Physics.Raycast(MainCameraController.MainCamera.ScreenPointToRay(aimAction.ReadValue<Vector2>()), out var hit, Mathf.Infinity, LayerMask.GetMask("CombatTile"))
            ? hit.collider.gameObject.GetComponentInParent<CombatGridTile>()
            : default;
   }
}