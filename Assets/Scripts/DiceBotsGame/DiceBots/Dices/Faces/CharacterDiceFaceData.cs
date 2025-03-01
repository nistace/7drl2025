using System;
using DiceBotsGame.CombatActions;
using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices.Faces {
   [Serializable]
   public class CharacterDiceFaceData {
      [SerializeField] protected int healthPoints;
      [SerializeField] protected int constantStrength = 1;
      [SerializeField] protected int variableStrength;
      [SerializeField] protected CombatAction combatAction;

      public int HealthPoints => healthPoints;
      public int ConstantStrength => constantStrength;
      public int VariableStrength => variableStrength;
      public CombatAction CombatAction => combatAction;

      public CharacterDiceFaceData() { }

      public CharacterDiceFaceData(int healthPoints, int constantStrength, int variableStrength, CombatAction combatAction) {
         this.healthPoints = healthPoints;
         this.constantStrength = constantStrength;
         this.variableStrength = variableStrength;
         this.combatAction = combatAction;
      }

      public CharacterDiceFaceData(CharacterDiceFaceData source) : this(source.healthPoints, source.constantStrength, source.variableStrength, source.combatAction) { }
   }
}