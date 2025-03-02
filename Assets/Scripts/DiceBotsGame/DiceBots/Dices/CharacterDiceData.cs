using System;
using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.CombatActions;
using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices {
   [Serializable]
   public class CharacterDiceData {
      [SerializeField] protected int coreHealth = 3;
      [SerializeField] protected CombatActionDefinition[] coreCombatActions;

      public int CoreHealth => coreHealth;
      public IReadOnlyList<CombatActionDefinition> CoreCombatActions => coreCombatActions;

      public CharacterDiceData() { }

      public CharacterDiceData(int coreHealth, CombatActionDefinition[] coreCombatActions) {
         this.coreHealth = coreHealth;
         this.coreCombatActions = coreCombatActions;
      }

      public CharacterDiceData(CharacterDiceData source) : this(source.coreHealth, source.coreCombatActions.ToArray()) { }
   }
}