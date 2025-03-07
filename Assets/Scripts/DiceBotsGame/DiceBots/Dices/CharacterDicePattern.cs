using System;
using System.Collections.Generic;
using DiceBotsGame.CombatActions;
using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices {
   [Serializable]
   public class CharacterDicePattern {
      [SerializeField] protected CharacterDiceData data;
      [SerializeField] protected CombatActionDefinition[] faceActions = new CombatActionDefinition[6];

      public CharacterDiceData Data => data;
      public IReadOnlyList<CombatActionDefinition> FaceActions => faceActions;
   }
}