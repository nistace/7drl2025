using UnityEngine;

namespace DiceBotsGame.UI {
   [CreateAssetMenu]
   public class DiceBotActionConfig : ScriptableObject {
      [SerializeField] protected Sprite noActionSprite;
      [SerializeField] protected Sprite rollingActionSprite;
      [SerializeField] protected Sprite[] digits;
      [SerializeField] protected Sprite digitDefault;

      public Sprite NoActionSprite => noActionSprite;
      public Sprite RollingActionSprite => rollingActionSprite;

      public Sprite Digit(int value) => value < 0 || value >= digits.Length ? digitDefault : digits[value];
   }
}