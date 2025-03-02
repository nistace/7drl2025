﻿using System;
using DiceBotsGame.CombatActions;
using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices.Faces {
   [Serializable]
   public class CharacterDiceFaceData {
      [SerializeField] private CombatActionDefinition combatAction;

      public bool HasCombatAction => combatAction.Action;
      public Mesh CombatActionMesh => combatAction.Action.Mesh;
      public CombatActionDefinition CombatAction => combatAction;

      public CharacterDiceFaceData() { }

      public CharacterDiceFaceData(CombatActionDefinition combatAction) {
         this.combatAction = combatAction;
      }

      public CharacterDiceFaceData(CharacterDiceFaceData source) : this(source.combatAction) { }
   }
}