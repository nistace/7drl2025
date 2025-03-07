using System;
using UnityEngine;

namespace DiceBotsGame.CombatActions {
   [Serializable]
   public class CombatActionDefinition {
      [SerializeField] protected CombatAction action;
      [SerializeField] protected int constantStrength = 1;

      public CombatAction Action => action;
      public bool IsValidAction => action;
      public int ConstantStrength => constantStrength;
      public string DisplayName => IsValidAction ? $"{action.ActionName} ({constantStrength})" : "Doing nothing";

      public CombatActionDefinition() { }

      public CombatActionDefinition(CombatAction action) : this(action, 1) { }

      public CombatActionDefinition(CombatAction action, int constantStrength) {
         this.action = action;
         this.constantStrength = constantStrength;
      }
   }
}