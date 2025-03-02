using DiceBotsGame.DiceBots.Dices;
using UnityEngine;

namespace DiceBotsGame.DiceBots {
   [CreateAssetMenu]
   public class DiceBotPattern : ScriptableObject {
      [SerializeField] protected CharacterDicePattern[] dicePattern;
      [SerializeField] protected Color[] colors;

      public CharacterDicePattern RollDicePattern() => dicePattern[Random.Range(0, dicePattern.Length)];
      public Color RollColor() => colors[Random.Range(0, dicePattern.Length)];
   }
}