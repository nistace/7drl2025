using System;
using DiceBotsGame.CombatActions;
using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices.Faces {
   [Serializable]
   public class CharacterDiceFaceData {
      [SerializeField] private CombatActionDefinition combatAction;

      public bool HasCombatAction => combatAction.Action;
      public CombatActionDefinition CombatAction => combatAction;

      public CharacterDiceFaceData() { }

      public CharacterDiceFaceData(CombatActionDefinition combatAction) {
         this.combatAction = combatAction;
      }
   }
}