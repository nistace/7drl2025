using UnityEngine.InputSystem;

namespace DiceBotsGame.GameSates.InputUtils {
   public static class WorldInputUtils {
      public static InputActionMap ActionMap { get; } = InputSystem.actions.FindActionMap("World");
      public static InputAction Aim { get; } = ActionMap.FindAction("Aim");
      public static InputAction Interact { get; } = ActionMap.FindAction("Interact");
   }
}