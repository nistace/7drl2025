using System.Collections.Generic;
using DiceBotsGame.DiceBots.Dices.Faces;
using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices {
   [CreateAssetMenu]
   public class CharacterDicePattern : ScriptableObject {
      [SerializeField] protected CharacterDiceData data;
      [SerializeField] protected CharacterDiceFacePattern[] facePatterns = new CharacterDiceFacePattern[6];

      public CharacterDiceData Data => data;
      public IReadOnlyList<CharacterDiceFacePattern> FacePatterns => facePatterns;
   }
}