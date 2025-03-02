using System;
using DiceBotsGame.CombatActions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DiceBotsGame.DiceBots.Dices.Faces {
   [Serializable]
   public class CharacterDiceFaceData {
      [SerializeField] protected int healthPoints;
      [SerializeField] private CombatActionDefinition combatAction;

      public int HealthPoints => healthPoints;
      public bool HasCombatAction => combatAction.Action;
      public MeshRenderer CombatActionModel => combatAction.Action.Model;
      public CombatActionDefinition CombatAction => combatAction;

      public CharacterDiceFaceData() { }

      public CharacterDiceFaceData(int healthPoints, CombatActionDefinition combatAction) {
         this.healthPoints = healthPoints;
      }

      public CharacterDiceFaceData(CharacterDiceFaceData source) : this(source.healthPoints, source.combatAction) { }
   }
}