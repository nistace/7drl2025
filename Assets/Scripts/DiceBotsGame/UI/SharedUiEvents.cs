using DiceBotsGame.CombatActions;
using UnityEngine.Events;

namespace DiceBotsGame.UI {
   public static class SharedUiEvents {
      public static UnityEvent<CombatActionDefinition> OnActionHoverStarted { get; } = new UnityEvent<CombatActionDefinition>();
      public static UnityEvent<CombatActionDefinition> OnActionHoverStopped { get; } = new UnityEvent<CombatActionDefinition>();
   }
}