using UnityEngine;

namespace DiceBotsGame.DiceBots.Dices.Faces {
   [CreateAssetMenu]
   public class CharacterDiceFaceMeshConfig : ScriptableObject {
      [SerializeField] protected Mesh[] digits;
      [SerializeField] protected Mesh digitDefault;

      public Mesh Digit(int value) => value < 0 || value >= digits.Length ? digitDefault : digits[value];
   }
}