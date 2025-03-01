using System;
using System.Collections.Generic;
using System.Linq;
using DiceBotsGame.CombatActions;
using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices {
   [Serializable]
   public class CharacterDiceData {
      [SerializeField] protected int coreHealth = 3;
      [SerializeField] protected CombatAction[] coreCombatActions;

      public int CoreHealth => coreHealth;
      public IReadOnlyList<CombatAction> CoreCombatActions => coreCombatActions;

      public CharacterDiceData() { }

      public CharacterDiceData(int coreHealth, CombatAction[] coreCombatActions) {
         this.coreHealth = coreHealth;
         this.coreCombatActions = coreCombatActions;
      }

      public CharacterDiceData(CharacterDiceData source) : this(source.coreHealth, source.coreCombatActions.ToArray()) { }
   }
}