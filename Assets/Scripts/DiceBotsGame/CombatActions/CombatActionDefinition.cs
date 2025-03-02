using System;
using UnityEngine;

namespace DiceBotsGame.CombatActions {
   [Serializable]
   public class CombatActionDefinition {
      [SerializeField] protected CombatAction action;
      [SerializeField] protected int constantStrength = 1;
      [SerializeField] protected int variableStrength;

      public CombatAction Action => action;
      public bool IsValidAction => action;
      public int ConstantStrength => constantStrength;
      public int VariableStrength => variableStrength;
   }
}