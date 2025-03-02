using System;
using System.Collections.Generic;
using DiceBotsGame.DiceBots.Dices.Faces;
using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices {
   [Serializable]
   public class CharacterDicePattern {
      [SerializeField] protected CharacterDiceData data;
      [SerializeField] protected CharacterDiceFaceData[] facePatterns = new CharacterDiceFaceData[6];

      public CharacterDiceData Data => data;
      public IReadOnlyList<CharacterDiceFaceData> FacePatterns => facePatterns;
   }
}