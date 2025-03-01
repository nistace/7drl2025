using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices.Faces {
   [CreateAssetMenu]
   public class CharacterDiceFacePattern : ScriptableObject {
      [SerializeField] protected CharacterDiceFaceData data;

      public CharacterDiceFaceData Data => data;
   }
}