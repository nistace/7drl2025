using DiceBotsGame.Cameras;
using DiceBotsGame.CombatGrids;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DiceBotsGame.GameSates.CombatStates {
   public static class CombatInputUtils {
      public static InputActionMap ActionMap { get; } = InputSystem.actions.FindActionMap("Combat");
      public static InputAction Aim { get; } = ActionMap.FindAction("Aim");
      public static InputAction Interact { get; } = ActionMap.FindAction("Interact");
      public static InputAction Cancel { get; } = ActionMap.FindAction("Cancel");

      public static bool TryGetHitCombatTile(out CombatGridTile hitTile) => hitTile =
         Aim.enabled && Physics.Raycast(MainCameraController.MainCamera.ScreenPointToRay(Aim.ReadValue<Vector2>()), out var hit, Mathf.Infinity, LayerMask.GetMask("CombatTile"))
            ? hit.collider.gameObject.GetComponentInParent<CombatGridTile>()
            : default;
   }
}