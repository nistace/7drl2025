using DiceBotsGame.DiceBots.Dices;
using UnityEngine;

namespace DiceBotsGame.DiceBots {
   [CreateAssetMenu]
   public class DiceBotPattern : ScriptableObject {
      [SerializeField] protected string displayName;
      [SerializeField] protected CharacterDicePattern dicePattern;
      [SerializeField] protected Color[] colors;

      public string DisplayName => displayName;
      public CharacterDicePattern DicePattern => dicePattern;

      public Color RollColor() => colors[Random.Range(0, colors.Length)];
   }
}